using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using WMS.Infrastructure.Persistence;
using WMS.Presentation.Utilities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Persistence.Repositories;
using WMS.Application.Interfaces;
using WMS.Application.Services;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1) Configuration & Database
// ==========================================
builder.Services.AddControllers();
builder.Services.AddLocalization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration["ConnectionString"],
    b => b.MigrationsAssembly("WMS.Infrastructure")));

// Ã·» ≈⁄œ«œ«  JWT „‰ „·› appsettings.json
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
builder.Services.AddSingleton<JWTSettings>(jwtOptions);

// ==========================================
// 2) Dependency Injection (Repositories & Services)
// ==========================================
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ==========================================
// 3) Authentication & Authorization
// ==========================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigntureKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonOwnerOrAdmin", policy =>
        policy.Requirements.Add(new PersonOwnerOrAdminRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, PersonOwnerOrAdminHandler>();

// ==========================================
// 4) Rate Limiting (Õ„«Ì… «·„”«—« )
// ==========================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});

// ==========================================
// 5) Swagger / OpenAPI Configuration
// ==========================================
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// ==========================================
// 6) Middleware Pipeline
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many attempts. Please try again later.");
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var path = context.Request.Path.ToString();

        app.Logger.LogWarning(
            "Forbidden access attempt. UserId={UserId}, Path={Path}, IP={IP}",
            userId, path, ip
        );
    }
});

app.MapControllers();

app.Run();