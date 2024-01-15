using AutoMapper;
using Business.Mapper;
using Business.VbTransferCommand;
using Data;
using Data.DbContextCon;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VdDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VbTransferCommand).GetTypeInfo().Assembly));

var mapperConf = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
builder.Services.AddSingleton(mapperConf.CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
