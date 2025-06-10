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
	#Given I am logged in as a Pre enrolled user
	Given I am logged into the Employee Portal
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
	And I enter name "Test Signature" in the name field
	And I click on the Sign Button
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
	#Given I am logged in as a Pre enrolled user
	Given I am logged into the Employee Portal
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
	#And I enter name "Test Signature" in the name field
	#And I click on the Sign Button
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
	#Given I am logged in as a Pre enrolled user
	Given I am logged into the Employee Portal
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
	When I click on see all funds available in "Choice" option
	Then I verify the title of page should contains "Choice"
	When I click on the "Learn More" link
	When I click on see all funds available in "Select" option
	Then I verify the title of page should contains "Select"
	When I click on the "Return" link
	When I click on the "Managed Learn More" link
	Then I verify the title of page should contains "Managed"
	When I click on the "Return" link

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



Scenario: Validate Pre Enrollment for Select Account
	Given I am logged in as a Pre enrolled user
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Enroll in HSA Invest" banner link
	And I click on the "START ENROLLMENT" Button
	Then I should see the "HsaBank Investment Esign Agreement" letter displayed
	When I check on ESign checkbox
	And I click on the Sign Button
	And I click on the Next Button
	And I click on the "Select" Investment account types
	And I click on the Next Button
	And I check on ESign checkbox
	And I click on the Next Button
	#When I enter name "Test Signature" in the name field
	##When I click on the Sign Button
	And I click on the Next Button
	And I answer question "1" to "agree" from the questionnaire
	And I answer question "2" to "agree" from the questionnaire
	And I answer question "3" to "agree" from the questionnaire
	And I answer question "4" to "agree" from the questionnaire
	And I answer question "5" to "agree" from the questionnaire
	And I click on the SUBMIT Button
	And I click on the Next Button
	And I click on the Skip Button
	Then I should see the "Manage HSA Invest Enrollment" banner link
	When I click on the "Manage HSA Invest Enrollment" banner link
	Then I validate message "Your investments will activate after you reach a minimum HSA cash balance of $1,000.00.Your current HSA cash balance is $0.00."
	When I click on the "Cancel Enrollment" Button
	Then I validate message "Investment Enrollment has been cancelled"

Scenario: Validate HSA Advisory Agreements link for all investment types
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on "Resources" from the navigation menu
	And I click on the "HSA Invest" info link
	Then I validate the HSA Advisory Agreements links for following investment types
		| Investment Type | Document Key                      |
		| Select          | HSA_Curated_Advisory_Agreement_LH |
		| Choice          | HSA_Choice_Advisory_Agreement_LH  |
		| Managed         | abg_advisory_managed              |


Scenario: Validate Close Investment Option Is Disabled and Message Is Displayed When Holdings Exist
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	Then I validate the following close investment options are disabled
		| Investment Type |
		| Select          |
		| Choice          |
		| Managed         |
	And I validate the following close investment messages are displayed
		| Investment Type | Message                                                                                             |
		| Select          | To close your investment option, you must first sell all your holdings to bring your balance to $0. |
		| Choice          | To close your investment option, you must first sell all your holdings to bring your balance to $0. |
		| Managed         | To close your investment option, you must first sell all your holdings to bring your balance to $0. |



	Scenario: Enrolling a new Choice Investment Account with Threshold
	Given I am logged into the Employee Portal
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
    And I Set Investment Funding threshold "$500.00"
	And I search for stock symbol "AMZN"
	And I click on ADD Button
	Then I validate Fund display
	And I search for stock symbol "NVDA"
	And I click on ADD Button
	Then I validate Fund display
	And I allacote "%50" for "AMZN"
	And I allacote "%50" for "NVDA"
	And I click on the "REVIEW" Button
	Then I validate Fund display
	And I click on the "ACCEPT" Button
	Then I validate "Choice" account created
	And I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	And I click on the close Investment Option Button
	And I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays


	Scenario: Validate Fees Tab Displays Correct Annual Fees for Each Investment Account Type
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Managed" Account
	And I click on the "<Tabs>" tab
	Then I should see the url contains "<text>"
	And I validate the following close investment messages are displayed
		| Investment Type  | Fees                                                                                             |
		| Fees for Managed | Quarterly min: $2.50   Quarterly max: $50.00  |
		| Fees for Select  | Quarterly min: $2.50  Quarterly max: $37.50   |
		| Fees for Choice  | Annual fee 0.15% of AUA Quarterly max: $24.00 |
   



