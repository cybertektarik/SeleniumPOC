using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class SellInstrumentPage : BasePage
    {
        private PageControl chkSellAll = new PageControl(By.XPath("//label[contains(., 'Sell all')]"), "Sell all");
        private PageControl txtEnterAmount = new PageControl(With.PermId("sell-enter-amount-input"), "Enter Amount");
        private PageControl btnCancel = new PageControl(With.PermId("cancel-sell-btn"), "Cancel");
        private PageControl btnConfirmSell = new PageControl(With.PermId("confirm-sell-btn"), "Confirm Sell");
        private PageControl txtErrorText = new PageControl(By.XPath("//*[contains(@class, 'invalid-feedback')]"));
        private PageControl stcAvailableToSell = new PageControl(With.PermId("available-to-sell-label"));
        private PageControl stcSharePrice = new PageControl(By.XPath("//table[@class='table not-too-wide']//tbody/tr/td[1]"));
        private PageControl btnSell = new PageControl(With.PermId("sell-option"), "Sell");
        private PageControl tradeButton = new PageControl(With.PermId("trade-btn"), "TRADE Button");

        public SellInstrumentPage(IWebDriver driver) : base(driver) { }

        public void ClickTradeButton()
        {
            WaitForSpinners();
            Assert.IsTrue(tradeButton.IsDisplayed(), "Button Trade is not displayed");
            tradeButton.Click();
        }

        public void ClickTradeBtnSpecific(string stockName)
        {
            WaitForSpinners();
            Console.WriteLine($"Attempting to click Trade button for stock: {stockName}");
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement tradeButton = null;
            string lastError = "";
            
            // Try multiple XPath patterns to find the Trade button for the specific stock
            // Pattern 1: Exact match in table row - button element with text "Trade"
            try
            {
                var locator1 = By.XPath($"//tr[.//text()[normalize-space()='{stockName}']]//button[normalize-space()='Trade']");
                tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator1));
                Console.WriteLine($"Found Trade button using Pattern 1 for stock: {stockName}");
            }
            catch (Exception ex)
            {
                lastError = $"Pattern 1 failed: {ex.Message}";
                Console.WriteLine(lastError);
                
                // Pattern 2: More flexible - any element with "Trade" text in row containing stock name
                try
                {
                    var locator2 = By.XPath($"//tr[contains(., '{stockName}')]//*[normalize-space()='Trade' and (self::button or self::a)]");
                    tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator2));
                    Console.WriteLine($"Found Trade button using Pattern 2 for stock: {stockName}");
                }
                catch (Exception ex2)
                {
                    lastError = $"Pattern 2 failed: {ex2.Message}";
                    Console.WriteLine(lastError);
                    
                    // Pattern 3: Even more flexible - contains match with role button
                    try
                    {
                        var locator3 = By.XPath($"//tr[contains(., '{stockName}')]//*[normalize-space()='Trade' and (self::button or self::a or @role='button')]");
                        tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator3));
                        Console.WriteLine($"Found Trade button using Pattern 3 for stock: {stockName}");
                    }
                    catch (Exception ex3)
                    {
                        lastError = $"Pattern 3 failed: {ex3.Message}";
                        Console.WriteLine(lastError);
                        
                        // Pattern 4: Look in table[2] specifically (Current Holdings table)
                        try
                        {
                            var locator4 = By.XPath($"//table[2]//tr[contains(., '{stockName}')]//*[normalize-space()='Trade']");
                            tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator4));
                            Console.WriteLine($"Found Trade button using Pattern 4 (table[2]) for stock: {stockName}");
                        }
                        catch (Exception ex4)
                        {
                            lastError = $"Pattern 4 failed: {ex4.Message}";
                            Console.WriteLine(lastError);
                            
                            // Pattern 5: Case-insensitive search
                            try
                            {
                                var locator5 = By.XPath($"//tr[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), translate('{stockName}', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))]//*[normalize-space(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))='trade']");
                                tradeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator5));
                                Console.WriteLine($"Found Trade button using Pattern 5 (case-insensitive) for stock: {stockName}");
                            }
                            catch (Exception ex5)
                            {
                                lastError = $"All patterns failed. Last error: {ex5.Message}";
                                Console.WriteLine(lastError);
                            }
                        }
                    }
                }
            }
            
            if (tradeButton != null)
            {
                // Scroll into view if needed
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", tradeButton);
                    Thread.Sleep(500); // Brief pause after scroll
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Scroll failed: {ex.Message}");
                }
                
                // Try JavaScript click if regular click fails
                try
                {
                    tradeButton.Click();
                    Console.WriteLine($"Successfully clicked Trade button for stock: {stockName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Regular click failed, trying JavaScript click: {ex.Message}");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", tradeButton);
                    Console.WriteLine($"Successfully clicked Trade button using JavaScript for stock: {stockName}");
                }
                
                WaitForSpinners();
            }
            else
            {
                // Log available stocks for debugging
                try
                {
                    var allRows = driver.FindElements(By.XPath("//table[2]//tr"));
                    Console.WriteLine($"Available table rows: {allRows.Count}");
                    for (int i = 0; i < Math.Min(allRows.Count, 10); i++)
                    {
                        Console.WriteLine($"Row {i} text: {allRows[i].Text}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not log table contents: {ex.Message}");
                }
                
                throw new NoSuchElementException($"Trade button for stock '{stockName}' not found. {lastError}");
            }
        }

        public void VerifyOnTradeBtnBuyAndSellBtn()
        {
        }

        public void ClickCancel()
        {
            btnCancel.Click();
            Thread.Sleep(2000);
        }

        public void ClickConfirmSell()
        {
            Console.WriteLine("Share price: " + stcSharePrice.GetText());
            btnConfirmSell.Click();
            WaitForSpinners();
        }

        public string GetErrorText()
        {
            return txtErrorText.GetText().Trim();
        }

        public string GetAvailableToInvest()
        {
            return stcAvailableToSell.GetText().Replace("Available to sell: ", "").Trim();
        }

        public string GetAvailableToSellText()
        {
            WaitForSpinners();
            return stcAvailableToSell.GetText().Trim();
        }

        public double GetAvailableToSellAmount()
        {
            WaitForSpinners();
            return CommonFunctions.ExtractNumberFromText(stcAvailableToSell.GetText());
        }

        public void EnterAmount(string amount)
        {
            WaitForSpinners();
            txtEnterAmount.Clear();
            txtEnterAmount.SendKeys(amount);
        }

        public string GetAmount()
        {
            return txtEnterAmount.GetValue();
        }

        public bool IsConfirmSellButtonEnabled()
        {
            WaitForSpinners();
            // Find the button that contains the "Confirm Sell" span
            // Check if button is disabled via disabled attribute or disabled class
            try
            {
                var button = driver.FindElement(With.PermId("confirm-sell-btn"));
                bool hasDisabledAttribute = button.GetAttribute("disabled") != null;
                string classAttribute = button.GetAttribute("class") ?? "";
                bool hasDisabledClass = classAttribute.Contains("disabled", StringComparison.OrdinalIgnoreCase);
                
                // Button is disabled if it has disabled attribute OR disabled class
                bool isDisabled = hasDisabledAttribute || hasDisabledClass;
                return !isDisabled; // Return enabled state (opposite of disabled)
            }
            catch (NoSuchElementException)
            {
                // If button not found, try the original method
                return btnConfirmSell.IsEnabled;
            }
        }

        public void ClickSellAll()
        {
            chkSellAll.Click();
        }

        public void ClickSellButton()
        {
            WaitForSpinners();
            Assert.IsTrue(btnSell.IsDisplayed(), "Button Sell is not displayed");
            btnSell.Click();
        }

        public bool IsTradeButtonDisplayed()
        {
            WaitForSpinners();
            return tradeButton.IsDisplayed();
        }
    }
}

