using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class AutoFundingPage : BasePage
    {
        private PageControl stcAutomatedInvestmentFunding = new PageControl(By.XPath("//h4[contains(text(),'Automated Investment Funding')]"));

        private PageControl txtThreshold = new PageControl(By.XPath("//h4[contains(text(),'Set investment funding threshold')]//..//input"), "Threshold");
        private PageControl stcThresholdError = new PageControl(By.XPath("//h4[contains(text(),'Set investment funding threshold')]//..//div[contains(@class,'invalid-feedback')]"), "Threshold");

        private PageControl txtSearchForSelect = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Select option')]//..//input[@type='text']"), "Stock symbol");
        private PageControl tableRowSelectResults = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Select option')]//..//table/tbody/tr"));

        private PageControl txtSearchForChoice = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//input[@type='text']"), "Stock symbol");
        private PageControl tableRowChoiceResults = new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table/tbody/tr"));

        private PageControl btnAddLimitedResults(string symbol) => new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table//tbody//tr[contains(., '" + symbol + "')]//button"), "Add + " + symbol);

        private PageControl txtPercentAllocation(string symbol) => new PageControl(By.XPath("//h4[contains(text(),'Choose investments for Choice option')]//..//table//tbody//tr[contains(., '" + symbol + "')]//input"), "Percent");

        private PageControl stcCategoryHeaders(int index) => new PageControl(By.XPath("//div[@data-auto-test='GetInstrumentListAssetCategory(instrumentList)'][" + index + "]"));

        private PageControl btnCancelTop => new PageControl(By.XPath("(//span[text()='Cancel'])[last()-1]"), "Cancel");
        private PageControl btnReview => new PageControl(By.XPath("//span[text()='Review']"), "REVIEW");
        private PageControl btnAccept => new PageControl(By.XPath("//span[text()='Accept']"), "ACCEPT");

        private PageControl btnSkip => new PageControl(By.XPath("(//button[span[text()='Skip']])[2]"), "Skip");
        private PageControl txtSearchForLimitedLegacy => new PageControl(By.XPath("//h4[contains(text(),'Select Investments')]/..//input[@type='text']"), "Stock symbol");
        private PageControl switchShowAllFunds => new PageControl(By.XPath("//div[@class='custom-control' custom-switch"), "Show all funds");

        //New Code
        private PageControl lnkViewPerformanceData => new PageControl(By.XPath("//*[contains(text(),'View Performance Data')]"));
        private PageControl stSetupAutomatedInvestment = new PageControl(By.XPath("//*[contains(text(),'Setup Automated Investing')]"));
        private PageControl stManageAutomatedInvestment = new PageControl(By.XPath("//*[contains(text(),'Manage Automated Investing')]"));
        private PageControl btnSuspend => new PageControl(By.XPath("//span[text()='Suspend']"), "SUSPEND");
        private PageControl btnActivate = new PageControl(By.XPath("//span[text()='Activate']"), "ACTIVATE");
        private PageControl txtCashBalanceFunds = new PageControl(By.XPath("//*[contains(text(),'Cash balance funds in excess')]"));
        private PageControl btnEdit = new PageControl(By.XPath("//span[text()='Edit']"), "EDIT");

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
            btnCancelTop.Click();
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

        //New code
        public IList<IWebElement> GetViewPerformanceDataLinks()
        {
            WaitForSpinners();
            return lnkViewPerformanceData.FindElements();
        }

        public void ValidateViewPerformanceDataOptions(Table table)
        {
            var links = GetViewPerformanceDataLinks();

            Assert.That(links.Count, Is.GreaterThan(0), "No 'View Performance Data' links found.");
            for (int i = 0; i < links.Count; i++)
            {
                // Re-locate to avoid stale references
                links = GetViewPerformanceDataLinks();
                var link = links[i];

                wait.Until(_ => link.Displayed && link.Enabled);
                link.Click();

                // Verify URL contains "instrument-performance"
                VerifyCurrentUrlIncludes("instrument-performance");

                // Validate Trade, Buy, Sell, Auto Funding
                foreach (var row in table.Rows)
                {
                    var buttonText = row[0];
                    var optionButton = wait.Until(d => d.FindElement(By.XPath($"//*[normalize-space(text())='{buttonText}']")));
                    if (optionButton.Text.Contains("TRADE"))
                        optionButton.Click();
                    Assert.That(optionButton.Displayed, Is.True, $"'{buttonText}' button is not visible.");
                }

                // Go back and wait for page to reload
                driver.Navigate().Back();
                wait.Until(d => GetViewPerformanceDataLinks().Count > 0);
            }
        }

        public void ClickOnManageAutomatedInvestment()
        {
            stManageAutomatedInvestment.Click();
            WaitForSpinners();
        }

        public void ClickOnSetupAutomatedInvestment()
        {
            stSetupAutomatedInvestment.Click();
            WaitForSpinners();
        }

        public bool IsManageAutomatedInvestmentDisplayed()
        {
            WaitForSpinners();
            return stManageAutomatedInvestment.IsDisplayed();
        }

        public bool IsSetupAutomatedInvestmentDisplayed()
        {
            WaitForSpinners();
            return stSetupAutomatedInvestment.IsDisplayed();
        }

        public void ClickOnActivate()
        {
            btnActivate.Click();
            WaitForSpinners();
        }

        public void ClickOnSuspend()
        {
            btnSuspend.Click();
            WaitForSpinners();
        }

        public string getTextAboveInvestmentList()
        {
            WaitForSpinners();
            return txtCashBalanceFunds.GetText().Trim();
        }

        public bool IsButtonCancelTopDisplayed()
        {
            WaitForSpinners();
            return btnCancelTop.IsDisplayed();
        }

    }
}

