using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using Reportium.Client;
using Reportium.Model;
using Reportium.Test;
using Reportium.Test.Result;
using System.Drawing;

namespace SeleniumPOC.Common
{
    internal class SeleniumDriverHelper
    {
        private const string PERFECTO_URL = "https://webster.perfectomobile.com/nexperience/perfectomobile/wd/hub";
        private const string PERFECTO_TOKEN = ""; // Set your token or fetch from env
        private ReportiumClient? _reportiumClient;

        public static WebDriver GetLocalDriver(string browserType, bool headless, bool desktopSize)
        {
            try
            {
                WebDriver? driver = null;

                if (browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
                {
                    ChromeOptions chromeOptions = new();
                    if (headless)
                        chromeOptions.AddArguments("--headless");
                    driver = new ChromeDriver(chromeOptions);
                }
                else if (browserType.Equals("edge", StringComparison.OrdinalIgnoreCase))
                {
                    EdgeOptions edgeOptions = new();
                    if (headless)
                        edgeOptions.AddArguments("--headless");
                    driver = new EdgeDriver(edgeOptions);
                }
                else
                {
                    throw new ArgumentException($"Unsupported browser type: {browserType}");
                }

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

                if (desktopSize)
                    driver.Manage().Window.Size = new Size(1200, 800);
                else
                    driver.Manage().Window.Size = new Size(390, 844);

                driver.Manage().Window.Maximize();
                return driver;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error initializing local driver: {e.Message}");
                throw;
            }
        }

        public static WebDriver GetPerfectoRemoteDriver(string browserType, string platformName, string desktopSize, string testName)
        {
            if (string.IsNullOrEmpty(PERFECTO_TOKEN))
                throw new InvalidOperationException("Perfecto token is not set. Please configure 'PERFECTO_TOKEN' as an environment variable.");

            try
            {
                string scriptName = $"{testName}-{platformName}-{browserType}";
                Dictionary<string, object> perfectoOptions = new Dictionary<string, object>
            {
                {"securityToken", PERFECTO_TOKEN},
                {"resolution", desktopSize},
                {"scriptName", scriptName},
                {"location", "US East"}
            };

                DriverOptions driverOptions = browserType.ToLowerInvariant() switch
                {
                    "chrome" => new ChromeOptions { BrowserVersion = "latest" },
                    "firefox" => new FirefoxOptions { BrowserVersion = "latest" },
                    "edge" => new EdgeOptions { BrowserVersion = "latest" },
                    "safari" => new SafariOptions { BrowserVersion = "14" },
                    _ => throw new ArgumentException($"Unrecognized browser type: {browserType}")
                };

                driverOptions.PlatformName = platformName;
                driverOptions.AddAdditionalOption("perfecto:options", perfectoOptions);

                var driver = new RemoteWebDriver(new Uri(PERFECTO_URL), driverOptions);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                driver.Manage().Window.Size = new Size(1920, 1080);

                return driver;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error initializing Perfecto remote driver: {e.Message}");
                throw;
            }
        }

        public void StartPerfectoReporting(WebDriver driver, string browserType, string platformName, string scriptName)
        {
            try
            {
                DateTime date = DateTime.Now;
                int dateJob = 100000 + date.Month * 100 + date.Day;

                var perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                    .WithProject(new Project("Employee Portal", "feature27"))
                    .WithJob(new Job("Sample CI Job", dateJob))
                    .WithWebDriver(driver)
                    .Build();

                _reportiumClient = PerfectoClientFactory.CreatePerfectoReportiumClient(perfectoExecutionContext);

                var testContext = new TestContext.Builder()
                    .WithTestExecutionTags(browserType, platformName)
                    .Build();

                _reportiumClient.TestStart(scriptName, testContext);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error starting Perfecto reporting: {e.Message}");
                throw;
            }
        }

        public void StopPerfectoReporting(bool pass)
        {
            try
            {
                if (_reportiumClient == null)
                    throw new InvalidOperationException("Reportium client is not initialized. Make sure to call StartPerfectoReporting before StopPerfectoReporting.");

                if (pass)
                    _reportiumClient.TestStop(TestResultFactory.CreateSuccess());
                else
                    _reportiumClient.TestStop(TestResultFactory.CreateFailure("Test failed"));

                string reportURL = _reportiumClient.GetReportUrl();
                Console.WriteLine($"Perfecto Report URL: {reportURL}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error stopping Perfecto reporting: {e.Message}");
                throw;
            }
        }
    }
}

