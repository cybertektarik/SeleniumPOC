using Microsoft.Playwright;

namespace PlaywrightProject;

/// <summary>
/// Builds a <see cref="BrowserNewContextOptions"/> for desktop or iPhone-style mobile emulation (Chromium).
/// Enable mobile with <c>PLAYWRIGHT_MOBILE_IOS=1</c> or <c>PLAYWRIGHT_IPHONE=1</c>.
/// Optional <c>PLAYWRIGHT_DEVICE</c> (default <c>iPhone 15</c>) must match a key in <see cref="IPlaywright.Devices"/>.
/// </summary>
internal static class PlaywrightMobileContext
{
    internal static BrowserNewContextOptions CreateContextOptions(IPlaywright playwright)
    {
        var mobile = GetEnvBool("PLAYWRIGHT_MOBILE_IOS", false) || GetEnvBool("PLAYWRIGHT_IPHONE", false);
        if (!mobile)
        {
            return new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
            };
        }

        var deviceName = Environment.GetEnvironmentVariable("PLAYWRIGHT_DEVICE")?.Trim();
        if (string.IsNullOrEmpty(deviceName))
            deviceName = "iPhone 15";

        if (!playwright.Devices.ContainsKey(deviceName))
        {
            var sample = string.Join(", ", playwright.Devices.Keys.Take(25));
            throw new InvalidOperationException(
                $"Unknown Playwright device '{deviceName}'. Set PLAYWRIGHT_DEVICE to a built-in device name. Examples: {sample}, ...");
        }

        return playwright.Devices[deviceName];
    }

    private static bool GetEnvBool(string envName, bool defaultVal)
    {
        var v = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(v)) return defaultVal;
        return string.Equals(v, "1", StringComparison.Ordinal) ||
               string.Equals(v, "true", StringComparison.OrdinalIgnoreCase);
    }
}
