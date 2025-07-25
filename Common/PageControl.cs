using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace SeleniumPOC.Common
{
    public class PageControl
    {
        [ThreadStatic] private static IWebDriver driver;
        [ThreadStatic] private static bool remote;
        private By locator;
        private string? name = null;
        private string controlName => name == null ? locator.ToString() : name;

        public PageControl(By locator) => this.locator = locator;

        public PageControl(By locator, string name)
        {
            this.locator = locator;
            this.name = name;
        }

        public static IEnumerable<PageControl> FindAllByXPath(IWebDriver driver, string xPath)
        {
            var elements = driver.FindElements(By.XPath(xPath));
            return elements.Select(element => new PageControl(By.XPath(xPath)));
        }

        public bool IsSelected => driver.FindElement(locator).Selected;
        public bool IsEnabled => driver.FindElement(locator).Enabled;

        public static void InitDriver(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public bool IsDisplayed()
        {
            return IsDisplayed(0);
        }

        public bool IsDisplayed(int secondsToWait)
        {
            return Count(secondsToWait) > 0;
        }

        public int Count()
        {
            return Count(0);
        }

        public int Count(int secondsToWait)
        {
            int count;
            var currentTimeout = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(secondsToWait);
            count = driver.FindElements(locator).Count;
            driver.Manage().Timeouts().ImplicitWait = currentTimeout;
            return count;
        }

        public void Click()
        {
            Console.WriteLine($"[Click] -> {controlName}");
            driver.FindElement(locator).Click();
        }

        public void SendKeys(string keys)
        {
            Console.WriteLine($"[Send keys] -> {controlName} -> {keys}");
            driver.FindElement(locator).SendKeys(keys);
        }

        public void SendKeysUsingActions(string keys)
        {
            Actions actions = new Actions(driver);
            this.Click();
            Console.WriteLine($"[Send Keys] -> {controlName} -> {keys}");
            actions.SendKeys(keys).Build().Perform();
        }

        public void SendPasswordKeys(string keys)
        {
            Console.WriteLine($"[Send Keys] -> {controlName} -> ********");
            if (remote)
                driver.FindElement(locator).SendKeys("perfecto:secure=" + keys);
            else
                driver.FindElement(locator).SendKeys(keys);
        }

        public void Clear()
        {
            driver.FindElement(locator).Clear();
        }

        public void SetText(string text)
        {
            Console.WriteLine($"[Set Text] -> {controlName} -> {text}");
            Clear();
            driver.FindElement(locator).SendKeys(text);
        }

        public string GetText()
        {
            return driver.FindElement(locator).Text;
        }

        public string GetText(int index)
        {
            return driver.FindElements(locator)[index].Text;
        }

        public string GetValue()
        {
            return driver.FindElement(locator).GetAttribute("value");
        }

        public string GetClass()
        {
            return driver.FindElement(locator).GetAttribute("class");
        }

        public string GetAttribute(string attribute)
        {
            return driver.FindElement(locator).GetAttribute(attribute);
        }

        public void VerifyIsVisible()
        {
            Console.WriteLine($"Verifying visible: {controlName}");
            driver.FindElement(locator).Displayed.Should().BeTrue();
        }

        public void VerifyEnabled()
        {
            Console.WriteLine($"Verifying enabled: {controlName}");
            driver.FindElement(locator).Enabled.Should().BeTrue();
        }

        public void VerifyContainsText(string text)
        {
            Console.WriteLine($"Verifying: {controlName} contains text: {text}");
            driver.FindElement(locator).Text.Should().Contain(text);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By childLocator)
        {
            return driver.FindElement(locator).FindElements(childLocator);
        }

        public void SelectByText(string text)
        {
            SelectElement dropDown = new SelectElement(driver.FindElement(locator));
            Console.WriteLine($"[Select] -> {controlName} -> {text}");
            dropDown.SelectByText(text);
        }

        public void SelectByValue(string value)
        {
            SelectElement dropDown = new SelectElement(driver.FindElement(locator));
            Console.WriteLine($"[Select] -> {controlName} -> {value}");
            dropDown.SelectByValue(value);
        }

        public string GetSelectedValue()
        {
            SelectElement dropDown = new SelectElement(driver.FindElement(locator));
            return dropDown.SelectedOption.GetAttribute("value");
        }

        public void SelectByIndex(int index)
        {
            SelectElement dropDown = new SelectElement(driver.FindElement(locator));
            Console.WriteLine($"[Select] -> {controlName} -> {index}");
            dropDown.SelectByIndex(index);
        }

        public IList<IWebElement> FindElements() => driver.FindElements(locator);

        public By Locator => locator;
    }
}

