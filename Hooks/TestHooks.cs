using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using Reqnroll;
using NUnit.Framework;



namespace SeleniumPOC.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private SeleniumDriverHelper seleniumDriverHelper = new SeleniumDriverHelper();
        public IWebDriver? driver;
        protected AllPages? Pages;


        private readonly string DEFAULT_URL = "https://employee-feature2.live-test-domain.com/#/auth/login?returnUrl=%2F";
        private readonly bool RUN_REMOTE = false;
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;
        private readonly string BROWSER_TYPE = Constants.BROWSER_CHROME;
        private readonly bool RUN_HEADLESS = false;
        private readonly bool RUN_DESKTOP_SIZE = true;
        private readonly string TEST_NAME = "";


        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        /*  [BeforeScenario]
          public void BeforeScenario()
          {
              ChromeOptions options = new ChromeOptions();
              options.AddArgument("--no-sandbox");
              options.AddArgument("--disable-dev-shm-usage");
              options.AddArgument("--ignore-certificate-errors");
              options.AddArgument("--disable-gpu");
              options.AddArgument("--disable-extensions");
              options.AddArgument("--allow-insecure-localhost");
              options.AcceptInsecureCertificates = true;
              driver = new ChromeDriver(options);
              driver.Manage().Window.Maximize();
              driver.Navigate().GoToUrl(DEFAULT_URL);
              Thread.Sleep(2000);
              Pages = new AllPages(driver);
              _scenarioContext["driver"] = driver;
              _scenarioContext["Pages"] = Pages;
          }*/

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (RUN_REMOTE)
                driver = seleniumDriverHelper.GetPerfectoRemoteDriver(BROWSER_TYPE, PLATFORM, "1920x1080", TEST_NAME);
            else
                driver = seleniumDriverHelper.GetLocalDriver(BROWSER_TYPE, RUN_HEADLESS, RUN_DESKTOP_SIZE);

            Console.WriteLine();
            Console.WriteLine("********** STARTING TEST: " + TestContext.CurrentContext.Test.Name + " **********");
            Console.WriteLine("********** TIME: " + DateTime.Now + " **********");
            Console.WriteLine();

            GoToUrl(DEFAULT_URL);
            Pages = new AllPages(driver);
            // Logger = new Logger();
        }


        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("***************************************************************");
            Console.WriteLine("       ENDING Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
            Console.WriteLine("       Time: " + DateTime.Now);
            Console.WriteLine("***************************************************************");

            if (driver != null)
            {
                driver.Quit();
            }
        }

        public void GoToUrl(string url)
        {
            Console.WriteLine("Go to URL: " + url);
            driver.Navigate().GoToUrl(url);
        }
    }
}

