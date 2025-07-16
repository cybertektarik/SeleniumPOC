using OpenQA.Selenium;
using SeleniumPOC.Common;

namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class NotificationAlert : BasePage
    {
        private PageControl stcSuccessMessage => new PageControl(By.XPath("//div[@class='notification alert alert-success']//p"), "Manage Investments");
        private PageControl stcErrorMessage(int index) => new PageControl(By.XPath("//div[@class='notification alert alert-danger']//p[" + index + "]"), "Manage Investments");
        private PageControl btnDismiss(int index) => new PageControl(By.XPath("//div[@class='notification-container']//button[text()='Dismiss'][" + index + "]"), "Dismiss");

        //New Code
        public PageControl notificationIcon => new(By.XPath("//*[normalize-space()='Confirm Cancellation']"), "Notification Button");
        public PageControl notificationList => new(By.XPath("//*[normalize-space()='Confirm Cancellation']"), "Notification Button");


        public NotificationAlert(IWebDriver driver) : base(driver)
        {
        }

        public string GetSuccessMessage()
        {
            return stcSuccessMessage.GetText();
        }

        public string WaitAndGetSuccessMessage(int maxTimeToWait)
        {
            if (stcSuccessMessage.IsDisplayed(maxTimeToWait))
                return stcSuccessMessage.GetText();
            else
                return null;
        }

        public string GetErrorMessage()
        {
            return stcErrorMessage(1).GetText();
        }

        public string GetErrorMessage(int secsToWait)
        {
            if (stcErrorMessage(1).IsDisplayed(secsToWait))
                return stcErrorMessage(1).GetText();
            else
                return null;
        }

        public void Dismiss()
        {
            btnDismiss(1).Click();
        }

        public void ClickNotificationButton()
        {
            WaitForSpinners();
            notificationIcon.Click();
        }

        public string? GetTopNotificationText() => notificationList.GetText()?.Trim();
    }
}

