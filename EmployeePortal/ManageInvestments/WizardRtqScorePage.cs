using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class WizardRqtoScorePage : BasePage
    {
        private PageControl arcxoRisk => new PageControl(By.XPath("//*[name()='polygon']"));

        private PageControl btnNoWantToReviewAgain => new PageControl(By.XPath("//button[contains(., 'No, I want to review the questions again.')]"), "No, I want to review the questions again.");
        private PageControl btnYesIWantToChoose => new PageControl(By.XPath("//button[contains(., 'Yes, I want to choose this portfolio.')]"), "Yes, I want to choose this portfolio.");
        private PageControl stcCategoryHeaders(int index) => new PageControl(By.XPath("(//h4)[" + index + "]"));
        private PageControl tableListInstruments(string category) => new PageControl(By.XPath("//table[@data-auto-test='grouped-instrument-list_" + category + "']"));

        private PageControl btnNext = new PageControl(By.XPath("//button[contains(., 'Next')]"), "Next");
        private PageControl btnPrevious = new PageControl(By.XPath("//span[text()='Previous ']"), "Previous");
        private PageControl stcRiskToleranceType = new PageControl(By.XPath("//h1[contains(., 'Your risk tolerance type is:')]"));

        public WizardRqtoScorePage(IWebDriver driver) : base(driver)
        {
        }

        public void NoWantToReviewAgain()
        {
            btnNoWantToReviewAgain.Click();
            WaitForSpinners();
        }

        public void YesIWantToChooseThisPortfolio()
        {
            WaitForElementToBeVisible(btnYesIWantToChoose);
            btnYesIWantToChoose.Click();
            WaitForSpinners();
        }

        public void Next()
        {
            btnNext.Click();
            WaitForSpinners();
        }

        public void Previous()
        {
            btnPrevious.Click();
            WaitForSpinners();
        }

        public string GetRiskToleranceType()
        {
            return stcRiskToleranceType.GetText().Replace("Your risk tolerance type is:", "").Trim();
        }

        public string GetRiskArcxoLocation()
        {
            string text = arcxoRisk.GetAttribute("transform");
            return text.Split("translate(")[1].Split(",")[0];
        }

        public List<string> GetCategories()
        {
            List<string> result = new List<string>();

            for (int i = 1; i < 10; i++)
            {
                if (stcCategoryHeaders(i).IsDisplayed())
                    result.Add(stcCategoryHeaders(i).GetText().Trim());
                else
                    break;
            }

            return result;
        }

        public List<string> GetInstruments(string category)
        {
            List<string> result = new List<string>();
            var rows = tableListInstruments(category).FindElements(By.XPath(".//tr"));

            // We're skipping the first "row" as it's the header
            for (int i = 1; i < rows.Count; i++)
            {
                result.Add(rows[i].Text.Replace(Environment.NewLine, "|"));
            }

            return result;
        }
    }
}

