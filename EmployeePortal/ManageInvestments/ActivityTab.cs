//using AngleSharp.Dom;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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

        public PageControl cancelButton => new(By.XPath("//*[@id='cancelButton']"), "Cancel Button");
        public PageControl cancelPopupMessage => new(By.XPath("//*[@class='modal-body']//strong"), "Cancel pop-up Message");
        public PageControl popUpCancelButton => new(By.XPath("//*[@class='modal-footer']//button[text()='Cancel']"), "Cancel Button");
        public PageControl confirmCancellationButton => new(By.XPath("//*[@class='modal-footer']//button[text()='Confirm Cancellation']"), "Confirm Cancellation Button");

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
            var cells = driver.FindElements(By.XPath("((//table[@role='table'])[last()]//tr[@tabindex='0'])[position()=1]//td"));

            string dateInitiated = cells[1].Text.Trim();
            string executedDate = cells[2].Text.Trim();
            string investment = cells[3].Text.Trim();
            string[] parts = investment.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string investmentTicker = parts.Length > 0 ? parts[0].Trim() : string.Empty;

            string type = cells[4].Text.Trim();

            string status = cells[5].Text.Trim();
            string mainStatus = status.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

            string amount = cells[6].Text.Trim();

            bool match =
                dateInitiated.Equals(expectedDateInitiated, StringComparison.OrdinalIgnoreCase) &&
                executedDate.Equals(expectedExecutedDate, StringComparison.OrdinalIgnoreCase) &&
                investmentTicker.Equals(expectedInvestment, StringComparison.OrdinalIgnoreCase) &&
                type.Equals(expectedType, StringComparison.OrdinalIgnoreCase) &&
                mainStatus.StartsWith(expectedStatus, StringComparison.OrdinalIgnoreCase) &&  // partial match allowed
                amount.Equals(expectedAmount, StringComparison.OrdinalIgnoreCase);

            if (match)
            {
                Console.WriteLine("✅ Executed transaction matched.");
                return;
            }

            throw new Exception($"❌ No matching transaction found.\n" +
                                $"Expected: {expectedDateInitiated} - {expectedExecutedDate} - {expectedInvestment} - {expectedType} - {expectedStatus} - {expectedAmount}\n" +
                                $"Actual: {dateInitiated} - {executedDate} - {investmentTicker} - {type} - {mainStatus} - {amount}");
        }

        public bool IsCancelButtonDisplayed()
        {
            WaitForSpinners();
            return cancelButton.IsDisplayed();
        }

        public void ClickCancelButton()
        {
            WaitForSpinners();
            cancelButton.Click();
        }

        public void ClickPopUpCancelButton()
        {
            WaitForSpinners();
            popUpCancelButton.Click();
        }

        public void ClickConfirmCancellationButton()
        {
            WaitForSpinners();
            confirmCancellationButton.Click();
        }

        public bool IsCancelPopUpMessageDisplayed()
        {
            WaitForSpinners();
            return cancelPopupMessage.IsDisplayed();
        }

        public void ValidatePendingTransactionRow(string expectedDateInitiated, string expectedInvestment, string expectedType, string expectedAmount)
        {
            var cells = driver.FindElements(By.XPath("(//table[@role='table'])[position()=1]//tr//td"));

            string dateInitiated = cells[0].Text.Trim();
            string investment = cells[1].Text.Trim();
            string[] parts = investment.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string investmentTicker = parts.Length > 0 ? parts[0].Trim() : string.Empty;
            string type = cells[2].Text.Trim();
            string amount = cells[3].Text.Trim();

            bool match =
                dateInitiated.Equals(expectedDateInitiated, StringComparison.OrdinalIgnoreCase) &&
                investmentTicker.Equals(expectedInvestment, StringComparison.OrdinalIgnoreCase) &&
                type.Equals(expectedType, StringComparison.OrdinalIgnoreCase) &&
                amount.Equals(expectedAmount, StringComparison.OrdinalIgnoreCase);

            if (match)
            {
                Console.WriteLine("✅ Pending transaction matched.");
                return;
            }

            throw new Exception($"❌ No matching transaction found.\n" +
                                $"Expected: {expectedDateInitiated} - {expectedInvestment} - {expectedType} - {expectedAmount}\n" +
                                $"Actual: {dateInitiated} - {investmentTicker} - {type} - {amount}");
        }

        public void CancelAllPendingTransactions()
        {
            string cancelButtonXPath = "(//table[@role='table'])[1]//tbody//tr//td//span[@id='cancelButton']";

            try
            {
                wait.Until(d => d.FindElement(By.XPath(cancelButtonXPath)));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("✅ No CANCEL buttons found. Skipping cancellation.");
                return;
            }

            while (true)
            {
                var cancelButtons = driver.FindElements(By.XPath(cancelButtonXPath))
                                          .Where(b => b.Displayed && b.Enabled)
                                          .ToList();

                if (!cancelButtons.Any())
                {
                    Console.WriteLine("✅ No more CANCEL buttons found.");
                    break;
                }

                foreach (var button in cancelButtons)
                {
                    try
                    {
                        button.Click();
                        confirmCancellationButton.Click();
                        driver.Navigate().Refresh();
                        WaitForSpinners();
                        break; // After refresh, break to refetch buttons
                    }
                    catch (StaleElementReferenceException)
                    {
                        Console.WriteLine("⚠️ Stale element detected. Re-fetching elements...");
                        break; // Re-fetch buttons in next iteration
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error: {ex.Message}");
                    }
                }
            }

            // Final check
            Assert.That(driver.FindElements(By.XPath(cancelButtonXPath)).Any(b => b.Displayed && b.Enabled), Is.False,
                         "❌ Some CANCEL buttons are still present after attempted cancellations.");

            Console.WriteLine("✅ All pending transactions successfully canceled.");
        }
    }
}

