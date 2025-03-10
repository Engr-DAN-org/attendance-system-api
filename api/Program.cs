using api.Data;
using api.Interfaces.Service;
using api.Models.DTOs;
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
    options.ListenAnyIP(int.Parse(port)); // Allow external access on the assigned port
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

// Add services with dependency injection to the container.
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add controller service
builder.Services.AddControllers();

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
builder.Services.AddTransient<EmailService>();

// Add Authentication and Authorization
builder.Services.AddAuthorization();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHttpsRedirection();
}

// use the Authentication and Authorization setup
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();