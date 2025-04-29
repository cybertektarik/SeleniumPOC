using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentUserTests : EmployeePortalBaseTestSuite
    {
        [Test]
        public void VerifyHasCashButNotEnrolledInvestmentAccount()
        {
           // Logger.Step("Login to the Employee Portal");
            Pages.LoginPage.Login("bendlocatest+notenrolled+0102@gmail.com"); // QATesting+NotYetEnrolled@hsabank.com

           // Logger.Step("Go to Manage Investments for a user who can sign up for DW");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyEnrollPageShown();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyIsVisible();

           // Logger.Step("Verify Learn More");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.LearnMore();

          //  Logger.Step("Select verify");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.SeeFundsAvailableInSelectOption();
            List<string> funds = Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.GetSelectListFunds();
            funds.Count.Should().Be(6);
            funds[0].Should().Be("AGZ (ETF)");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.Return();
        }

        [Test]
        public void VerifyHsabUserHasCashButNotEnrolled()
        {
           // Logger.Step("Login to the OKTA Employee Portal");
            GoToOktaLoginPage();
            Pages.OktaLoginPage.Login("Feature2HSABTester002");

          //  Logger.Step("Go to Manage Investments for a user who can sign up for DW");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyEnrollPageShown();
            Pages.ManageInvestmentsPage.ChooseEnroll();

          //  Logger.Step("Accept the Esign agreement");
            Pages.ManageInvestmentsPage.WizardSignaturePage.CheckAcknowledge();
            Pages.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Sign();
            Pages.ManageInvestmentsPage.WizardSignaturePage.Next();

          //  Logger.Step("Verify the account type tabs are visible");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyIsVisible(false, true, true);

          //  Logger.Step("Verify Learn More link works");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.LearnMore();

         //   Logger.Step("Verify Learn More about Select");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.SeeFundsAvailableInSelectOption();
            List<string> funds = Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.GetSelectListFunds();
            funds.Count.Should().BeGreaterThan(20);
            funds.Should().Contain("DDXIX (Mutual Fund)");
            funds.Should().Contain("CEMIX (Mutual Fund)");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.Return();
        }

        [Test]
        public void VerifyHsabUserMultiEnrolledInvestmentAccounts()
        {
          //  Logger.Step("Login to the OKTA Employee Portal");
            GoToOktaLoginPage();
            Pages.OktaLoginPage.Login("Feature2HSABTester003");

         //   Logger.Step("Go to Manage Investments to grab balances for DW and DevenirSchwab accounts");
            Pages.SidebarNavPage.GoToManageInvestments();
            string totalDrivewealthBalance = Pages.ManageInvestmentsPage.GetHsaInvestButtonBalance();
            string totalDevenirSchwabBalance = Pages.ManageInvestmentsPage.GetDevenirOrSchwabButtonBalance();
            string expectedInvestmentBalance = CommonFunctions.ConvertDecimalToCurrencyString(
                CommonFunctions.ConvertCurrencyToDecimal(totalDrivewealthBalance) +
                CommonFunctions.ConvertCurrencyToDecimal(totalDevenirSchwabBalance)
            );

         //   Logger.Step("Go to Dashboard to verify Investment Balance matches");
            Pages.SidebarNavPage.GoToSummary();
            Pages.DashboardPage.AccountBalanceCard.ClickInvestmentTab();
            string actualInvestmentBalance = Pages.DashboardPage.AccountBalanceCard.GetInvestmentBalance();
            CommonFunctions.FuzzyCurrencyCompare(expectedInvestmentBalance, actualInvestmentBalance, (decimal)1.00).Should().BeTrue();

         //   Logger.Step("Verify the Chooser tabs add up to DW individual account tab totals");
            Pages.SidebarNavPage.GoToManageInvestments();
            totalDrivewealthBalance = Pages.ManageInvestmentsPage.GetHsaInvestButtonBalance();
            string selectBalance = Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.GetSelectButtonBalance();
            string choiceBalance = Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.GetChoiceButtonBalance();
            string expectedDrivewealthBalance = CommonFunctions.ConvertDecimalToCurrencyString(
                CommonFunctions.ConvertCurrencyToDecimal(selectBalance) +
                CommonFunctions.ConvertCurrencyToDecimal(choiceBalance)
            );
            CommonFunctions.FuzzyCurrencyCompare(expectedDrivewealthBalance, totalDrivewealthBalance, (decimal)1.00).Should().BeTrue();

           // Logger.Step("Go to Select account and verify HSA Invest doc");
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.ViewHSAInvestTerms();
        }

        [Test]
        public void VerifyNoCashButPreEnrolledInvestmentAccount()
        {
           // Logger.Step("Login to the Employee Portal");
            Pages.LoginPage.Login("bendlocatest+preenrolled+0102@gmail.com");

           // Logger.Step("Go to Manage Investments for a user who is already pre-enrolled");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyAlreadyPreEnrolledPageShown("Select");

          //  Logger.Step("Go away and back to Manage Investments for a user who is already pre-enrolled");
            Pages.SidebarNavPage.GoToSettings();
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyAlreadyPreEnrolledPageShown("Select");
        }

        [Test]
        public void VerifyLowCashOffersPreEnrollOption()
        {
            Pages.LoginPage.Login("bendlocatest+nocash+0105@gmail.com");

            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.VerifyStartPreEnrollOptionShown("You may enroll even before you meet the $1,000.00 cash balance threshold.");
        }

        [Test]
        public void VerifyMultiEnrolledInvestmentAccount()
        {
         //   Logger.Step("Login to the Employee Portal as a user with multiple DW investment accounts");
            Pages.LoginPage.Login("bendlocatest+bothinvest+11210@gmail.com");

        //    Logger.Step("Go to the manage investments page for the Select fund");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseSelectFund();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.GetAccountTypeFromCollapseButton().Should().Be("Select option");

          //  Logger.Step("Go to the manage investments page for the Choice fund");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ChooseYourInvestmentPage.ChooseChoice();
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.GetAccountTypeFromCollapseButton().Should().Be("Choice option");
        }

        [Test]
        public void VerifySingleEnrolledInvestmentAccount()
        {
            Pages.LoginPage.Login("bendlocatest+limited+11210@gmail.com");

            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");

            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.GetAccountTypeFromCollapseButton().Should().Be("Choice option");
        }
    }
}
