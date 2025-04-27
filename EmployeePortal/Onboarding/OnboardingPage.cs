using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Onboarding
{
    public class OnboardingPage : BasePage
    {
        private PageControl lnkInvestments => new PageControl(By.XPath("//span[text()='INVESTMENTS']/.."), "INVESTMENTS");
        private PageControl lnkLogout => new PageControl(By.LinkText("Logout"), "Logout");

        public InvestmentsPage InvestmentsPage;

        public OnboardingPage(IWebDriver driver) : base(driver)
        {
            this.InvestmentsPage = new InvestmentsPage(driver);
        }

        public void GoToInvestmentsStep()
        {
            lnkInvestments.Click();
            WaitForSpinners();
        }

        public void Logout()
        {
            lnkLogout.Click();
            WaitForSpinners();
        }
    }
}

