using FluentAssertions;
using SeleniumPOC.Common;
using System.Globalization;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Parallelizable(ParallelScope.Self)]
    public class ManageInvestmentsCurrentHoldings : EmployeePortalBaseTestSuite
    {
        [Test]
        public void VerifyCurrentHoldingsSetupAutoFunding()
        {
            //Logger.Step("Login as a user with a single investment account");
            Pages.LoginPage.Login("bendolcattest+user03#0123@gmail.com");

           // Logger.Step("Go to Manage Investments and launch the setup form auto-funding");
            Pages.SidebarNavPage.GoToManageInvestments();
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.SetupAutomatedFunding();
            Pages.ManageInvestmentsPage.AutoFundingPage.VerifyIsCurrentPage();

           // Logger.Step("Verify some error scenarios on the page");
            Pages.ManageInvestmentsPage.AutoFundingPage.SetThreshold("999");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetThresholdError().Should().Be("The threshold field must be $1,000.00 or more.");

            Pages.ManageInvestmentsPage.AutoFundingPage.SetThreshold("10000");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetThresholdError().Should().BeNull();

            Pages.ManageInvestmentsPage.AutoFundingPage.SearchForSelectFund("XYZ");
            Pages.ManageInvestmentsPage.AutoFundingPage.GetSelectFundResult(0).Should().Be("No stocks or funds found");

            Pages.ManageInvestmentsPage.AutoFundingPage.IsReviewButtonEnabled().Should().BeFalse();

            //Logger.Step("Cancel the auto-funding setup and verify we're back on the correct tab");
            Pages.ManageInvestmentsPage.AutoFundingPage.Cancel();
            Pages.ManageInvestmentsPage.GetCurrentlySelectedTab().Should().Be("Current Holdings");
        }

        [Test]
        public void SellLimitedStockEndToEndTest()
        {
            string stockToSell = "TFLO";
            string stockName = "iShares Treasury Floating Rate Bond ETF";
            string amountToSell = CommonFunctions.GenerateRandomDollarAmount(1, 5);

            Assert.Ignore("This test won’t work on Feature2 anymore because of DW callbacks not hooked up");

            if (!CommonFunctions.IsWithinStandardTradingHours())
                Assert.Fail("Not within trading hours!");

            //Logger.Step("Login to the Employee Portal as a user with limited account");
            Pages.LoginPage.Login("QATesting+LimitedTrader@hsabank.com");

           // Logger.Step("Get the current cash balance from the Dashboard page");
            Sleep(5);
            string beginningCashBalance = Pages.DashboardPage.AccountBalanceCard.GetAccountCashBalance();
          //  Logger.Info("Starting cash balance: " + beginningCashBalance);

          //  Logger.Step("Go to Manage Investments Current Holdings tab");
            Pages.SidebarNavPage.GoToManageInvestments();
            string instrumentRowBefore = Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetInstrument(stockToSell);
            string stockPriceBefore = GetStockPrice(instrumentRowBefore);
          //  Logger.Info("Starting investment balance: " + instrumentRowBefore);

           // Logger.Step("Search for the stock and sell it - amount: " + amountToSell);
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.SellInstrument(stockToSell);
            Pages.ManageInvestmentsPage.SellInstrumentPage.EnterAmount(amountToSell);
            Pages.ManageInvestmentsPage.SellInstrumentPage.ClickConfirmSell();
            Pages.NotificationAlert.GetSuccessMessage().Should().Contain("Sale of " + amountToSell + " " + stockToSell + " sent");
            Pages.NotificationAlert.Dismiss();

           // Logger.Step("Go to the Activity tab to verify we see the transaction as Pending, then Settled");
            Pages.ManageInvestmentsPage.ClickActivityTab();
            List<string> pendingRows = Pages.ManageInvestmentsPage.ActivityTab.GetPendingTransactions();

            if (pendingRows.Count > 0)
                pendingRows[0].Should().Contain(stockToSell + " Sell " + amountToSell);

            bool settled = Pages.ManageInvestmentsPage.ActivityTab.WaitForTransactionToExecute(stockToSell, "Sell", amountToSell);
            List<string> settledRows = Pages.ManageInvestmentsPage.ActivityTab.GetExecutedTransactions();
            settledRows[0].Should().Contain(stockToSell + " Sell " + amountToSell);

            Pages.ManageInvestmentsPage.ActivityTab.ExpandExecutedTransaction(1);
            string transactionDetails = Pages.ManageInvestmentsPage.ActivityTab.GetTransactionDetails(1);
            transactionDetails.Should().Contain("Sell order for " + stockToSell + " Transaction involved");

           // Logger.Step("Dashboard - Verify Available Cash Account balance increased");
            string expectedCashBalance = CommonFunctions.ConvertDecimalToCurrencyString(CommonFunctions.ConvertCurrencyToDecimal(beginningCashBalance) + CommonFunctions.ConvertCurrencyToDecimal(amountToSell));

            if (Pages.DashboardPage.AccountBalanceCard.GetAccountCashBalance() != expectedCashBalance)
            {
                Sleep(10);
                Pages.SidebarNavPage.GoToCashAccount();
                Pages.SidebarNavPage.GoToDashboard();
            }

            Pages.DashboardPage.AccountBalanceCard.GetAccountCashBalance().Should().Be(expectedCashBalance);

           // Logger.Step("Manage Investments - Verify balances");
            string expectedAvailableToInvest = CommonFunctions.ConvertDecimalToCurrencyString(CommonFunctions.ConvertCurrencyToDecimal(expectedCashBalance) - 1000);
            string expectedInvestmentBalance = CommonFunctions.ConvertDecimalToCurrencyString(CommonFunctions.ConvertCurrencyToDecimal(beginningCashBalance));
            Pages.SidebarNavPage.GoToManageInvestments();

            WaitForStockPrice(stockToSell, stockPriceBefore);
            string actualInvestmentBalance = Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetInvestmentAccountBalance();
            CommonFunctions.FuzzyCurrencyCompare(expectedInvestmentBalance, actualInvestmentBalance, (decimal)0.50).Should().BeTrue();
            Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetAvailableToInvestBalance().Should().Be(expectedAvailableToInvest);

          //  Logger.Step("Manage Investments - Verify the instrument row has updated");
            string instrumentRowCurrent = Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetInstrument(stockToSell);
            string expectedInstrumentBalance = CommonFunctions.ConvertDecimalToCurrencyString(CommonFunctions.ConvertCurrencyToDecimal(instrumentRowBefore.Split('|')[5]) - CommonFunctions.ConvertCurrencyToDecimal(amountToSell));
            CommonFunctions.FuzzyCurrencyCompare(expectedInstrumentBalance, instrumentRowCurrent.Split('|')[5], (decimal)0.50).Should().BeTrue();

            Console.WriteLine("Before: " + instrumentRowBefore);
            Console.WriteLine("After: " + instrumentRowCurrent);

           // Logger.Step("Dashboard - Verify Investment Balance decreased");
            Pages.SidebarNavPage.GoToDashboard();
            Pages.DashboardPage.AccountBalanceCard.ClickInvestmentTab();
            Pages.DashboardPage.AccountBalanceCard.GetInvestmentBalance().Should().Be(expectedInstrumentBalance);

           // Logger.Step("Cash Account - Verify balance and new transaction activity row added");
            Pages.SidebarNavPage.GoToCashAccount();
            Pages.CashAccountPage.Refresh();
            Pages.CashAccountPage.GetAvailableAccountBalance().Should().Be(expectedCashBalance);

            string expectedRow = "|SELL " + stockToSell + " |INVESTMENT| " + amountToSell + " |Complete|";
            string actualRow = Pages.CashAccountPage.WaitForAndGetTransactionRowByPattern(expectedRow);
            actualRow.Should().EndWith(expectedRow);
        }

        private void WaitForStockPrice(string stock, string price)
        {
            for (int i = 0; i < 25; i++)
            {
                string instrumentRow = Pages.ManageInvestmentsPage.CurrentHoldingsTab.GetInstrument(stock);
                string stockPrice = GetStockPrice(instrumentRow);

                if (stockPrice == price)
                    break;

                Sleep(3);
                Pages.ManageInvestmentsPage.CurrentHoldingsTab.Refresh();
            }
        }

        private string GetStockPrice(string instRow)
        {
            return instRow.Split('|')[4];
        }
    }
}

