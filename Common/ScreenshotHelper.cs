using OpenQA.Selenium;

namespace SeleniumProject.Common
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshotBase64(IWebDriver driver)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            return screenshot.AsBase64EncodedString;
        }
    }
}
