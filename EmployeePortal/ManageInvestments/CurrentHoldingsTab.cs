using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class CurrentHoldingsTab : BasePage
    {
        // ======================== P A G E   C O N T R O L S ========================
        private PageControl stcInvestmentAccountBalance = new PageControl(By.XPath("//p[text()='Investment Account Balance:']/following-sibling::h2"));
        private PageControl stcAvailableToInvest = new PageControl(By.XPath("//p[text()='Available to Invest']/following-sibling::h2"));
        private PageControl tableInstruments = new PageControl(By.XPath("//table[2]"));

        private PageControl btnSellInstrument(string inst) =>
            new PageControl(By.XPath($"//table[2]//tr[td[contains(., '{inst}')]]/ancestor::tr//a[contains(@class, 'btn-secondary')]"), "SELL");

        private PageControl btnSetupAutoFunding = new PageControl(By.XPath("//span[text()='Setup Automated Funding']/.."), "SETUP AUTOMATED FUNDING");
        private PageControl btnManageAutoFunding = new PageControl(By.XPath("//span[text()='Manage Automated Funding']/.."), "MANAGE AUTOMATED FUNDING");

        public CurrentHoldingsTab(IWebDriver driver) : base(driver) { }

        public void SetupAutomatedFunding()
        {
            ClickAndWaitForSpinners(btnSetupAutoFunding);
        }

        public void ManageAutomatedFunding()
        {
            ClickAndWaitForSpinners(btnManageAutoFunding);
        }

        public string GetInvestmentAccountBalance()
        {
            if (stcInvestmentAccountBalance.IsDisplayed(3))
                return stcInvestmentAccountBalance.GetText();
            else
                return null;
        }

        public string GetAvailableToInvestBalance()
        {
            return stcAvailableToInvest.GetText();
        }

        private List<string> GetInstruments()
        {
            List<string> result = new List<string>();
            var rows = tableInstruments.FindElements(By.XPath(".//tr"));

            // We're skipping the first "row" as it's the header
            for (int i = 1; i < rows.Count; i++)
            {
                result.Add(rows[i].Text.Replace(Environment.NewLine, " | "));
            }

            return result;
        }

        public string GetInstrument(string instrument)
        {
            List<string> instruments = GetInstruments();
            string foundInstrument = null;

            foreach (var instr in instruments)
            {
                if (instr.Contains(instrument))
                {
                    foundInstrument = instr;
                    break;
                }
            }

            return foundInstrument;
        }

        public void SellInstrument(string instrument)
        {
            btnSellInstrument(instrument).Click();
        }
    }
}
