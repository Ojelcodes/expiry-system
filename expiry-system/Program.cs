using Application.ConfigSettings;
using Application.DTOs;
using Application.DTOs.Mail;
using Application.InfraInterfaces;
using Application.Services.Implementation;
using Application.Services.Implementations;
using Application.Services.Implementations.Auth;
using Application.Services.Interface;
using Application.Services.Interfaces;
using Domain.Entities;
using Humanizer.Configuration;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.Seed;
using System;
using System.Collections.Generic;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//adding dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().
           AddDefaultTokenProviders();

builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
                     "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


//------------------------------------JWT Authentication Settings--------------------------------------//
var appsettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(appsettings);
var appSettingValues = appsettings.Get<JwtSettings>();

//Encoding The Secret
var key = Encoding.ASCII.GetBytes(appSettingValues.Secret);


//------------------------------------- Authentication Middleware ---------------------------------------//            

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    LifetimeValidator = TokenLifetimeValidator.Validate,
    ValidIssuer = appSettingValues.Site,
    ValidAudience = appSettingValues.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(key)
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents();
    options.Events.OnChallenge = context =>
    {
        // Skip the default logic.
        context.HandleResponse();

        var payload = BaseResponse.Failure("01", "Unauthorised");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 401;
        return context.Response.WriteAsync(payload.ToString());
    };
});

//----------------------------------------  Configuration Settings -------------------------------------------------//

builder.Services.Configure<AppEndpointSettings>(builder.Configuration.GetSection("AppEndpointSettings"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IOTPService, OTPService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
    var context = services.GetRequiredService<ApplicationDbContext>();
    await Seed.SeedData(context, userManager, roleManager);
}

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
