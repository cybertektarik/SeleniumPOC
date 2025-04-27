using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class WizardSignaturePage : BasePage
    {
        private PageControl txtSignature => new PageControl(By.Id("captureName"), "Signature");
        private PageControl stcAgreementText => new PageControl(By.XPath("//div[@class='card-body']"), "Agreement Text");
        private PageControl stcSignedByText => new PageControl(By.XPath("//div[contains(@class, 'card-footer')]/span"));
        private PageControl chkAcknowledge => new PageControl(By.XPath("//label[contains(text(),'Signature')]"), "Signature");
        private PageControl chkBoxEsign => new PageControl(By.XPath("//div[contains(@class, 'custom-control custom-checkbox')]"), "custom-control custom-checkbox");
        private PageControl btnSign => new PageControl(By.XPath("//button[.='Sign']"), "Sign");

        private PageControl btnNext => new PageControl(By.XPath("//button[contains(., 'Next')]"), "Next");
        private PageControl btnPrevious => new PageControl(By.XPath("//span[text()='Previous ']"), "Previous");
        private PageControl nameInputBox => new PageControl(By.Id("captureName"));

        public bool IsSigned => stcSignedByText.IsDisplayed();

        public WizardSignaturePage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterSignature(string signature)
        {
            txtSignature.SetText(signature);
        }

        public void CheckAcknowledge()
        {
            chkAcknowledge.Click();
        }

        public void CheckEsignCheckBox()
        {
            chkBoxEsign.Click();
        }

        public bool IsEsignCheckBoxChecked()
        {
            return chkBoxEsign.IsSelected;
        }

        public void ScrollTextToEnableButton()
        {
            if (!btnSign.IsEnabled)
                stcAgreementText.SendKeysUsingActions(Keys.End);
        }

        public void EnterName(string name)
        {
            ScrollTextToEnableButton();
            nameInputBox.SendKeys(name);
        }

        public void Next()
        {
            btnNext.Click();
            WaitForSpinners();
        }

        public void Previous()
        {
            btnPrevious.Click();
            WaitForSpinners();
        }

        public void Sign()
        {
            WaitForSpinners();
            btnSign.Click();
            WaitForSpinners();
        }

        public string GetSignedByText()
        {
            return stcSignedByText.GetText();
        }
    }
}

