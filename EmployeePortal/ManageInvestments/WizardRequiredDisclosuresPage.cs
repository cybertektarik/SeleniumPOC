using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class WizardRequiredDisclosuresPage : BasePage
    {
        private PageControl radioEmploymentStatus(string status) => new PageControl(By.XPath($"//input[@value='{status}'])[2]/.."), status);
        private PageControl selectSector => new PageControl(By.Id("employment-sector"), "Sector");
        private PageControl selectOccupation => new PageControl(By.Id("occupation"), "Occupation");
        private PageControl radioBroker(string value) => new PageControl(By.XPath($"//input[@name='broker-or-stock-exchange' and @value='{value}']/.."), "Broker-" + value);
        private PageControl radioOwn(string value) => new PageControl(By.XPath($"//input[@name='own-10-percent' and @value='{value}']/.."), "Own 10 Percent-" + value);
        private PageControl radioPolitical(string value) => new PageControl(By.XPath($"//input[@name='politically-affiliated' and @value='{value}']/.."), "Political-" + value);
        private PageControl selectCitizenship => new PageControl(By.Id("citizenship"));
        private PageControl txtIncome => new PageControl(By.XPath("//div[@id='annual-income']/input"), "Income");
        private PageControl txtTotalNetWorth => new PageControl(By.XPath("//div[@id='net-worth-total']/input"), "Net Worth");

        private PageControl btnNext => new PageControl(By.XPath("//button[contains(., 'Next')]"), "Next");
        private PageControl btnPrevious => new PageControl(By.XPath("//span[text()='Previous ']"), "Previous");

        public WizardRequiredDisclosuresPage(IWebDriver driver) : base(driver)
        {
        }

        // valid values: EMPLOYED, RETIRED, STUDENT, UNEMPLOYED, SELF_EMPLOYED
        public void EnterEmploymentStatusInfo(string status, string sector, string occupation)
        {
            radioEmploymentStatus(status).Click();
            selectSector.SelectByValue(sector);
            selectOccupation.SelectByValue(occupation);
        }

        public void EnterEmploymentStatusInfo(string status)
        {
            radioEmploymentStatus(status).Click();
        }

        public void SetYesNoQuestionAnswers(bool broker, bool own10Percent, bool political)
        {
            radioBroker(broker.ToString().ToLower()).Click();
            radioOwn(own10Percent.ToString().ToLower()).Click();
            radioPolitical(political.ToString().ToLower()).Click();
        }

        public void SetCitizenship(string value)
        {
            selectCitizenship.SelectByValue(value);
        }

        public void SetAnnualIncome(string income)
        {
            txtIncome.SetText(income);
        }

        public void SetTotalNetWorth(string worth)
        {
            txtTotalNetWorth.SetText(worth);
        }

        public string GetSelectedSector()
        {
            return selectSector.GetSelectedValue();
        }

        public string GetSelectedOccupation()
        {
            return selectOccupation.GetSelectedValue();
        }

        public string GetAnnualIncome()
        {
            return txtIncome.GetValue();
        }

        public string GetNetWorth()
        {
            return txtTotalNetWorth.GetValue();
        }

        public void Next()
        {
            btnNext.Click();
            WaitForSpinners();
        }

        public void Previous()
        {
            btnPrevious.Click();
        }
    }
}

