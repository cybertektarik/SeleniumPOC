using OpenQA.Selenium;
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
    }
}

