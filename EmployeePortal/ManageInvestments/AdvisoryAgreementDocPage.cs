using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class AdvisorAgreementDocPage : BasePage
    {
        private PageControl headerText => new PageControl(By.XPath("//h4"), "Confirm Input");

        public AdvisorAgreementDocPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetHeaderText()
        {
            return headerText.GetText();
        }
    }
}

