using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class AutoFundingPage : BasePage
    {
        private PageControl stcAutomatedInvestmentFunding = new PageControl(By.XPath("//h4[contains(text(),'Automated Investment Funding')]"));
        private PageControl txtActivate = new PageControl(By.XPath("//h4[contains(text(),'Set investment funding threshold')]//..//input"), "ACTIVATE");
        private PageControl txtThreshold = new PageControl(By.XPath("//h4[contains(text(),'Set investment funding threshold')]//..//input"), "Threshold");
        private PageControl stcThresholdError = new PageControl(By.XPath("//h4[contains(text(),'Set investment funding threshold')]//..//div[contains(@class,'invalid-feedback')]"), "Threshold");

        private PageControl txtSearchForSelect = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Select option')]//..//input[@type='text']"), "Stock symbol");
        private PageControl tableRowSelectResults = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Select option')]//..//table/tbody/tr"));

        private PageControl txtSearchForChoice = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//input[@type='text']"), "Stock symbol");
        private PageControl tableRowChoiceResults = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table/tbody/tr"));

        private PageControl btnAddLimitedResults(string symbol) => new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table//tbody//tr[contains(., '" + symbol + "')]//button"), "Add + " + symbol);

        private PageControl txtPercentAllocation(string symbol) => new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table//tbody//tr[contains(., '" + symbol + "')]//input"), "Percent");

        private PageControl stcCategoryHeaders(int index) => new PageControl(By.XPath("//div[@data-auto-test='GetInstrumentListAssetCategory(instrumentList)'][" + index + "]"));

        private PageControl btnCancel => new PageControl(By.XPath("//span[text()='Cancel']"), "Cancel");
        private PageControl btnReview => new PageControl(By.XPath("//span[text()='Review']"), "Review");
        private PageControl btnAccept => new PageControl(By.XPath("//span[text()='ACCEPT']"), "ACCEPT");

         private PageControl btnSkip => new PageControl(By.XPath("//span[text()='Skip']"), "Skip");
        private PageControl txtSearchForLimitedLegacy => new PageControl(By.XPath("//h4[contains(text(),'Select Investments')]/..//input[@type='text']"), "Stock symbol");
        private PageControl switchShowAllFunds => new PageControl(By.XPath("//div[@class='custom-control' custom-switch"), "Show all funds");
        public AutoFundingPage(IWebDriver driver) : base(driver) { }

        public void VerifyIsCurrentPage()
        {
            stcAutomatedInvestmentFunding.VerifyIsVisible();
        }

        public void SetThreshold(string threshold)
        {
            txtThreshold.SendKeys(threshold);
        }

        public string GetThreshold()
        {
            return txtThreshold.GetValue();
        }

        public string GetThresholdError()
        {
            if (stcThresholdError.IsDisplayed())
                return stcThresholdError.GetText().Trim();
            else
                return null;
        }

        public void SearchForSelectFund(string searchFor)
        {
            txtSearchForSelect.SetText(searchFor);
            WaitForSpinners();
        }

        public string GetSelectFundResult(int index)
        {
            return tableRowSelectResults.GetText(index);
        }

        public void SearchForChoiceFund(string searchFor)
        {
            txtSearchForChoice.SetText(searchFor);
            WaitForSpinners();
        }

        public void SearchForLimitedFundLegacy(string searchFor)
        {
            txtSearchForLimitedLegacy.SetText(searchFor);
            WaitForSpinners();
        }

        public void AddLimitedStockInstrument(string symbol)
        {
            btnAddLimitedResults(symbol).Click();
            WaitForSpinners();
        }

        public void SetPercentPurchaseAllocation(string symbol, string allocation)
        {
            txtPercentAllocation(symbol).SetText(allocation);
            WaitForSpinners();
        }

        public string GetPercentPurchaseAllocation(string symbol)
        {
            return txtPercentAllocation(symbol).GetValue();
        }

        public void Cancel()
        {
            btnCancel.Click();
            WaitForSpinners();
        }

        public void Skip()
        {
            btnSkip.Click();
        }

        public void Review()
        {
            btnReview.Click();
        }

        public void Accept()
        {
            btnAccept.Click();
            WaitForSpinners();
        }

        public bool IsReviewButtonEnabled()
        {
            return btnReview.IsEnabled;
        }

        public void ToggleShowAllFunds()
        {
            switchShowAllFunds.Click();
            WaitForSpinners();
        }

        public List<string> GetSelectCategories()
        {
            List<string> result = new List<string>();

            for (int i = 1; i <= 10; i++)
            {
                if (stcCategoryHeaders(i).IsDisplayed())
                    result.Add(stcCategoryHeaders(i).GetText().Trim());
                else
                    break;
            }

            return result;
        }
    }
}

