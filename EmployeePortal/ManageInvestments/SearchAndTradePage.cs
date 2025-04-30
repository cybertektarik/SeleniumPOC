using System.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class SearchAndTradePage : BasePage
    {
        private PageControl btnUnavailableToBuy = new PageControl(By.XPath("//*[@class='custom-control-input']//label"), "include unavailable to buy");
        private PageControl btnFundType = new PageControl(By.XPath("//*[text()='Fund Type']");
        private PageControl btnStocks = new PageControl(By.XPath("//span[text()='Cancel']"), "Cancel");
        private PageControl btnETF = new PageControl(By.XPath("//span[text()='Confirm Sell']"), "Confirm Sell");
        private PageControl btnMutualFunds = new PageControl(By.XPath("//*[contains(@class, 'invalid-feedback')]"));
        private PageControl stcAvailableToSell = new PageControl(By.XPath("//div[@role='main']//div/div/div[contains(., 'Available to sell:')]"));
        private PageControl stcSharePrice = new PageControl(By.XPath("//table[@class='table not-too-wide']//tbody/tr/td[1]"));
        private PageControl btnSell = new PageControl(By.XPath("(//*[contains(text(),'Sell')])[last()]"), "Sell");
        private PageControl tradeButton = new PageControl(By.XPath("(//*[normalize-space(text())='Trade'])[last()]"), "TRADE Button");

        public SearchAndTradePage(IWebDriver driver) : base(driver) { }


        public void ValidateInvestmentSearchResult(string status, string tradeOption, string tradeOptionBtnStatus)
        {
            // Wait until at least one investment row is visible
            var investmentRows = driver.FindElements(By.XPath("//*[@role='table']//tbody"));
            Assert.IsTrue(investmentRows.Count >= 1, "No investment rows found");

            // Check the status is Active or Unable to purchase
            var statusElement = driver.FindElement(By.XPath("//*[@role='table']//tbody//td[text()='" + status + "']"));
            Assert.AreEqual("Active", statusElement.Text.Trim(), "Status is not Active");

            // Check the BUY or SELL button is enabled
            var tradeOptionBtn = driver.FindElement(By.XPath("//*[@role='table']//tbody//td//span[text()='" + tradeOption + "']"));
            if (tradeOptionBtnStatus.Equals("enable"))
                Assert.IsTrue(tradeOptionBtn.Enabled, "" + tradeOption + " button is not enabled");
            else Assert.IsFalse(tradeOptionBtn.Enabled, "" + tradeOption + " button is not disabled");

        }


        public void ClearSearchAndValidate(IWebDriver driver)
        {
            var searchBox = driver.FindElement(By.Id("searchBox")); // Update this with the correct ID or locator
            searchBox.Clear();

            string valueAfterClear = searchBox.GetAttribute("value");
            Assert.IsTrue(string.IsNullOrEmpty(valueAfterClear), "Search box is not empty after clearing");
        }


        public void CheckkUnavailableToBuyButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnUnavailableToBuy.IsDisplayed(), "Button Trade is not displayed");
            btnUnavailableToBuy.Click();
        }

        public void UnCheckkUnavailableToBuyButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnUnavailableToBuy.IsDisplayed(), "Button Trade is not displayed");
            if (btnUnavailableToBuy.IsSelected)
                btnUnavailableToBuy.Click();
        }

        public void SelectFundType(string fundType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Fund Type is not displayed");
            var fundTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + fundType + "']"));
            fundTypeElement.Click();
        }

        public void DeSelectFundType(string fundType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Fund Type is not displayed");
            var fundTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + fundType + "']"));
            if (fundTypeElement.Selected)
                fundTypeElement.Click();
        }

        public void SelectCompanyType(string companyType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Company Type is not displayed");
            var companyTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + companyType + "']"));
            companyTypeElement.Click();
        }

        public void DeSelectCompanyType(string companyType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Company Type is not displayed");
            var companyTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + companyType + "']"));
            if (companyTypeElement.Selected)
                companyTypeElement.Click();
        }

        public void SelectAssetClassType(string assetClassType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Asset Class Type is not displayed");
            var assetClassTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + assetClassType + "']"));
            assetClassTypeElement.Click();
        }

        public void DeSelectAssetClassType(string assetClassType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Asset Class Type is not displayed");
            var assetClassTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + assetClassType + "']"));
            if (assetClassTypeElement.Selected)
                assetClassTypeElement.Click();
        }
    }
}

