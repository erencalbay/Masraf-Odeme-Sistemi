using AutoMapper;
using Business.Mapper;
using Business.VbTransferCommand;
using Data;
using Data.DbContextCon;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Business;
using FluentValidation.AspNetCore;
using Business.Validator;
using System.Text.Json.Serialization;
using Business.Abstract;
using Business.Services.AuthenticationService;
using Business.Services.TokenService;
using WebAPI.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Business.Services.Sign;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Schema;
using Microsoft.OpenApi.Models;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>();
}).AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Expense API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
}); builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.AddDbContext<VdDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VbTransferCommand).GetTypeInfo().Assembly));
    
var mapperConf = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
builder.Services.AddSingleton(mapperConf.CreateMapper());

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();



builder.Services.AddAuthentication(options =>
{
    //set Schema : "Bearer"
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // merge two schema each other (Authentication schema and jwt schema)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // validation parameters choose (valid payload)
    // for using token options parameters
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audiences[0], // enough 0 index for this api
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // servers times diff 0
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseMiddleware<ErrorHandlerMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
