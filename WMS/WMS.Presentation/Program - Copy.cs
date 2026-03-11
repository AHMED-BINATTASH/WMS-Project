//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using WMS.Infrastructure.Persistence;
//using Microsoft.IdentityModel.Tokens;
//using WMS.Presentation.Utilities;
//using System.Text;
//using WMS.Domain.Interfaces;
//using WMS.Infrastructure.Persistence.Repositories;
//using WMS.Application.Interfaces;
//using WMS.Application.Services;
//using Microsoft.OpenApi.Models;
//using WMS.Application.DTOs;
//using WMS.Domain.Entities;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddLocalization();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<ICountryService, CountryService>();
//builder.Services.AddScoped<ICountryRepository, CountryRepository>();

//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddScoped<IService<PersonDto, Person>, PersonService>();
//builder.Services.AddScoped<IRepository<Person>, PersonRepository>();


//builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration["ConnectionString"],
//    b => b.MigrationsAssembly("WMS.Infrastructure")));

//var jwtOptions = builder.Configuration.GetSection("JWT").Get<JWTSettings>();
//builder.Services.AddSingleton<JWTSettings>(jwtOptions);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(option =>
//    {
//        option.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = jwtOptions.Issuer,
//            ValidateAudience = true,
//            ValidAudience = jwtOptions.Audience,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigntureKey)),
//            ClockSkew = TimeSpan.Zero
//        };
//    });

//builder.Services.AddAuthorization();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",

//        Type = SecuritySchemeType.Http,

//        Scheme = "Bearer",

//        BearerFormat = "JWT",

//        In = ParameterLocation.Header,

//        Description = "Enter: Bearer {your JWT token}"
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },

//            new string[] {}
//        }
//    });
//});

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();