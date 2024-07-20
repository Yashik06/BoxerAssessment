using Boxer.BL.Interfaces;
using Boxer.BL.Services;
using Boxer.DL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// var to hold connection string to DB
var connectionString = builder.Configuration.GetConnectionString("Boxer.DB") ?? throw new InvalidOperationException("Connection string 'Boxer.DB' not found.");

//BoxerDBContext
builder.Services.AddDbContext<BoxerDBContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.

//Services
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
