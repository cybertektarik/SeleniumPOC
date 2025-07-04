using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class BuyInstrumentPage : BasePage
    {
        private PageControl txtEnterAmount = new PageControl(By.XPath("//*[@data-vv-name='amount']//input"), "Enter Amount");
        private PageControl btnCancel = new PageControl(By.XPath("//span[text()='Cancel']/.."), "Cancel");
        private PageControl btnConfirmBuy = new PageControl(By.XPath("//span[text()='Confirm Buy']"), "Confirm Buy");
        private PageControl stcErrorText = new PageControl(By.XPath("//div[contains(@class, 'invalid-feedback')]"));
        private PageControl stcAvailableToInvest = new PageControl(By.XPath("//div[@role='main']//div/div/div[contains(., 'Available to invest:')]"));
        private PageControl btnBuy = new PageControl(By.XPath("(//*[contains(text(),'Buy')])[last()]"), "BUY");

        private PageControl btnByAmount = new PageControl(By.XPath("//*[contains(text(),'By Amount')]"));
        private PageControl btnByShare = new PageControl(By.XPath("//*[contains(text(),'By Share')]"));
        private PageControl txtAvailableToInvest = new PageControl(By.XPath("//*[contains(text(),'Available to invest')]"));
        private PageControl txtAvailableToSell = new PageControl(By.XPath("//*[contains(text(),'Available to sell')]"));
        private PageControl txtEnterShares = new PageControl(By.XPath("//*[@data-vv-name='shares']"));


        public BuyInstrumentPage(IWebDriver driver) : base(driver)
        {
        }

        public void ClickCancel()
        {
            btnCancel.Click();
        }

        public void ClickBuyButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnBuy.IsDisplayed(), "Button Buy is not displayed");
            btnBuy.Click();
        }

        public void ClickConfirmBuy()
        {
            btnConfirmBuy.Click();
            WaitForSpinners();
        }

        public string GetErrorText()
        {
            return stcErrorText.GetText().Trim();
        }

        public string GetAvailableToInvest()
        {
            return stcAvailableToInvest.GetText().Replace("Available to invest:", "").Trim();
        }

        public void EnterAmount(string amount)
        {
            txtEnterAmount.Clear();
            txtEnterAmount.SendKeys(amount);
        }

        public bool IsByAmountRadioButtonVisible()
        {
            WaitForSpinners();
            return btnByAmount.IsDisplayed();
        }

        public bool IsByShareRadioButtonVisible()
        {
            WaitForSpinners();
            return btnByShare.IsDisplayed();
        }

        public double GetAvailableToSellAmount()
        {
            WaitForSpinners();
            return CommonFunctions.ExtractNumberFromText(txtAvailableToSell.GetText());
        }

        public double GetAvailableToInvestAmount()
        {
            WaitForSpinners();
            return CommonFunctions.ExtractNumberFromText(txtAvailableToInvest.GetText());
        }

        public void SelectByAmount()
        {
            WaitForSpinners();
            btnByAmount.Click();
        }

        public void SelectByShare()
        {
            WaitForSpinners();
            btnByShare.Click();
        }

        public void EnterNumberOfShares(string shareCount)
        {
            WaitForSpinners();
            txtEnterShares.Clear();
            txtEnterShares.SendKeys(shareCount);
        }
    }
}

