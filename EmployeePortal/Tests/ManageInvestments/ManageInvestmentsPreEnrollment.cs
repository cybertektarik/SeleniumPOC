using FluentAssertions;
using SeleniumPOC.Common;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsPreEnrollment : EmployeePortalBaseTestSuite
    {
        [TearDown]
        public void Cleanups()
        {
        }

        [Test]
        public void PreEnrollDoSelectDuringInitialOnboarding()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(10000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(50000, 2000000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

            //Logger.Step("Login as a user still in onboarding");
            Pages.LoginPage.Login("QATesting+PreEnroll@hsabank.com");

           // Logger.Step("Start Enrollment for Curated");
            Pages.OnboardingPage.GoToInvestmentsStep();
            Pages.OnboardingPage.InvestmentsPage.StartEnrollment();
            Pages.OnboardingPage.InvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

            //Logger.Step("Fill out the Required Disclosures Wizard Page");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED", sector, occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetYesNoQuestionAnswers(false, false, false);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetCitizenship("USA");
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetAnnualIncome(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.SetTotalNetWorth(netWorth);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.Next();

            //Logger.Step("Accept the Disclosure Agreements");
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.AcknowledgeAgreement();
            Pages.ManageInvestmentsPage.WizardDisclosureAgreementsPage.Next();

           // Logger.Step("Sign the Agreement if needed");
            if (Pages.ManageInvestmentsPage.WizardSignaturePage.IsSigned)
            {
               // Logger.Info("Already signed!");
            }
            else
            {
                Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature("auto signed");
                Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
                Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();
                Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document has been signed.");
                Pages.NotificationAlert.Dismiss();
            }

            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

            //Logger.Step("Verify can search for Curated on Auto-Funding page, then skip");
            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Enrollment in investment account successful.");
            Pages.NotificationAlert.Dismiss();

            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForSelectFund("BIV");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetSelectFundResult(0).Should().StartWith("BIV");
            Pages.ManageInvestmentsPage.AutoFundingPage.Skip();
        }

        [Test]
        public void PreEnrollDoSelectAccount()
        {
            string annualIncome = CommonFunctions.GenerateRandomDollarAmount(10000, 100000);
            string netWorth = CommonFunctions.GenerateRandomDollarAmount(50000, 2000000);
            string sector = InvestmentFunctions.GetRandomSector();
            string occupation = InvestmentFunctions.GetRandomOccupation();

          //  Logger.Step("Login to the Employee Portal as a user with no cash");
            Pages.LoginPage.Login("bendlocaltest+user03+0226@gmail.com");

         //   Logger.Step("Go to Manage Investments and start enrollment for curated");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.StartPreEnrollment();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();

         //   Logger.Step("Fill out the Required Disclosures Wizard Page");
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
            Pages.ManageInvestmentsPage.WizardSignaturePage.EnterSignature("auto signed");
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();
            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Document signed.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

           // Logger.Step("Verify can search for Curated on Auto-Funding page, then skip");
            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Enrollment in investment account successful.");
            Pages.NotificationAlert.Dismiss();
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForSelectFund("BIV");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetSelectFundResult(0).Should().StartWith("BIV");
            Pages.ManageInvestmentsPage.AutoFundingPage.Skip();

           // Logger.Step("Verify we’re now pre-enrolled");
            Pages.ManageInvestmentsPage.VerifyAlreadyPreEnrolledPageShown("Select");

           // Logger.Step("Choose the update option to verify the questionnaire values were saved");
            Pages.ManageInvestmentsPage.UpdatePreEnrollment();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetSelectedSector().Should().Be(sector);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetSelectedOccupation().Should().Be(occupation);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetAnnualIncome().Should().Be(annualIncome);
            Pages.ManageInvestmentsPage.WizardRequiredDisclosuresPage.GetNetWorth().Should().Be(netWorth);

            //Logger.Step("Cancel the pre-enrollment");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.CancelPreEnrollment();
            Pages.NotificationAlert.GetSuccessMessage().Should().Be("Investment Enrollment has been cancelled");

           // Logger.Step("Refresh the page and verify we can start enrollment again");
            Pages.ManageInvestmentsPage.Refresh();
            Pages.ManageInvestmentsPage.StartPreEnrollment();

            // Add test for auto-funding during pre-enrollment and verify main page text contains...
            // "Funds in excess of $4,000.00 will automatically be moved to your investments"
        }
    }
}
