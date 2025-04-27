using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.CashAccount
{
    public class CashAccountPage : BasePage
    {
        private PageControl stcAvailableAccountBalance = new PageControl(By.XPath("//h3[text()='Available Account Balance']/following-sibling::h2"));
        private PageControl tableRow(int index) => new PageControl(By.XPath("(//div[contains(@class,'event-list-wrap')])[" + index + "]"));

        public CashAccountPage(IWebDriver driver) : base(driver)
        {
        }

        public string GetAvailableAccountBalance()
        {
            return stcAvailableAccountBalance.GetText();
        }

        public string GetTransactionRow(int index)
        {
            return tableRow(index).GetText().Replace(Environment.NewLine, " | ");
        }

        public string WaitForAndGetTransactionRowByPattern(string pattern)
        {
            for (int i = 0; i < 20; i++)
            {
                string row = GetTransactionRow(i);
                if (row.Contains(pattern))
                    return row;

                Sleep(5);
                Refresh();
            }

            return null;
        }
    }
}

