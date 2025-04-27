using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class WizardDisclosureAgreements : BasePage
    {
        private PageControl chkAcknowledge => new PageControl(By.XPath("//*[@id='terms-checkbox']"), "Terms Checkbox");
        private PageControl chkAcknowledgeLabel => new PageControl(By.XPath("//label[@for='terms-checkbox']"), "Terms Checkbox");

        private PageControl btnNext => new PageControl(By.XPath("//button[contains(., 'Next')]"), "Next");
        private PageControl btnPrevious => new PageControl(By.XPath("//span[text()='Previous ']"), "Previous");

        private PageControl hsaBankInvestmentEsignAgreement = new PageControl(By.XPath("//div[contains(@class, 'card-header') and contains(text(), 'HsaBank Investment Esign Agreement')]"));

        public WizardDisclosureAgreements(IWebDriver driver) : base(driver)
        {
        }

        public void AcknowledgeAgreement()
        {
            if (!chkAcknowledge.IsSelected)
                chkAcknowledgeLabel.Click();
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

        public void VerifyHsaEsignLetterIsDisplayed()
        {
            hsaBankInvestmentEsignAgreement.VerifyIsVisible();
        }
    }
}

