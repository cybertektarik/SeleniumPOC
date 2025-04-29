using FluentAssertions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsSingleAccountTests : EmployeePortalBaseTestSuite
    {
        // These tests are for the legacy single account functionality.
        // They are no longer needed after multi-account flags are turned on

        //[Test]
        public void LegacyLimitedUserValidations()
        {
            // Logger.Step("Login to the Employee Portal as a user with limited account");
            Pages.LoginPage.Login("QATesting+LegacyLimited@hsabank.com");

            //  Logger.Step("Go to Manage Investments and verify");
            Pages.SidebarNavPage.GoToManageInvestments();
            //Pages.ManageInvestmentsPage.VerifyInvestmentAccountHeaderTextContains("(Limited self directed brokerage account)");
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");

            //Logger.Step("Verify the auto funding page launches");
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.SetupAutomatedFunding();
            Pages.ManageInvestmentsPage.AutoFundingPage.VerifyIsCurrentPage();
            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForLimitedFundLegacy("VICT");
            Pages.ManageInvestmentsPage.AutoFundingPage.Cancel();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");

            // Logger.Step("Go to the Available Investments tab");
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();

            // Logger.Step("Verify some searches");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("HTGC");
            var instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(1);
            instruments[0].Should().Be("THGC");

            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("Nvidia");
            instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(1);
            instruments[0].Should().Be("NVDA");

            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("XYZ");
            instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(0);
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.VerifyNoStocksFound();

            // Logger.Step("Go to the Activity tab and verify nothing is there yet");
            Pages.ManageInvestmentsPage.ClickActivityTab();
            List<string> settledRows = Pages.ManageInvestmentsPage.ActivityTab.GetExecutedTransactions();
            settledRows.Count.Should().Be(0);

            //  Logger.Step("Go to the Documents tab");
            Pages.ManageInvestmentsPage.ClickDocumentsTab();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Documents");

            //  Logger.Step("Go to the Preferences tab and verify");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.ChangeAccountType();
            Pages.ManageInvestmentsPage.PreferencesTab.NoIWantToKeep();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");
        }

        //[Test]
        public void LegacyCuratedUserValidations()
        {
            //  Logger.Step("Login to the Employee Portal as a user with Curated account");
            Pages.LoginPage.Login("QATesting+LegacyCurated@hsabank.com");

            //    Logger.Step("Go to Manage Investments and verify");
            Pages.SidebarNavPage.GoToManageInvestments();
            //Pages.ManageInvestmentsPage.VerifyInvestmentAccountHeaderTextContains("(Curated fund lineup account)");
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");

            // Logger.Step("Verify the auto funding page launches");
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.SetupAutomatedFunding();
            Pages.ManageInvestmentsPage.AutoFundingPage.VerifyIsCurrentPage();
            Pages.ManageInvestmentsPage.AutoFundingPage.ToggleShowAllFunds();
            Pages.ManageInvestmentsPage.AutoFundingPage.Cancel();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");

            //   Logger.Step("Go to the Available Investments tab");
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();
            var instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(6);
            instruments[0].Should().Be("AZG");

           // Logger.Step("Go to the Activity tab and verify nothing is there yet");
            Pages.ManageInvestmentsPage.ClickActivityTab();
            List<string> settledRows = Pages.ManageInvestmentsPage.ActivityTab.GetExecutedTransactions();
            settledRows.Count.Should().Be(0);

          //  Logger.Step("Go to the Documents tab");
            Pages.ManageInvestmentsPage.ClickDocumentsTab();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Documents");


            //Logger.Step("Go to the Preferences tab and verify");
            Pages.ManageInvestmentsPage.ClickPreferencesTab();
            Pages.ManageInvestmentsPage.PreferencesTab.ChangeAccountType();
            Pages.ManageInvestmentsPage.PreferencesTab.NoIWantToKeep();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");
        }
        //bendlocaltest+LegacyManaged@gmail.com
    }
}