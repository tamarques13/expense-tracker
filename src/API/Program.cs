using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Spentir.Infrastructure.Persistence.Configurations;
using Spentir.Application.Services.Interfaces;
using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Application.Services;
using Spentir.Infrastructure.Persistence.Repositories;
using Spentir.API.Middleware;
using Spentir.Domain.Services.Interfaces;
using Spentir.Domain.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY environment variable is not set.");

builder.Services.AddDbContext<SpentirDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllers();

// Development-only CORS policy.
// WARNING: Allowing any origin, method and header is not safe for production.
// Review and restrict origins before deploying.

builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAnalyticService, AnalyticService>();

builder.Services.AddScoped<IDateRangeService, DateRangeService>();
builder.Services.AddScoped<ICategoryAggregateService, CategoryAggregateService>();
builder.Services.AddScoped<ICategoryTrendCalculator, CategoryTrendCalculator>();
builder.Services.AddScoped<IMonthAnalyticsCalculator, MonthAnalyticsCalculator>();
builder.Services.AddScoped<IYearAnalyticsCalculator, YearAnalyticsCalculator>();

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization : 'Bearer Genreated-JWT-Token'",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{ }
            }
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
