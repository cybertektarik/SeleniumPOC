using OpenQA.Selenium;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class LoginPage : BasePage
    {
        private PageControl txtUserName => new PageControl(By.Id("idp-discovery-username"), "Username");
        private PageControl txtPassword => new PageControl(By.Id("okta-signin-password"), "password");
        private PageControl btnNext => new PageControl(By.Id("idp-discovery-submit"), "Next");
        private PageControl lnkSignIn => new PageControl(By.Id("okta-signin-submit"), "Sign in");
        public IWebElement userName => driver.FindElement(By.Id("idp-discovery-username"));

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void Login(string userName, string password)
        {
            txtUserName.SendKeys(userName);
            ClickAndWaitForSpinners(btnNext);

            WaitForElementToBeVisible(txtPassword);
            txtPassword.SendPasswordKeys(password);
            ClickAndWaitForSpinners(lnkSignIn);
        }

        public void Login(string userName)
        {
            Login(userName, "$BetterHsa777");
        }
    }
}

