using OpenQA.Selenium;

namespace SeleniumPOC.Common
{
    public static class With
    {
        public static By PermId(string permId) =>
            By.CssSelector($"[data-perm-id='{permId}']");

        public static By Attribute(string attributeName, string value) =>
            By.CssSelector($"[{attributeName}='{value}']");
    }
}

