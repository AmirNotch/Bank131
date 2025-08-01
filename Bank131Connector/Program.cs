using System.Text.Json;
using Bank131Connector.Clients;
using Bank131Connector.Clients.IClients;

var builder = WebApplication.CreateBuilder(args);

// Настройка JSON для контроллеров
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Настройка HttpClient с общими JSON параметрами
builder.Services
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

builder.Services
    .AddHttpClient<IVaslClient, VaslClient>("VaslClient",client =>
    {
        client.BaseAddress = new Uri("http://172.35.11.168:2023");
    });

builder.Services
    .AddHttpClient<IVaslClient, VaslClient>("Bank131Client", client =>
    {
        client.BaseAddress = new Uri("https://demo.bank131.ru");
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();