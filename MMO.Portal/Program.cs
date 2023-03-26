using Microsoft.EntityFrameworkCore;
using MMO.Portal.Models;

var builder = WebApplication.CreateBuilder(args);
var portalConnectionString = builder.Configuration.GetConnectionString("portal");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<PortalDbContext>(
    options => options.UseMySql(portalConnectionString, ServerVersion.AutoDetect(portalConnectionString))
);

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
