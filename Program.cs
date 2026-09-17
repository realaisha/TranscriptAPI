using ClosedXML.Excel;
using TranscriptAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<TranscriptService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.MapGet("/students/search", (string matricNumber, TranscriptService service) =>
{
    Console.WriteLine($"Searching for: [{matricNumber}]");

    var transcript = service.GetTranscriptByMatricNumber(matricNumber);

    return transcript is not null
        ? Results.Ok(transcript)
        : Results.NotFound();
});

app.MapGet("/students", (TranscriptService service) => service.GetAll());

app.Run();