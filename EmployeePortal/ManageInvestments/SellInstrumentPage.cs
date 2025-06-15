using NUnit.Framework;
using OpenQA.Selenium;
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
        private PageControl btnSell = new PageControl(By.XPath("(//*[contains(text(),'Sell')])[last()]"), "Sell");
        private PageControl tradeButton = new PageControl(By.XPath("(//*[normalize-space(text())='Trade'])[last()]"), "TRADE Button");

        public SellInstrumentPage(IWebDriver driver) : base(driver) { }

        public void ClickTradeButton()
        {
            WaitForSpinners();
            Assert.That(tradeButton.IsDisplayed(), Is.True, "Button Trade is not displayed");
            tradeButton.SendKeysUsingActions(Keys.End);
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
            Assert.That(btnSell.IsDisplayed(), Is.True, "Button Sell is not displayed");
            btnSell.Click();
        }
    }
}

