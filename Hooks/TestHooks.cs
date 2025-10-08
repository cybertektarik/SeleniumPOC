using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using NUnit.Framework;
using SeleniumProject.Common;



namespace SeleniumPOC.Hooks
{
    // CLASS: TestHooks
    // PURPOSE: SpecFlow hooks that manage test lifecycle, driver creation, and reporting
    // FLOW: Intercepts test execution → Creates/manages WebDriver → Handles reporting → Cleans up
    // CONNECTS TO: SpecFlow test execution → AllPages (via driver) → ReportManager (via logging)
    [Binding]
    public class TestHooks
    {
        // FIELD: _scenarioContext - SpecFlow scenario context for test information
        // PURPOSE: Provides access to scenario details, tags, and execution status
        // FLOW: Used throughout test lifecycle for context and status tracking
        private readonly ScenarioContext _scenarioContext;
        
        // FIELD: driver - WebDriver instance for browser automation
        // PURPOSE: Stores the active WebDriver instance for test execution
        // FLOW: Created in BeforeScenario → Used by AllPages → Cleaned up in AfterScenario
        private IWebDriver driver = null!;
        
        // FIELD: Pages - AllPages object containing all page objects
        // PURPOSE: Central access point to all page objects in the application
        // FLOW: Created in BeforeScenario → Passed to scenario context → Used by step definitions
        protected AllPages? Pages;

        // FIELDS: Test execution configuration constants
        // PURPOSE: Control browser type, execution mode, and test environment
        // FLOW: Used in BeforeScenario to determine driver creation strategy
        private readonly bool RUN_REMOTE = false;                        // Remote execution flag
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;  // Target platform
        private readonly string BROWSER_TYPE = Constants.BROWSER_CHROME; // Browser type
        private readonly bool RUN_HEADLESS = false;                    // Headless mode flag
        private readonly bool RUN_DESKTOP_SIZE = true;                  // Desktop size flag
        private readonly string TEST_NAME;                             // Current test name

        // FIELD: RUN_MULTI_BROWSER - Multi-browser execution flag
        // PURPOSE: Controls whether multiple browsers can run simultaneously
        public static readonly bool RUN_MULTI_BROWSER = true;

        // FIELD: RUN_PARALLEL - Parallel execution control
        // PURPOSE: Toggle for parallel execution behavior (report + driver safety)
        // FLOW: Used by ReportManager to determine parallel execution mode
        private static readonly bool RUN_PARALLEL = true;

        // CONSTRUCTOR: TestHooks
        // PURPOSE: Initializes TestHooks with SpecFlow scenario context
        // FLOW: Receives scenario context → Stores test name → Ready for test execution
        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;                    // Store scenario context
            TEST_NAME = _scenarioContext.ScenarioInfo.Title;      // Extract test name from scenario
        }

