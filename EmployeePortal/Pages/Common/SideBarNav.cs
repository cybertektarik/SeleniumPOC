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
        private PageControl lnkManageInvestments => new PageControl(By.XPath("//span[@role='button' and normalize-space()='Manage Investments']"), "Manage Investments");
        private PageControl lnkSettings => new PageControl(By.LinkText("Settings"), "Settings");
        private PageControl lnkResources => new PageControl(By.LinkText("Resources"), "Resources");

        private PageControl SelectedTab => new PageControl(By.XPath("//div[@class='sidebar']//a[contains(@class, 'router-link-exact-active')]"));
        private PageControl lnkManageInvestmentsDropdown => new PageControl(By.XPath("//span[@role='button' and normalize-space(text())='Automated Investments']"), "Automated Investments (Dropdown Sub Menu)");
        private PageControl lnkInvestmentSummary => new PageControl(By.XPath("//a[@data-cy='nav-investment' and normalize-space(text())='Investment Summary']"), "Investment Summary");
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
            // Wait for page to be fully loaded
            Thread.Sleep(3000);
            WaitForSpinners();
            
            // Debug: Print all available navigation links first
            Console.WriteLine("=== DEBUG: Looking for Manage Investments link ===");
            var allNavLinks = driver.FindElements(By.XPath("//span[@role='button' and contains(text(), 'Manage') or contains(text(), 'Investment')]"));
            Console.WriteLine($"Found {allNavLinks.Count} navigation links:");
            foreach (var link in allNavLinks)
            {
                Console.WriteLine($"- Text: '{link.Text}', Visible: {link.Displayed}");
            }
            
            // Try to click on the "Manage Investments" button we found
            try
            {
                var manageInvestmentsButton = driver.FindElement(By.XPath("//span[@role='button' and normalize-space()='Manage Investments']"));
                Console.WriteLine($"Found Manage Investments button: '{manageInvestmentsButton.Text}', clicking to expand dropdown...");
                
                // Try JavaScript click first
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", manageInvestmentsButton);
                    Console.WriteLine("JavaScript click successful");
                }
                catch (Exception jsEx)
                {
                    Console.WriteLine($"JavaScript click failed: {jsEx.Message}, trying regular click...");
                    manageInvestmentsButton.Click();
                }
                
                Thread.Sleep(2000); // Wait for dropdown to expand
                Console.WriteLine("Manage Investments dropdown expanded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to click Manage Investments button: {ex.Message}");
                // Try the original approach as fallback
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => lnkManageInvestments.GetElement().Displayed);
                    Console.WriteLine($"Found Manage Investments element: {lnkManageInvestments.GetElement().Text}");
                    GoToLink(lnkManageInvestments);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Fallback approach failed: {ex2.Message}");
                    throw;
                }
            }
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
            try
            {
                // First expand the Manage Investments dropdown
                Console.WriteLine("Expanding Manage Investments dropdown first");
                GoToManageInvestments();
                Thread.Sleep(2000); // Wait for dropdown to expand
                
                // Debug: Print all available links in the dropdown
                Console.WriteLine("=== DEBUG: Looking for Automated Investments link ===");
                var allLinks = driver.FindElements(By.XPath("//a[contains(text(), 'Investment') or contains(text(), 'Automated')]"));
                Console.WriteLine($"Found {allLinks.Count} investment-related links:");
                foreach (var link in allLinks)
                {
                    Console.WriteLine($"- Text: '{link.Text}', Data-cy: '{link.GetAttribute("data-cy")}'");
                }
                
                // Try the more specific selector first
                try
                {
                    var element = AutomatedInvestment.GetElement();
                    Console.WriteLine("Found Automated Investments using data-cy selector");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                }
                catch (NoSuchElementException)
                {
                    // Fallback to the generic selector
                    Console.WriteLine("Trying generic selector for Automated Investments");
                    var element = lnkAutomatedInvestments.GetElement();
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GoToAutomatedInvestments: {ex.Message}");
                // Don't throw immediately, try to continue
                Console.WriteLine("Attempting to continue despite error...");
            }
            
            // Wait for page to load completely
            Thread.Sleep(3000);
            WaitForSpinners();
        }
    }
}

