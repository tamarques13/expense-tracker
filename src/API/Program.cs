using DotNetEnv;
using Hangfire;
using Hangfire.PostgreSql;
using OpenAI;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Spentir.Application.Services.Interfaces;
using Spentir.Application.Services.Analytics.Interfaces;
using Spentir.Application.Services.Auth.Interfaces;
using Spentir.Application.Services;
using Spentir.Application.Services.Auth;
using Spentir.Application.Services.Auth.Tokens;
using Spentir.Application.Services.Analytics;
using Spentir.Application.Services.Analytics.Builders;
using Spentir.Application.Services.Analytics.Loaders;
using Spentir.Application.Services.Analytics.Ranges;
using Spentir.Application.Jobs.Subscription.Interfaces;
using Spentir.Application.Jobs.Subscription;

using Spentir.Infrastructure.Persistence.Configurations;
using Spentir.Infrastructure.Persistence.Repositories.Interfaces;
using Spentir.Infrastructure.Persistence.Transactions.Interfaces;
using Spentir.Infrastructure.Persistence.Repositories;
using Spentir.Infrastructure.Persistence.Transactions;

using Spentir.Domain.Services.Interfaces;
using Spentir.Domain.Services;

using Spentir.API.Middleware;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY environment variable is not set.");
var openAIKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("AI_KEY environment variable is not set.");

builder.Services.AddDbContext<SpentirDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllers();

// Development-only CORS policy.
// WARNING: Allowing any origin, method and header is not safe for production.
// Review and restrict origins before deploying.

builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthToken, AuthToken>();

builder.Services.AddScoped<IAnalyticService, AnalyticService>();

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddScoped<IDateRangeService, DateRangeService>();
builder.Services.AddScoped<ICategoryAggregateService, CategoryAggregateService>();
builder.Services.AddScoped<ICategoryTrendCalculator, CategoryTrendCalculator>();
builder.Services.AddScoped<IMonthAnalyticsCalculator, MonthAnalyticsCalculator>();
builder.Services.AddScoped<IYearAnalyticsCalculator, YearAnalyticsCalculator>();
builder.Services.AddScoped<IAnalyticsBuilder, AnalyticsBuilder>();
builder.Services.AddScoped<IRangeCalculator, RangeCalculator>();
builder.Services.AddScoped<IExpenseLoader, ExpenseLoader>();

builder.Services.AddScoped<ISubscriptionRules, SubscriptionRules>();
builder.Services.AddScoped<ISubscriptionActions, SubscriptionActions>();
builder.Services.AddScoped<ISubscriptionProcessor, SubscriptionProcessor>();
builder.Services.AddScoped<ISubscriptionJob, SubscriptionJob>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton(new OpenAIClient(openAIKey));
builder.Services.AddScoped<IReceiptLlmService, ReceiptLlmService>();

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

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connectionString);
    })
);
builder.Services.AddHangfireServer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseHangfireDashboard("/dashboard");

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var subscriptionJob = scope.ServiceProvider.GetRequiredService<ISubscriptionJob>();

    recurringJobs.AddOrUpdate("Create-Subscription-Expense", () => subscriptionJob.CreateSubscriptionsExpenseAsync(), Cron.Daily());
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
