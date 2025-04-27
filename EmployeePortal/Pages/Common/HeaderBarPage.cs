using OpenQA.Selenium;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class HeaderBarPage : BasePage
    {
        public HeaderBarPage(IWebDriver driver) : base(driver)
        {
        }

        private PageControl lnkLogout = new PageControl(By.LinkText("Logout"), "Logout");

        public void Logout()
        {
            if (lnkLogout.IsDisplayed(3))
            {
                lnkLogout.Click();
                WaitForSpinners();
            }
        }
    }
}

