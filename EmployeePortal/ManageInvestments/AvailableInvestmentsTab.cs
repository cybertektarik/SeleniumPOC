using OpenQA.Selenium;
using NUnit.Framework;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class AvailableInvestmentsTab : BasePage
    {
        private PageControl txtSearchField => new PageControl(By.XPath("//input[@placeholder='Stock symbol or name of company or fund']"), "Stock Search");
        private PageControl stcRowInstrumentSymbol(int itemIndex) => new PageControl(By.XPath("//table/tbody/tr/td[@title='Additional Information'][" + itemIndex + "]"));
        private PageControl stcNoStocksFound => new PageControl(By.XPath("//table//h3[text()='No stocks or funds found']"), "No stocks or funds found");
        private PageControl btnBuyStock(string stockSymbol) => new PageControl(By.XPath("//table//tbody//tr//a[text()='" + stockSymbol + "']/ancestor::tr//a[contains(@class, 'btn-primary')]"));
        private By lnkFundSymbolLocator(string fundSymbol) => By.XPath($"(//table//a[normalize-space()='{fundSymbol.Trim()}'] | //a[normalize-space()='{fundSymbol.Trim()}'])[1]");
        private By backButtonLocator => By.XPath("(//button[normalize-space()='Back'] | //a[normalize-space()='Back'] | //*[@role='button' and normalize-space()='Back'])[1]");

        public AvailableInvestmentsTab(IWebDriver driver) : base(driver)
        {
        }

        public void searchForStock(string searchText)
        {
            txtSearchField.Clear();
            txtSearchField.SendKeys(searchText);
            WaitForSpinners();
        }

        public List<string> GetInstrumentList()
        {
            List<string> instruments = new List<string>();

            for (int i = 1; i <= 10; i++)
            {
                if (stcRowInstrumentSymbol(i).IsDisplayed())
                    instruments.Add(stcRowInstrumentSymbol(i).GetText());
                else
                    break;
            }

            return instruments;
        }

        public void VerifyNoStocksFound()
        {
            stcNoStocksFound.VerifyIsVisible();
        }

        public void BuyStock(string stockSymbol)
        {
            btnBuyStock(stockSymbol).Click();
            WaitForSpinners();
        }

        public void clearStock()
        {
            WaitForSpinners();
            txtSearchField.Clear();
        }

        public void ClickEtfFundLink(string fundSymbol)
        {
            WaitForSpinners();
            var locator = lnkFundSymbolLocator(fundSymbol);
            var element = WaitForElementToBeClickable(locator, timeoutSeconds: 30);
            element.Click();
            WaitForSpinners();
        }

        public void ValidateNavigatingToEtfFundPage(string fundSymbol)
        {
            WaitForSpinners();
            string currentUrl = driver.Url;
            Assert.That(currentUrl, Does.Contain("/instrument-performance/"));
            Assert.That(currentUrl, Does.EndWith($"/{fundSymbol.Trim()}"));
        }

        public void VerifySearchAndTradeIsVisible()
        {
            WaitForSpinners();
            txtSearchField.VerifyIsVisible();
        }

        public void ValidateBackButtonDisplays()
        {
            WaitForSpinners();
            WaitForElementToBeVisible(backButtonLocator, timeoutSeconds: 15);
        }
   
    }
}