        // METHOD: BeforeTestRun (SpecFlow Hook)
        // PURPOSE: Initializes reporting system before any tests run
        // FLOW: Called once at start → Initializes ReportManager → Sets up parallel execution
        // DRIVER FLOW: No driver involved - reporting setup only
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ReportManager.InitReport(RUN_PARALLEL); // Initialize reporting with parallel mode
        }

        // METHOD: BeforeFeature (SpecFlow Hook)
        // PURPOSE: Creates feature-level reporting entry
        // FLOW: Called before each feature → Creates feature report → Tracks feature execution
        // DRIVER FLOW: No driver involved - reporting setup only
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            ReportManager.CreateFeature(featureContext.FeatureInfo.Title); // Create feature report entry
        }

        // METHOD: BeforeScenario (SpecFlow Hook)
        // PURPOSE: Sets up test environment before each scenario execution
        // FLOW: Logs scenario start → Creates WebDriver → Navigates to URL → Creates AllPages
        // DRIVER FLOW: Creates driver → Navigates to URL → Passes to AllPages → Stores in context
        [BeforeScenario]
        public void BeforeScenario()
        {
            // STEP 1: Log scenario start information
            Console.WriteLine("********** SCENARIO STARTING **********");
            Console.WriteLine("Scenario Title: " + TEST_NAME);
            Console.WriteLine("Tags: " + string.Join(", ", _scenarioContext.ScenarioInfo.Tags));
            Console.WriteLine("Time: " + DateTime.Now);

            // STEP 2: Create scenario report entry
            ReportManager.CreateScenario(TEST_NAME); // Thread-safe reporting

            // STEP 3: Load test data based on scenario tags
            SetTestDataFileBasedOnTags(_scenarioContext.ScenarioInfo.Tags);

            // STEP 4: Create WebDriver based on configuration
            if (RUN_REMOTE)
                driver = SeleniumDriverHelper.GetPerfectoRemoteDriver(BROWSER_TYPE, PLATFORM, "1920x1080", TEST_NAME);
            else
                driver = SeleniumDriverHelper.GetLocalDriver(BROWSER_TYPE, RUN_HEADLESS, RUN_DESKTOP_SIZE);

            // STEP 5: Navigate to application URL
            NavigateToDefaultUrl(TestUserManager.GetDefaultUrl()); // Use TestUserManager after Init
            Thread.Sleep(2000); // Wait for page to load

            // STEP 6: Create AllPages object and store in context
            Pages = new AllPages(driver);                    // Create all page objects with driver
            _scenarioContext["driver"] = driver;              // Store driver in scenario context
            _scenarioContext["Pages"] = Pages;               // Store Pages in scenario context
        }

        // METHOD: AfterScenario (SpecFlow Hook)
        // PURPOSE: Cleans up after each scenario execution
        // FLOW: Logs scenario end → Handles remote reporting → Quits driver → Releases resources
        // DRIVER FLOW: Checks scenario status → Stops remote reporting → Quits driver
        [AfterScenario]
        public void AfterScenario()
        {
            // STEP 1: Determine scenario execution status
            bool isScenarioPassed = _scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.OK;

            // STEP 2: Log scenario completion
            Console.WriteLine("***************************************************************");
            Console.WriteLine("       ENDING SCENARIO: " + TEST_NAME);
            Console.WriteLine("       Time: " + DateTime.Now);
            Console.WriteLine("       Scenario Status: " + (isScenarioPassed ? "PASSED" : "FAILED"));
            Console.WriteLine("***************************************************************");

            // STEP 3: Handle remote reporting cleanup
            var seleniumDriverHelper = new SeleniumDriverHelper();
            if (RUN_REMOTE)
            {
                seleniumDriverHelper.StopPerfectoReporting(isScenarioPassed); // Stop remote reporting
            }

            // STEP 4: Clean up WebDriver resources
            driver?.Quit(); // Close browser and release driver resources
        }

        // METHOD: AfterEachStep (SpecFlow Hook)
        // PURPOSE: Handles step-level reporting and screenshot capture on failures
        // FLOW: Checks step status → Captures screenshot on failure → Logs to report
        // DRIVER FLOW: Uses driver from context → Captures screenshot → Logs to report
        [AfterStep]
        public void AfterEachStep(ScenarioContext scenarioContext)
        {
            // STEP 1: Extract step information
            var stepInfo = scenarioContext.StepContext.StepInfo;
            string stepText = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";
            string status = scenarioContext.TestError == null ? "Pass" : "Fail";

            // STEP 2: Handle failure with screenshot capture
            if (status == "Fail" && scenarioContext.TryGetValue("driver", out IWebDriver driver))
            {
                try
                {
                    // STEP 2a: Capture screenshot on failure
                    string base64Screenshot = ScreenshotHelper.CaptureScreenshotBase64(driver);
                    ReportManager.LogStepWithScreenshot(stepText, status, base64Screenshot);
                }
                catch (Exception ex)
                {
                    // STEP 2b: Fallback to text-only logging if screenshot fails
                    Console.WriteLine($"Screenshot capture failed: {ex.Message}");
                    ReportManager.LogStep(stepText, status);
                }
            }
            else
            {
                // STEP 3: Log successful step without screenshot
                ReportManager.LogStep(stepText, status);
            }
        }

        // METHOD: AfterTestRun (SpecFlow Hook)
        // PURPOSE: Finalizes reporting after all tests complete
        // FLOW: Called once at end → Flushes report → Finalizes reporting system
        // DRIVER FLOW: No driver involved - reporting finalization only
        [AfterTestRun]
        public static void AfterTestRun()
        {
            ReportManager.FlushReport(); // Finalize and save all reports
        }
        
        // METHOD: NavigateToDefaultUrl
        // PURPOSE: Navigates WebDriver to the specified URL with validation
        // FLOW: Validates driver → Logs URL → Navigates to URL
        // DRIVER FLOW: Uses stored driver → Navigates to target URL
        // USAGE: Called in BeforeScenario to navigate to application start page
        public void NavigateToDefaultUrl(string url)
        {
            // STEP 1: Validate driver is initialized
            if (driver == null)
            {
                throw new InvalidOperationException("Driver is not initialized.");
            }

            // STEP 2: Log navigation and execute
            Console.WriteLine("Go to URL: " + url);
            driver.Navigate().GoToUrl(url); // Navigate to target URL
        }

        // METHOD: TakeScreenshot
        // PURPOSE: Captures and saves screenshot to file system
        // FLOW: Captures screenshot → Creates directory → Saves file → Attaches to test
        // DRIVER FLOW: Uses stored driver → Captures screenshot → Saves to file system
        public void TakeScreenshot()
        {
            // STEP 1: Capture screenshot using WebDriver
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var screenshotDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots\\");

            // STEP 2: Ensure screenshot directory exists
            if (!Directory.Exists(screenshotDirectoryPath))
            {
                Directory.CreateDirectory(screenshotDirectoryPath);
            }

            // STEP 3: Generate unique filename and save
            var currentDate = DateTime.Now;
            var filePath = $"{screenshotDirectoryPath}{currentDate:yyyy.MM.dd-HH.mm.ss}_{TestContext.CurrentContext.Test.Name}.png";
            screenshot.SaveAsFile(filePath);
            TestContext.AddTestAttachment(filePath); // Attach to test report
        }

        // METHOD: Sleep
        // PURPOSE: Provides thread sleep functionality for test synchronization
        // FLOW: Converts seconds to milliseconds → Sleeps thread
        // DRIVER FLOW: No driver involved - thread synchronization only
        protected static void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000); // Convert seconds to milliseconds
        }

        // METHOD: SetTestDataFileBasedOnTags
        // PURPOSE: Selects appropriate test data file based on scenario tags
        // FLOW: Checks tags → Selects data file → Sets TestUserManager data file
        // DRIVER FLOW: No driver involved - test data configuration only
        private void SetTestDataFileBasedOnTags(string[] tags)
        {
            // STEP 1: Set default test data file
            string testDataPath = "Data/UserRoles_Set1.json"; // Default

            // STEP 2: Select data file based on scenario tags
            if (tags.Contains("feature2"))
            {
                testDataPath = "Data/UserRoles_Set1.json"; // Feature2 environment data
            }
            else if (tags.Contains("external"))
            {
                testDataPath = "Data/UserRoles_Set2.json"; // External environment data
            }
            
            // STEP 3: Log selection and set data file
            Console.WriteLine("Using Test Data File: " + testDataPath);
            TestUserManager.SetDataFile(testDataPath); // Configure TestUserManager with selected data
        }
    }
}
