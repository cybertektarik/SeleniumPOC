using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
//using Reportium.Client;
//using Reportium.Model;
//using Reportium.Test.Result;

namespace SeleniumPOC.Common
{
    internal class SeleniumDriverHelper
    {
        private readonly string PATH_TO_CHROME = @"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
        private readonly string PATH_TO_CHROMEDRIVER = @"C:\\Compile\\NuGetPackages\\selenium.webdriver.chromedriver\\112.0.6478.12600\\driver\\win32\\chromedriver.exe";
        //private readonly string PERFECTO_URL = "https://webster.perfectomobile.com/nexperience/perfectomobile/wd/hub";
       // private readonly string PERFECTO_TOKEN = "<Paste token here!>";

        // private ReportiumClient reportiumClient;

        public WebDriver GetLocalDriver(string browserType, bool headless, bool desktopSize)
        {
            WebDriver driver = null;

            switch (browserType)
            {
                case Constants.BROWSER_CHROME:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.BinaryLocation = PATH_TO_CHROME;

                    if (headless)
                        chromeOptions.AddArguments("headless");

                    driver = new ChromeDriver(PATH_TO_CHROMEDRIVER, chromeOptions);

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

                    if (desktopSize)
                        driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080); // Desktop
                    else
                        driver.Manage().Window.Size = new System.Drawing.Size(390, 844); // Mobile
                    break;
            }

            return driver;
        }

        /*public WebDriver GetPerfectoRemoteDriver(string browserType, string platformName, string desktopSize, string testName)
        {
            string scriptName = testName + "-" + platformName + "-" + browserType;
            WebDriver driver = null;
            DriverOptions driverOptions = null;
            Dictionary<string, object> perfectoOptions = new Dictionary<string, object>();
            perfectoOptions.Add("securityToken", PERFECTO_TOKEN);
            perfectoOptions.Add("resolution", desktopSize);
            perfectoOptions.Add("scriptName", scriptName);
            perfectoOptions.Add("location", "US East");

            switch (browserType)
            {
                case Constants.BROWSER_CHROME:
                    driverOptions = new ChromeOptions();
                    ((ChromeOptions)driverOptions).BrowserVersion = "latest";
                    break;

                case Constants.BROWSER_FIREFOX:
                    driverOptions = new FirefoxOptions();
                    ((FirefoxOptions)driverOptions).BrowserVersion = "latest";
                    break;

                case Constants.BROWSER_EDGE:
                    driverOptions = new EdgeOptions();
                    ((EdgeOptions)driverOptions).BrowserVersion = "latest";
                    break;

                case Constants.BROWSER_SAFARI:
                    driverOptions = new SafariOptions();
                    ((SafariOptions)driverOptions).BrowserVersion = "14";
                    break;

                default:
                    throw new ArgumentException("Unrecognized browser type: " + browserType);
            }

            switch (platformName)
            {
                case Constants.PLATFORM_MACOS:
                    perfectoOptions.Add("platformVersion", "macOS Big Sur");
                    perfectoOptions.Add("location", "NA-US-BOS");
                    break;
            }

            driverOptions.PlatformName = platformName;
            driverOptions.AddAdditionalOption("perfecto:options", perfectoOptions);

            driver = new RemoteWebDriver(new Uri(string.Format(PERFECTO_URL)), driverOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

            TestContext.WriteLine(driver.ToString());
            StartPerfectoReporting(driver, browserType, platformName, scriptName);

            return driver;
        }

        public void StartPerfectoReporting(WebDriver driver, string browserType, string platformName, string scriptName)
        {
            DateTime date = DateTime.Now;
            int dateJob = 100000 + date.Month * 100 + date.Day;

            PerfectoExecutionContext perfectoExecutionContext = new PerfectoExecutionContext.PerfectoExecutionContextBuilder()
                .WithProject(new Project("Employee Portal", "feature27"))
                .WithJob(new Job("Sample CI Job", dateJob))
                .WithWebDriver(driver)
                .Build();

            reportiumClient = PerfectoClientFactory.CreatePerfectoReportiumClient(perfectoExecutionContext);
            reportiumClient.TestStart(scriptName, new TestContext.TestContextTags(browserType, platformName));
        }

        public void StopPerfectoReporting(bool pass)
        {
            if (pass)
                reportiumClient.TestStop(TestResultFactory.CreateSuccess());
            else
                reportiumClient.TestStop(TestResultFactory.CreateFailure("Failed"));

            string reportURL = reportiumClient.GetReportUrl();
            Console.WriteLine(reportURL);
        }
    }*/
    }
}

