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
        data = new { 
            token = "test-token", 
            user = new { 
                idUser = 1,
                email = "admin@test.com",
                firstName = "Admin",
                lastName = "User",
                enrollmentNumber = "ADMIN001",
                isFirstTime = false,
                lastAccessDate = DateTime.Now,
                isAdmin = true,
                cohort = "Admin"
            } 
        }
    });
});

// Test single user creation endpoint
app.MapPost("/api/User/create", (object request) => 
{
    return Results.Ok(new { 
        success = true, 
        message = "User created successfully - testing mode",
        data = new { 
            idUser = new Random().Next(1000, 9999),
            message = "Student registered successfully"
        }
    });
});

// Test bulk user creation endpoint
app.MapPost("/api/User/createmultipleusers", (object request) => 
{
    return Results.Ok(new { 
        success = true, 
        message = "Multiple users created successfully - testing mode",
        data = new { 
            message = "All students registered successfully"
        }
    });
});

// Test get all users endpoint
app.MapGet("/api/User", () => 
{
    return Results.Ok(new {
        success = true,
        data = new {
            users = new[] {
                new {
                    idUser = 1,
                    firstName = "Magda",
                    middleName = "",
                    lastName = "Sánchez Morales", 
                    enrollmentNumber = "10808",
                    cohort = "Maestria Puebla 36"
                },
                new {
                    idUser = 2,
                    firstName = "Sergio",
                    middleName = "", 
                    lastName = "Rosas navarro",
                    enrollmentNumber = "10486", 
                    cohort = "Maestria GDL 36"
                }
            }
        }
    });
});

// Test get cohorts endpoint
app.MapGet("/api/Cohort", () => 
{
    return Results.Ok(new[] {
        "Maestria Puebla 36",
        "Maestria GDL 36"
    });
});

app.MapControllers();

app.Run();
