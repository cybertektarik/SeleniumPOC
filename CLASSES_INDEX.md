# All Classes in SeleniumPOC Project

## Common Namespace

### Base Classes
- **BaseTestSuite** - Central test setup class that creates and configures WebDriver
- **BasePage** - Base class for all page objects

### Helper Classes
- **PageControl** - Wrapper class providing element interaction methods
- **SeleniumDriverHelper** (internal) - Helper for driver management
- **CommonFunctions** (internal static) - Common utility functions
- **TestUserManager** (static) - Manages test user accounts
- **Logger** - Logging functionality
- **ReportManager** (static) - Test report management
- **ScreenshotHelper** (static) - Screenshot capture functionality
- **InvestmentFunctions** (internal static) - Investment-related utility functions
- **Constants** (internal static) - Application constants

## EmployeePortal Namespace

### Test Classes
- **EmployeePortalBaseTestSuite** - Base test suite for Employee Portal tests
- **ParallelPerfectoTests** - Parallel test execution class (implements IDisposable)

### Test Suites (ManageInvestments)
- **ManageInvestmentsSingleAccountTests** - Single account investment tests
- **ManageInvestmentsSelectRTQ** - RTQ selection tests
- **ManageInvestmentsPreferences** - Investment preferences tests
- **ManageInvestmentsPreEnrollment** - Pre-enrollment tests
- **ManageInvestmentsDocuments** - Document management tests
- **ManageInvestmentsCurrentHoldings** - Current holdings tests
- **ManageInvestmentsAvailableInvestments** - Available investments tests
- **ManageInvestmentUserTests** - User-related investment tests

### Page Objects - Common
- **AllPages** - Central hub containing all page objects
- **LoginPage** - Login page functionality
- **OktaLoginPage** - Okta authentication page
- **HeaderBarPage** - Application header navigation
- **SidebarNavPage** - Sidebar navigation menu
- **NotificationAlert** - Notification handling
- **BasePage** - Base class for all page objects

### Page Objects - Dashboard
- **DashboardPage** - Main dashboard page
- **AccountBalanceCard** - Account balance card component

### Page Objects - Cash Account
- **CashAccountPage** - Cash account management page

### Page Objects - Onboarding
- **OnboardingPage** - User onboarding flow
- **InvestmentsPage** - Investments onboarding page

### Page Objects - Manage Investments
- **ManageInvestmentPage** - Investment management page
- **AutoFundingPage** - Auto funding configuration
- **SellInstrumentPage** - Sell instrument functionality
- **BuyInstrumentPage** - Buy instrument functionality
- **ChooseYourInvestmentPage** - Investment selection page
- **SearchAndTradePage** - Search and trade functionality
- **WizardRtqQuestionsPage** - RTQ questions wizard
- **WizardRqtScorePage** - RTQ score wizard
- **WizardRequiredDisclosuresPage** - Required disclosures wizard
- **WizardSignaturePage** - Signature collection wizard
- **WizardDisclosureAgreements** - Disclosure agreements wizard
- **AdvisorAgreementDocPage** - Advisory agreement document page

### Page Objects - Manage Investments Tabs
- **ActivityTab** - Activity tab component
- **PreferencesTab** - Preferences tab component
- **AvailableInvestmentsTab** - Available investments tab
- **CurrentHoldingsTab** - Current holdings tab
- **DocumentsTab** - Documents tab component

## StepDefinitions Namespace
- **ManageInvestmentsSearchFundsSteps** - Step definitions for managing investment search funds

## Model Namespace
- **TestAccountUser** - Represents a single test user account
- **TestAccountSet** - Container for test configuration and user account data

## Hooks Namespace
- **TestHooks** - Test hooks for setup/teardown operations

---

**Total Classes: 53**

