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

// Working students endpoint for frontend (supports optional cohort filter)
app.MapGet("/api/students", (string? cohort) => 
{
    var allStudents = new[] {
        new {
            idUser = 1,
            firstName = "Magda",
            middleName = "",
            lastName = "Sanchez Morales", 
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
    };

    var filtered = string.IsNullOrEmpty(cohort)
        ? allStudents
        : Array.FindAll(allStudents, s => string.Equals(s.cohort, cohort, StringComparison.OrdinalIgnoreCase));

    var response = new {
        success = true,
        data = new {
            users = filtered
        }
    };
    
    return Results.Json(response);
});

// Students filtered by cohort endpoint
app.MapGet("/api/User/cohort", (HttpRequest req) => 
{
    string cohort = req.Query.ContainsKey("cohort") ? req.Query["cohort"].ToString() : string.Empty;

    var allStudents = new[] {
        new {
            idUser = 1,
            firstName = "Magda",
            middleName = "",
            lastName = "Sanchez Morales", 
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
    };

    var filteredStudents = string.IsNullOrEmpty(cohort)
        ? allStudents
        : Array.FindAll(allStudents, s => string.Equals(s.cohort, cohort, StringComparison.OrdinalIgnoreCase));

    var response = new {
        success = true,
        data = new {
            users = filteredStudents
        }
    };

    return Results.Json(response);
});

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

// Reset activities for all students in a cohort (testing mode)
app.MapPost("/api/UserActivities/ResetActivitiesByCohort", (HttpRequest req) =>
{
    string cohort = req.Query.ContainsKey("cohort") ? req.Query["cohort"].ToString() : string.Empty;
    var response = new {
        success = true,
        data = new { message = string.IsNullOrEmpty(cohort) ? "Activities reset for all cohorts (testing)" : $"Activities reset for cohort '{cohort}' (testing)" }
    };
    return Results.Json(response);
});

// Simple test endpoint first
app.MapGet("/api/User/test", () => 
{
    return Results.Json("API User endpoint working");
});

// Test get all users endpoint - using different path to avoid conflicts
app.MapGet("/api/Users/all", () => 
{
    try
    {
        var response = new {
            success = true,
            data = new {
                users = new[] {
                    new {
                        idUser = 1,
                        firstName = "Magda",
                        middleName = "",
                        lastName = "Sanchez Morales", 
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
        };
        
        return Results.Json(response);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in /api/Users/all: {ex.Message}");
        return Results.Problem("Internal server error");
    }
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
