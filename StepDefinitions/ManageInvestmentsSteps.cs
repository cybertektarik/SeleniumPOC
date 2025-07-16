//using AngleSharp.Dom;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
using SeleniumProject.Common;
using System.Globalization;
using System.Text.RegularExpressions;
//using TechTalk.SpecFlow;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Binding]
    public class ManageInvestmentsSearchFundsSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver? driver;
        protected AllPages? Pages;

        public ManageInvestmentsSearchFundsSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            driver = _scenarioContext["driver"] as IWebDriver;
            Pages = _scenarioContext["Pages"] as AllPages;
        }

        [Given(@"I am logged in as a user who has not created a Choice account")]
        public void GivenILoginToTheEmployeePortalAsLimitedAccountUser()
        {
            var username = TestUserManager.GetUsername("LimitedAccountUser");
            Pages?.LoginPage.Login(username);
        }

        [Given(@"I am logged in as a user who has an enrolled account")]
        public void GivenILoginToTheEmployeePortalAsAnEnrolledUser()
        {
            var username = TestUserManager.GetUsername("EnrolledUser");
            Pages?.LoginPage.Login(username);
        }

        [Given(@"I am logged in as a Pre enrolled user")]
        public void GivenILoginToTheEmployeePortalAsAPreEnrolledUser()
        {
            var username = TestUserManager.GetUsername("PreEnrolledUser");
            Pages?.LoginPage.Login(username);
        }

        [Given(@"I am logged into the Employee Portal")]
        public void GivenILoginToTheEmployeePortalAsUser()
        {
            var username = TestUserManager.GetUsername("DefaultUser");
            Pages?.LoginPage.Login(username);
        }


        [When(@"I click on ""(.*)"" from the navigation menu")]
        public void WhenINavigateToTheTab(string tabName)
        {
            switch (tabName)
            {
                case "Manage Investment":
                    Pages.SidebarNavPage.GoToManageInvestments();
                    break;
                case "Settings":
                    Pages.SidebarNavPage.GoToSettings();
                    break;
                case "Resources":
                    Pages.SidebarNavPage.GoToResources();
                    break;
                default:
                    throw new ArgumentException($"No navigation action defined for tab: {tabName}");
            }
        }

        [When(@"I click on ""(.*)"" from the Risk Tolerance Selection")]
        public void WhenIClickRiskToleranceSelection(string tabName)
        {
            switch (tabName)
            {
                case "Yes, I want to choose this portfolio.":
                    Pages.WizardRtqScorePage.YesIWantToChooseThisPortfolio();
                    break;
                case "No, I want to review the questions again.":
                    Pages.WizardRtqScorePage.NoWantToReviewAgain();
                    break;
                default:
                    throw new ArgumentException($"No navigation action defined for tab: {tabName}");
            }
        }
        [When(@"I click on the ""(.*)"" logo link")]
        public void WhenISelectInvestmentOption(string investmentOption)
        {
            if (investmentOption == "HSA Investment")
            {
                Pages?.ManageInvestmentsPage.ChooseHsaInvest();
                Thread.Sleep(5000);
            }
        }

        [When(@"I click on the ""(.*)"" info link")]
        public void WhenIClickHSAInvestInfo(string str)
        {
            if (str == "HSA Invest Info")
            {
                Pages?.ManageInvestmentsPage.HSAInvestInfo();
            }
            else if (str == "HSA Invest")
            {
                Pages?.ManageInvestmentsPage.ChooseHsaInvest();
            }
        }

        [When(@"I click on the ""(.*)"" banner link")]
        public void WhenISelectHSAInvestOption(string bannerLink)
        {
            if (bannerLink == "Enroll in HSA Invest")
            {
                Pages?.ManageInvestmentsPage.ChooseEnrollInHsaInvest();
            }
            else if (bannerLink == "Manage HSA Invest Enrollment")
            {
                Pages?.ManageInvestmentsPage.ClickManageHsaInvestEnroll();
            }
        }

        [Then(@"I should see the ""(.*)"" link displayed")]
        public void ThenIShouldSeeTheLinkDisplayed(string linkName)
        {
            if (linkName == "Learn More")
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyLearnMoreLinkIsDisplayed();
            }
        }

        [Then(@"I should see the ""(.*)"" Button displayed")]
        public void IShouldSeeEnrollBtnDisplayed(string BtnName)
        {
            if (BtnName == "Enroll")
            {
                Pages?.ManageInvestmentsPage.ChooseEnroll();
            }
        }

        [Then(@"I should see the (.*) letter displayed")]
        public void ThenIShouldSeeHsaEsignAgreementDisplayed(string ESignLetter)
        {
            if (ESignLetter == "HsaBank Investment ESign Agreement")
            {
                Pages?.ManageInvestmentsPage.WizardDisclosureAgreementsPage.VerifyHsaEsignLetterIsDisplayed();
            }
        }

        [When(@"I click on the ""(.*)"" Button")]
        public void WhenIClickEnrollBtn(string BtnName)
        {
            if (BtnName == "ENROLL")
            {
                Pages?.ManageInvestmentsPage.ChooseEnroll();
            }

            else if (BtnName == "START ENROLLMENT")
            {
                Pages?.ManageInvestmentsPage.StartPreEnrollment();
            }

            else if (BtnName == "Cancel Enrollment")
            {
                Pages?.ManageInvestmentsPage.CancelEnrollment();
            }
        }

        [When(@"I click on the ""(.*)"" link")]
        public void WhenIClickOnTheLink(string linkName)
        {
            if (linkName == "Learn More")
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.LearnMore();
            }
            else if (linkName == "Return")
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.Return();
            }
            else if (linkName == "Managed Learn More")
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.LearnMoreManaged();
            }
        }

        [Then(@"I should see ""(.*)"" banner link displays")]
        public void ThenIShouldSeeEnrollHsaBannerLink(string banner)
        {
            if (banner == "Enroll in HSA Invest")
            {
                Thread.Sleep(2000);
                Pages?.ManageInvestmentsPage.IsDisplayHsaEnrollInHsaInvest();
            }
        }

        [Then(@"I should see that each investment account type has a hyperlink")]
        public void ThenIShouldSeeThatEachInvestmentTypeHasHyperlink()
        {
            Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyAccountTypeHyperlinksExist();
        }

        [When(@"I click on the funds available in choice account")]
        public void WhenIClickOnTheFundsAvailableInChoiceAccount()
        {
            Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.SeeFundsAvailableInChoiceOption();
        }

        [When(@"I click on the ""(.*)"" Account")]
        public void WhenIClickOnTheInvestmentType(string accountType)
        {
            switch (accountType)
            {
                case "Managed":
                    Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Managed");
                    break;
                case "Select":
                    Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Select");
                    break;
                case "Choice":
                    Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Choice");
                    break;
                default:
                    throw new ArgumentException($"Invalid account type: {accountType}");
            }
        }
        [When(@"I enter name ""(.*)"" in the name field")]
        public void WhenIEnterName(string name)
        {
            Pages?.ManageInvestmentsPage.WizardSignaturePage.EnterName(name);
            Thread.Sleep(2000);
        }

        [When(@"I click on the ""(.*)"" Investment account types")]
        public void ClickOnInvestmentAccountType(string accountType)
        {
            if (accountType.Equals("managed", StringComparison.OrdinalIgnoreCase))
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Managed");
            }
            else if (accountType.Equals("select", StringComparison.OrdinalIgnoreCase))
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Select");
            }
            else if (accountType.Equals("choice", StringComparison.OrdinalIgnoreCase))
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.ClickOnInvestmentAccountType("Choice");
            }
            else
            {
                throw new ArgumentException($"Invalid account type: {accountType}");
            }
        }

        [When(@"I click ""(.*)"" employment status")]
        public void WhenIClickEmploymentStatus(string empStatus)
        {
            switch (empStatus.ToLower())
            {
                case "retired":
                    Pages?.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("RETIRED");
                    break;
                case "employed":
                    Pages?.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("EMPLOYED");
                    break;
                case "student":
                    Pages?.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("STUDENT");
                    break;
                case "unemployed":
                    Pages?.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("UNEMPLOYED");
                    break;
                case "selfemployed":
                    Pages?.ManageInvestmentsPage.WizardRequiredDisclosuresPage.EnterEmploymentStatusInfo("SELF_EMPLOYED");
                    break;
                default:
                    throw new ArgumentException($"'{empStatus}' is not supported.");
            }
        }

        [When(@"I answer question ""(.*)"" to ""(.*)"" from the questionnaire")]
        public void WhenIClickOnAgree(string question, string answer)
        {
            // Validate the answer is one of the accepted values
            if (!new[] { "agree", "neutral", "stronglyAgree", "disagree", "stronglyDisagree" }.Contains(answer))
            {
                throw new ArgumentException($"Invalid answer: {answer}. Accepted values are: agree, neutral, stronglyAgree, disagree, stronglyDisagree");
            }

            // Set the answer for the question
            Pages.ManageInvestmentsPage.WizardRtqQuestionsPage.SetAnswerForQuestion(question, answer);
        }


        [When(@"I click on the Sign Button")]
        public void WhenIClickOnSignBtn()
        {
            Pages?.ManageInvestmentsPage.WizardSignaturePage.ScrollTextToEnableButton();
            Pages?.ManageInvestmentsPage.WizardSignaturePage.Sign();
        }

        [When(@"I click on the SUBMIT Button")]
        public void WhenIClickOnSubmitBtn()
        {
            Pages?.ManageInvestmentsPage.WizardRtqQuestionsPage.Submit();
        }

        [When(@"I click on the Skip Button")]
        public void WhenIClickOnSkipBtn()
        {
            Thread.Sleep(5000);
            Pages?.ManageInvestmentsPage.AutoFundingPage.Skip();
            Thread.Sleep(5000);
        }

        [When(@"I click on the Next Button")]
        public void WhenIClickOnNextBtn()
        {
            Thread.Sleep(2000);
            Pages?.ManageInvestmentsPage.WizardSignaturePage.Next();
            Thread.Sleep(1000);
        }

        [When(@"I check on ESign checkbox")]
        public void WhenICheckESignCheckBox()
        {
            if (!Pages.ManageInvestmentsPage.WizardSignaturePage.IsEsignCheckBoxChecked())
            {
                Pages.ManageInvestmentsPage.WizardSignaturePage.CheckEsignCheckBox();
            }
        }
        [Then(@"I should see a search box for funds")]
        public void ThenIShouldSeeASearchBoxForFunds()
        {
            Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifySearchBoxIsDisplayed();
        }

        [When(@"I enter a random stock fund name in the search box")]
        public void WhenIEnterAStockFundNameInTheSearchBox()
        {
            Pages?.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock("AAPL");
        }

        [When(@"I search for stock symbol ""(.*)""")]
        public void WhenISearchForStockSymbol(string symbol)
        {
            Pages?.ManageInvestmentsPage.AvailableInvestmentsTab.searchForStock(symbol);
        }

        [Then(@"I should see all matching funds displayed")]
        public void ThenIShouldSeeAllMatchingFundsDisplayed()
        {
            Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyMatchingFundsDisplayed("AAPL");
        }

        [Then(@"I should see that the ""(.*)"" button is disabled for each fund in the search results")]
        public void ThenIShouldSeeThatTheButtonIsDisabledForEachFundInTheSearchResults(string buttonName)
        {
            if (buttonName == "Buy")
            {
                Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyAllBuyButtonsDisabled();
            }
        }

        [When(@"I navigate to the ""(.*)"" section")]
        public void WhenINavigateToTheSection(string sectionName)
        {
            Pages?.SidebarNavPage.GoToManageInvestments();
        }

        [When(@"I click on the close Investment Option Button")]
        public void WhenIClickOnCloseInvOptBtn()
        {
            Pages?.ManageInvestmentsPage.PreferencesTab.ButtonCloseAccount();
        }

        [When(@"I confirm ""(.*)""")]
        public void WhenIConfirm(string confirmText)
        {
            Pages?.ManageInvestmentsPage.PreferencesTab.IConfirm(confirmText);
        }

        [Then(@"I should see (.*) matching stock\(s\) with symbol ""(.*)""")]
        public void ThenIShouldSeeMatchingStocksWithSymbol(int expectedCount, string symbol)
        {
            var instruments = Pages?.ManageInvestmentsPage.AvailableInvestmentsTab.GetInstrumentList();

            instruments.Should().NotBeNull("Expected an instrument list, but none was found.");
            instruments!.Count.Should().Be(expectedCount, $"Expected {expectedCount} instruments, but found {instruments?.Count ?? 0}.");

            if (expectedCount > 0 && instruments != null)
            {
                foreach (var instrument in instruments)
                {
                    instrument.Should().Contain(symbol, $"Expected instrument symbol to contain '{symbol}', but it did not.");
                }
            }
            else if (expectedCount == 0)
            {
                Pages?.ManageInvestmentsPage.AvailableInvestmentsTab.VerifyNoStocksFound();
                instruments.Should().BeEmpty("Expected no instruments, but some were found.");
            }
        }

        [When(@"I click on the ""(.*)"" tab")]
        public void WhenIClickOnTheTab(string tabName)
        {
            Pages?.ManageInvestmentsPage.ActivityTab.ClickOnManagementTab(tabName);
        }

        [When(@"I click on the ""(.*)"" tab in Manage Investments")]
        public void WhenIClickOnAndTradeTab(string tabName)
        {
            Pages?.ManageInvestmentsPage.SearchAndTradeTab();
        }

        [When(@"I click on the Preferences tab")]
        public void WhenIClickOnPreferencesTab()
        {
            Pages?.ManageInvestmentsPage.ActivityTab.ClickOnPreferencesTab();
        }

        [Then(@"I should see the url contains ""(.*)""")]
        public void ThenIShouldSeeTheUrlContains(string text)
        {
            Thread.Sleep(2000);
            driver.Url.Contains(text);
        }

        [Then(@"I validate the investment accounts displays")]
        public void ThenIValidatedTheInvestmentAccountDisplays()
        {
            Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.VerifyIsVisible(true, true, true);
        }

        [When(@"I click on the Close button for windows pop-up")]
        public void WhenIClickOnTheCloseButtonForWindowsPopUp()
        {
            Pages?.ChooseYourInvestmentPage.CloseWindowsPopup();
        }

        /*[Then(@"I should see the Select's Modal Dialog Message displayed")]
        public void ThenIShouldSeeTheSelectsModalDialogMessageIsDisplayed()
        {
            string actualModalText = Pages.ChooseYourInvestmentPage.VerifySelectModalMessageIsDisplayed();

            string expectedText = "Effective February 26, 2025, the Select option provides streamlined asset allocation models that include four to six mutual fund recommendations (previously 17 to 19). This adjustment was made by an SEC-registered investment adviser (RIA) based on your existing HSA risk tolerance profile. The new model will not affect your current investments unless you choose to make changes.";

            actualModalText.Trim().Should().Contain(expectedText.Trim(),
                because: "Modal should display the correct message");

            string normalizedActualText = System.Text.RegularExpressions.Regex.Replace(actualModalText, @"\s+", " ");
            string normalizedExpectedText = System.Text.RegularExpressions.Regex.Replace(expectedText, @"\s+", " ");

            normalizedActualText.Should().Contain(normalizedExpectedText,
                because: "Modal should display the correct message");
        }*/

        [When(@"I click on TRADE Button")]
        public void WhenIClickOnTradeButton()
        {
            Pages?.ManageInvestmentsPage.SellInstrumentPage.ClickTradeButton();
        }

        [When(@"I click on BUY Button")]
        public void WhenIClickOnBuyButton()
        {
            Pages?.ManageInvestmentsPage.BuyInstrumentPage.ClickBuyButton();
        }

        [When("I click on SELL Button")]
        public void WhenIClickOnSELLButton()
        {
            Pages?.ManageInvestmentsPage.SellInstrumentPage.ClickSellButton();
        }

        [When("I enter more than one dollar amount")]
        public void WhenIEnterMoreThanOneDollarAmount()
        {
            string amountToSell = CommonFunctions.GenerateRandomDollarAmount(1, 5);
            Pages?.ManageInvestmentsPage.SellInstrumentPage.EnterAmount(amountToSell);
        }

        [When("I click on confirm sell Button")]
        public void WhenIClickOnConfirmSellButton()
        {
            Pages?.ManageInvestmentsPage.SellInstrumentPage.ClickConfirmSell();
        }

        [When("I validate success message for sell")]
        public void WhenIValidateSuccessMessage()
        {
            Pages?.NotificationAlert.GetSuccessMessage().Should().Contain("Sale");
            Pages?.NotificationAlert.Dismiss();
        }

        [When("I click on confirm buy Button")]
        public void WhenIClickOnConfirmBuyButton()
        {
            Pages?.ManageInvestmentsPage.BuyInstrumentPage.ClickConfirmBuy();
        }

        [When("I validate success message for buy")]
        public void WhenIValidateSuccessMessageForBuy()
        {
            Pages.NotificationAlert.GetSuccessMessage().Should().Contain("Purchase");
            Pages.NotificationAlert.Dismiss();
        }


        //Search & Trade
        [Then("I validate Status {string} Funds displays and {string} button should be {string}")]
        public void ThenIValidateStatusFundsDisplaysAndButtonEnable(string status, string tradeOption, string tradeOptionBtnStatus)
        {
            Pages.ManageInvestmentsPage.SearchAndTradePage.ValidateInvestmentSearchResult(status, tradeOption, tradeOptionBtnStatus);
        }

        [When("I check Include Unavailable To Buy checkbox")]
        public void WhenICheckIncludeUnavailableToBuyCheckbox()
        {
            Pages.ManageInvestmentsPage.SearchAndTradePage.CheckkUnavailableToBuyButton();
        }

        [Then("I uncheck Include Unavailable To Buy checkbox")]
        public void ThenIUncheckIncludeUnavailableToBuyCheckbox()
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.UnCheckkUnavailableToBuyButton();
            Pages?.ManageInvestmentsPage.AvailableInvestmentsTab.clearStock();
        }

        [Then("I select Fund Type as {string}")]
        public void ThenISelectFundTypeAs(string fundType)
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.SelectFundType(fundType);
        }

        [Then("I validate one or more investment products are available")]
        public void ThenIValidateOneOrMoreInvestmentProductsAreAvailable()
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.ValidateOneOrMoreProductsAvailable();
        }

        [Then("I deslect Fund Type as {string}")]
        public void ThenIDeslectFundTypeAs(string fundType)
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.DeSelectFundType(fundType);
        }

        [Then("I validate zero investment products are available")]
        public void ThenIValidateZeroInvestmentProductsAreAvailable()
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.ValidateZeroProductsAvailable();
        }


        [Then("I select Fund Company as {string}")]
        public void ThenISelectFundCompanyAs(string companyType)
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.SelectCompanyType(companyType);
        }

        [Then("I select Asset Class as {string}")]
        public void ThenISelectAssetClassAs(string assetClassType)
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.SelectAssetClassType(assetClassType);
        }

        [Then("I toggle on index fund")]
        public void ThenIToggleOnIndexFund()
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.toggleIndexFund();
        }

        [Then("I toggle off index fund")]
        public void ThenITogglwOffIndexFund()
        {
            Pages?.ManageInvestmentsPage.SearchAndTradePage.toggleIndexFund();
        }

        [Then(@"I should see the ""(.*)"" banner link")]
        public void ThenIShouldSeeTitle(string linkName)
        {
            if (linkName == "Manage HSA Invest Enrollment")
            {
                Pages?.ManageInvestmentsPage.VerifyManageHsaInvestEnrollIsDisplayed();
            }
        }

        [Then(@"I validate message ""(.*)""")]
        public void ThenIValidateMessage(string linkName)
        {
            if (linkName == "Investment Enrollment has been cancelled")
            {
                Pages?.ManageInvestmentsPage.VerifyInvestEnrollCancelledIsDisplayed();
            }
            else
                Pages?.ManageInvestmentsPage.VerifyYourInvestmentsWillActivateIsDisplayed();
        }

        [When(@"I click on see all funds available in ""(.*)"" option")]
        public void WhenIClickOnAllFundsAvailableAccountType(string accountType)
        {
            switch (accountType)
            {
                case "Select":
                    Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.SeeFundsAvailableInSelectOption();
                    break;
                case "Choice":
                    Pages?.ManageInvestmentsPage.ChooseYourInvestmentPage.SeeFundsAvailableInChoiceOption();
                    break;
                default:
                    throw new ArgumentException($"Invalid account type: {accountType}");
            }
        }

        [Then(@"I verify the title of page should contains ""(.*)""")]
        public void ThenIVerifyTitlePageContainsAccountType(string accountType)
        {
            switch (accountType)
            {
                case "Managed":
                    driver?.Title.Contains("managed");
                    break;
                case "Select":
                    driver?.Title.Contains("select-list");
                    break;
                case "Choice":
                    driver?.Title.Contains("choice-option");
                    break;
                default:
                    throw new ArgumentException($"Invalid account type: {accountType}");
            }
        }

        [Then(@"I validate the HSA Advisory Agreements links for following investment types")]
        public void ThenIValidateHSAAdvisoryAgreementsOpenInNewTab(Table table)
        {
            foreach (var row in table.Rows)
            {
                string investmentType = row["Investment Type"];
                string expectedDocumentKey = row["Document Key"];

                // Click the advisory agreement link based on investment type
                switch (investmentType)
                {
                    case "Select":
                        Pages.ManageInvestmentsPage.ClickHsaAdvisorySelect();
                        break;
                    case "Choice":
                        Pages.ManageInvestmentsPage.ClickHsaAdvisoryChoice();
                        break;
                    case "Managed":
                        Pages.ManageInvestmentsPage.ClickHsaAdvisoryManaged();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(investmentType), $"Unsupported type: {investmentType}");
                }

                // Validate the document key in the new tab's URL
                var allTabs = driver.WindowHandles;
                var newTabHandle = allTabs.FirstOrDefault(handle => handle != driver.CurrentWindowHandle);
                Assert.IsNotNull(newTabHandle, "Expected a new tab to open, but none was found.");
                driver.SwitchTo().Window(newTabHandle);

                string currentUrl = driver.Url;
                string fragment = new Uri(currentUrl).Fragment;
                string actualDocumentKey = null;

                // Handle fragments like "#/document-view?documentKey=..."
                if (fragment.Contains("documentKey="))
                {
                    // Extract query string part after "?"
                    var queryStartIndex = fragment.IndexOf('?');
                    if (queryStartIndex != -1)
                    {
                        string query = fragment.Substring(queryStartIndex + 1); // skip '?'
                        var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                        actualDocumentKey = queryParams.Get("documentKey");
                    }
                }

                Assert.IsNotNull(actualDocumentKey, $"Document key not found in the URL fragment: {fragment}");
                Assert.That(actualDocumentKey, Is.EqualTo(expectedDocumentKey),
                    $"Expected document key to be '{expectedDocumentKey}' for '{investmentType}', but got '{actualDocumentKey}'.");

                // Close the new tab and switch back
                driver.Close();
                var remainingTabHandle = driver.WindowHandles.FirstOrDefault();
                Assert.IsNotNull(remainingTabHandle, "No remaining tab found after closing the current one.");
                driver.SwitchTo().Window(remainingTabHandle);
            }
        }

        [Then(@"I validate the following close investment options are disabled")]
        public void ThenIValidateCloseInvestmentOptionsAreDisabled(Table table)
        {
            foreach (var row in table.Rows)
            {
                var type = row["Investment Type"];

                bool isDisabled = type switch
                {
                    "Select" => Pages.ManageInvestmentsPage.closeInvestmentButtonSelect(),
                    "Choice" => Pages.ManageInvestmentsPage.closeInvestmentButtonChoice(),
                    "Managed" => Pages.ManageInvestmentsPage.closeInvestmentButtonManaged(),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported investment type: {type}")
                };

                Assert.That(isDisabled, Is.True, $"Close Investment option button for '{type}' should be disabled.");
            }
        }

        [Then(@"I validate the following close investment messages are displayed")]
        public void ThenIValidateCloseInvestmentMessagesAreDisplayed(Table table)
        {
            foreach (var row in table.Rows)
            {
                var type = row["Investment Type"];
                var expectedMessage = row["Message"];

                // Perform click on the respective Close Investment button
                switch (type)
                {
                    case "Select":
                        Pages.ManageInvestmentsPage.clickCloseInvestmentButtonSelect();
                        break;
                    case "Choice":
                        Pages.ManageInvestmentsPage.clickCloseInvestmentButtonChoice();
                        break;
                    case "Managed":
                        Pages.ManageInvestmentsPage.clickCloseInvestmentButtonManaged();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported investment type: {type}");
                }

                // Retrieve the actual message shown
                string actualMessage = type switch
                {
                    "Select" => Pages.ManageInvestmentsPage.geCloseInvestmentOptionSelectText(),
                    "Choice" => Pages.ManageInvestmentsPage.geCloseInvestmentOptionChoiceText(),
                    "Managed" => Pages.ManageInvestmentsPage.geCloseInvestmentOptionManagedText(),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported investment type: {type}")
                };

                Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Mismatch in close investment option message for '{type}'.");
            }
        }

        [Then(@"I validate the following Fee messages are displayed for each investment type")]
        public void ThenIValidateFeeMessagesAreDisplayed(Table table)
        {
            foreach (var row in table.Rows)
            {
                var type = row["Investment Type"];
                var expectedMessage = row["Message"];

                // Normalize whitespace for both expected and actual
                string actualMessage = type switch
                {
                    "Fees for Managed" => Pages.ManageInvestmentsPage.GetFeeMessageForManaged(),
                    "Fees for Select" => Pages.ManageInvestmentsPage.GetFeeMessageForSelect(),
                    "Fees for Choice" => Pages.ManageInvestmentsPage.GetFeeMessageForChoice(),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported investment type: {type}")
                };

                actualMessage = Regex.Replace(actualMessage, @"\s+", " ").Trim();
                expectedMessage = Regex.Replace(expectedMessage, @"\s+", " ").Trim();

                Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Fee message mismatch for '{type}'.");
            }
        }

        [Given(@"I Set Investment Funding threshold ""(.*)""")]
        public void GivenISetInvestmentFundingThreshold(string amount)
        {
            string amountToSell = CommonFunctions.FormatDollarAmount(amount);
            Pages?.ManageInvestmentsPage.EnterAmount(amountToSell);
        }

        [Then(@"I click on the Stock ""(.*)"" Button")]
        public void WhenIClickOnTheButton(string buttonName)
        {
            if (buttonName == "ADD")
            {
                Pages?.ManageInvestmentsPage.clickAddStock();
            }
            else if (buttonName == "REVIEW")
            {
                Pages?.ManageInvestmentsPage.clickReviewStock();
            }
            else if (buttonName == "ACCEPT")
            {
                Pages?.ManageInvestmentsPage.clickAcceptStock();
            }
        }

        [Then(@"I validate stocks added in the allocated section")]
        public void ThenIValidateStocksDisplay(Table table)
        {
            foreach (var row in table.Rows)
            {
                var stock = row["stocks"];
                Pages?.ManageInvestmentsPage.verifyStockAdded(stock);
            }
        }

        [Then(@"I allacote equal portion for all added stocks")]
        public void WhenIAllocatePercentageFor()
        {
            Pages?.ManageInvestmentsPage.AllocateEquallyToAllStocks();
        }

        [Then(@"I validate ""(.*)"" account created")]
        public void ThenIValidateAccountCreated(string expectedAccountName)
        {
            Assert.That(Pages?.ManageInvestmentsPage.IsChoiceAccountCreated(), Is.True, $"{expectedAccountName} account was not created or not displayed.");
        }

        [Then(@"I close investment option if investment is active")]
        public void ThenICloseInvestmentOption()
        {
            bool hsaInvest = Pages?.ManageInvestmentsPage.IsDisplayedHSAInvestInfo() ?? false;
            if (hsaInvest)
            {
                Pages?.ManageInvestmentsPage.HSAInvestInfo();
                Pages?.ManageInvestmentsPage.PreferencesTab.ButtonCloseAccount();
                Pages?.ManageInvestmentsPage.PreferencesTab.ISelectCloseAccountReason("The fees are too high");
                Pages?.ManageInvestmentsPage.PreferencesTab.IConfirm("Yes");
            }
        }

        [Then(@"I select the close investment option as ""(.*)""")]
        public void ThenISelectCLoseInvestmentOption(String reasonType)
        {
            Pages?.ManageInvestmentsPage.PreferencesTab.ISelectCloseAccountReason(reasonType);
        }

        [Then(@"I should see both ""(.*)"" and ""(.*)"" radio buttons")]
        public void ThenIShouldSeeBothRadioButtons(string option1, string option2)
        {
            Assert.That(Pages?.ManageInvestmentsPage.BuyInstrumentPage.IsByAmountRadioButtonVisible(),
                Is.True, $"{option1} radio button is not visible.");

            Assert.That(Pages?.ManageInvestmentsPage.BuyInstrumentPage.IsByShareRadioButtonVisible(),
                Is.True, $"{option2} radio button is not visible.");
        }

        [Then(@"I validate that the minimum available to sell should be greater than ""(.*)""")]
        public void ThenIValidateAvailableToSellGreaterThan(string minShares)
        {
            double minSharesValue = CommonFunctions.ExtractNumberFromText(minShares);
            double? actualShares = Pages?.ManageInvestmentsPage.BuyInstrumentPage.GetAvailableToSellAmount();

            Assert.That(actualShares, Is.Not.Null, "Available to Sell value is null.");

            Assert.That(actualShares.Value,
                Is.GreaterThan(minSharesValue),
                $"Available to Sell expected to be greater than {minSharesValue}, but found {actualShares.Value}.");
        }

        [Then(@"I validate that the minimum available to invest should be greater than ""(.*)""")]
        public void ThenIValidateMinimumAmountGreaterThan(string minAmount)
        {
            double minAmountValue = CommonFunctions.ExtractNumberFromText(minAmount);
            double? actualAmount = Pages?.ManageInvestmentsPage.BuyInstrumentPage.GetAvailableToInvestAmount();

            Assert.That(actualAmount, Is.Not.Null, "Available to Invest value is null.");

            Assert.That(actualAmount.Value,
                Is.GreaterThan(minAmountValue),
                $"Available to Invest expected to be greater than {minAmountValue}, but found {actualAmount.Value}.");
        }

        [When(@"I select ""(.*)""")]
        public void WhenISelectOption(string option)
        {
            if (option == "By Share")
            {
                Pages?.ManageInvestmentsPage.BuyInstrumentPage.SelectByShare();
            }
            else if (option == "By Amount")
            {
                Pages?.ManageInvestmentsPage.BuyInstrumentPage.SelectByAmount();
            }
            else
            {
                throw new ArgumentException($"Invalid option: {option}. Expected 'By Share' or 'By Amount'.");
            }
        }

        [When(@"I enter ""(.*)"" as the number of shares")]
        public void WhenIEnterNumberOfShares(string shareCount)
        {
            Pages?.ManageInvestmentsPage.BuyInstrumentPage.EnterNumberOfShares(shareCount);
        }

        //15 July
        [When(@"I click on ""(.*)"" pop-up")]
        public void WhenIClickOnPopUp(string popUp)
        {
            if (popUp == "Dismiss")
                Pages?.NotificationAlert.Dismiss();
            else
            {
                throw new ArgumentException($"Invalid popUp: {popUp}. Expected 'Dismiss'.");
            }
        }

        [When(@"I refresh the application web page")]
        public void RefreshApplicationWebPage()
        {
            driver?.Navigate().Refresh();
        }

        [When(@"I click on ""(.*)"" tab under investment account")]
        public void ClickTabUnderInvestmentAccount(string tabName)
        {
            if (tabName == "Current Holdings")
                Pages?.ManageInvestmentsPage.ClickCurrentHoldingsTab();
            else if (tabName == "Search & Trade")
                Pages?.ManageInvestmentsPage.SearchAndTradeTab();
            else if (tabName == "Activity")
                Pages?.ManageInvestmentsPage.ClickActivityTab();
            else if (tabName == "Documents")
                Pages?.ManageInvestmentsPage.ClickDocumentsTab();
            else if (tabName == "Fees")
                Pages?.ManageInvestmentsPage.ClickFeesTab();
            else
            {
                throw new ArgumentException($"Invalid tabName: {tabName}. Expected 'Activity'.");
            }
        }

        [Then(@"I validate following details for the executed transaction")]
        public void ThenIValidateBuyTransactionDetailsOnChoiceAccount(Table table)
        {
            foreach (var row in table.Rows)
            {
                string dateInitiated = row["Date Initiated"];
                string executedDate = row["Executed Date"];
                string investment = row["Investsment"];
                string transactionType = row["Transaction Type"];
                string status = row["Status"];
                string amount = row["Amount"];

                string today = DateTime.Today.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);

                if (dateInitiated.Equals("Current date", StringComparison.OrdinalIgnoreCase))
                    dateInitiated = today;

                if (executedDate.Equals("Current date", StringComparison.OrdinalIgnoreCase))
                    executedDate = today;

                Pages?.ManageInvestmentsPage.ActivityTab.ValidateTransactionRow(
                    dateInitiated,
                    executedDate,
                    investment,
                    transactionType,
                    status,
                    amount
                );
            }
        }


        [Then(@"I validate ""(.*)"" button displays")]
        public void ValidateButtonDisplays(string buttonText)
        {
            Assert.That(Pages?.ManageInvestmentsPage.ActivityTab.IsCancelButtonDisplayed(), Is.True, $"{buttonText} button should be displayed.");
        }

        [When(@"I click on the ""(.*)"" button in Activity tab")]
        public void ClickButtonInActivityTab(string buttonText)
        {
            if (buttonText == "Cancel")
                Pages?.ManageInvestmentsPage.ActivityTab.ClickCancelButton();
            else
                throw new ArgumentException($"Invalid buttonText: {buttonText}. Expected 'Cancel' or 'Close'.");
        }

        [Then(@"I validate following details for cancellation pop-up in Activity tab")]
        public void ValidateCancellationOptions(Table table)
        {
            foreach (var row in table.Rows)
            {
                string text = row[0];

                if (text.Contains("Are you sure you want to cancel this for ASCGX with $1.00 trade?"))
                {
                    string actualMessage = Pages.ManageInvestmentsPage.ActivityTab.cancelPopupMessage.GetText().Trim();
                    Assert.That(actualMessage, Is.EqualTo(text), "Cancel pop-up message mismatch.");
                }
                else if (text.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.That(Pages.ManageInvestmentsPage.ActivityTab.cancelButton.IsDisplayed(), "Cancel button not displayed.");
                }
                else if (text.Equals("Confirm Cancellation", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.That(Pages.ManageInvestmentsPage.ActivityTab.confirmCancellationButton.IsDisplayed(), "Confirm Cancellation button not displayed.");
                }
                else
                {
                    Assert.Fail($"Unexpected row value: {text}");
                }
            }

            Console.WriteLine("✅ Cancellation pop-up validated.");
        }


        [Then(@"I click on ""(.*)"" button in pop-up")]
        public void ClickButtonInPopup(string buttonName)
        {
            PageControl button = buttonName switch
            {
                "Cancel" => Pages.ManageInvestmentsPage.ActivityTab.cancelButton,
                "Confirm Cancellation" => Pages.ManageInvestmentsPage.ActivityTab.confirmCancellationButton,
                _ => throw new ArgumentException($"Unsupported button: {buttonName}")
            };

            Assert.That(button.IsDisplayed(), $"❌ '{buttonName}' button is not displayed.");
            button.Click();
            Console.WriteLine($"✅ Clicked '{buttonName}' button in pop-up.");
        }


        [Then(@"I validate ""(.*)"" pop-up not displays")]
        public void ValidateCancellationPopupNotDisplayed(string popupName)
        {
            PageControl popup = popupName switch
            {
                "Confirmation" => Pages.ManageInvestmentsPage.ActivityTab.cancelPopupMessage,
                _ => throw new ArgumentException($"Unsupported pop-up: {popupName}")
            };

            Assert.That(!popup.IsDisplayed(), $"❌ '{popupName}' pop-up is still displayed.");
            Console.WriteLine($"✅ '{popupName}' pop-up is not displayed.");
        }

        [When(@"I validate Order was cancelled message")]
        public void ValidatOrderCancelledMessage()
        {
            Pages.NotificationAlert.GetSuccessMessage().Should().Contain("Order was cancelled");
        }

        [When(@"I click on Notification Icon")]
        public void ClickOnNotificationIcon()
        {
            Pages.NotificationAlert.ClickNotificationButton();
        }

        [Then(@"I validate Cancel notification for ""(.*)""")]
        public void ValidateCancelNotificationForAction(string actionType)
        {
            string topNotification = Pages.NotificationAlert.GetTopNotificationText();

            Console.WriteLine($"🔔 Top Notification: {topNotification}");

            bool isMatch = topNotification.Contains("failed", StringComparison.OrdinalIgnoreCase)
                           && topNotification.Contains(actionType, StringComparison.OrdinalIgnoreCase);

            Assert.That(isMatch,
                $"❌ Notification mismatch. Expected action '{actionType}' with failure, but found: '{topNotification}'");

            Console.WriteLine($"✅ Cancel notification for '{actionType.ToUpper()}' validated successfully.");
        }
    }
}

