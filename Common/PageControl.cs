using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace SeleniumPOC.Common
{
    // CLASS: PageControl
    // PURPOSE: Wrapper class that provides element interaction methods using stored WebDriver
    // FLOW: Receives driver from AllPages → Stores it statically → Provides element operations
    // CONNECTS TO: AllPages (via InitDriver) → All page objects (via element operations)
    public class PageControl
    {
        // FIELD: driver - Static WebDriver instance shared by all PageControl objects
        // PURPOSE: Stores the WebDriver so all PageControl methods can access it
        [ThreadStatic] private static IWebDriver driver;
        
        // FIELD: remote - Flag for remote execution (currently unused)
        [ThreadStatic] private static bool remote;
        
        // FIELD: locator - The By locator for finding elements
        private By locator;
        
        // FIELD: name - Optional name for the element (used in logging)
        private string? name = null;
        
        // PROPERTY: controlName - Returns name if set, otherwise returns locator string
        private string controlName => name == null ? locator.ToString() : name;

        // CONSTRUCTOR: PageControl(By locator)
        // PURPOSE: Creates PageControl with just a locator
        public PageControl(By locator) => this.locator = locator;

        // CONSTRUCTOR: PageControl(By locator, string name)
        // PURPOSE: Creates PageControl with locator and descriptive name
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

        // METHOD: InitDriver
        // PURPOSE: Stores the WebDriver globally so that PageControl methods can access it
        // FLOW: Called by AllPages → Stores driver → All PageControl methods can use it
        // DRIVER FLOW: AllPages calls this → Stores driver → All element operations use stored driver
        public static void InitDriver(IWebDriver webDriver)
        {
            driver = webDriver;  // Store the driver for all PageControl instances
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

        // METHOD: Click
        // PURPOSE: Clicks on the element using the stored WebDriver
        // FLOW: Uses stored driver → Finds element → Clicks it
        public void Click()
        {
            Console.WriteLine($"[Click] -> {controlName}");  // Log the action
            driver.FindElement(locator).Click();             // Use stored driver to click element
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

        public void ScrollToElement(IWebElement element)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            if (element == null) throw new ArgumentNullException(nameof(element));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        // METHOD: GetElement
        // PURPOSE: Returns the WebElement using the stored WebDriver
        // FLOW: Checks if driver is initialized → Uses stored driver → Returns element
        // ERROR HANDLING: Throws exception if driver not initialized
        public IWebElement GetElement()
        {
            if (driver == null)
                throw new InvalidOperationException("PageControl driver not initialized. Call PageControl.InitDriver in AllPages.");
            return driver.FindElement(locator);  // Use stored driver to find element
        }

    }
}

