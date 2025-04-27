using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using Reqnroll;



namespace SeleniumPOC.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private SeleniumDriverHelper seleniumDriverHelper = new SeleniumDriverHelper();
        public IWebDriver? driver;
        protected AllPages? Pages;
       

        private readonly string DEFAULT_URL = "https://employee-feature2.live-test.domain.com/#/auth/login";

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--dns-prefetch-disable");
            options.AddArgument("--disable-features=NetworkService");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            options.AddArgument("--remote-allow-origins=*");
            options.AcceptInsecureCertificates = true;

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(DEFAULT_URL);
            Thread.Sleep(2000);

            Pages = new AllPages(driver);
            _scenarioContext["driver"] = driver;
            _scenarioContext["Pages"] = Pages;
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
    }
}

