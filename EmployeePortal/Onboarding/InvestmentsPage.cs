using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using SeleniumPOC.EmployeePortal.Pages.ManageInvestments;

namespace SeleniumPOC.EmployeePortal.Pages.Onboarding
{
    public class InvestmentsPage : BasePage
    {
        private PageControl btnStartEnrollment => new PageControl(By.XPath("//span[text()='Start Enrollment']/.."), "START ENROLLMENT >");

        public ChooseYourInvestmentPage ChooseYourInvestmentPage;

        public InvestmentsPage(IWebDriver driver) : base(driver)
        {
            this.ChooseYourInvestmentPage = new ChooseYourInvestmentPage(driver);
        }

        public void StartEnrollment()
        {
            btnStartEnrollment.Click();
            WaitForSpinners();
        }
    }
}

