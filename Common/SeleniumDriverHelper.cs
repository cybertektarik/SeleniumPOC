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
using System.IO;

namespace SeleniumPOC.Common
{
    internal class SeleniumDriverHelper
    {
        private const string PERFECTO_URL = "https://webster.perfectomobile.com/nexperience/perfectomobile/wd/hub";
        private const string PERFECTO_TOKEN = "";
        private static ReportiumClient? _reportiumClient;

        static SeleniumDriverHelper()
        {
            // Default: allow Selenium Manager to resolve matching browser drivers automatically.
            // Set DISABLE_SELENIUM_MANAGER=1 only if you explicitly want to use a pinned/bundled driver.
            var disableManager = Environment.GetEnvironmentVariable("DISABLE_SELENIUM_MANAGER");
            if (string.Equals(disableManager, "1", StringComparison.Ordinal) ||
                string.Equals(disableManager, "true", StringComparison.OrdinalIgnoreCase))
            {
                Environment.SetEnvironmentVariable("SE_MANAGER_DISABLE", "true", EnvironmentVariableTarget.Process);
            }

            Environment.SetEnvironmentVariable("SE_SESSION_REQUEST_TIMEOUT", "0", EnvironmentVariableTarget.Process);
        }

        public static WebDriver GetLocalDriver(string browserType, bool headless, bool desktopSize)
        {
            // Only Chrome is supported for local execution
            if (!browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Only Chrome is supported for local execution. Requested: {browserType}");
            }

            try
            {
                // Configure Chrome options (set all at once for efficiency)
                ChromeOptions chromeOptions = new();
                chromeOptions.AddArguments("--no-sandbox", "--disable-dev-shm-usage");
                
                if (headless)
                {
                    chromeOptions.AddArguments("--headless", "--disable-gpu");
                }
                else
                {
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddAdditionalOption("useAutomationExtension", false);
                }

                // Default: Selenium Manager resolves the matching driver for the installed Chrome.
                // If you need to force using the bundled chromedriver.exe, set USE_BUNDLED_CHROMEDRIVER=1.
                var useBundled = Environment.GetEnvironmentVariable("USE_BUNDLED_CHROMEDRIVER");
                if (string.Equals(useBundled, "1", StringComparison.Ordinal) ||
                    string.Equals(useBundled, "true", StringComparison.OrdinalIgnoreCase))
                {
                    string driverDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string chromeDriverPath = Path.Combine(driverDirectory, "chromedriver.exe");
                    if (!File.Exists(chromeDriverPath))
                        throw new FileNotFoundException($"ChromeDriver not found at: {chromeDriverPath}");

                    ChromeDriverService chromeService = ChromeDriverService.CreateDefaultService(
                        Path.GetDirectoryName(chromeDriverPath)!,
                        Path.GetFileName(chromeDriverPath));
                    chromeService.SuppressInitialDiagnosticInformation = true;
                    chromeService.HideCommandPromptWindow = true;

                    return ConfigureLocalDriver(new ChromeDriver(chromeService, chromeOptions), desktopSize);
                }

                return ConfigureLocalDriver(new ChromeDriver(chromeOptions), desktopSize);
            }
            catch (DriverServiceNotFoundException ex)
            {
                throw new InvalidOperationException(
                    "Failed to create ChromeDriver service. Ensure ChromeDriver is installed and compatible.", ex);
            }
            catch (WebDriverException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to initialize ChromeDriver. Check Chrome browser version compatibility.\n" +
                    "If you are using a bundled chromedriver.exe, it may not match your Chrome version.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unexpected error initializing ChromeDriver: {ex.Message}\n" +
                    "Check Selenium Manager / ChromeDriver setup.", ex);
            }
        }

        private static WebDriver ConfigureLocalDriver(WebDriver driver, bool desktopSize)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);

            if (desktopSize)
            {
                driver.Manage().Window.Size = new Size(1200, 800);
                driver.Manage().Window.Maximize();
            }
            else
            {
                driver.Manage().Window.Size = new Size(390, 844);
            }

            return driver;
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

                DriverOptions driverOptions = browserType switch
                {
                    "Chrome" => new ChromeOptions { BrowserVersion = "latest" },
                    "Firefox" => new FirefoxOptions { BrowserVersion = "latest" },
                    "Edge" => new EdgeOptions { BrowserVersion = "latest" },
                    "Safari" => new SafariOptions { BrowserVersion = "14" },
                    _ => throw new ArgumentException($"Unrecognized browser type: {browserType}")
                };

                driverOptions.PlatformName = platformName;
                driverOptions.AddAdditionalOption("perfecto:options", perfectoOptions);

                var driver = new RemoteWebDriver(new Uri(PERFECTO_URL), driverOptions);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                driver.Manage().Window.Size = new Size(1920, 1080);

                StartPerfectoReporting(driver, browserType, platformName, scriptName);
                return driver;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error initializing Perfecto remote driver: {e.Message}");
                throw;
            }
        }

        // Fix for CS0426: The type name 'TestContextTags' does not exist in the type 'TestContext'  
        // The issue is that 'TestContextTags' is not a valid member of 'TestContext'.  
        // Instead, we should use 'TestContext.Builder' to build a TestContext object with tags.

        public static void StartPerfectoReporting(WebDriver driver, string browserType, string platformName, string scriptName)
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

                // Correctly build the TestContext with tags
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

