using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumPOC.EmployeePortal.Pages.Common;
// using WebDriverManager.DriverConfigs.Impl;
// using WebDriverManager;

namespace SeleniumPOC.Common
{
    // CLASS: BaseTestSuite
    // PURPOSE: Central test setup class that creates and configures WebDriver before every test run.
    // FLOW: This is the entry point - creates ChromeDriver and passes it to AllPages.
    // CONNECTS TO: AllPages (via constructor) → PageControl (via InitDriver)
    public class BaseTestSuite
    {
        // FIELD: DEFAULT_URL - Target application URL for all tests
        protected readonly string DEFAULT_URL = "https://employee-feature2.live-test-domain.com/#/auth/login";
        // protected readonly string DEFAULT_URL = "http://localhost:8080/";

        // FIELDS: Test configuration constants (currently unused but available for future use)
        private readonly bool RUN_REMOTE = false;
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;
        private readonly string BROWSER_TYPE = Constants.BROWSER_CHROME;
        private readonly bool RUN_HEADLESS = false;
        private readonly bool RUN_DESKTOP_SIZE = true;
        private readonly string TEST_NAME = "";

        // FIELD: Helper for driver management (currently unused)
        private SeleniumDriverHelper seleniumDriverHelper = new SeleniumDriverHelper();
        
        // FIELD: driver - The shared WebDriver instance that flows through entire framework
        // PURPOSE: Stores the active ChromeDriver instance shared by all page objects
        public static IWebDriver driver;

        // FIELD: Pages - Central access point to all page objects
        // PURPOSE: Contains all page objects and passes driver to each one
        protected AllPages Pages;
        // protected Logger Logger;

        //[SetUp]
        //public void Setup()
        //{
        //    if (RUN_REMOTE)
        //        driver = seleniumDriverHelper.GetPerfectoRemoteDriver(BROWSER_TYPE, PLATFORM, "1920x1080", TEST_NAME);
        //    else
        //        driver = seleniumDriverHelper.GetLocalDriver(BROWSER_TYPE, RUN_HEADLESS, RUN_DESKTOP_SIZE);

        //    Console.WriteLine();
        //    Console.WriteLine("********** STARTING TEST: " + TestContext.CurrentContext.Test.Name + " **********");
        //    Console.WriteLine("********** TIME: " + DateTime.Now + " **********");
        //    Console.WriteLine();

        //    GoToUrl(DEFAULT_URL);
        //    Pages = new AllPages(driver);
        //   // Logger = new Logger();
        //}

        // METHOD: Test (SetUp)
        // PURPOSE: Initializes ChromeDriver with special options and creates AllPages object
        // FLOW: Creates driver → Configures Chrome → Navigates to URL → Creates AllPages
        // DRIVER FLOW: BaseTestSuite.driver → AllPages constructor → All page objects
        [SetUp]
        public void Test()
        {
            // STEP 1: Configure Chrome with special options for stability
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");                    // Bypass OS security model
            options.AddArgument("--disable-dev-shm-usage");         // Overcome limited resource problems
            options.AddArgument("--ignore-certificate-errors");     // Ignore SSL certificate errors
            options.AddArgument("--disable-gpu");                   // Disable GPU acceleration
            options.AddArgument("--disable-extensions");            // Disable browser extensions
            options.AddArgument("--allow-insecure-localhost");      // Allow insecure localhost
            options.AcceptInsecureCertificates = true;             // Accept insecure certificates
            
            // STEP 2: Create and configure the WebDriver instance
            driver = new ChromeDriver(options);                    // Create ChromeDriver with options
            driver.Manage().Window.Maximize();                     // Maximize browser window
            driver.Navigate().GoToUrl(DEFAULT_URL);                // Navigate to target application
            Thread.Sleep(2000);                                     // Wait for page to load
            
            // STEP 3: Create AllPages object - this initializes all page objects with the driver
            // CRITICAL: This is where the driver flows to all page objects
            Pages = new AllPages(driver);                          // Pass driver to AllPages constructor
            //_scenarioContext["driver"] = driver;
            //_scenarioContext["Pages"] = Pages;
            //Logger = new Logger();
        }

        // METHOD: TearDown
        // PURPOSE: Cleanup after each test - closes browser and disposes driver
        // FLOW: Logs test completion → Quits browser → Disposes driver
        [TearDown]
        public void TearDown()
        {
            /* if (RUN_REMOTE)
            {
            seleniumDriverHelper.StopPerfectoReporting(TestContext.CurrentContext.Result.Outcome == ResultState.Success);
            } */

            // Log test completion
            Console.WriteLine();
            Console.WriteLine("********** ENDING TEST: " + TestContext.CurrentContext.Test.Name + " **********");
            Console.WriteLine("********** TIME: " + DateTime.Now + " **********");
            Console.WriteLine();

            // Clean up driver resources
            if (driver != null)
            {
                driver.Quit();        // Close browser window
                driver.Dispose();     // Release driver resources
            }
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

        protected void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public void GoToUrl(string url)
        {
            Console.WriteLine("Go to URL: " + url);
            driver.Navigate().GoToUrl(url);
        }
    }
}
