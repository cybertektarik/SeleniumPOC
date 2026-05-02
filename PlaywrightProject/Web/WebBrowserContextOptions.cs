using Microsoft.Playwright;

namespace PlaywrightProject.Web;

/// <summary>Desktop browser context defaults for web UI tests.</summary>
public static class WebBrowserContextOptions
{
    public static BrowserNewContextOptions CreateDesktop() => new()
    {
        ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
    };
}
