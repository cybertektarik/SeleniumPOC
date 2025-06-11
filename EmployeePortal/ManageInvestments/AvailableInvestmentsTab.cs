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

        private PageControl btnAddStock => new PageControl(By.XPath("//*[contains(text(),'ADD')]"), "ADD");
        private PageControl btnReviewStock => new PageControl(By.XPath("//*[contains(text(),'REVIEW')]"), "REVIEW");
        private PageControl btnAcceptStock => new PageControl(By.XPath("//*[contains(text(),'ACCEPt')]"), "ACCEPT");

        private PageControl stAllocateStockSection => new PageControl(By.XPath("//div[contains(@class, 'allocation-table') or contains(text(),'Asset allocations must equal 100%')]/ancestor::div[1]"), "Allocation Section");

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

        public void clickAddStock()
        {
            btnAddStock.Click();
            WaitForSpinners();
        }

        public void clickReviewStock()
        {
            btnAddStock.Click();
            WaitForSpinners();
        }

        public void clickAcceptStock()
        {
            btnAddStock.Click();
            WaitForSpinners();
        }

        public void AllocateEquallyToAllStocks()
        {
            // Narrow down to input[type=number] under the allocation section
            var allocationInputs = driver.FindElements(By.XPath("//input[@type='number']"));

            int count = allocationInputs.Count;
            if (count == 0)
                throw new InvalidOperationException("No allocation input fields found.");

            int equalPercentage = 100 / count;
            int remainder = 100 - (equalPercentage * count);

            for (int i = 0; i < count; i++)
            {
                int allocation = equalPercentage;

                // Adjust last one to ensure total = 100%
                if (i == count - 1)
                    allocation += remainder;

                var input = allocationInputs[i];
                input.Clear();
                input.SendKeys(allocation.ToString());
            }
        }
    }
}

