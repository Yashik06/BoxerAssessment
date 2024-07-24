using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// var to hold connection string to DB
var connectionString = builder.Configuration.GetConnectionString("Boxer.DB") ?? throw new InvalidOperationException("Connection string 'Boxer.DB' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the HTTP client
builder.Services.AddHttpClient("Boxer.API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7188/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
