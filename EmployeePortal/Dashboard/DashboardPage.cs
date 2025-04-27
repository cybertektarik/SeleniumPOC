using OpenQA.Selenium;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Dashboard
{
    public class DashboardPage : BasePage
    {
        public AccountBalanceCard AccountBalanceCard;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            AccountBalanceCard = new AccountBalanceCard(driver);
        }
    }
}

