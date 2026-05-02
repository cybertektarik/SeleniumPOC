using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class BuyInstrumentPage : BasePage
    {
        private PageControl txtEnterAmount = new PageControl(With.PermId("buy-enter-amount-input"), "Enter Amount");
        private PageControl btnCancel = new PageControl(With.PermId("cancel-buy-btn"), "Cancel");
        private PageControl btnConfirmBuy = new PageControl(With.PermId("confirm-buy-btn"), "Confirm Buy");
        private PageControl stcErrorText = new PageControl(By.XPath("//div[contains(@class, 'invalid-feedback')]"));
        private PageControl stcAvailableToInvest = new PageControl(With.PermId("available-to-invest-label"));
        private PageControl btnBuy = new PageControl(With.PermId("buy-option"), "Buy");
        private PageControl btnSearchBuy => new PageControl(By.XPath("//a[.//span[normalize-space()='Buy']]"), "Search BUY from Choice");

        private PageControl btnByAmount = new PageControl(With.PermId("by-amount-radio"));
        private PageControl btnByShare = new PageControl(With.PermId("by-share-radio"));
        private PageControl txtAvailableToInvest = new PageControl(With.PermId("available-to-invest-label"));
        private PageControl txtAvailableToSell = new PageControl(By.XPath("//*[contains(text(),'Available to sell')]"));
        private PageControl txtEnterShares = new PageControl(With.PermId("buy-enter-shares-input"));


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
        public void ClickSearchBuyButton()
        {
            WaitForSpinners();
            Assert.That(btnSearchBuy.IsDisplayed(), Is.True, "Search BUY button is not displayed");
            btnSearchBuy.Click();
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

        public string GetAvailableToBuyText()
        {
            WaitForSpinners();
            return stcAvailableToInvest.GetText().Trim();
        }

        public double GetAvailableToBuyAmount()
        {
            WaitForSpinners();
            return CommonFunctions.ExtractNumberFromText(stcAvailableToInvest.GetText());
        }

        public bool IsConfirmBuyButtonEnabled()
        {
            WaitForSpinners();
            // Find the button that contains the "Confirm Buy" span
            // Check if button is disabled via disabled attribute or disabled class
            try
            {
                var button = driver.FindElement(With.PermId("confirm-buy-btn"));
                bool hasDisabledAttribute = button.GetAttribute("disabled") != null;
                string classAttribute = button.GetAttribute("class") ?? "";
                bool hasDisabledClass = classAttribute.Contains("disabled", StringComparison.OrdinalIgnoreCase);
                
                // Button is disabled if it has disabled attribute OR disabled class
                bool isDisabled = hasDisabledAttribute || hasDisabledClass;
                return !isDisabled; // Return enabled state (opposite of disabled)
            }
            catch (NoSuchElementException)
            {
                // If button not found, try the original method
                return btnConfirmBuy.IsEnabled;
            }
        }
    }
}

