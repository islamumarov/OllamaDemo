using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddChatClient(c => {
    var endpoint = new Uri(builder.Configuration["AI:Ollama:Chat:Endpoint"] ?? "http://localhost:11434/");
    var modelId = builder.Configuration["AI:Ollama:Chat:ModelId"];
    return c.Use(new OllamaChatClient(endpoint, modelId));
});

builder.Services.AddEmbeddingGenerator<string,Embedding<float>>(c => {
    var endpoint = new Uri(builder.Configuration["AI:Ollama:Embedding:Endpoint"] ?? "http://localhost:11434/");
    var modelId = builder.Configuration["AI:Ollama:Embedding:ModelId"];
    
    return c.Use(new OllamaEmbeddingGenerator(endpoint, modelId));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapPost("/chat", async (IChatClient client, [FromBody] string message) =>
{
    var response = await client.CompleteAsync(message, cancellationToken: default);
    return response?.Message?.Text;
});

app.MapPost("/embedding", async (IEmbeddingGenerator<string,Embedding<float>> client, [FromBody] string message) =>
{
    var response = await client.GenerateAsync(message);
    return response;
});


app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
