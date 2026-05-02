using Microsoft.Playwright;

namespace PlaywrightProject.Mobile;

/// <summary>
/// Mobile (iOS-style) emulation using Chromium device profiles from <see cref="IPlaywright.Devices"/>.
/// Optional <c>PLAYWRIGHT_DEVICE</c> (default <c>iPhone 15</c>) must match a device key.
/// </summary>
public static class MobileBrowserContextOptions
{
    public static BrowserNewContextOptions CreateForIos(IPlaywright playwright)
    {
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
}
