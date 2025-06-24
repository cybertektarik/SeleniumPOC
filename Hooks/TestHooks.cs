using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using NUnit.Framework;
using SeleniumProject.Common;



namespace SeleniumPOC.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver = null!;
        protected AllPages? Pages;

        private readonly string DEFAULT_URL = "https://employee-feature2.live-test-domain.com/#/auth/login?returnUrl=%2F";
        private readonly bool RUN_REMOTE = false;
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;
        private readonly string BROWSER_TYPE = Constants.BROWSER_CHROME;
        private readonly bool RUN_HEADLESS = false;
        private readonly bool RUN_DESKTOP_SIZE = true;
        private readonly string TEST_NAME;

        public static readonly bool RUN_MULTI_BROWSER = true;

        // Toggle this to enable/disable parallel execution behavior (report + driver safety)
        private static readonly bool RUN_PARALLEL = false;

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            TEST_NAME = _scenarioContext.ScenarioInfo.Title;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ReportManager.InitReport(RUN_PARALLEL); // 👈 updated
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

            ReportManager.CreateScenario(TEST_NAME); // Thread-safe

            if (RUN_REMOTE)
                driver = SeleniumDriverHelper.GetPerfectoRemoteDriver(BROWSER_TYPE, PLATFORM, "1920x1080", TEST_NAME);
            else
                driver = SeleniumDriverHelper.GetLocalDriver(BROWSER_TYPE, RUN_HEADLESS, RUN_DESKTOP_SIZE);

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
    }

}

