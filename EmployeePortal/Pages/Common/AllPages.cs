using OpenQA.Selenium;
using SeleniumPOC.EmployeePortal.Pages.Dashboard;
using SeleniumPOC.EmployeePortal.Pages.CashAccount;
using SeleniumPOC.EmployeePortal.Pages.Onboarding;
using SeleniumPOC.EmployeePortal.Pages.ManageInvestments;
using SeleniumPOC.Common;



namespace SeleniumPOC.EmployeePortal.Pages.Common
{
    public class AllPages
    {
        public LoginPage LoginPage;
        public OktaLoginPage OktaLoginPage;
        public HeaderBarPage HeaderBarPage;
        public SidebarNavPage SidebarNavPage;
        public DashboardPage DashboardPage;
        public ManageInvestmentPage ManageInvestmentsPage;
        public NotificationAlert NotificationAlert;
        public CashAccountPage CashAccountPage;
        public OnboardingPage OnboardingPage;
        public WizardRequiredDisclosuresPage WizardRequiredDisclosuresPage;
        public WizardSignaturePage WizardSignaturePage;
        public WizardRqtScorePage WizardRtqScorePage;
        public ChooseYourInvestmentPage ChooseYourInvestmentPage;
        public AutoFundingPage AutoFundingPage;
        public PageControl PageControl;

        public AllPages(IWebDriver driver)
        {
            LoginPage = new LoginPage(driver);
            OktaLoginPage = new OktaLoginPage(driver);
            HeaderBarPage = new HeaderBarPage(driver);
            SidebarNavPage = new SidebarNavPage(driver);
            DashboardPage = new DashboardPage(driver);
            ManageInvestmentsPage = new ManageInvestmentPage(driver);
            NotificationAlert = new NotificationAlert(driver);
            CashAccountPage = new CashAccountPage(driver);
            OnboardingPage = new OnboardingPage(driver);
            WizardRequiredDisclosuresPage = new WizardRequiredDisclosuresPage(driver);
            WizardSignaturePage = new WizardSignaturePage(driver);
            WizardRtqScorePage = new WizardRqtScorePage(driver);
            ChooseYourInvestmentPage = new ChooseYourInvestmentPage(driver);
            AutoFundingPage = new AutoFundingPage(driver);

            PageControl.InitDriver(driver);
        }
    }
}

