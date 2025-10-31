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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

var currentEnvironment = env.IsDevelopment() ? "Development" : "Production";
Console.WriteLine($"Starting the Program in {currentEnvironment} Environment...");

// prefetch the port from the environment variable or use 5182 as default
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
    var dbUrl = VariableParser.GetEnvString("DATABASE_URL");
    if (string.IsNullOrEmpty(dbUrl))
        throw new InvalidOperationException("DATABASE_URL environment variable is not set.");

    var connectionString = DBUrlParser.ParseDatabaseUrl(dbUrl);
    options.UseNpgsql(connectionString);
});

builder.Services.AddOptions();

// Repository Dependency Injection
builder.Services.AddScoped<IClassScheduleRepository, ClassScheduleRepository>();
builder.Services.AddScoped<IClassSessionRepository, ClassSessionRepository>();
builder.Services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
builder.Services.AddScoped<IGuardianRepository, GuardianRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectTeacherRepository, SubjectTeacherRepository>();
builder.Services.AddScoped<ITwoFactorRepository, TwoFactorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Services Dependency Injection
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

// Authorization Handlers Dependency Injection
builder.Services.AddSingleton<IAuthorizationHandler, OwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, OwnerOrRoleHandler>();

// ✅ Configure Brevo SMTP Email Service
if (env.IsDevelopment())
{
    var smtpSettings = builder.Configuration.GetSection("SmtpSettings");
    if (!smtpSettings.Exists())
        throw new InvalidOperationException("SmtpSettings section is not configured in your appsettings.json.");
    builder.Services.Configure<EmailSettings>(smtpSettings);
}
else
{
    builder.Services.Configure<EmailSettings>((option) =>
    {
        option.Server = VariableParser.GetEnvString("SMTP_SERVER");
        option.Port = VariableParser.GetEnvInt("SMTP_PORT");
        option.SenderEmail = VariableParser.GetEnvString("SMTP_SENDER_EMAIL");
        option.SenderName = VariableParser.GetEnvString("SMTP_SENDER_NAME");
        option.Username = VariableParser.GetEnvString("SMTP_USERNAME");
        option.Password = VariableParser.GetEnvString("SMTP_PASSWORD");
        option.APIKey = VariableParser.GetEnvString("BREVO_API_KEY");
    });
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register HttpClient for DI
builder.Services.AddHttpClient();

// Add controller service
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (env.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins("https://attendance-system-app-vpu1.onrender.com")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
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
    .AddPolicy("RequireStudent", policy =>
        policy.RequireRole(UserRole.Student.ToString()))
    .AddPolicy("RequireOwnerOrRole", policy =>
        policy.Requirements.Add(new OwnerOrRoleRequirement()))
    .AddPolicy("RequireOwnerOrAdmin", policy =>
        policy.Requirements.Add(new OwnerOrAdminRequirement()));

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    try
    {
        await next();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred.");
    }
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

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