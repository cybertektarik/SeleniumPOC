$ErrorActionPreference = "Stop"

# Installs Playwright browsers for this repo (Chromium only is enough for now).
# Run from repo root: powershell -ExecutionPolicy Bypass -File .\PlaywrightProject\playwright.ps1
#
# Web tests (desktop viewport): Tests\Web\ — e.g.
#   dotnet test .\PlaywrightProject\PlaywrightProject.csproj --filter "FullyQualifiedName~Tests.Web"
# Mobile tests (Chromium + device profile): Tests\Mobile\ — optional device name:
#   $env:PLAYWRIGHT_DEVICE = "iPhone 15"
#   dotnet test .\PlaywrightProject\PlaywrightProject.csproj --filter "FullyQualifiedName~Tests.Mobile"

dotnet build .\PlaywrightProject\PlaywrightProject.csproj

$playwrightScript = Join-Path (Resolve-Path .\PlaywrightProject\bin\Debug\net8.0) "playwright.ps1"
if (!(Test-Path $playwrightScript)) {
  throw "Could not find playwright.ps1 at $playwrightScript. Build may have failed."
}

powershell -ExecutionPolicy Bypass -File $playwrightScript install chromium

