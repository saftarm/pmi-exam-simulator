@echo off
REM Batch script to run both React client and ASP.NET API simultaneously
REM Usage: run-both.bat

echo Starting React Client and TestAPI...

REM Start the ASP.NET API in a new window
start "TestAPI" cmd /k "cd /d %~dp0TestAPI && dotnet run"

REM Wait a moment for the API to start
timeout /t 3 /nobreak >nul

REM Start the React client in a new window
start "React Client" cmd /k "cd /d %~dp0reactclient && npm run dev"

echo.
echo Both services are starting in separate windows.
echo React Client: http://localhost:5173
echo API: http://localhost:5033
echo.
pause

