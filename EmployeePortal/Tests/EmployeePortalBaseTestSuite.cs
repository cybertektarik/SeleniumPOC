using NUnit.Framework.Interfaces;
using SeleniumPOC.Common;
using NUnit.Framework;

namespace SeleniumPOC.EmployeePortal.Tests
{
    public class EmployeePortalBaseTestSuite : BaseTestSuite
    {
        [TearDown]
        public void Finished()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
                TakeScreenshot();

            //Logger.Step("Log out of the Employee Portal");
            Pages.HeaderBarPage.Logout();
        }

        protected List<string> GetAndDismissAllErrors()
        {
            List<string> messages = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                string errorMessage = Pages.NotificationAlert.GetErrorMessage();

                if (errorMessage != null)
                {
                    messages.Add(errorMessage);
                    Pages.NotificationAlert.Dismiss();
                }
                else
                    break;
            }

            return messages;
        }

        protected void GoToOktaLoginPage()
        {
            GoToUrl(DEFAULT_URL + "#/auth/login");
            Sleep(2);
        }
    }
}

