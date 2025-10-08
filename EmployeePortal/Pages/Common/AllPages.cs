using OpenQA.Selenium;
using SeleniumPOC.EmployeePortal.Pages.Dashboard;
using SeleniumPOC.EmployeePortal.Pages.CashAccount;
using SeleniumPOC.EmployeePortal.Pages.Onboarding;
using SeleniumPOC.EmployeePortal.Pages.ManageInvestments;
using SeleniumPOC.Common;



namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    // CLASS: AllPages
    // PURPOSE: Central hub that contains all page objects and passes WebDriver to each one
    // FLOW: Receives driver from BaseTestSuite → Creates all page objects → Initializes PageControl
    // CONNECTS TO: All page objects (via constructor) → PageControl (via InitDriver)
    public class AllPages
    {
        // FIELDS: All page objects in the application
        // PURPOSE: Each page object receives the same WebDriver instance
        public LoginPage LoginPage;                                    // Login page functionality
        public OktaLoginPage OktaLoginPage;                            // Okta authentication page
        public HeaderBarPage HeaderBarPage;                           // Application header navigation
        public SidebarNavPage SidebarNavPage;                         // Sidebar navigation menu
        public DashboardPage DashboardPage;                           // Main dashboard page
        public ManageInvestmentPage ManageInvestmentsPage;            // Investment management page
        public NotificationAlert NotificationAlert;                    // Notification handling
        public CashAccountPage CashAccountPage;                       // Cash account management
        public OnboardingPage OnboardingPage;                         // User onboarding flow
        public WizardRequiredDisclosuresPage WizardRequiredDisclosuresPage; // Required disclosures wizard
        public WizardSignaturePage WizardSignaturePage;               // Signature collection wizard
        public WizardRqtScorePage WizardRtqScorePage;                 // Risk tolerance questionnaire
        public ChooseYourInvestmentPage ChooseYourInvestmentPage;     // Investment selection page
        public AutoFundingPage AutoFundingPage;                       // Auto funding configuration
        public PageControl PageControl;                                // Static utility for element operations

        // CONSTRUCTOR: AllPages
        // PURPOSE: Initializes all page objects with the same WebDriver instance
        // FLOW: Receives driver → Creates all page objects → Initializes PageControl
        // DRIVER FLOW: BaseTestSuite.driver → AllPages constructor → Each page object constructor
        public AllPages(IWebDriver driver)
        {
            // STEP 1: Create all page objects - each receives the same driver instance
            LoginPage = new LoginPage(driver);                         // Pass driver to LoginPage
            OktaLoginPage = new OktaLoginPage(driver);                 // Pass driver to OktaLoginPage
            HeaderBarPage = new HeaderBarPage(driver);                 // Pass driver to HeaderBarPage
            SidebarNavPage = new SidebarNavPage(driver);               // Pass driver to SidebarNavPage
            DashboardPage = new DashboardPage(driver);                 // Pass driver to DashboardPage
            ManageInvestmentsPage = new ManageInvestmentPage(driver); // Pass driver to ManageInvestmentPage
            NotificationAlert = new NotificationAlert(driver);         // Pass driver to NotificationAlert
            CashAccountPage = new CashAccountPage(driver);             // Pass driver to CashAccountPage
            OnboardingPage = new OnboardingPage(driver);               // Pass driver to OnboardingPage
            WizardRequiredDisclosuresPage = new WizardRequiredDisclosuresPage(driver); // Pass driver to wizard
            WizardSignaturePage = new WizardSignaturePage(driver);     // Pass driver to signature wizard
            WizardRtqScorePage = new WizardRqtScorePage(driver);       // Pass driver to RTQ wizard
            ChooseYourInvestmentPage = new ChooseYourInvestmentPage(driver); // Pass driver to investment selection
            AutoFundingPage = new AutoFundingPage(driver);             // Pass driver to AutoFundingPage

            // STEP 2: Initialize PageControl with the driver
            // CRITICAL: This makes the driver available to all PageControl instances
            PageControl.InitDriver(driver);                            // Initialize PageControl with driver
        }
    }
}

