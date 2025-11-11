using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class SellInstrumentPage : BasePage
    {
        private PageControl chkSellAll = new PageControl(By.XPath("//label[contains(., 'Sell all')]"), "Sell all");
        private PageControl txtEnterAmount = new PageControl(By.XPath("//*[@data-vv-name='amount']//input"), "Enter Amount");
        private PageControl btnCancel = new PageControl(By.XPath("//span[text()='Cancel']"), "Cancel");
        private PageControl btnConfirmSell = new PageControl(By.XPath("//span[text()='Confirm Sell']"), "Confirm Sell");
        private PageControl txtErrorText = new PageControl(By.XPath("//*[contains(@class, 'invalid-feedback')]"));
        private PageControl stcAvailableToSell = new PageControl(By.XPath("//div[@role='main']//div/div/div[contains(., 'Available to sell:')]"));
        private PageControl stcSharePrice = new PageControl(By.XPath("//table[@class='table not-too-wide']//tbody/tr/td[1]"));
        private PageControl btnSell = new PageControl(By.XPath("//ul[contains(@class,'dropdown-menu') and contains(@class,'show')]//a[normalize-space()='Sell']"), "Sell");
        private PageControl tradeButton = new PageControl(By.XPath("(//*[normalize-space(text())='Trade'])[last()]"), "TRADE Button");

        public SellInstrumentPage(IWebDriver driver) : base(driver) { }

        /* public void ClickTradeButton()
         {
             WaitForSpinners();
             Assert.IsTrue(tradeButton.IsDisplayed(), "Button Trade is not displayed");
             tradeButton.SendKeysUsingActions(Keys.End);
         }*/

        public void ClickTradeBtnSpecific(string stockName)
        {
            var tradeButtonLocator = By.XPath(
                $"//tr[.//text()[normalize-space()='{stockName}']]//button[normalize-space()='Trade']"
            );

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(tradeButtonLocator));
            tradeButton.Click();
        }

        public void VerifyOnTradeBtnBuyAndSellBtn()
        {
        }

        public void ClickCancel()
        {
            btnCancel.Click();
            Thread.Sleep(2000);
        }

        public void ClickConfirmSell()
        {
            Console.WriteLine("Share price: " + stcSharePrice.GetText());
            btnConfirmSell.Click();
            WaitForSpinners();
        }

        public string GetErrorText()
        {
            return txtErrorText.GetText().Trim();
        }

        public string GetAvailableToInvest()
        {
            return stcAvailableToSell.GetText().Replace("Available to sell: ", "").Trim();
        }

        public void EnterAmount(string amount)
        {
            WaitForSpinners();
            txtEnterAmount.Clear();
            txtEnterAmount.SendKeys(amount);
        }

        public string GetAmount()
        {
            return txtEnterAmount.GetValue();
        }

        public bool IsConfirmSellButtonEnabled()
        {
            return btnConfirmSell.IsEnabled;
        }

        public void ClickSellAll()
        {
            chkSellAll.Click();
        }

        public void ClickSellButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Button Sell is not displayed");
            btnSell.Click();
        }

        public bool IsTradeButtonDisplayed()
        {
            WaitForSpinners();
            return tradeButton.IsDisplayed();
        }
    }
}

