using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class PreferencesTab : BasePage
    {
        private PageControl btnAccountTypeCollapse => new PageControl(By.XPath("//*[@id='pills-Investments--DW']/ancestor::button"), "Collapse Button");
        private PageControl btnCloseAccount => new PageControl(By.XPath(".//span[text()='Close Investment Option']/.."), "Close Investment Option");
        private PageControl btnAddAnotherAccount => new PageControl(By.XPath(".//span[text()='Add another investment option']/.."), "Add another option");
        private PageControl btnChangeAccountType => new PageControl(By.XPath(".//span[text()='Change investment account type']/.."), "Change investment account type");
        private PageControl btnReviewInvestmentProfileQuiz => new PageControl(By.XPath("//span[contains(text(),'Review investment profile quiz')]/.."), "Review investment profile quiz");
        private PageControl lnkInvestmentAccountNumber => new PageControl(By.XPath("//p[text()='Investment Account Number']/../div/span[1]"));
        private PageControl stcInvestmentAccountNumber => new PageControl(By.XPath("//p[text()='Investment Account Number']/../div/span[2]"));

        private PageControl lnkAdvisorAgreement => new PageControl(By.LinkText("HSA Advisor Agreement"), "HSA Advisor Agreement");
        private PageControl lnkHSAInvestTerms => new PageControl(By.LinkText("HSA Invest Terms and Conditions"), "HSA Invest Terms and Conditions");
        private PageControl stcDisclosureDocuments => new PageControl(By.XPath(".//p[@id='disclosure-documents']//.."));

        private PageControl txtConfirm => new PageControl(By.XPath(".//input"));
        private PageControl btnCloseYes => new PageControl(By.XPath(".//span[text()='Yes']"), "Yes");
        private PageControl btnCloseNo => new PageControl(By.XPath(".//span[text()='No']"), "No");

        // Legacy "single-account" buttons
        private PageControl btnNoIWantToKeep => new PageControl(By.XPath(".//span[text()='No, I want to keep the investment account type I have']"));

        public AdvisorAgreementDocPage AdvisorAgreementDocPage;

        public PreferencesTab(IWebDriver driver) : base(driver)
        {
            AdvisorAgreementDocPage = new AdvisorAgreementDocPage(driver);
        }

        public void CloseAccountYes(string confirmText)
        {
            btnCloseAccount.Click();
            txtConfirm.SendKeys(confirmText);
            Sleep(1);
            btnCloseYes.Click();
            WaitForSpinners();
        }

        public void ButtonCloseAccount()
        {
            btnCloseAccount.Click();
            Thread.Sleep(10000);
        }

        public void IConfirm(string confirmText)
        {
            txtConfirm.SendKeys(confirmText);
            Sleep(1);
            btnCloseYes.Click();
            WaitForSpinners();
        }

        public void AddAnotherAccount()
        {
            btnAddAnotherAccount.Click();
            WaitForSpinners();
        }

        public void ChangeAccountType()
        {
            btnChangeAccountType.Click();
        }

        public void NoIWantToKeep()
        {
            btnNoIWantToKeep.Click();
            WaitForSpinners();
        }

        public void ViewHSAAdvisorAgreement()
        {
            lnkAdvisorAgreement.Click();
            WaitForSpinners();
        }

        public void ViewHSAInvestTerms()
        {
            lnkHSAInvestTerms.Click();
            WaitForSpinners();
        }

        public bool IsHsaInvestDocDisplayed()
        {
            return lnkHSAInvestTerms.IsDisplayed();
        }

        public string GetDisclosureDocumentsText()
        {
            return stcDisclosureDocuments.GetText();
        }

        public bool IsReviewQuizButtonPresent()
        {
            return btnReviewInvestmentProfileQuiz.IsDisplayed();
        }

        public string GetAccountTypeFromCollapseButton()
        {
            return btnAccountTypeCollapse.GetText().Trim();
        }

        public string GetInvestmentAccountNumber()
        {
            return stcInvestmentAccountNumber.GetText();
        }

        public void ToggleInvestmentAccountNumber()
        {
            lnkInvestmentAccountNumber.Click();
        }

        public void ReviewInvestmentProfileQuiz()
        {
            btnReviewInvestmentProfileQuiz.Click();
            WaitForSpinners();
        }
    }
}

