<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
=======
using AutoMapper;
>>>>>>> ac19846bd93b067af162a7dabd8eb20841b7dc97
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WMS.Infrastructure.Persistence;
<<<<<<< HEAD
using Microsoft.IdentityModel.Tokens;
using WMS.Presentation.Utilities;
using System.Text;

=======
using WMS.Application;
using WMS.Application.Mappings;
>>>>>>> ac19846bd93b067af162a7dabd8eb20841b7dc97

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registring AppDbContext Option
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration["ConnectionString"],
    b => b.MigrationsAssembly("WMS.Infrastructure")));

<<<<<<< HEAD
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

builder.Services.AddSingleton<JWTSettings>(jwtOptions);

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigntureKey)),

            ClockSkew = TimeSpan.Zero,

        };
    }
);

builder.Services.AddAuthorization();
=======
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


>>>>>>> ac19846bd93b067af162a7dabd8eb20841b7dc97
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
