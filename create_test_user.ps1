# PowerShell script to create a test admin user

$apiUrl = "http://localhost:4000/api/User"

# Test admin user data
$testUser = @{
    email = "admin@test.com"
    firstName = "Admin"
    lastName = "Test"
    middleName = "User"
    enrollmentNumber = "ADMIN001"
    password = "admin123"
    cohort = "2024-A"
} | ConvertTo-Json

Write-Host "Creating test admin user..." -ForegroundColor Yellow
Write-Host "API URL: $apiUrl" -ForegroundColor Cyan
Write-Host "User Data: $testUser" -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri $apiUrl -Method POST -Body $testUser -ContentType "application/json"
    
    if ($response.success) {
        Write-Host "✅ Test admin user created successfully!" -ForegroundColor Green
        Write-Host "Login credentials:" -ForegroundColor Green
        Write-Host "  📧 Email/Matrícula: admin@test.com or ADMIN001" -ForegroundColor White
        Write-Host "  🔑 Password: admin123" -ForegroundColor White
        Write-Host ""
        Write-Host "⚠️  NOTE: You'll need to manually set IsAdmin=1 in the database for this user." -ForegroundColor Red
        Write-Host "   UPDATE Users SET IsAdmin = 1 WHERE EnrollmentNumber = 'ADMIN001';" -ForegroundColor Yellow
    } else {
        Write-Host "❌ Failed to create user: $($response.error_Message)" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Error creating user: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "🔧 Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  1. Make sure the API is running on port 4000" -ForegroundColor White
    Write-Host "  2. Check if the backend can connect to the database" -ForegroundColor White
    Write-Host "  3. Verify the User table exists with all required columns" -ForegroundColor White
}

Write-Host ""
Write-Host "🌐 You can also try these existing test credentials:" -ForegroundColor Cyan
Write-Host "  📧 Try: Test-id or any enrollment number you might have" -ForegroundColor White