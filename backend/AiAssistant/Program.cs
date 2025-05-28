using AiAssistant.models;
using AssistanService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<ChatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IAssistanceService, AssistanService.AssistanService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost",
                "http://127.0.0.1:5500",
                "http://192.168.1.218:8080",
                "http://localhost:8080",
                "http://localhost:7134",
                "http://localhost:5174",
                "http://localhost:5132"
            )
            .AllowAnyHeader()
            .AllowAnyMethod(); // <-- Important
        });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



// Register IAssistanceService with its implementation AssistanService
builder.Services.AddScoped<IAssistanceService, AssistanService.AssistanService>();

// Register HttpClient for use in the AssistanService
builder.Services.AddHttpClient();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
