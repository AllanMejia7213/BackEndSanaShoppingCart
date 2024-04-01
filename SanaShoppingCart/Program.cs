using Microsoft.EntityFrameworkCore;
using SanaShoppingCart.Models;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SanaShoppingCartContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SanaConnection")));
builder.Services.AddControllers().AddJsonOptions(opt => { opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

var ReglasCors = "ReglasCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ReglasCors, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(ReglasCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
