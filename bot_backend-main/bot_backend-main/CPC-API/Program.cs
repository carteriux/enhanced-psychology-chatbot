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
    // For testing - accept any login
    string? id = null;
    
    // Try to extract id from request body if it's a dictionary
    if (request is System.Collections.Generic.Dictionary<string, object> dict)
    {
        dict.TryGetValue("id", out var idVal);
        id = idVal?.ToString();
    }

    bool isAdmin = string.Equals(id, "ADMIN001", StringComparison.OrdinalIgnoreCase);

    // Map some sample students by enrollment
    int idUser = 1;
    string firstName = "Estudiante";
    string lastName = "Demo";
    string enrollmentNumber = id ?? "STUDENT";
    string cohort = "Demo";

    if (string.Equals(id, "10808", StringComparison.OrdinalIgnoreCase))
    {
        idUser = 1; firstName = "Magda"; lastName = "Sanchez Morales"; enrollmentNumber = "10808"; cohort = "Maestria Puebla 36";
    }
    else if (string.Equals(id, "10486", StringComparison.OrdinalIgnoreCase))
    {
        idUser = 2; firstName = "Sergio"; lastName = "Rosas navarro"; enrollmentNumber = "10486"; cohort = "Maestria GDL 36";
    }

    if (isAdmin)
    {
        idUser = 999; firstName = "Admin"; lastName = "User"; enrollmentNumber = "ADMIN001"; cohort = "Admin";
    }

    return Results.Ok(new { 
        success = true, 
        message = "Login endpoint working - authentication disabled for testing",
        data = new { 
            token = "test-token", 
            user = new { 
                idUser,
                email = isAdmin ? "admin@test.com" : $"{enrollmentNumber}@example.com",
                firstName,
                lastName,
                enrollmentNumber,
                isFirstTime = false,
                lastAccessDate = DateTime.Now,
                isAdmin,
                cohort
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

// Get individual user by ID
app.MapGet("/api/User/1", () => 
{
    var response = new { 
        success = true, 
        data = new { 
            user = new { 
                idUser = 1, email = "magda@example.com", firstName = "Magda", lastName = "Sanchez Morales",
                middleName = "", enrollmentNumber = "10808", cohort = "Maestria Puebla 36"
            }
        }
    };
    return Results.Json(response);
});

app.MapGet("/api/User/2", () => 
{
    var response = new { 
        success = true, 
        data = new { 
            user = new { 
                idUser = 2, email = "sergio@example.com", firstName = "Sergio", lastName = "Rosas navarro",
                middleName = "", enrollmentNumber = "10486", cohort = "Maestria GDL 36"
            }
        }
    };
    return Results.Json(response);
});

// Get activities for a user
app.MapGet("/api/UserActivities/GetActivitiesByUserId/{userId}", (int userId) => 
{
    var activities = new[] {
        new {
            id = 1,
            progressPercentage = 0,
            filePath = "/activities/chatbot",
            idActivity = 1,
            activityName = "Chatbot",
            endDateTime = (string?)null
        },
        new {
            id = 2,
            progressPercentage = 0,
            filePath = "/activities/assessment",
            idActivity = 2,
            activityName = "Assessment",
            endDateTime = (string?)null
        }
    };
    
    var response = new { 
        success = true, 
        data = activities
    };
    return Results.Json(response);
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
