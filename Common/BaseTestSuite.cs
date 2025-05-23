﻿using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumPOC.EmployeePortal.Pages.Common;
// using WebDriverManager.DriverConfigs.Impl;
// using WebDriverManager;

namespace SeleniumPOC.Common
{
    public class BaseTestSuite
    {
        protected readonly string DEFAULT_URL = "https://employee-feature2.live-test-domain.com/#/auth/login";
        // protected readonly string DEFAULT_URL = "http://localhost:8080/";

        private readonly bool RUN_REMOTE = false;
        private readonly string PLATFORM = Constants.PLATFORM_WINDOWS;
        private readonly string BROWSER_TYPE = Constants.BROWSER_CHROME;
        private readonly bool RUN_HEADLESS = false;
        private readonly bool RUN_DESKTOP_SIZE = true;
        private readonly string TEST_NAME = "";

        private SeleniumDriverHelper seleniumDriverHelper = new SeleniumDriverHelper();
        public static IWebDriver driver;

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

        [SetUp]
        public void Test()
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
            //_scenarioContext["driver"] = driver;
            //_scenarioContext["Pages"] = Pages;
            //Logger = new Logger();
        }

        [TearDown]
        public void TearDown()
        {
            /* if (RUN_REMOTE)
            {
            seleniumDriverHelper.StopPerfectoReporting(TestContext.CurrentContext.Result.Outcome == ResultState.Success);
            } */

            Console.WriteLine();
            Console.WriteLine("********** ENDING TEST: " + TestContext.CurrentContext.Test.Name + " **********");
            Console.WriteLine("********** TIME: " + DateTime.Now + " **********");
            Console.WriteLine();

            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
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
