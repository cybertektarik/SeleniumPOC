$ErrorActionPreference = "Stop"

# Installs Playwright browsers for this repo (Chromium only is enough for now).
# Run from repo root: powershell -ExecutionPolicy Bypass -File .\PlaywrightProject\playwright.ps1
#
# iPhone-style mobile emulation (same Chromium browser, device viewport + touch UA):
#   $env:PLAYWRIGHT_MOBILE_IOS = "1"   # or PLAYWRIGHT_IPHONE=1
#   $env:PLAYWRIGHT_DEVICE = "iPhone 15"   # optional; must match a key in Playwright's built-in device list
#   dotnet test .\PlaywrightProject\PlaywrightProject.csproj

dotnet build .\PlaywrightProject\PlaywrightProject.csproj

$playwrightScript = Join-Path (Resolve-Path .\PlaywrightProject\bin\Debug\net8.0) "playwright.ps1"
if (!(Test-Path $playwrightScript)) {
  throw "Could not find playwright.ps1 at $playwrightScript. Build may have failed."
}

powershell -ExecutionPolicy Bypass -File $playwrightScript install chromium

