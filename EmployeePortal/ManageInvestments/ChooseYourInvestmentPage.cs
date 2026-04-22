using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class ChooseYourInvestmentPage : BasePage
    {
        public ChooseYourInvestmentPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        //private PageControl btnManaged => new PageControl(By.XPath("//h4[text()= 'Managed']//ancestor::a"), "Managed");
        //private PageControl btnManaged => new PageControl(By.XPath("//h4[contains(text(),'Managed')]"), "Managed");
        //private PageControl btnSelect => new PageControl(By.XPath("//h4[contains(text(),'Select')]//ancestor::a"), "Select");
        //private PageControl btnChoice => new PageControl(By.XPath("//h4[contains(text(),'Choice')]//ancestor::a"), "Choice");

        private PageControl btnManaged => new PageControl(By.XPath("(//a[.//*[contains(normalize-space(.), 'Managed')]] | //button[contains(normalize-space(.), 'Managed')])[1]"), "Managed");
        private PageControl btnSelect => new PageControl(By.XPath("(//a[.//*[contains(normalize-space(.), 'Select')]] | //button[contains(normalize-space(.), 'Select')])[1]"), "Select");
        private PageControl btnChoice => new PageControl(By.XPath("(//a[.//*[contains(normalize-space(.), 'Choice')]] | //button[contains(normalize-space(.), 'Choice')])[1]"), "Choice");

        private PageControl stcSelectButtonBalance => new PageControl(By.XPath("//h4[text()='Select']//ancestor::a//strong"), "Select button balance");
        private PageControl stcChoiceButtonBalance => new PageControl(By.XPath("//h4[text()='Choice']//ancestor::a//strong"), "Choice button balance");

        private PageControl inkLearnMore => new PageControl(By.LinkText("Learn More"), "Learn More");
        private PageControl inkLearnMoreManaged => new PageControl(By.XPath("//*[text()='Learn more.']"), "Learn more managed");
        private PageControl btnReturn => new PageControl(By.XPath("//span[text()='Return']"), "Return");

        private PageControl txtSearchStocksAndFunds => new PageControl(By.XPath("//input[@placeholder='Stock symbol or name of company or fund']"), "Stock Search");

        private PageControl stcRowFundSymbol(int itemIndex) => new PageControl(By.XPath("//table//tbody//tr//td[2][" + itemIndex + "]"));
        private PageControl stcRowFundType(int itemIndex) => new PageControl(By.XPath("//table//tbody//tr[" + itemIndex + "]//td[3]"));
        private PageControl inkFundsAvailableSelect => new PageControl(By.XPath("//a[contains(text(),'funds available') and contains(text(), 'Select option')]"), "See all funds available in the Select option");
        private PageControl inkFundsAvailableChoice => new PageControl(By.XPath("//a[contains(text(),'funds available') and contains(text(), 'Choice option')]"), "See all funds available in the Choice option");
        private PageControl btnClose => new PageControl(By.XPath("//button[text()='Close']"), "Close");


        private PageControl SelectModalMessage => new PageControl(By.XPath("//div[contains(@class,'modal-body')]"), "Select's Modal Dialog Message");

        public void VerifyIsVisible()
        {
            VerifyIsVisible(true, true, true);
        }

        public void VerifyIsVisible(bool managed, bool Select, bool Choice)
        {
            if (managed)
                btnManaged.VerifyIsVisible();
            if (Select)
                btnSelect.VerifyIsVisible();
            if (Choice)
                btnChoice.VerifyIsVisible();
        }

        public void ClickOnInvestmentAccountType(string accountType)
        {
            WaitForSpinners();

            if (string.IsNullOrWhiteSpace(accountType))
                throw new ArgumentException("Account type cannot be null/empty.", nameof(accountType));

            // Debug: log the available account tiles/text to help diagnose env differences
            try
            {
                var headers = driver.FindElements(By.XPath("//h4"));
                Console.WriteLine($"Account tiles found (h4 count): {headers.Count}");
                foreach (var h in headers.Take(10))
                {
                    var t = (h.Text ?? string.Empty).Trim();
                    if (!string.IsNullOrWhiteSpace(t))
                        Console.WriteLine(" - tile: " + t);
                }
            }
            catch { /* best-effort debug only */ }

            // Strategy A: use the existing PageControl (fast path)
            PageControl? buttonToClick =
                accountType.Equals("Managed", StringComparison.OrdinalIgnoreCase) ? btnManaged :
                accountType.Equals("Select", StringComparison.OrdinalIgnoreCase) ? btnSelect :
                accountType.Equals("Choice", StringComparison.OrdinalIgnoreCase) ? btnChoice :
                null;

            if (buttonToClick != null)
            {
                try
                {
                    WaitForElementToBeClickable(buttonToClick, timeoutSeconds: 30);
                    buttonToClick.Click();
                    WaitForSpinners();
                    return;
                }
                catch
                {
                    // fall through to broader locator below
                }
            }

            // Strategy B: broader locator (handles UI text not inside h4 / different markup)
            string text = accountType.Trim();
            var clickableXPath =
                $"(//a[contains(normalize-space(.), '{text}')] | //button[contains(normalize-space(.), '{text}')] | " +
                $"//*[@role='button' and contains(normalize-space(.), '{text}')])[1]";

            var fallback = new PageControl(By.XPath(clickableXPath), $"{accountType} (fallback)");
            WaitForElementToBeClickable(fallback, timeoutSeconds: 30);
            fallback.Click();
            WaitForSpinners();
        }

        public void VerifyAllButtonsEnabled()
        {
            VerifyButtonsEnabled(true, true, true);
        }

        public void VerifyButtonsEnabled(bool managed, bool select, bool choice)
        {
            if (managed)
                btnManaged.GetClass().Should().NotContain("disabled");
            if (select)
                btnSelect.GetClass().Should().NotContain("disabled");
            if (choice)
                btnChoice.GetClass().Should().NotContain("disabled");
        }

        public void VerifyLearnMoreLinkIsDisplayed()
        {
            Thread.Sleep(5000);
            WaitForElementToBeVisible(inkLearnMore);
            inkLearnMore.VerifyIsVisible();
        }

        public void VerifyAllInvestmentAccountLinksVisible()
        {
            WaitForElementToBeVisible(inkFundsAvailableSelect);
            inkFundsAvailableSelect.VerifyIsVisible();
            WaitForElementToBeVisible(inkFundsAvailableChoice);
            inkFundsAvailableChoice.VerifyIsVisible();
            WaitForElementToBeVisible(inkLearnMoreManaged);
            inkLearnMoreManaged.VerifyIsVisible();
        }

        public string VerifySelectModalMessageIsDisplayed()
        {
            WaitForSpinners();
            Thread.Sleep(2000);
            WaitForElementToBeVisible(SelectModalMessage);
            SelectModalMessage.VerifyIsVisible();
            return SelectModalMessage.GetText();
        }

        public void VerifyAccountTypeHyperlinksExist()
        {
            WaitForElementToBeVisible(inkFundsAvailableChoice);
            inkFundsAvailableChoice.VerifyIsVisible();
            WaitForElementToBeVisible(inkFundsAvailableSelect);
            inkFundsAvailableSelect.VerifyIsVisible();
            WaitForElementToBeVisible(inkLearnMoreManaged);
            inkLearnMoreManaged.VerifyIsVisible();
        }

        public void ChooseSelectFund()
        {
            ClickAndWaitForSpinners(btnSelect);
        }

        public void ChooseChoice()
        {
            ClickAndWaitForSpinners(btnChoice);
        }

        public void LearnMore()
        {
            WaitForElementToBeVisible(inkLearnMore);
            inkLearnMore.Click();
            Thread.Sleep(3000);
        }

        public void CloseWindowsPopup()
        {
            try
            {
                // Wait for spinners to disappear
                WaitForSpinners();
                // Wait until the Close button is visible
                WaitForElementToBeVisible(btnClose);
                Thread.Sleep(1000); // Short wait
                try
                {
                    // Try clicking the Close button
                    btnClose.Click();
                }
                catch
                {
                    // If normal click fails, try clicking with JavaScript
                    var jsExecutor = (IJavaScriptExecutor)driver;
                    jsExecutor.ExecuteScript("arguments[0].click();", driver.FindElement(By.XPath("//button[text()='Close']")));
                }

                // Verify that the modal has closed
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => !btnClose.IsDisplayed());
                WaitForSpinners();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Modal close error: {ex.Message}");
                throw;
            }
        }

        public void Return()
        {
            btnReturn.Click();
            WaitForSpinners();
        }

        public void SeeFundsAvailableInSelectOption()
        {
            inkFundsAvailableSelect.Click();
            WaitForSpinners();
        }

        public void SeeFundsAvailableInChoiceOption()
        {
            inkFundsAvailableChoice.Click();
        }

        public void SearchStocksAndFunds(string search)
        {
            txtSearchStocksAndFunds.SendKeys(search);
            WaitForSpinners();
        }

        public void VerifyAllBuyButtonsDisabled()
        {
            var allBuyButtons = PageControl.FindAllByXPath(driver, "//table//tbody//tr//a[contains(text(),'Buy')]");
            bool allButtonsDisabled = allBuyButtons.All(button => button.IsDisplayed() && !button.IsEnabled);
            allButtonsDisabled.Should().BeTrue("because all 'Buy' buttons should be disabled.");
        }

        public void VerifySearchBoxIsDisplayed()
        {
            txtSearchStocksAndFunds.VerifyIsVisible();
        }

        public void VerifyMatchingFundsDisplayed(string fundSymbol)
        {
            var matchFound = Enumerable.Range(1, 50)
                .Any(i => stcRowFundSymbol(i).IsDisplayed() && stcRowFundSymbol(i).GetText().Contains(fundSymbol));
            matchFound.Should().BeTrue($"because at least one fund should display '{fundSymbol}'");
        }

        public List<string> GetSelectListFunds()
        {
            List<string> funds = new List<string>();

            for (int i = 1; i <= 50; i++)
            {
                if (stcRowFundSymbol(i).IsDisplayed())
                    funds.Add(stcRowFundSymbol(i).GetText() + " (" + stcRowFundType(i).GetText() + ")");
                else
                    break;
            }

            return funds;
        }

        public string GetSelectButtonBalance()
        {
            return stcSelectButtonBalance.GetText().Trim();
        }

        public string GetChoiceButtonBalance()
        {
            return stcChoiceButtonBalance.GetText().Trim();
        }

        public void LearnMoreManaged()
        {
            WaitForElementToBeVisible(inkLearnMoreManaged);
            inkLearnMoreManaged.Click();
            Thread.Sleep(3000);
        }
    }
}

