using OpenQA.Selenium;
using SeleniumPOC.Common;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        private PageControl spinners = new PageControl(By.XPath("//*[@id='generic-loading']"));

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
        }

        protected void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        protected void ClickAndWaitForSpinners(PageControl elementToClick)
        {
            elementToClick.Click();
            WaitForSpinners();
        }

        protected void WaitForSpinners()
        {
            int count = 0;

            for (int i = 0; i < 5000; i++)
            {
                if (spinners.Count(1) > 0)
                    count++;
                else
                    break;
            }

        }

        public void Refresh()
        {
            driver.Navigate().Refresh();
            WaitForSpinners();
        }

        public string GetCurrentUrl()
        {
            return driver.Url;
        }

        public void WaitForElementToBeVisible(PageControl element)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => element.IsDisplayed());
        }

        public string SwitchToNewTabAndGetUrl()
        {
            var mainHandle = driver.CurrentWindowHandle;
            var handles = driver.WindowHandles;

            foreach (var handle in handles)
            {
                if (mainHandle == handle)
                    continue;

                driver.SwitchTo().Window(handle);
                break;
            }

            var result = driver.Url;
            return result;
        }

        public void CloseCurrentTab()
        {
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);
        }

        public void VerifyCurrentUrlIncludes(string expectedPart)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            if (string.IsNullOrWhiteSpace(expectedPart)) throw new ArgumentException("Expected part cannot be null or empty.");

            string currentUrl = driver.Url;

            Assert.That(currentUrl, Does.Contain(expectedPart).IgnoreCase,
                $"Expected URL to contain '{expectedPart}', but got '{currentUrl}'.");
        }

      
    }
}

