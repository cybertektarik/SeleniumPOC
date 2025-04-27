using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Dashboard
{
    public class AccountBalanceCard : BasePage
    {
        private PageControl TabToSelect(string tabName) => new PageControl(By.XPath("//div[@data-cy='AccountBalance']//ul/li[contains(text(),'" + tabName + "')]"), tabName);
        private PageControl stcCashBalance => new PageControl(By.XPath("//h2[@class='cash-amount']"));
        private PageControl stcInvestmentBalance => new PageControl(By.XPath("//h2[@class='investment-amount']"));

        public AccountBalanceCard(IWebDriver driver) : base(driver)
        {
        }

        public string GetAccountCashBalance()
        {
            return stcCashBalance.GetText();
        }

        public void ClickInvestmentTab()
        {
            TabToSelect("Investment").Click();
            WaitForSpinners();
        }

        public string GetInvestmentBalance()
        {
            return stcInvestmentBalance.GetText();
        }
    }
}

