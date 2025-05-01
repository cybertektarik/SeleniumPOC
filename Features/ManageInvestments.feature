Feature: Manage Investments - Search Funds

Scenario Outline: Search for stock symbols in Available Investments tab
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	And I click on the "Search & Trade" tab in Manage Investments
	And I search for stock symbol "<symbol>"
	Then I should see <expected_count> matching stock(s) with symbol "<symbol>"

Examples:
	| symbol | expected_count |
	| HTGC   | 1              |
	| NVDA   | 1              |
	| XYZ    | 0              |
	| 123    | 0              |
	| 12*^h  | 0              |

Scenario Outline: Validate Manage Investments Page Tabs
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Managed" Account
	And I click on the "<Tabs>" tab
	Then I should see the url contains "<text>"

Examples:
	| Tabs             | text             |
	| Current Holdings | Current Holdings |
	| Activity         | activity         |
	| Documents        | documents        |
	| Fees             | fees             |

Scenario Outline: Validate Select Investments Page Tabs
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on the "<Tabs>" tab
	Then I should see the url contains "<text>"

Examples:
	| Tabs             | text             |
	| Current Holdings | Current Holdings |
	| Search & Trade   | Search & Trade   |
	| Activity         | activity         |
	| Documents        | documents        |
	| Fees             | fees             |

Scenario Outline: Validate Choice Investments Page Tabs
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	And I click on the "<Tabs>" tab
	Then I should see the url contains "<text>"

Examples:
	| Tabs             | text             |
	| Current Holdings | Current Holdings |
	| Search & Trade   | Search & Trade   |
	| Activity         | activity         |
	| Documents        | documents        |
	| Fees             | fees             |

Scenario: Enrolling a new Select Investment Account
	Given I am logged in as a Pre enrolled user
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Enroll in HSA Invest" banner link
	And I click on the "ENROLL" Button
	Then I should see the "HsaBank Investment ESign Agreement" letter displayed
	When I check on ESign checkbox
	And I click on the Sign Button
	And I click on the Next Button
	And I click on the "Select" Investment account types
	And I click "Retired" employment status
	And I click on the Next Button
	And I check on ESign checkbox
	And I click on the Next Button
 # When I enter name "Test Signature" in the name field
 # And I click on the Sign Button
	And I click on the Next Button
	And I answer question "1" to "stronglyAgree" from the questionnaire
	And I answer question "2" to "stronglyAgree" from the questionnaire
	And I answer question "3" to "stronglyAgree" from the questionnaire
	And I answer question "4" to "stronglyAgree" from the questionnaire
	And I answer question "5" to "stronglyAgree" from the questionnaire
	And I click on the SUBMIT Button
	And I click on the Next Button
	And I click on the Skip Button
	And I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	And I click on the close Investment Option Button
	And I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

Scenario: Enrolling a new Choice Investment Account
	Given I am logged in as a Pre enrolled user
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Enroll in HSA Invest" banner link
	And I click on the "ENROLL" Button
	Then I should see the "HsaBank Investment ESign Agreement" letter displayed
	When I check on ESign checkbox
	And I click on the Sign Button
	And I click on the Next Button
	And I click on the "Choice" Investment account types
	And I click on the Next Button
	And I check on ESign checkbox
	And I click on the Next Button
	And I enter name "Test Signature" in the name field
	And I click on the Sign Button
	And I click on the Next Button
  #And I answer question "1" to "agree" from the questionnaire
  #And I answer question "2" to "agree" from the questionnaire
 # And I answer question "3" to "agree" from the questionnaire
 # And I answer question "4" to "agree" from the questionnaire
 # And I answer question "5" to "agree" from the questionnaire
 # And I click on the SUBMIT Button
 # And I click on the Next Button
	And I click on the Skip Button
	And I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	And I click on the close Investment Option Button
	And I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

Scenario: Enrolling a new Managed Investment Account
	Given I am logged in as a Pre enrolled user
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Enroll in HSA Invest" banner link
	And I click on the "ENROLL" Button
	Then I should see the "HsaBank Investment ESign Agreement" letter displayed
	When I check on ESign checkbox
	And I click on the Sign Button
	And I click on the Next Button
	Then I validate the investment accounts displays
	When I click on the "Managed" Investment account types
	And I click on the Next Button
	And I check on ESign checkbox
	And I click on the Next Button
 # When I enter name "Test Signature" in the name field
 # And I click on the Sign Button
	And I click on the Next Button
	And I answer question "1" to "stronglyAgree" from the questionnaire
	And I answer question "2" to "stronglyAgree" from the questionnaire
	And I answer question "3" to "stronglyAgree" from the questionnaire
	And I answer question "4" to "stronglyAgree" from the questionnaire
	And I answer question "5" to "stronglyAgree" from the questionnaire
	And I answer question "6" to "stronglyAgree" from the questionnaire
	And I answer question "7" to "stronglyAgree" from the questionnaire
	And I answer question "8" to "stronglyAgree" from the questionnaire
	And I answer question "9" to "stronglyAgree" from the questionnaire
	And I answer question "10" to "stronglyAgree" from the questionnaire
	And I click on the Next Button
	And I click on "Yes, I want to choose this portfolio." from the Risk Tolerance Selection
	And I click on the Skip Button
	And I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	And I click on the close Investment Option Button
	And I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

# Scenario: Validate Select's Modal Dialog Message
# Given I am logged in as a user who has an enrolled account
# When I click on "Manage Investment" from the navigation menu
# And I click on the "Select" Account
# Then I should see the Select's Modal Dialog Message displayed
# And I click on the Close button for windows pop-up

Scenario: Validate Learn More Link on Account Selection Page
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	Then I should see the "Learn More" link displayed
	When I click on the "Learn More" link
	Then I should see that each investment account type has a hyperlink

Scenario: Validate Sell Button on the Select account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on TRADE Button
	And I click on SELL Button
	And I enter more than one dollar amount
	And I click on confirm sell Button
	And I validate success message for sell

Scenario: Validate TRADE Button on the Managed account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Managed" Account
	And I click on TRADE Button
	And I click on BUY Button
	And I enter more than one dollar amount
	And I click on confirm buy Button
	And I validate success message for buy

Scenario: Validate Search Button on the Choice account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	And I click on the "Search & Trade" tab in Manage Investments
	And I search for stock symbol "AMZN"
	Then I validate Status "Active" Funds displays and "Buy" button should be "enable"

	When I search for stock symbol "QABA"
	And I check Include Unavailable To Buy checkbox
	Then I validate Status "Unavailable To Purchase" Funds displays and "Buy" button should be "disable"
	And I uncheck Include Unavailable To Buy checkbox

	And I select Fund Type as "Stocks"
	And I validate one or more investment products are available
	And I deslect Fund Type as "Stocks"
	And I validate zero investment products are available

	And I select Fund Type as "ETF"
	And I validate one or more investment products are available
	And I deslect Fund Type as "ETF"
	And I validate zero investment products are available

	And I select Fund Type as "Mutual Funds"
	And I validate one or more investment products are available
	And I deslect Fund Type as "Mutual Funds"
	And I validate zero investment products are available

	And I select Fund Company as "6 Meridian ETF"
	And I validate one or more investment products are available
	And I select Fund Company as "All"
	And I validate zero investment products are available

	And I select Asset Class as "Equity"
	And I validate one or more investment products are available
	And I select Asset Class as "All"
	And I validate zero investment products are available

	And I toggle on index fund
	And I validate one or more investment products are available
	And I toggle off index fund
	And I validate zero investment products are available


   



