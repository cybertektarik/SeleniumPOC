# Install Playwright browsers for this project.
# Run from: C:\Users\thoma\SeleniumPOC\PlaywrightProject

Write-Host "Building PlaywrightProject.csproj..."
dotnet build .\PlaywrightProject.csproj

$binPath = Resolve-Path ".\bin\Debug\net8.0"
$pwScript = Join-Path $binPath "playwright.ps1"

Write-Host "Running Playwright installer..."
powershell -ExecutionPolicy Bypass -File $pwScript install


