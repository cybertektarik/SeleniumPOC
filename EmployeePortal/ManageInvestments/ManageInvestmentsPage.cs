using FluentAssertions;
using OpenQA.Selenium;
//using OpenQA.Selenium.Bidi.Modules.BrowsingContext;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class ManageInvestmentPage : BasePage
    {
        // ======================== P A G E   C O N T R O L S ========================
        private PageControl stcInvestmentAccountHeader = new PageControl(By.XPath("//h1[text()='Manage Investments']//.."), "Manage Investments");
        private PageControl lnkCreateAccount = new PageControl(By.LinkText("CREATE ACCOUNT")); // ToDo: Remove!
        private PageControl btnEnroll = new PageControl(By.XPath("//span[text()='Enroll']//.."));
        private PageControl btnEnrollHsaInvest = new PageControl(By.XPath("//h4[contains(text(), 'Enroll in HSA Invest')]//.."), "Enroll in HSA Invest");
        private PageControl btnHsaInvest => new PageControl(By.XPath("(//*[contains(text(), 'HSA Invest')])[last()]"), "HSA Invest");

        private PageControl btnCancelPreEnrollment = new PageControl(By.XPath("//button[text()='Cancel Pre-Enrollment']"));
        private PageControl btnStartEnrollment = new PageControl(By.XPath("//*[contains(text(),'Start Enrollment')]"));
        private PageControl linkHsaInvestInfo = new PageControl(By.XPath("//*[contains(text(),'HSA Invest Info')]"), "HSA Invest Info");

        private PageControl stcHsaInvestButtonBalance = new PageControl(By.XPath("//h4[contains(text(),'HSA Invest')]/../div[3]"), "HSA Invest balance");
        private PageControl btnDevenirOrSchwab = new PageControl(By.XPath("//h4[contains(text(), 'Devenir or Schwab')]//.."), "Devenir or Schwab");
        private PageControl stcDevenirOrSchwabButtonBalance = new PageControl(By.XPath("//h4[contains(text(), 'Devenir or Schwab')]/../div[3]"), "Devenir or Schwab balance");
        private PageControl stcCongrats = new PageControl(By.XPath("//strong[contains(text(),'Congrats')]"), ""); // ToDo: Remove!
        private PageControl stcYouAreEligible = new PageControl(By.XPath("//h3[contains(text(), 'Invest your HSA')]"), "");
        private PageControl stcPreEnrolledCardTitle = new PageControl(By.XPath("//h4[contains(text(), 'You have Enrolled for the')]"));
        private PageControl stcPreEnrolledCardBody = new PageControl(By.ClassName("card-body"));
        private PageControl stcPreEnrollHeader = new PageControl(By.XPath("//h3[text()='Automated Enrollment']"));
        private PageControl btnUpdatePreEnrollment = new PageControl(By.XPath("//button[text()='Update Pre-Enrollment']"));
        private PageControl stcPreEnrollBodyText = new PageControl(By.XPath("//div[@id='invest-automated']"));
        private PageControl tabCurrentlySelected = new PageControl(By.XPath("//li[contains(@class, 'active')]"));
        private PageControl tabToSelect(string tabName) => new PageControl(By.XPath($"//ul[contains(@class, 'tabs-container')]/li/a[contains(text(), '{tabName}')]"), tabName);
        private PageControl tabSearchAndTrade = new PageControl(By.XPath("//ul[contains(@class,'tabs-container')]/li/a[contains(text(),'Search & Trade')]"), "Search & Trade");
        private PageControl linkManageHsaInvestEnroll = new PageControl(By.XPath("//*[contains(text(),'Manage HSA Invest Enrollment')]"), "Manage HSA Invest Enrollment");
        private PageControl btnCancelEnrollment = new PageControl(By.XPath("//*[contains(text(),'Cancel Enrollment')]"));
        private PageControl strYourInvestmentsWillActivateText = new PageControl(By.XPath("//*[contains(text(),'Your investments will activate after you reach a minimum HSA cash balance')]"));
        private PageControl strInvestEnrollCancelledText = new PageControl(By.XPath("//*[contains(text(),'Investment Enrollment has been cancelled')]"));


        private PageControl stCloseInvestmentOptionSelectText = new PageControl(By.XPath("//*[contains(text(),'To close your investment option, you must first sell all your holdings to bring your balance to $0.')]"));
        private PageControl stCloseInvestmentOptionChoiceText = new PageControl(By.XPath("//*[contains(text(),'To close your investment option, you must first sell all your holdings to bring your balance to $0.')]"));
        private PageControl stCloseInvestmentOptionManagedText = new PageControl(By.XPath("//*[contains(text(),'To close your investment option, you must first sell all your holdings to bring your balance to $0.')]"));
        private PageControl btnCloseInvestmentOptionSelect = new PageControl(By.XPath("(//*[contains(text(),'Close Investment Option')]//ancestor::*//button)[last()-3]"));
        private PageControl btnCloseInvestmentOptionChoice = new PageControl(By.XPath("(//*[contains(text(),'Close Investment Option')]//ancestor::*//button)[last()-2]"))
            ;
        private PageControl btnCloseInvestmentOptionManaged = new PageControl(By.XPath("(//*[contains(text(),'Close Investment Option')]//ancestor::*//button)[last()-1]"));

        private PageControl lnkHsaAvisorySelect = new PageControl(By.XPath("//*[normalize-space(text())='HSA Advisory Agreement Select']"));
        private PageControl lnkHsaAvisoryChoice = new PageControl(By.XPath("//*[normalize-space(text())='HSA Advisory Agreement Choice']"));
        private PageControl lnkHsaAvisoryManaged = new PageControl(By.XPath("//*[normalize-space(text())='HSA Advisory Agreement Managed']"));

        private PageControl stFeesForManaged = new PageControl(By.XPath("//div[contains(text(),'Fees for Managed')]/following-sibling::div"));
        private PageControl stFeesForSelect = new PageControl(By.XPath("//div[contains(text(),'Fees for Select')]/following-sibling::div"));
        private PageControl stFeesForChoice = new PageControl(By.XPath("//div[contains(text(),'Fees for Choice')]/following-sibling::div"));

        private PageControl stChoiceAccountCreated = new PageControl(By.XPath("//*[contains(text(), 'Choice') and contains(text(), 'Limited Brokerage')]"));

        private PageControl stockToVerify(string stockName) => new PageControl(By.XPath($"//ul[contains(@class, 'tabs-container')]/li/a[contains(text(), '{stockName}')]"), stockName);

        // ======================== C H I L D   P A G E S ======================== private PageControl stCloseInvestmentOptionSelectText = new PageControl(By.XPath("//*[contains(text(),'To close your investment option, you must first sell all your holdings to bring your balance to $0.')]"));
        public ChooseYourInvestmentPage ChooseYourInvestmentPage;
        public CurrentHoldingsTab CurrentHoldingsTab;
        public AutoFundingPage AutoFundingPage;
        public AvailableInvestmentsTab AvailableInvestmentsTab;
        public ActivityTab ActivityTab;
        public DocumentsTab DocumentsTab;
        public PreferencesTab PreferencesTab;
        public BuyInstrumentPage BuyInstrumentPage;
        public SellInstrumentPage SellInstrumentPage;
        public WizardRequiredDisclosuresPage WizardRequiredDisclosuresPage;
        public WizardDisclosureAgreements WizardDisclosureAgreementsPage;
        public WizardSignaturePage WizardSignaturePage;
        public WizardRtqQuestionsPage WizardRtqQuestionsPage;
        public WizardRqtScorePage WizardRqtScorePage;
        public SearchAndTradePage SearchAndTradePage;

        public ManageInvestmentPage(IWebDriver driver) : base(driver)
        {
            ChooseYourInvestmentPage = new ChooseYourInvestmentPage(driver);
            CurrentHoldingsTab = new CurrentHoldingsTab(driver);
            AutoFundingPage = new AutoFundingPage(driver);
            AvailableInvestmentsTab = new AvailableInvestmentsTab(driver);
            ActivityTab = new ActivityTab(driver);
            DocumentsTab = new DocumentsTab(driver);
            BuyInstrumentPage = new BuyInstrumentPage(driver);
            SellInstrumentPage = new SellInstrumentPage(driver);
            WizardRequiredDisclosuresPage = new WizardRequiredDisclosuresPage(driver);
            WizardDisclosureAgreementsPage = new WizardDisclosureAgreements(driver);
            WizardSignaturePage = new WizardSignaturePage(driver);
            WizardRtqQuestionsPage = new WizardRtqQuestionsPage(driver);
            WizardRqtScorePage = new WizardRqtScorePage(driver);
            PreferencesTab = new PreferencesTab(driver);
            SearchAndTradePage = new SearchAndTradePage(driver);
        }

        // ======================== P A G E   M E T H O D S ========================
        public void VerifyEnrollPageShown()
        {
            stcInvestmentAccountHeader.VerifyIsVisible();
            stcYouAreEligible.VerifyContainsText("You’re eligible to invest your HSA funds! Enroll to potentially grow HSA funds for healthcare expenses now and in retirement.");
            btnEnroll.VerifyIsVisible();
        }

        public void ChooseEnroll()
        {
            ClickAndWaitForSpinners(btnEnroll);
        }

        public void ChooseEnrollInHsaInvest()
        {
            ClickAndWaitForSpinners(btnEnrollHsaInvest);
        }

        public void IsDisplayHsaEnrollInHsaInvest()
        {
            btnCancelPreEnrollment.IsDisplayed();
        }

        public void ChooseHsaInvest()
        {
            ClickAndWaitForSpinners(btnHsaInvest);
        }

        public void HSAInvestInfo()
        {
            ClickAndWaitForSpinners(linkHsaInvestInfo);
        }

        public void ChooseDevenirOrSchwab()
        {
            btnDevenirOrSchwab.Click();
        }

        public void VerifyAlreadyPreEnrolledPageShown(string accountType)
        {
            stcInvestmentAccountHeader.VerifyIsVisible();
            stcPreEnrolledCardTitle.VerifyContainsText("You have Enrolled for the " + accountType + " Option");
            stcPreEnrolledCardBody.VerifyContainsText("You may cancel enrollment or update your automated funding settings.");
            btnCancelPreEnrollment.VerifyIsVisible();
            btnUpdatePreEnrollment.VerifyIsVisible();
        }

        public void VerifyStartPreEnrollOptionShown(string bodyText)
        {
            stcInvestmentAccountHeader.VerifyIsVisible();
            stcPreEnrollHeader.VerifyIsVisible();
            btnStartEnrollment.VerifyIsVisible();
            stcPreEnrollBodyText.GetText().Should().Contain(bodyText);
        }

        public string GetCurrentlySelectedTab()
        {
            return tabCurrentlySelected.GetText().Trim();
        }

        public void ClickDocumentsTab()
        {
            tabToSelect("Documents").Click();
            WaitForSpinners();
        }

        public void ClickCurrentHoldingsTab()
        {
            tabToSelect("Current Holdings").Click();
            WaitForSpinners();
        }

        public void ClickPreferencesTab()
        {
            tabToSelect("Preferences").Click();
            WaitForSpinners();
        }

        public void ClickAvailableInvestmentsTab()
        {
            tabToSelect("Available Investments").Click();
            WaitForSpinners();
        }

        public void ClickActivityTab()
        {
            ClickActivityTab(true);
        }

        public void ClickActivityTab(bool waitForSpinners)
        {
            tabToSelect("Activity").Click();

            if (waitForSpinners)
                WaitForSpinners();
        }

        public void SearchAndTradeTab()
        {
            WaitForSpinners();
            tabToSelect("Search & Trade").Click();
            WaitForSpinners();
        }

        public void StartPreEnrollment()
        {
            btnStartEnrollment.Click();
            WaitForSpinners();
        }

        public void CancelPreEnrollment()
        {
            if (btnCancelPreEnrollment.IsDisplayed())
            {
                btnCancelPreEnrollment.Click();
                WaitForSpinners();
            }
        }

        public void UpdatePreEnrollment()
        {
            btnUpdatePreEnrollment.Click();
        }

        public string GetHsaInvestButtonBalance()
        {
            return stcHsaInvestButtonBalance.GetText().Trim();
        }

        public string GetDevenirOrSchwabButtonBalance()
        {
            return stcDevenirOrSchwabButtonBalance.GetText().Trim();
        }

        public void ClickManageHsaInvestEnroll()
        {
            WaitForElementToBeVisible(linkManageHsaInvestEnroll);
            linkManageHsaInvestEnroll.Click();
            //ClickAndWaitForSpinners(linkManageHsaInvestEnroll);
        }

        public void VerifyManageHsaInvestEnrollIsDisplayed()
        {
            Thread.Sleep(2000);
            WaitForElementToBeVisible(linkManageHsaInvestEnroll);
            linkManageHsaInvestEnroll.VerifyIsVisible();
        }

        public void CancelEnrollment()
        {
            btnCancelEnrollment.Click();
            WaitForSpinners();
        }

        public void VerifyYourInvestmentsWillActivateIsDisplayed()
        {
            Thread.Sleep(2000);
            WaitForElementToBeVisible(strYourInvestmentsWillActivateText);
            strYourInvestmentsWillActivateText.VerifyIsVisible();
        }

        public void VerifyInvestEnrollCancelledIsDisplayed()
        {
            Thread.Sleep(2000);
            WaitForElementToBeVisible(strInvestEnrollCancelledText);
            strInvestEnrollCancelledText.VerifyIsVisible();
        }

        public string geCloseInvestmentOptionSelectText()
        {
            return stCloseInvestmentOptionSelectText.GetText();
        }

        public string geCloseInvestmentOptionChoiceText()
        {
            return stCloseInvestmentOptionChoiceText.GetText();
        }

        public string geCloseInvestmentOptionManagedText()
        {
            return stCloseInvestmentOptionManagedText.GetText();
        }

        public bool closeInvestmentButtonSelect()
        {
            return !btnCloseInvestmentOptionSelect.IsEnabled;
        }

        public bool closeInvestmentButtonChoice()
        {
            return !btnCloseInvestmentOptionChoice.IsEnabled;
        }

        public bool closeInvestmentButtonManaged()
        {
            return !btnCloseInvestmentOptionManaged.IsEnabled;
        }

        public void ClickHsaAdvisorySelect()
        {
            lnkHsaAvisorySelect.Click();
            WaitForSpinners();
        }

        public void ClickHsaAdvisoryChoice()
        {
            lnkHsaAvisoryChoice.Click();
            WaitForSpinners();
        }

        public void ClickHsaAdvisoryManaged()
        {
            lnkHsaAvisoryManaged.Click();
            WaitForSpinners();
        }

        public void clickCloseInvestmentButtonSelect()
        {
            btnCloseInvestmentOptionSelect.Click();
            WaitForSpinners();
        }

        public void clickCloseInvestmentButtonChoice()
        {
            btnCloseInvestmentOptionChoice.Click();
            WaitForSpinners();
        }

        public void clickCloseInvestmentButtonManaged()
        {
            btnCloseInvestmentOptionManaged.Click();
            WaitForSpinners();
        }

        public string GetFeeMessageForManaged()
        {
            return stFeesForManaged.GetText();
        }

        public string GetFeeMessageForSelect()
        {
            return stFeesForSelect.GetText();
        }

        public string GetFeeMessageForChoice()
        {
            return stFeesForChoice.GetText();
        }

        public bool IsChoiceAccountCreated()
        {
            return stChoiceAccountCreated.IsDisplayed();
        }

        public bool verifyStockAdded(String stockName)
        {
            return stockToVerify(stockName).IsDisplayed();
        }

    }
}

