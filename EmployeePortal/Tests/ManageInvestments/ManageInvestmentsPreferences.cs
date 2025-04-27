using FluentAssertions;
using SeleniumPOC.Common;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsPreferences : EmployeePortalBaseTestSuite
    {
        [Test]
        public void CreateDoSelectAndCloseAccount()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(10000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(25000, 100000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

            //Logger.Step("Login to the Employee Portal as a user ready to create");
            Pages.LoginPage.Login("QATesting+AccountCreator@hsabank.com");

            //Logger.Step("Go to the Manage Investments tab and choose to create a Curated account");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

          //  Logger.Step("Fill out the Required Disclosures Wizard Page");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED", sector, occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetYesNoQuestionAnswers(false, false, false);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetCitizenship("USA");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetAnnualIncome(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetTotalNetWorth(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

           // Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

           // Logger.Step("Sign the Agreement");
            string signer = "Select " + CommonFunctions.GenerateRandomName(10);
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature(signer);
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.WizardSignaturePage.GetSignedByText().Should().Contain("Signed by " + signer);
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

            string messageText = Pages.NotificationAlert.WaitAndGetSuccessMessage(60);
            messageText.Should().Be("Select option created.");

           // Logger.Step("Skip auto funding");
            Pages.ManageInvestmentsPage.AutoFundingPage.Skip();

           // Logger.Step("Log out and back in, and go back to Manage Investments page");
            Pages.HeaderBarPage.Logout();
            Pages.LoginPage.Login("QATesting+AccountCreator@hsabank.com");
            Pages.SidebarNavPage.GoToManageInvestments();

            string currentAccountUrl = Pages.ManageInvestmentsPage.GetCurrentUrl();

            //Logger.Step("Close the account and verify we're back on the Dashboard");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.CloseAccountYes("yes");

            string message = Pages.NotificationAlert.WaitAndGetSuccessMessage(30);
            message.Should().Be("Account was closed successfully.");

            Pages.SidebarNavPage.WaitForTabChange("Dashboard");
            Pages.SidebarNavPage.GetSelected().Should().Be("Dashboard");

           // Logger.Step("Make sure we see the Create account screen again and all chooser buttons are enabled again");
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyAllButtonsEnabled();

           // Logger.Step("Make sure we can't deep link back to the closed account");
            GoToUrl(currentAccountUrl);
            List<string> messages = GetAndDismissAllErrors();
            messages.Should().Contain("Error processing your request. Please try again later.");
        }

        [Test]
        public void CreateDoChoiceWithAutoFundingAndCloseAccount()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(10000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(25000, 100000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

           // Logger.Step("Login to the Employee Portal as a user ready to create");
            Pages.LoginPage.Login("QATesting+AutoCloser@hsabank.com");

           // Logger.Step("Go to the Manage Investments tab and choose to create a Limited account");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseChoice();

           // Logger.Step("Fill out the Required Disclosures Wizard Page");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED", sector, occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetYesNoQuestionAnswers(false, false, false);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetCitizenship("USA");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetAnnualIncome(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetTotalNetWorth(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

           // Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

          //  Logger.Step("Sign the Agreement and verify success message");
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature("Choice Signer");
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

            string messageText = Pages.NotificationAlert.WaitAndGetSuccessMessage(60);
            messageText.Should().Be("Choice option created.");

          //  Logger.Step("Fill out auto-funding info");
            string threshold = CommonFunctions.GenerateRandomDollarAmount(10000, 20000);
            string searchFor1 = "TXRH";
            string searchFor2 = "TFLO";

            Pages.ManageInvestmentsPage.AutoFundingPage.SetThreshold(threshold);
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForChoiceFund(searchFor1);
            Pages.ManageInvestmentsPage.AutoFundingPage.AddLimitedStockInstrument(searchFor1);
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForChoiceFund(searchFor2);
            Pages.ManageInvestmentsPage.AutoFundingPage.AddLimitedStockInstrument(searchFor2);
            Pages.ManageInvestmentsPage.AutoFundingPage.SetPercentPurchaseAllocation(searchFor1, "40");
            Pages.ManageInvestmentsPage.AutoFundingPage.SetPercentPurchaseAllocation(searchFor2, "60");
            Pages.ManageInvestmentsPage.AutoFundingPage.Review();
            Pages.ManageInvestmentsPage.AutoFundingPage.Accept();

           // Logger.Step("Verify the auto-funding settings");
            Pages.ManageInvestmentsPage.Refresh();
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.ManageAutomatedFunding();
            Pages.ManageInvestmentsPage.AutoFundingPage.GetThreshold().Should().Be(threshold);
            Pages.ManageInvestmentsPage.AutoFundingPage.GetPercentPurchaseAllocation(searchFor1).Should().Be("40");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetPercentPurchaseAllocation(searchFor2).Should().Be("60");
            Pages.ManageInvestmentsPage.AutoFundingPage.Cancel();

            //Logger.Step("Close the account and verify we're back on the Dashboard");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.CloseAccountYes("yes");

            string message = Pages.NotificationAlert.WaitAndGetSuccessMessage(30);
            message.Should().Be("Account was closed successfully.");

            Pages.SidebarNavPage.WaitForTabChange("Dashboard");
            Pages.SidebarNavPage.GetSelected().Should().Be("Dashboard");
        }
        [Test]
        public void CreateTwoAccountsWithAutoFundingOnOneAndCloseBoth()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(10000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(25000, 100000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

           // Logger.Step("Login to the Employee Portal as a user ready to create");
            Pages.LoginPage.Login("QATesting+SweepCloser@hsabank.com");

          //  Logger.Step("Go to the Manage Investments tab and choose to create a Select account");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

          //  Logger.Step("Fill out the Required Disclosures Wizard Page");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED", sector, occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetYesNoQuestionAnswers(false, false, false);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetCitizenship("USA");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetAnnualIncome(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetTotalNetWorth(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

          //  Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

          //  Logger.Step("Sign the Agreement");
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature("Select This");
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();
            Pages.NotificationAlert.WaitAndGetSuccessMessage(60);
            Pages.NotificationAlert.Dismiss();

           // Logger.Step("Skip auto funding");
            Pages.ManageInvestmentsPage.AutoFundingPage.Skip();

          //  Logger.Step("Verify the account number is hidden and can be displayed");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.GetInvestmentAccountNumber().Should().Be("*********");
            Pages.ManageInvestmentsPage.PreferencesTab.ToggleInvestmentAccountNumber();
            string selectAccountNumber = Pages.ManageInvestmentsPage.PreferencesTab.GetInvestmentAccountNumber();
            selectAccountNumber.Should().NotBe("*********");

           // Logger.Step("View the Advisor Agreement");
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAAdvisorAgreement();
            Pages.ManageInvestmentsPage.PreferencesTab.GetDisclosureDocumentsText().Should().Contain("ABG Consultants, LLC");
            string newTabUrl = Pages.ManageInvestmentsPage.SwitchToNewTabAndGetUrl();
            newTabUrl.Should().Contain("abg-advisory.curated");
            Pages.ManageInvestmentsPage.PreferencesTab.AdvisorAgreementDocPage.GetHeaderText()
                .Should().Contain("NON-DISCRETIONARY ADVISORY AGREEMENT: LIMITED BROKERAGE");
            Pages.ManageInvestmentsPage.CloseCurrentTab();

           // Logger.Step("Add another account: Choice");
            Pages.ManageInvestmentsPage.PreferencesTab.AddAnotherAccount();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseChoice();

          //  Logger.Step("Verify the values still match what we entered and click [Next]");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetSelectedSector().Should().Be(sector);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetSelectedOccupation().Should().Be(occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetAnnualIncome().Should().Be(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetNetWorth().Should().Be(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

           // Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

           // Logger.Step("Sign the Agreement");
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature("Taster’s Choice");
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();
            Pages.NotificationAlert.WaitAndGetSuccessMessage(60);
            Pages.NotificationAlert.Dismiss();
         //   Logger.Step("Fill out auto-funding info for limited");
            string threshold = CommonFunctions.GenerateRandomDollarAmount(10000, 20000);
            string searchFor1 = "JEPI";
            string searchFor2 = "SCHD";

            Pages.ManageInvestmentsPage.AutoFundingPage.SetThreshold(threshold);
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForChoiceFund(searchFor1);
            Pages.ManageInvestmentsPage.AutoFundingPage.AddLimitedStockInstrument(searchFor1);
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForChoiceFund(searchFor2);
            Pages.ManageInvestmentsPage.AutoFundingPage.AddLimitedStockInstrument(searchFor2);
            Pages.ManageInvestmentsPage.AutoFundingPage.SetPercentPurchaseAllocation(searchFor1, "40");
            Pages.ManageInvestmentsPage.AutoFundingPage.SetPercentPurchaseAllocation(searchFor2, "60");
            Pages.ManageInvestmentsPage.AutoFundingPage.Review();

         //   Logger.Step("Verify the account number is different from the first and is hidden");
            Pages.SidebarNavPage.GoToDashboard();
            Pages.ManageInvestmentsPage.PreferencesTab.ToggleInvestmentAccountNumber();
            Pages.ManageInvestmentsPage.PreferencesTab.GetInvestmentAccountNumber().Should().Be("*********");

          //  Logger.Step("View the Advisor Agreement");
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAAdvisorAgreement();
            Pages.ManageInvestmentsPage.PreferencesTab.GetDisclosureDocumentsText().Should().Contain("ABG Consultants, LLC");
            newTabUrl = Pages.ManageInvestmentsPage.SwitchToNewTabAndGetUrl();
            newTabUrl.Should().Contain("abg-advisory.selfdirected");
            Pages.ManageInvestmentsPage.PreferencesTab.AdvisorAgreementDocPage.GetHeaderText()
                .Should().Contain("NON-DISCRETIONARY ADVISORY AGREEMENT: LIMITED BROKERAGE");
            Pages.ManageInvestmentsPage.CloseCurrentTab();

          //  Logger.Step("Close the account w/o auto-funding and verify we're back on the Dashboard");
            Pages.ManageInvestmentsPage.PreferencesTab.CloseAccountYes("yes");

            string message = Pages.NotificationAlert.WaitAndGetSuccessMessage(30);
            message.Should().Be("Account was closed successfully.");
            Pages.SidebarNavPage.WaitForTabChange("Dashboard");
            Pages.SidebarNavPage.GetSelected().Should().Be("Dashboard");

           // Logger.Step("Do validations on the Manage Investments tab");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyEnrollPageShown();
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyIsVisible();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyAllButtonsEnabled();
        }

        [Test]
        public void VerifyDisclosureAndOtherDocumentLinks()
        {
          //  Logger.Step("Login and go to the Manage Investments tab");
            Pages.LoginPage.Login("QATesting+AdvisorAgreement@hsabank.com");
            Pages.SidebarNavPage.GoToManageInvestments();

          //  Logger.Step("Go to the Select Preferences tab");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();

         //   Logger.Step("View the Advisor Agreement");
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAAdvisorAgreement();
            Pages.ManageInvestmentsPage.PreferencesTab.GetDisclosureDocumentsText().Should().Contain("ABG Consultants, LLC");
            string newTabUrl = Pages.ManageInvestmentsPage.SwitchToNewTabAndGetUrl();
            newTabUrl.Should().Contain("abg_advisory_curated");
            Pages.ManageInvestmentsPage.PreferencesTab.AdvisorAgreementDocPage.GetHeaderText().Should().Contain("NON-DISCRETIONARY ADVISORY AGREEMENT: CURATED LIST");
            Pages.ManageInvestmentsPage.CloseCurrentTab();

          //  Logger.Step("Go to the Choice Preferences tab");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseChoice();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();

          //  Logger.Step("View the Advisor Agreement");
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAAdvisorAgreement();
            Pages.ManageInvestmentsPage.PreferencesTab.GetDisclosureDocumentsText().Should().Contain("ABG Consultants, LLC");
            newTabUrl = Pages.ManageInvestmentsPage.SwitchToNewTabAndGetUrl();
            newTabUrl.Should().Contain("abg_advisory_selfdirected");
            Pages.ManageInvestmentsPage.PreferencesTab.AdvisorAgreementDocPage.GetHeaderText().Should().Contain("NON-DISCRETIONARY ADVISORY AGREEMENT: LIMITED BROKERAGE");
            Pages.ManageInvestmentsPage.CloseCurrentTab();

         //   Logger.Step("Verify that the HSA Invest agreement link is not there (BEND-19677)");
            Pages.ManageInvestmentsPage.PreferencesTab.IsHsaInvestDocDisplayed().Should().Be(false);










        }
    }
}

