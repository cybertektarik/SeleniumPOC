using OpenQA.Selenium;
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
    }
}

