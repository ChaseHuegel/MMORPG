using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MMO.DataServer.Data;
using MMO.DataServer.Util;

var builder = WebApplication.CreateBuilder(args);
var portalConnectionString = builder.Configuration.GetConnectionString("portal");

var registration = builder.Configuration.GetSection("Registration").Get<PortalRegistrationConfig>();
await PortalUtils.RegisterWithPortalAsync(registration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http
        }
    );
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.Authority = builder.Configuration.GetValue<string>("JWT:Authority");
            options.Audience = builder.Configuration.GetValue<string>("JWT:Audience");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))
                ),
                ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience")
            };
        }
    );

builder.Services.AddDbContextPool<CharactersDbContext>(
    options =>
        options.UseMySql(portalConnectionString, ServerVersion.AutoDetect(portalConnectionString))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
