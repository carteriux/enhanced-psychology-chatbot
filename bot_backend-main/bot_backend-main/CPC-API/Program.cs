using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Minimal configuration to ensure startup

// Basic CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CPC - API", Version = "v1" });
});

var app = builder.Build();

// Configure port for Cloud Run
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "CPC - API"));
app.UseCors("AllowAll");

// Simple health check endpoint
app.MapGet("/health", () => "API is running");

// Test login endpoint that doesn't crash
app.MapPost("/api/Security/login", (object request) => 
{
    return Results.Ok(new { 
        success = true, 
        message = "Login endpoint working - authentication disabled for testing",
        data = new { token = "test-token", user = new { isAdmin = true } }
    });
});

app.MapControllers();

app.Run();
