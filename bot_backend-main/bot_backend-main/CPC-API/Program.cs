using CPC.Common.APIMiddleware;
using CPC.Domain.Models;
using CPC.Domain.ValueObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models; // Add this using directive

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIntegrationsDI();

var apiBaseSettings = new APIBaseSettings();
builder.Configuration.GetSection("APIBaseSettings").Bind(apiBaseSettings);
builder.Services.Configure<APIBaseSettings>(builder.Configuration.GetSection("APIBaseSettings"));

builder.Services.AddDbContext<CpcContext>(options =>
{
    options.UseMySQL(apiBaseSettings.ConnectionStrings.CPConnection);
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiBaseSettings.Jwt.Key))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CPC - API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure port for Cloud Run
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.UseSwagger();
app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "CPC - API"));

app.UseCors("NewPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
