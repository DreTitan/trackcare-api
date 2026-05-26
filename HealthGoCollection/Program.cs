using Microsoft.EntityFrameworkCore;
using HealthGoCollection.Data;
using HealthGoCollection.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add MudBlazor services
builder.Services.AddMudServices();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=healthgo.db";
builder.Services.AddDbContext<HealthGoDbContext>(options =>
    options.UseSqlite(connectionString));

// Services
builder.Services.AddScoped<IRecolhimentoService, RecolhimentoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HealthGoDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
