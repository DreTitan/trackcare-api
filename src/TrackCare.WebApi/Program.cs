using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrackCare.Application;
using TrackCare.Infrastructure;
using TrackCare.Infrastructure.Persistence;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Azure / Cloud: usa variáveis de ambiente
var dbConn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? Environment.GetEnvironmentVariable("WEBSITE_DB_CONNECTION_STRING");
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL")
    ?? Environment.GetEnvironmentVariable("WEBSITE_SUPABASE_URL");
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY")
    ?? Environment.GetEnvironmentVariable("WEBSITE_SUPABASE_KEY");
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? "http://localhost:5173";

// Load .env file if exists (local dev only)
var envPath = Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]))
        {
            var key = parts[0].Trim();
            var value = parts[1].Trim();
            if (key == "DB_CONNECTION_STRING" && string.IsNullOrEmpty(dbConn)) dbConn = value;
            if (key == "SUPABASE_URL" && string.IsNullOrEmpty(supabaseUrl)) supabaseUrl = value;
            if (key == "SUPABASE_KEY" && string.IsNullOrEmpty(supabaseKey)) supabaseKey = value;
        }
    }
}

if (!string.IsNullOrEmpty(dbConn))
    builder.Configuration["ConnectionStrings:DefaultConnection"] = dbConn;
if (!string.IsNullOrEmpty(supabaseUrl))
    builder.Configuration["Supabase:Url"] = supabaseUrl;
if (!string.IsNullOrEmpty(supabaseKey))
    builder.Configuration["Supabase:Key"] = supabaseKey;

// Porta
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TrackCare API", Version = "v1" });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        var allowedOrigins = new[] { "http://localhost:5173", "http://localhost:3000" };
        if (!string.IsNullOrEmpty(frontendUrl))
            allowedOrigins = allowedOrigins.Append(frontendUrl).ToArray();
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.Run();
