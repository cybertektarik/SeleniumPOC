using System.Data;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class SearchAndTradePage : BasePage
    {
        private PageControl btnUnavailableToBuy = new PageControl(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='Include Unavailable To Buy']"), "include unavailable to buy");
        private PageControl btnFundType = new PageControl(By.XPath("(//*[contains(@id,'BV_toggle_')])[last()-2]"), "Fund Type)");
        private PageControl btnCompanyType = new PageControl(By.XPath("(//*[contains(@id,'BV_toggle_')])[last()-1]"), "Fund Company");
        private PageControl btnAssetClassType = new PageControl(By.XPath("(//*[contains(@id,'BV_toggle_')])[last()]"), "Asset Class");
        private PageControl btnIndexFund = new PageControl(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='Index Fund']"), "Index Fund");

        public SearchAndTradePage(IWebDriver driver) : base(driver) { }


        public void ValidateInvestmentSearchResult(string status, string tradeOption, string tradeOptionBtnStatus)
        {
            WaitForSpinners();
            // Wait until at least one investment row is visible
            var investmentRows = driver.FindElements(By.XPath("//*[@role='table']//tbody"));
            Assert.IsTrue(investmentRows.Count >= 1, "No investment rows found");

            // Check the status is Active or Unable to purchase
            var statusElement = driver.FindElement(By.XPath("//*[@role='table']//tbody//td[text()='" + status + "']"));
            Assert.AreEqual(status, statusElement.Text.Trim(), "Status is not Active");

            // Check the BUY or SELL button is enabled
            var tradeOptionBtn = driver.FindElement(By.XPath("//*[@role='table']//tbody//td//span[text()='" + tradeOption + "']"));
            var btnStatus = null as IWebElement;
            if (tradeOptionBtnStatus.Equals("enable"))
            {
                Assert.IsTrue(tradeOptionBtn.Enabled, "" + tradeOption + " button is not enabled");
            }

            else if (tradeOptionBtnStatus.Equals("disable"))
            {
                btnStatus = tradeOptionBtn.FindElement(By.XPath("//ancestor::div//a[@aria-disabled='true']"));
                Assert.IsTrue(btnStatus.GetAttribute("aria-disabled").Contains("true"), "" + tradeOption + " button is not disabled");
            }
        }
        public void ValidateOneOrMoreProductsAvailable()
        {
            // Wait until at least one investment row is visible
            WaitForSpinners();
            var investmentRows = driver.FindElements(By.XPath("//*[@role='table']//tbody//tr"));
            Assert.IsTrue(investmentRows.Count >= 1, "No investment rows found");
        }

        public void ValidateZeroProductsAvailable()
        {
            // Wait until at least one investment row is visible
            WaitForSpinners();
            var investmentRows = driver.FindElements(By.XPath("//*[@role='table']//tbody//tr"));
            Assert.IsTrue(investmentRows.Count == 0, "More than one investment rows found");
        }


        public void CheckkUnavailableToBuyButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnUnavailableToBuy.IsDisplayed(), "Include Unavailable To Buy is not displayed");
            btnUnavailableToBuy.Click();
        }

        public void UnCheckkUnavailableToBuyButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnUnavailableToBuy.IsDisplayed(), "Include Unavailable To Buy is not displayed");
            if (btnUnavailableToBuy.IsEnabled)
                btnUnavailableToBuy.Click();
        }

        public void SelectFundType(string fundType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnFundType.IsDisplayed(), "Fund Type is not displayed");
            btnFundType.Click();
            var fundTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + fundType + "']"));
            fundTypeElement.Click();
        }

        public void DeSelectFundType(string fundType)
        {
            WaitForSpinners();
            var fundTypeElement = driver.FindElement(By.XPath("//*[@class='custom-control-label'][normalize-space(text())='" + fundType + "']"));
            fundTypeElement.Click();
            btnFundType.Click();
        }

        public void SelectCompanyType(string companyType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnCompanyType.IsDisplayed(), "Company Type is not displayed");
            btnCompanyType.Click();
            var companyTypeElement = driver.FindElement(By.XPath("//*[@class='dropdown-menu dropdown-menu-fix show']//li//a[normalize-space(text())='" + companyType + "']"));
            companyTypeElement.Click();
        }

        public void DeSelectCompanyType(string companyType)
        {
            WaitForSpinners();
            var companyTypeElement = driver.FindElement(By.XPath("//*[@class='dropdown-menu dropdown-menu-fix show']//li//a[normalize-space(text())='" + companyType + "']"));
            companyTypeElement.Click();
            btnCompanyType.Click();
        }

        public void SelectAssetClassType(string assetClassType)
        {
            WaitForSpinners();
            Assert.IsTrue(btnAssetClassType.IsDisplayed(), "Asset Class Type is not displayed");
            btnAssetClassType.Click();
            var assetClassTypeElement = driver.FindElement(By.XPath("//*[@class='dropdown-menu show']//li//a[normalize-space(text())='" + assetClassType + "']"));
            assetClassTypeElement.Click();
        }

        public void DeSelectAssetClassType(string assetClassType)
        {
            WaitForSpinners();
            var assetClassTypeElement = driver.FindElement(By.XPath("//*[@class='dropdown-menu show']//li//a[normalize-space(text())='" + assetClassType + "']"));
            assetClassTypeElement.Click();
            btnAssetClassType.Click();
        }

        public void toggleIndexFund()
        {
            WaitForSpinners();
            btnIndexFund.Click();
        }
    }
}

