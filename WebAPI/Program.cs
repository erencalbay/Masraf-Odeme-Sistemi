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
using Serilog;
using MassTransit;
using Business.Consumers;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// FluentValidation konfigurasyonu
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>();
}).AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// RabbitMQ konfigurasyonu
builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<ReceiptsEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);

        configurator.ReceiveEndpoint("receipt_queue", e => { 
            e.ConfigureConsumer<ReceiptsEventConsumer>(context); 
            e.DiscardSkippedMessages(); });
    });
});

// Swagger configurasyonu, APIlere eriþmek ve token iþlemler için
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
}); 


// Dbcontext tanýmýnýn yapýlmasý ve db baðlanmasý
builder.Services.AddDbContext<VdDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));

// Mediatr konfigurasyonu
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VbTransferCommand).GetTypeInfo().Assembly));
    
// Mapper konfigurasyonu 
var mapperConf = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
builder.Services.AddSingleton(mapperConf.CreateMapper());

// Auth ve token servislerinin implemantasyonu
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Log file'ýn yaratýlmasý ve kullanýlmasýnýn konfigurasyonu
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
Log.Information("App server is starting");
builder.Host.UseSerilog();
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
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

// Redis konfigurasyonu
builder.Services.AddStackExchangeRedisCache(action =>
{
    action.Configuration = "localhost:6379";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
