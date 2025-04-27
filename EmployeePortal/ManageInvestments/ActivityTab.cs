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
    }
}

