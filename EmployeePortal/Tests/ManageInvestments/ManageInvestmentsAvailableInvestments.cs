using FluentAssertions;
using SeleniumPOC.Common;
using System.Globalization;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsAvailableInvestments : EmployeePortalBaseTestSuite
    {
        [Test]
        public void VerifyLimitedSearchForStock()
        {
            //Logger.Step("Login to the Employee Portal as a user with limited account");
            Pages.LoginPage.Login("endolcast+limited+1121@gmail.com");

           // Logger.Step("Go to Manage Investments Available Investments tab");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();

           // Logger.Step("Search by stock symbol");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("HTGC");
            var instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(1);
            instruments[0].Should().Be("HTGC");

            //Logger.Step("Search by stock name");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("Nvidia");
            instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(1);
            instruments[0].Should().Be("NVDA");

           // Logger.Step("Search for non-existing stock symbol");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("XYZ");
            instruments = Pages.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();
            instruments.Count.Should().Be(0);
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.VerifyNoStocksFound();
        }

        [Test]
        public void VerifyLimitedBuyStockPageErrorHandling()
        {
           // Logger.Step("Login to the Employee Portal as a user with limited account");
            Pages.LoginPage.Login("endolcast+limited+1121@gmail.com");

            //Logger.Step("Go to Manage Investments Available Investments tab");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();

            //Logger.Step("Search by stock symbol and choose to BUY");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("NVDA");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.BuyStock("NVDA");

           // Logger.Step("Verify zero amount error handling on the page");
            Pages.ManageInvestmentsPage.BuyInstrumentPage.EnterAmount("0");
            Pages.ManageInvestmentsPage.BuyInstrumentPage.GetErrorText().Should().Be("The amount field must be $1.00 or more.");

           // Logger.Step("Verify too much money");
            string available = Pages.ManageInvestmentsPage.BuyInstrumentPage.GetAvailableToInvest();
            decimal amountToEnter = decimal.Parse(available, NumberStyles.Currency);
            Pages.ManageInvestmentsPage.BuyInstrumentPage.EnterAmount((amountToEnter + 10).ToString());
            Pages.ManageInvestmentsPage.BuyInstrumentPage.ClickConfirmBuy();
            Pages.ManageInvestmentsPage.BuyInstrumentPage.GetErrorText().Should().Be($"The amount field must be {available} or less.");

            Pages.ManageInvestmentsPage.BuyInstrumentPage.ClickCancel();
        }

        [Test]
        public void BuyLimitedStockEndToEndTest()
        {
            string stockToBuy = "TFLO";
            string stockName = "iShares Treasury Floating Rate Bond ETF";
            string stockToBuyAbbrv = CommonFunctions.GenerateRandomDollarAmount(1, 5);

            // This user's account is set to "IgnoreMarketHoursForTest" on DW
            // if (!CommonFunctions.IsWithinStandardTradingHours())
            //     Assert.Fail("Not within trading hours!");

            Assert.Ignore("This test won't work on Feature2 anymore because of DW callbacks not hooked up");

           // Logger.Step("Login to the Employee Portal as a user with limited account");
            Pages.LoginPage.Login("QATestingLimitedUser@hsabank.com");
            Sleep(3); // wait a sec for balance to load

           // Logger.Step("Get the current cash balance from the Cash Account page");
            Pages.SidebarNavPage.GoToCashAccount();
            string beginningCashBalance = Pages.CashAccountPage.GetAvailableAccountBalance();
           // Logger.Info("Starting cash balance: " + beginningCashBalance);

          //  Logger.Step("Go to Manage Investments Available Investments tab");
            Pages.SidebarNavPage.GoToManageInvestments();
            string beginningInvestmentBalance = Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetInvestmentAccountBalance();
         //   Logger.Info("Starting investment balance: " + beginningInvestmentBalance);
            Pages.ManageInvestmentsPage.ClickAvailableInvestmentsTab();

           // Logger.Step("Search for the stock and Buy one share of it");
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock(stockToBuy);
            Pages.ManageInvestmentsPage.AvailableInvestmentsTab.BuyStock(stockToBuy);
            Pages.ManageInvestmentsPage.BuyInstrumentPage.EnterAmount("1");
            Pages.ManageInvestmentsPage.BuyInstrumentPage.ClickConfirmBuy();
            Pages.NotificationAlert.GetSuccessMessage().Should().Contain($"Purchase of 1 {stockToBuy} sent");
            Pages.NotificationAlert.Dismiss();

           // Logger.Step("Go to the Activity tab to verify we see the transaction as Pending, then Executed");
            Pages.ManageInvestmentsPage.ClickActivityTab();
            List<string> pendingRows = Pages.ManageInvestmentsPage.ActivityTab.GetPendingTransactions();
            pendingRows[0].Should().Contain(stockToBuy);
            Pages.ManageInvestmentsPage.ActivityTab.WaitForTransactionToExecute(stockToBuy, "Buy", "1");
            List<string> executedRows = Pages.ManageInvestmentsPage.ActivityTab.GetExecutedTransactions();
            executedRows[0].Should().Contain(stockToBuy);
            Pages.ManageInvestmentsPage.ActivityTab.ExpandExecutedTransaction(1);
            string transactionDetails = Pages.ManageInvestmentsPage.ActivityTab.GetTransactionDetails(1);
            transactionDetails.Should().Contain("Buy of 1 " + stockToBuy + " Transaction involved");

            // Additional assertions on dashboard and investment balance follow...
        }
    }
}

