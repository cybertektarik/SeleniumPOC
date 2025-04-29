using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsSelectRTQ : EmployeePortalBaseTestSuite
    {
        [SetUp]
        public void NavigateToLogin()
        {
            GoToOktaLoginPage();
        }

        [Test]
        public void EnrollHsabUserToSelectWithRtqAndCloseAccount()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(1000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(2500, 100000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

            string username = "Feature2HSABTester010";

            //Logger.Step("Login to the Employee Portal as a user ready to create");
            Pages.OktaLoginPage.Login(username);

            //Logger.Step("Go to the Manage Investments tab and choose to Enroll");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseEnrollInHsaInvest();
            Pages.ManageInvestmentsPage.ChooseEnroll();

           // Logger.Step("Accept the Esign agreements");
            Pages.ManageInvestmentsPage.WizardSignaturePage.CheckAcknowledge();
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            //Logger.Step("Choose a Select account type");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

           // Logger.Step("Fill out the Required Disclosures Wizard page");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED", sector, occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetYesNoQuestionAnswers(false, false, false);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetCitizenship("USA");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetAnnualIncome(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetTotalNetWorth(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

            //Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

           // Logger.Step("Sign the Agreement");
            string signer = "Select " + CommonFunctions.GenerateRandomName(10);
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature(signer);
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();

            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();

            string signed = "Signed by " + signer + " on " + DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            Pages.ManageInvestmentsPage.WizardSignaturePage.GetSignedByText().Should().Contain(signed);

            // Logger.Step("Fill out the RTQ and Submit");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("1", "stronglyDisagree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("2", "disagree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("3", "stronglyDisagree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("4", "stronglyDisagree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("5", "stronglyDisagree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.Submit();

           // Logger.Step("Verify the RTQ Score page for score, categories and fund list(s)");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskToleranceType().Should().Be("Conservative.");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskArrowLocation().Should().Be("50");

            List<string> categories = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetCategories();
            categories.Count.Should().Be(2);
            categories.Should().Contain("Category 2 - 50%");

            List<string> investments = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetInstruments("Category 1");
            investments.Count.Should().Be(3);

            investments = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetInstruments("Category 2");
            investments.Count.Should().Be(3);
            investments.Should().Contain("DODIX|Dodge & Cox Income Fund|Mutual Fund Active");

            // ToDo: Validate donut graph display

            Pages.ManageInvestmentsPage.WizardRqtScorePage.Next();

            string message1Text = Pages.NotificationAlert.WaitAndGetSuccessMessage(60);
            message1Text.Should().Be("Select option created.");

           // Logger.Step("Cancel auto funding");
            Pages.ManageInvestmentsPage.AutoFundingPage.Skip();

           // Logger.Step("Verify Retake RTQ is there (BEND-19321)");
            Pages.ManageInvestmentsPage.ChooseHsaInvest();
            Pages.ManageInvestmentsPage.PreferencesTab.IsReviewQuizButtonPresent().Should().Be(true);

          //  Logger.Step("Verify the HSAB Invest doc");
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAInvestTerms();
            String newTabUrl = Pages.ManageInvestmentsPage.SwitchToNewTabAndGetUrl();
            newTabUrl.Should().Contain("documentKey-hsabank_investment_esign");
            Pages.ManageInvestmentsPage.PreferencesTab.AdvisorAgreementDocPage.GetHeaderText().Should().Contain("Signing document content goes here!");
            Pages.ManageInvestmentsPage.PreferencesTab.CloseCurrentTab();

           // Logger.Step("Log out and back in, and go back to Manage Investments page");
            Pages.HeaderBarPage.Logout();
            Pages.OktaLoginPage.Login(username);
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseHsaInvest();

            string currentAccountId = Pages.ManageInvestmentsPage.GetCurrentUrl();

          //  Logger.Step("Verify we don't ask user to sign again and verify choice option is enabled");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.AddAnotherAccount();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyButtonsEnabled(false, false, true);

          //  Logger.Step("Close the account and verify we're back on the Dashboard");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseHsaInvest();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.IsReviewQuizButtonPresent().Should().Be(true);
            string message = Pages.NotificationAlert.WaitAndGetSuccessMessage(30);
            message.Should().Be("Account was closed successfully.");

            Pages.SidebarNavPage.WaitForTabChange("Summary");
            Pages.SidebarNavPage.GetSelected().Should().Be("Summary");

            //Logger.Step("Log out and back in, and go back to Manage Investments page");
            Pages.HeaderBarPage.Logout();
            Pages.OktaLoginPage.Login(username);
            Pages.SidebarNavPage.GoToManageInvestments();

            //  Logger.Step("Make sure we see the Create account screen again and all choose buttons are enabled again");
            Pages.ManageInvestmentsPage.ChooseEnrollInHsaInvest();
            Pages.ManageInvestmentsPage.ChooseEnroll();
            Pages.ManageInvestmentsPage.WizardSignaturePage.CheckAcknowledge();
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

            //Logger.Step("Make sure we can't log back to the closed account");
            GoToUrl(currentAccountId);
            List<string> messages = GetAndDismissAllErrors();
            messages.Should().Contain("Error processing your request. Please try again later");
        }

        [Test]
        public void RetakeSelectAccountRTQ()
        {
            string username = "Feature2HSABTester003";

           // Logger.Step("Login to the Employee Portal as a HSAB user with an existing Select account");
            Pages.OktaLoginPage.Login(username);

          //  Logger.Step("Go to the Manage Investments tab and choose the Select account");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseHsaInvest();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

            //Logger.Step("Go to preferences and choose to Retake the RTQ");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.ReviewInvestmentProfileQuiz();
            string currentType = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskToleranceType();

          //  Logger.Step("Choose to get an aggressive score");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("1", "stronglyAgree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("2", "stronglyAgree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("3", "stronglyAgree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("4", "agree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("5", "stronglyAgree");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.Submit();

            //Logger.Step("Verify the RTQ Score page for score, categories and fund list(s)");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskToleranceType().Should().Be("Aggressive.");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskArrowLocation().Should().Be("242");

            List<string> categories = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetCategories();
            categories.Count.Should().Be(3);
            categories.Should().BeEquivalentTo("Cat1 - 80%", "Cat2 - 20%", "Others - 0%");

            List<string> investments = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetInstruments("Cat1");
            investments.Count.Should().Be(1);

            investments = Pages.ManageInvestmentsPage.WizardRqtScorePage.GetInstruments("Cat2");
            investments.Count.Should().Be(1);
            investments.Should().Contain("TEAX|American Funds Tax-Exempt Bond Fund of America|Mutual Fund Active");

           // Logger.Step("Choose yes and verify we're on the Preferences tab");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.YesIWantToChooseThisPortfolio();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Preferences");

          //  Logger.Step("Verify the instruments on the Available Investments tab");
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();
            var list = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            list.Count.Should().Be(3);

           // Logger.Step("Verify the auto-funding page has all categories");
            Pages.ManageInvestmentsPage.ClickCurrentHoldingsTab();
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.SetupAutomatedFunding();
            Pages.ManageInvestmentsPage.AutoFundingPage.ToggleShowAllFunds();

            categories = Pages.ManageInvestmentsPage.AutoFundingPage.GetSelectCategories();
            categories.Count.Should().Be(3);
            categories.Should().BeEquivalentTo("Cat1 - 80%", "Cat2 - 20%", "Others - 0%");

            Pages.ManageInvestmentsPage.AutoFundingPage.Cancel();

            // Logger.Step("Retake the RTQ back to neutral / moderate");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.ReviewInvestmentProfileQuiz();
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("1", "neutral");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("2", "neutral");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("3", "neutral");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("4", "neutral");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion("5", "neutral");
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.Submit();

            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskToleranceType().Should().Be("Moderate.");
            Pages.ManageInvestmentsPage.WizardRqtScorePage.GetRiskArrowLocation().Should().Be("141");

           // Logger.Step("Verify the instruments update on the Available Investments tab");
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();
            list = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            list.Count.Should().Be(10); // Only 10 displayed at a time until you scroll down
        }
    }
}

