using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class SidebarNavPage : BasePage
    {
        // ************* PAGE CONTROLS *************
        private PageControl lnkDashboard => new PageControl(By.LinkText("Dashboard"), "Dashboard");
        private PageControl lnkSummary => new PageControl(By.LinkText("Summary"), "Summary");
        private PageControl lnkCashAccount => new PageControl(By.LinkText("Cash Account"), "Cash Account");
        private PageControl lnkManageInvestments => new PageControl(By.LinkText("Manage Investments"), "Manage Investments");
        private PageControl lnkSettings => new PageControl(By.LinkText("Settings"), "Settings");
        private PageControl lnkResources => new PageControl(By.LinkText("Resources"), "Resources");

        private PageControl SelectedTab => new PageControl(By.XPath("//div[@class='sidebar']//a[contains(@class, 'router-link-exact-active')]"));
        private PageControl lnkManageInvestmentsDropdown => new PageControl(By.XPath("//span[@role='button' and normalize-space(text())='Automated Investments']"), "Automated Investments (Dropdown Sub Menu)");
        private PageControl lnkInvestmentSummary => new PageControl(By.XPath("//a[data-cy='investment-summary' and normalize-space(text())='Investment Summary']"), "Investment Summary");
        private PageControl lnkAutomatedInvestments => new PageControl(By.XPath("//a[normalize-space(text())='Automated Investments']"), "Automated Investments");
        private PageControl AutomatedInvestment => new PageControl(By.XPath("//a[@data-cy='automated-investments' and normalize-space(text())='Automated Investments']"), "Automated Investments");
        public SidebarNavPage(IWebDriver driver) : base(driver)
        {
        }

        // ************* PAGE METHODS *************

        public void GoToDashboard()
        {
            GoToLink(lnkDashboard);
        }

        public void GoToSummary()
        {
            GoToLink(lnkSummary);
        }

        public void GoToCashAccount()
        {
            GoToLink(lnkCashAccount);
        }

        public void GoToManageInvestments()
        {
            WaitForElementToBeVisible(lnkManageInvestments);
            // WaitForSpinners(); // Uncomment if needed
            GoToLink(lnkManageInvestments);
        }

        public void GoToSettings()
        {
            WaitForElementToBeVisible(lnkSettings);
            GoToLink(lnkSettings);
        }

        public string GetSelected()
        {
            return SelectedTab.GetText().Trim();
        }

        private void GoToLink(PageControl control)
        {
            control.Click();
            WaitForSpinners();
        }

        public void WaitForTabChange(string tabToWaitFor)
        {
            for (int i = 0; i < 30; i++)
            {
                WaitForSpinners();
                if (GetSelected() == tabToWaitFor)
                    break;
            }
        }

        public void GoToResources()
        {
            GoToLink(lnkResources);
        }

        public void ClickManageInvestmentsDropdown()
        {
            var element = lnkManageInvestmentsDropdown.GetElement();

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);

            // make the window smaller
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 800);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException ex)
            {
                Console.WriteLine($"Error clicking element: {ex.Message}");

                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            }
        }

        public void GoTolnkInvestmentSummary()
        {
            WaitForElementToBeVisible(lnkInvestmentSummary);
            GoToLink(lnkInvestmentSummary);
        }

        public void GoToAutomatedInvestments()
        {
            // Use JavaScript click to handle element not interactable issue
            var element = lnkAutomatedInvestments.GetElement();
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
            // Wait for page to load completely
            Thread.Sleep(3000);
            WaitForSpinners();
        }
    }
}

