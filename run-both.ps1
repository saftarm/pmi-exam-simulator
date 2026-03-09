# PowerShell script to run both React client and ASP.NET API simultaneously
# Usage: .\run-both.ps1

Write-Host "Starting React Client and TestAPI..." -ForegroundColor Green

# Start the ASP.NET API in a new window
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\TestAPI'; dotnet run"

# Wait a moment for the API to start
Start-Sleep -Seconds 3

# Start the React client in a new window
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\reactclient'; npm run dev"

Write-Host "Both services are starting in separate windows." -ForegroundColor Green
Write-Host "React Client: http://localhost:5173" -ForegroundColor Cyan
Write-Host "API: http://localhost:5033" -ForegroundColor Cyan

