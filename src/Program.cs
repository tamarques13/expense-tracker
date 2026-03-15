using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Spentir.Data;
using Spentir.Services.Interfaces;
using Spentir.Repositories.Interfaces;
using Spentir.Services;
using Spentir.Repositories;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");

builder.Services.AddDbContext<SpentirDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddControllers();

// Development-only CORS policy.
// WARNING: Allowing any origin, method and header is not safe for production.
// Review and restrict origins before deploying.

builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
