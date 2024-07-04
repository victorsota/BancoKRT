using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using BancoKRT.API.Application.Interfaces;
using BancoKRT.API.Application.Services;
using BancoKRT.Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registrar ILimitePixRepository e LimitePixRepository
builder.Services.AddScoped<ILimitePixRepository, LimitePixRepository>();
// Ajuste o namespace conforme sua estrutura de projeto

builder.Services.AddScoped<ILimitePixService, LimitePixService>();
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
