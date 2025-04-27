using OpenQA.Selenium;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class OktaLoginPage : BasePage
    {
        private PageControl txtUsername => new PageControl(By.Id("idp-discovery-username"), "Username");
        private PageControl txtPassword => new PageControl(By.Id("okta-signin-password"), "Password");
        private PageControl btnNext => new PageControl(By.Id("idp-discovery-submit"), "Next");
        private PageControl btnSignIn => new PageControl(By.Id("okta-signin-submit"), "Sign in");
        private PageControl loginSpinner => new PageControl(By.XPath("//*[contains(@class, 'beacon-loading')]"));

        public OktaLoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void Login(string username, string password)
        {
            txtUsername.SetText(username);
            Sleep(1); // Okta can be slower
            btnNext.Click();
            WaitForLoginSpinners();
            WaitForSpinners();

            txtPassword.SetText(password);
            btnSignIn.Click();
            WaitForLoginSpinners();
            WaitForSpinners();
        }

        public void Login(string email)
        {
            Login(email, "$BetterHas3777");
        }

        private void WaitForLoginSpinners()
        {
            int count = 0;

            for (int i = 0; i < 7500; i++)
            {
                if (loginSpinner.Count(1) > 0)
                    count++;
                else
                    break;
            }

        }
    }
}

