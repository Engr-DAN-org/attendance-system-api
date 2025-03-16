using api.Authorization;
using api.Data;
using api.Data.Seeders;
using api.Enums;
using api.Interfaces.Repository;
using api.Interfaces.Service;
using api.Models.DTOs;
using api.Repositories;
using api.Services;
using api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var port = Environment.GetEnvironmentVariable("PORT") ?? "5182";

// open the port for outside the Container
builder.WebHost.ConfigureKestrel(options =>
{
    // options.ListenAnyIP(int.Parse(port)); // Allow external access on the assigned port
    options.ListenAnyIP(int.Parse(port));  // IPv4
    // options.ListenLocalhost(int.Parse(port)); // Ensures localhost access
    // options.Listen(System.Net.IPAddress.IPv6Any, int.Parse(port)); // IPv6
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // ✅ Parse the DATABASE_URL into a connection string
    var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(dbUrl))
        throw new InvalidOperationException("DATABASE_URL environment variable is not set.");

    var connectionString = DBUrlParser.ParseDatabaseUrl(dbUrl);
    options.UseNpgsql(connectionString);
});

builder.Services.AddOptions();

// Repository Dependency Injection
// builder.Services.AddScoped<IClassScheduleRepository, ClassScheduleRepository>();
builder.Services.AddScoped<IGuardianRepository, GuardianRepository>();
// builder.Services.AddScoped<ISectionRepository, SectionRepository>();
// builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ITwoFactorRepository, TwoFactorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Services Dependency Injection
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
// ✅ Configure Brevo SMTP Email Service
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SmtpSettings"));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add controller service
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Add EmailService configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(static option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(new TokenGenerator().GetSecretKey()),
                ClockSkew = TimeSpan.Zero // Tokens expire exactly after 1 hour
            };

        });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireTeacherOrAdmin", policy =>
        policy.RequireRole(UserRole.Teacher.ToString(), UserRole.Admin.ToString()))
    .AddPolicy("RequireAdmin", policy =>
        policy.RequireRole(UserRole.Admin.ToString()))
    .AddPolicy("RequireSelfOrTeacherOrAdmin", policy =>
        policy.Requirements.Add(new RequireSelfOrRoleRequirement()))
    .AddPolicy("RequireSelfOrAdmin", policy =>
        policy.Requirements.Add(new RequireSelfOrAdminRequirement()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<DefaultUsersSeeder>>();
    var seeder = new DefaultUsersSeeder(dbContext, logger);
    await seeder.SeedAsync();
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Axios);

    });
}
else
{
    app.UseHttpsRedirection();
}

// use the Authentication and Authorization setup
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();