using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPOC.EmployeePortal.Pages.Common;

namespace SeleniumPOC.EmployeePortal.Pages.ManageInvestments
{
    public class InstrumentPerformancePage : BasePage
    {
        private By chartContainerLocator =>
            By.XPath("(//*[contains(@class,'box') and contains(@class,'shadow')] | //*[@id='highcharts-container'] | //canvas | //*[name()='svg'])[1]");

        private By backButtonLocator =>
            By.XPath("(//button[normalize-space()='Back'] | //a[normalize-space()='Back'] | //*[@role='button' and normalize-space()='Back'])[1]");

        /// <summary>Chart caption like "04/22/2026 - 04/29/2026" under the performance chart (see DOM: div.col.text-center &gt; p &gt; strong).</summary>
        private static readonly By ChartDateRangeStrongLocator =
            By.XPath("//div[contains(@class,'col') and contains(@class,'text-center')]//p/strong");

        private By timePeriodFilterLocator(string timePeriod)
        {
            var normalized = (timePeriod ?? string.Empty).Trim();
            var noSpaces = normalized.Replace(" ", "");
            // Buttons in this UI often render as "1 W" (with space); this locator ignores spaces.
            return By.XPath($"(//button | //*[@role='button'] | //a)[translate(normalize-space(.), ' ', '')='{noSpaces}'][1]");
        }

        public InstrumentPerformancePage(IWebDriver driver) : base(driver) { }

        public void ValidateNavigatedToInstrument(string fundSymbol)
        {
            WaitForSpinners();
            var currentUrl = driver.Url;
            Assert.That(currentUrl, Does.Contain("/instrument-performance/"));
            Assert.That(currentUrl, Does.EndWith($"/{fundSymbol.Trim()}"));
        }

        public void ValidatePerformanceChartVisible()
        {
            WaitForSpinners();
            WaitForElementToBeVisible(chartContainerLocator, timeoutSeconds: 20);
        }

        public void ValidateTimePeriodFilterDisplayed(string timePeriod)
        {
            WaitForSpinners();
            WaitForElementToBeVisible(timePeriodFilterLocator(timePeriod), timeoutSeconds: 15);
        }

        public void ClickTimePeriodFilter(string timePeriod)
        {
            WaitForSpinners();
            var locator = timePeriodFilterLocator(timePeriod);
            var element = WaitForElementToBeClickable(locator, timeoutSeconds: 20);
            element.Click();
            WaitForSpinners();

            // Wait until the clicked filter is visually selected (implementation varies by environment).
            var waitSelected = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            waitSelected.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            waitSelected.Until(_ =>
            {
                var el = driver.FindElement(locator);
                var cls = el.GetAttribute("class") ?? string.Empty;
                return cls.Contains("theme-primary", StringComparison.OrdinalIgnoreCase)
                       || cls.Contains("btn-primary", StringComparison.OrdinalIgnoreCase)
                       || cls.Contains("active", StringComparison.OrdinalIgnoreCase)
                       || cls.Contains("selected", StringComparison.OrdinalIgnoreCase);
            });
        }

        public void ValidateChartDataDisplayedFor(string timePeriod)
        {
            // We can't reliably assert specific series values across environments,
            // so we validate the chart is present and the requested time period is selected.
            ValidatePerformanceChartVisible();

            var locator = timePeriodFilterLocator(timePeriod);
            var el = WaitForElementToBeVisible(locator, timeoutSeconds: 10);
            var cls = el.GetAttribute("class") ?? string.Empty;
            Assert.That(
                cls.Contains("theme-primary", StringComparison.OrdinalIgnoreCase)
                || cls.Contains("btn-primary", StringComparison.OrdinalIgnoreCase)
                || cls.Contains("active", StringComparison.OrdinalIgnoreCase)
                || cls.Contains("selected", StringComparison.OrdinalIgnoreCase),
                $"Expected time period '{timePeriod}' filter to be selected. Class was: '{cls}'."
            );

            ValidateChartDateRangeCaptionIncludesToday();
        }

        /// <summary>
        /// Asserts the visible chart date-range caption includes <see cref="DateTime.Today"/> (MM/dd/yyyy),
        /// matching the UI pattern shown under the chart (e.g. "04/22/2026 - 04/29/2026").
        /// </summary>
        public void ValidateChartDateRangeCaptionIncludesToday()
        {
            WaitForSpinners();
            var today = DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            var yesterday = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(_ =>
            {
                var strongs = driver.FindElements(ChartDateRangeStrongLocator);
                return strongs.Any(s => s.Displayed && LooksLikeUsDateRange(s.Text));
            });

            var visibleRange = driver.FindElements(ChartDateRangeStrongLocator)
                .FirstOrDefault(s => s.Displayed && LooksLikeUsDateRange(s.Text));

            Assert.That(visibleRange, Is.Not.Null, "Expected a visible chart date range caption (MM/dd/yyyy - MM/dd/yyyy).");

            var text = visibleRange!.Text.Trim();
            var hasToday = text.Contains(today, StringComparison.Ordinal);
            var hasYesterday = text.Contains(yesterday, StringComparison.Ordinal);
            Assert.That(
                hasToday || hasYesterday,
                $"Expected chart date range '{text}' to include today's date '{today}' (or '{yesterday}' near midnight)."
            );
        }

        private static bool LooksLikeUsDateRange(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || !text.Contains('-'))
                return false;
            return Regex.IsMatch(text.Trim(), @"\d{1,2}/\d{1,2}/\d{4}\s*-\s*\d{1,2}/\d{1,2}/\d{4}");
        }

        public void ValidateBackButtonDisplayed()
        {
            WaitForSpinners();
            WaitForElementToBeVisible(backButtonLocator, timeoutSeconds: 15);
        }
    }
}

