//using AngleSharp.Dom;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumPOC.Common;
using SeleniumPOC.EmployeePortal.Pages.Common;
//using TechTalk.SpecFlow;

namespace SeleniumPOC.EmployeePortal.Tests.ManageInvestments
{
    [Binding]
    public class ManageInvestmentsSearchFundsSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver? driver;
        protected AllPages? Pages;
        private string mainTabHandle;

        public ManageInvestmentsSearchFundsSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            driver = _scenarioContext["driver"] as IWebDriver;
            Pages = _scenarioContext["Pages"] as AllPages;
        }

        [Given(@"I am logged in as a user who has not created a Choice account")]
        public void GivenILoginToTheEmployeePortalAsLimitedAccountUser()
        {
            Pages?.LoginPage.Login("Feature2HSABTester005");
        }

        [Given(@"I am logged in as a user who has an enrolled account")]
        public void GivenILoginToTheEmployeePortalAsAnEnrolledUser()
        {
            Pages?.LoginPage.Login("Feature2HSABTester023");
        }

        [Given(@"I am logged in as a Pre enrolled user")]
        public void GivenILoginToTheEmployeePortalAsAPreEnrolledUser()
        {
            Pages?.LoginPage.Login("Feature2HSABTester005");
        }

        [Given(@"I am logged into the Employee Portal")]
        public void GivenILoginToTheEmployeePortalAsUser()
        {
            Pages?.LoginPage.Login("Feature2HSABTester026");
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
                Pages?.ManageInvestmentsPage.HSAInvestInfo();
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
            Pages.ManageInvestmentsPage.SellInstrumentPage.EnterAmount(amountToSell);
        }

        [When("I click on confirm sell Button")]
        public void WhenIClickOnConfirmSellButton()
        {
            Pages.ManageInvestmentsPage.SellInstrumentPage.ClickConfirmSell();
        }

        [When("I validate success message for sell")]
        public void WhenIValidateSuccessMessage()
        {
            Pages.NotificationAlert.GetSuccessMessage().Should().Contain("Sale");
            Pages.NotificationAlert.Dismiss();
        }

        [When("I click on confirm buy Button")]
        public void WhenIClickOnConfirmBuyButton()
        {
            Pages.ManageInvestmentsPage.BuyInstrumentPage.ClickConfirmBuy();
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

        [Then(@"I verfy the title of page should contains ""(.*)""")]
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

        [Then(@"I click on HSA Advisory Agreement ""(.*)""")]
        public void ThenIClickOnHsaAdvisoryAgreement(string agreementType)
        {
            mainTabHandle = driver.CurrentWindowHandle;

            switch (agreementType)
            {
                case "Managed":
                    break;
                case "Select":
                    break;
                case "Choice":
                    break;
                default:
                    throw new ArgumentException($"Invalid agreementType: {agreementType}");
            }

            // Example: Find hyperlink by partial text or another locator strategy
            //var link = driver.FindElement(By.XPath($"//a[contains(text(),'{agreementType}')]"));
            //string openInNewTab = Keys.Control + Keys.Return;
            //link.SendKeys(openInNewTab);

            // Optional: Short delay to allow tab to open
            Thread.Sleep(1000);
        }

        [Then(@"I validate the new tab opens with document key ""(.*)"" in the url")]
        public void ThenIValidateNewTabOpensWithDocumentKey(string documentKey)
        {
            var allTabs = driver.WindowHandles;

            // Switch to the new tab
            string newTabHandle = allTabs.FirstOrDefault(handle => handle != mainTabHandle);
            Assert.IsNotNull(newTabHandle, "New tab did not open.");
            driver.SwitchTo().Window(newTabHandle);

            // Validate URL contains the document key
            string currentUrl = driver.Url;
            Assert.IsTrue(currentUrl.Contains(documentKey), $"Expected URL to contain '{documentKey}', but got: {currentUrl}");
        }

        [Then(@"I close the current tab and switch to main tab")]
        public void ThenICloseCurrentTabAndSwitchToMainTab()
        {
            driver.Close(); // Close current (new) tab
            driver.SwitchTo().Window(mainTabHandle); // Switch back to main
        }

        [Then(@"I validate close investment option is disabled for select, choice and managed")]
        public void ThenIValidateCloseInvestmentOptionIsDisabled()
        {
            var investmentTypes = new List<string> { "select", "choice", "managed" };

            foreach (var type in investmentTypes)
            {
                var closeButton = driver.FindElement(By.Id($"close-{type}-button"));
                bool isDisabled = !closeButton.Enabled || closeButton.GetAttribute("disabled") == "true";

                Assert.IsTrue(isDisabled, $"Close button for '{type}' should be disabled.");
            }
        }

        [Then(@"I validate the ""(.*)"" for select, choice and managed")]
        public void ThenIValidateInvestmentCloseMessage(string expectedMessage)
        {
            var investmentTypes = new List<string> { "select", "choice", "managed" };

            foreach (var type in investmentTypes)
            {
                var messageElement = driver.FindElement(By.Id($"close-message-{type}"));
                string actualMessage = messageElement.Text.Trim();

                Assert.AreEqual(expectedMessage, actualMessage, $"Mismatch in close message for '{type}'.");
            }
        }

        [Then(@"I validate username as ""(.*)""")]
        public void ThenIValidateUsernameAs(string expectedUsername)
        {
            var usernameElement = driver.FindElement(By.Id("username-label"));
            string actualUsername = usernameElement.Text.Trim();

            Assert.AreEqual(expectedUsername, actualUsername, "Username does not match.");
        }

    }
}

