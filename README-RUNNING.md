# How to Run React Client and TestAPI Simultaneously

This project consists of two parts:
- **React Client** (Vite + React) - Frontend application
- **TestAPI** (ASP.NET Core) - Backend API

## Prerequisites

1. **Node.js** and **npm** installed
2. **.NET SDK** installed
3. Dependencies installed:
   ```bash
   cd reactclient
   npm install
   ```

## Method 1: Using npm concurrently (Recommended)

1. Install the root-level dependencies:
   ```bash
   npm install
   ```

2. Run both services:
   ```bash
   npm run dev
   ```

This will start:
- React Client on `http://localhost:5173`
- TestAPI on `http://localhost:5033`

## Method 2: Using PowerShell Script

Simply run:
```powershell
.\run-both.ps1
```

This opens both services in separate PowerShell windows.

## Method 3: Using Batch Script

Simply run:
```cmd
run-both.bat
```

This opens both services in separate Command Prompt windows.

## Method 4: Manual (Two Terminal Windows)

### Terminal 1 - Start the API:
```bash
cd TestAPI
dotnet run
```

### Terminal 2 - Start the React Client:
```bash
cd reactclient
npm run dev
```

## URLs

- **React Client**: http://localhost:5173
- **API**: http://localhost:5033
- **API Endpoints**: http://localhost:5033/api/...

## Proxy Configuration

The React client is configured to proxy `/api` requests to `http://localhost:5033` automatically. This means you can make API calls from your React app using relative paths like `/api/auth/login` and they will be forwarded to the backend.

## Troubleshooting

- If port 5173 is already in use, Vite will automatically try the next available port
- If port 5033 is already in use, check your `launchSettings.json` or stop the conflicting service
- Make sure both services are running before making API calls from the React client

