using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using NUnit.Framework;
using SeleniumProject.Common;


[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]

namespace SeleniumPOC.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver = null!;
        protected AllPages? Pages;

        private readonly string DEFAULT_URL = "https://employee-feature2.live-test-domain.com/#/auth/login?returnUrl=%2F";
        private readonly bool RUN_REMOTE = true; // Set to true for Perfecto
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;
        private readonly bool RUN_HEADLESS = false;
        private readonly bool RUN_DESKTOP_SIZE = true;
        private readonly string TEST_NAME;

        // Store browser for current scenario
        private string _scenarioBrowser = "chrome"; // default

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            TEST_NAME = _scenarioContext.ScenarioInfo.Title;
            // Try: 1) Tag, 2) Scenario Argument (for Outline), 3) Env Variable, 4) Default
            _scenarioBrowser = GetBrowserFromTags(_scenarioContext.ScenarioInfo.Tags)
                ?? GetBrowserFromScenarioOutline(_scenarioContext)
                ?? GetBrowserFromEnv()
                ?? "chrome";
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Optionally load .env file if using DotNetEnv (uncomment if needed)
            // DotNetEnv.Env.Load();
            ReportManager.InitReport();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            ReportManager.CreateFeature(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Console.WriteLine("********** SCENARIO STARTING **********");
            Console.WriteLine("Scenario Title: " + TEST_NAME);
            Console.WriteLine("Tags: " + string.Join(", ", _scenarioContext.ScenarioInfo.Tags));
            Console.WriteLine("Time: " + DateTime.Now);
            Console.WriteLine("Browser: " + _scenarioBrowser);

            if (RUN_REMOTE)
            {
                driver = SeleniumDriverHelper.GetPerfectoRemoteDriver(
                    _scenarioBrowser,
                    _scenarioBrowser.Equals("safari", StringComparison.OrdinalIgnoreCase) ? Constants.PLATFORM_MACOS : PLATFORM,
                    "1920x1080",
                    TEST_NAME
                );
                // Optionally: start reporting here for Perfecto
                new SeleniumDriverHelper().StartPerfectoReporting((WebDriver)driver, _scenarioBrowser, PLATFORM, TEST_NAME);
            }
            else
            {
                driver = SeleniumDriverHelper.GetLocalDriver(_scenarioBrowser, RUN_HEADLESS, RUN_DESKTOP_SIZE);
            }

            NavigateToDefaultUrl(DEFAULT_URL);
            Thread.Sleep(2000);
            Pages = new AllPages(driver);

            _scenarioContext["driver"] = driver;
            _scenarioContext["Pages"] = Pages;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            bool isScenarioPassed = _scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.OK;

            Console.WriteLine("***************************************************************");
            Console.WriteLine("       ENDING SCENARIO: " + TEST_NAME);
            Console.WriteLine("       Time: " + DateTime.Now);
            Console.WriteLine("       Scenario Status: " + (isScenarioPassed ? "PASSED" : "FAILED"));
            Console.WriteLine("***************************************************************");

            var seleniumDriverHelper = new SeleniumDriverHelper();
            if (RUN_REMOTE)
            {
                seleniumDriverHelper.StopPerfectoReporting(isScenarioPassed);
            }

            driver?.Quit();
        }

        [AfterStep]
        public void AfterEachStep(ScenarioContext scenarioContext)
        {
            var stepInfo = scenarioContext.StepContext.StepInfo;
            string stepText = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";
            string status = scenarioContext.TestError == null ? "Pass" : "Fail";

            if (status == "Fail" && scenarioContext.TryGetValue("driver", out IWebDriver driver))
            {
                string base64Screenshot = ScreenshotHelper.CaptureScreenshotBase64(driver);
                ReportManager.LogStepWithScreenshot(stepText, status, base64Screenshot);
            }
            else
            {
                ReportManager.LogStep(stepText, status);
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ReportManager.FlushReport();
        }

        public void NavigateToDefaultUrl(string url)
        {
            if (driver == null)
            {
                throw new InvalidOperationException("Driver is not initialized.");
            }
            Console.WriteLine("Go to URL: " + url);
            driver.Navigate().GoToUrl(url);
        }

        public void TakeScreenshot()
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var screenshotDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots\\");
            if (!Directory.Exists(screenshotDirectoryPath))
            {
                Directory.CreateDirectory(screenshotDirectoryPath);
            }

            var currentDate = DateTime.Now;
            var filePath = $"{screenshotDirectoryPath}{currentDate:yyyy.MM.dd-HH.mm.ss}_{TestContext.CurrentContext.Test.Name}.png";
            screenshot.SaveAsFile(filePath);
            TestContext.AddTestAttachment(filePath);
        }

        protected static void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        // Helper: Get browser type from scenario tags
        private string? GetBrowserFromTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                var lower = tag.ToLowerInvariant();
                if (lower == "chrome" || lower == "edge" || lower == "safari" || lower == "firefox")
                    return lower;
                if (lower == "browser_chrome") return "chrome";
                if (lower == "browser_edge") return "edge";
                if (lower == "browser_safari") return "safari";
                if (lower == "browser_firefox") return "firefox";
            }
            return null;
        }

        // Helper: Get browser type from Scenario Outline Examples argument
        private string? GetBrowserFromScenarioOutline(ScenarioContext context)
        {
            if (context.ContainsKey("browser"))
                return context["browser"]?.ToString()?.ToLowerInvariant();
            return null;
        }

        // Helper: Get browser from environment variable (BROWSERS=chrome,edge,safari)
        private string? GetBrowserFromEnv()
        {
            var env = Environment.GetEnvironmentVariable("BROWSERS");
            if (!string.IsNullOrWhiteSpace(env))
            {
                // Pick the first browser from the env var if multiple are listed
                return env.Split(',')[0].Trim().ToLowerInvariant();
            }
            return null;
        }
    }
}

