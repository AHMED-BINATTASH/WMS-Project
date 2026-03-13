using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1) الخدمات الأساسية وقاعدة البيانات
// ==========================================
builder.Services.AddControllers();
builder.Services.AddLocalization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration["ConnectionString"],
    b => b.MigrationsAssembly("WMS.Infrastructure")));

var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
builder.Services.AddSingleton<JWTSettings>(jwtOptions!);

// ==========================================
// 2) حقن الاعتماديات (Dependency Injection)
// ==========================================
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ==========================================
// 3) إعدادات الـ CORS (مهمة جداً للكوكيز)
// ==========================================
builder.Services.AddCors(options => {
    options.AddPolicy("MyPolicy", policy => {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); 
    });
});

// ==========================================
// 4) المصادقة والتفويض (Authentication & Authorization)
// ==========================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigntureKey)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                // التحقق هل الخطأ بسبب انتهاء صلاحية التوكن؟
                bool isExpired = context.AuthenticateFailure is SecurityTokenExpiredException ||
                                 (context.ErrorDescription?.Contains("exp") ?? false);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                if (isExpired)
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Token has expired",
                        code = "TOKEN_EXPIRED",
                        suggestion = "Please refresh your token."
                    });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "You are not authorized",
                        code = "UNAUTHORIZED"
                    });
                }
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonOwnerOrAdmin", policy =>
        policy.Requirements.Add(new PersonOwnerOrAdminRequirement()));
    options.AddPolicy("UserOwnerOrAdmin", policy =>
        policy.Requirements.Add(new PersonOwnerOrAdminRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, PersonOwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();

// ==========================================
// 5) تحديد معدل الطلبات (Rate Limiting)
// ==========================================
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("AuthLimiter", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });
});

// ==========================================
// 6) إعدادات Swagger
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
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ==========================================
// 7) خط معالجة الطلبات (Middleware Pipeline)
// ==========================================

app.UseCors("MyPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRateLimiter();

// معالجة خطأ Too Many Requests
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

// تسجيل محاولات الوصول المرفوضة (Forbidden)
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        app.Logger.LogWarning("Forbidden access: UserId={UserId}, Path={Path}", userId, context.Request.Path);
    }
});

app.MapControllers();

app.Run();