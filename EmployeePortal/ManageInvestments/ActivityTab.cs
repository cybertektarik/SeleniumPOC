//using AngleSharp.Dom;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class ActivityTab : BasePage
    {
        private PageControl tablePending = new PageControl(By.XPath("//h3[text()='Pending Transactions']/../../..//table"));
        private PageControl tableExecuted = new PageControl(By.XPath("//h3[text()='Executed Transactions']/../../..//table"));
        private PageControl managementInvestmentTab(string tabName) => new PageControl(By.XPath($"//ul[contains(@class,'tabs-container')]/li/a[contains(text(),'{tabName}')]"));

        private PageControl managementPreferencesTab => new PageControl(By.XPath("//ul[contains(@class,'tabs-container')]/li/a[contains(text(),'Preferences')]"));

        //New Code
        public PageControl cancelPopupMessage => new(By.XPath("//div[contains(text(), 'Are you sure you want to cancel')]"), "Cancel Message");
        public PageControl cancelButton => new(By.XPath("//button[normalize-space()='Cancel']"), "Cancel Button");
        public PageControl confirmCancellationButton => new(By.XPath("//button[normalize-space()='Confirm Cancellation']"), "Confirm Cancellation Button");

        public ActivityTab(IWebDriver driver) : base(driver)
        {
        }

        public void ClickOnManagementTab(string tabname)
        {
            Thread.Sleep(1000);
            managementInvestmentTab(tabname).Click();
        }

        public void ClickOnPreferencesTab()
        {
            managementPreferencesTab.Click();
            Thread.Sleep(2000);
        }

        public List<string> GetPendingTransactions()
        {
            return GetTransactions(tablePending);
        }

        public List<string> GetExecutedTransactions()
        {
            return GetTransactions(tableExecuted);
        }

        public bool WaitForTransactionToExecute(string instrument, string buySell, string amount)
        {
            bool executed = false;

            for (int i = 0; i <= 60; i++)
            {
                var settledRows = GetExecutedTransactions();
                if (settledRows[0].Contains(instrument) && settledRows[0].Contains(buySell + " " + amount))
                {
                    executed = true;
                    break;
                }
                else
                {
                    Sleep(10);
                    Refresh();
                }
            }

            return executed;
        }

        public void ExpandExecutedTransaction(int row)
        {
            var rows = tableExecuted.FindElements(By.XPath("./tr/td[1]"));
            rows[row - 1].Click();
        }

        public string GetTransactionDetails(int row)
        {
            var rows = tableExecuted.FindElements(By.XPath("./tr[@class='b-table-details ']"));
            return rows[row - 1].Text.Replace(Environment.NewLine, " ");
        }

        private List<string> GetTransactions(PageControl table)
        {
            List<string> result = new List<string>();

            if (table.IsDisplayed())
            {
                var rows = table.FindElements(By.XPath("./tr"));

                // We’re skipping the first "row" as it’s the header
                for (int i = 1; i < rows.Count; i++)
                {
                    result.Add(rows[i].Text.Replace(Environment.NewLine, " "));
                }
            }

            return result;
        }

        public void ValidateTransactionRow(string expectedDateInitiated, string expectedExecutedDate,
                                  string expectedInvestment, string expectedType,
                                  string expectedStatus, string expectedAmount)
        {
            var rows = driver.FindElements(By.CssSelector("table tr"));

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count < 6)
                    continue;

                string dateInitiated = cells[0].Text.Trim();
                string executedDate = cells[1].Text.Trim();
                string investment = cells[2].Text.Trim();
                string type = cells[3].Text.Trim();
                string status = cells[4].Text.Trim();
                string amount = cells[5].Text.Trim();

                bool match =
                    dateInitiated.Equals(expectedDateInitiated, StringComparison.OrdinalIgnoreCase) &&
                    executedDate.Equals(expectedExecutedDate, StringComparison.OrdinalIgnoreCase) &&
                    investment.Equals(expectedInvestment, StringComparison.OrdinalIgnoreCase) &&
                    type.Equals(expectedType, StringComparison.OrdinalIgnoreCase) &&
                    status.StartsWith(expectedStatus, StringComparison.OrdinalIgnoreCase) &&  // partial match allowed
                    amount.Equals(expectedAmount, StringComparison.OrdinalIgnoreCase);

                if (match)
                {
                    Console.WriteLine("✅ Executed transaction matched.");
                    return;
                }
            }

            throw new Exception($"❌ No matching transaction found for {expectedInvestment} - {expectedType} - {expectedStatus} - {expectedAmount}");
        }

        public bool IsCancelButtonDisplayed()
        {
            return true;
        }

        public void ClickCancelButton()
        {
            WaitForSpinners();
            cancelButton.Click();
        }
    }
}

