Feature: Manage Investments - Search Funds

@feature2
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

@feature2
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

@feature2
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

@feature2
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

@feature2
Scenario: Enrolling a new Select Investment Account
	Given I am logged into the Employee Portal
	When I click on "Settings" from the navigation menu
	Then I close investment option if investment is active
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
	Then I select the close investment option as "The platform is hard to use"
	When I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

@feature2
Scenario: Enrolling a new Choice Investment Account
	Given I am logged into the Employee Portal
	When I click on "Settings" from the navigation menu
	Then I close investment option if investment is active
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
	Then I select the close investment option as "The platform is hard to use"
	When I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

@feature2
Scenario: Enrolling a new Managed Investment Account
	Given I am logged into the Employee Portal
	When I click on "Settings" from the navigation menu
	Then I close investment option if investment is active
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
	#And I enter name "Test Signature" in the name field
	#And I click on the Sign Button
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
	Then I select the close investment option as "The platform is hard to use"
	When I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

# Scenario: Validate Select's Modal Dialog Message
# Given I am logged in as a user who has an enrolled account
# When I click on "Manage Investment" from the navigation menu
# And I click on the "Select" Account
# Then I should see the Select's Modal Dialog Message displayed
# And I click on the Close button for windows pop-up

@feature2
Scenario: Validate Learn More Link on Account Selection Page
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	Then I should see the "Learn More" link displayed
	When I click on the "Learn More" link
	Then I should see that each investment account type has a hyperlink
	When I click on see all funds available in "Choice" option
	Then I verify the title of page should contains "Choice"
	When I click on the "Learn More" link
	And I click on see all funds available in "Select" option
	Then I verify the title of page should contains "Select"
	When I click on the "Return" link
	And I click on the "Managed Learn More" link
	Then I verify the title of page should contains "Managed"
	When I click on the "Return" link

@external
Scenario: Validate Sell Button on the Select account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on TRADE Button
	And I click on SELL Button
	And I enter 2 dollar amount
	And I click on confirm sell Button
	And I validate success message for sell

@external
Scenario: Validate TRADE Button on the Managed account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Managed" Account
	And I click on TRADE Button
	And I click on BUY Button
	And I enter 2 dollar amount
	And I click on confirm buy Button
	And I validate success message for buy
	
@feature2
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


@feature2
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

@feature2
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


@feature2
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


@feature2
Scenario: Enrolling a new Choice Investment Account with Threshold
	Given I am logged into the Employee Portal
	When I click on "Settings" from the navigation menu
	Then I close investment option if investment is active
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
	Given I Set Investment Funding threshold "500"
	When I search for stock symbol "AMZN"
	Then I click on the Stock "ADD" Button
	When I search for stock symbol "NVDA"
	Then I click on the Stock "ADD" Button
	And I validate stocks added in the allocated section
		| stocks |
		| AMZN   |
		| NVDA   |
	And I allacote equal portion for all added stocks
	And I click on the Stock "REVIEW" Button
	And I click on the Stock "ACCEPT" Button
	And I validate "Choice" account created
	When I click on "Settings" from the navigation menu
	And I click on the "HSA Invest Info" info link
	And I click on the close Investment Option Button
	Then I select the close investment option as "Other (please specify)"
	When I confirm "Yes"
	And I click on "Manage Investment" from the navigation menu
	Then I should see "Enroll in HSA Invest" banner link displays

@feature2
Scenario: Validate Fees Tab Displays Correct Annual Fees for Each Investment Account Type
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Managed" Account
	And I click on the "Fees" tab
	Then I should see the url contains "Fees"
	And I validate the following Fee messages are displayed for each investment type
		| Investment Type  | Message                                                               |
		| Fees for Managed | Annual fee:1 0.80% of AUA2 Quarterly min: $2.50 Quarterly max: $50.00 |
		| Fees for Select  | Annual fee:1 0.30% of AUA2 Quarterly min: $2.50 Quarterly max: $37.50 |
		| Fees for Choice  | Annual fee:1 0.15% of AUA2 Quarterly max: $24.00                      |

@external
Scenario: Validate Buy with Share on the Select account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on TRADE Button
	And I click on BUY Button
	Then I should see both "By Amount" and "By Share" radio buttons
	And I validate that the minimum available to invest should be greater than "$1000"
	When I select "By Share"
	And I enter "1" as the number of shares
	And I click on confirm buy Button
	And I validate success message for buy

@external
Scenario: Validate Sell with Share on the Choice account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	And I click on TRADE Button
	And I click on SELL Button
	Then I should see both "By Amount" and "By Share" radio buttons
	And I validate that the minimum available to sell should be greater than "10"
	When I select "By Share"
	And I enter "1" as the number of shares
	And I click on confirm sell Button
	And I validate success message for sell

@external
Scenario: Verify CANCEL button functionality and cancellation notification after Sell By Amount from Select account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on "Activity" tab under investment account
	Then I click on cancel button for pending transcations
	When I click on "Current Holdings" tab under investment account
	And I click on TRADE Button
	And I click on SELL Button
	Then I should see both "By Amount" and "By Share" radio buttons
	When I enter 1 dollar amount
	And I click on confirm sell Button
	And I validate success message for sell
	And I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 2 times
	When I click on the "Select" Account
	Then I refresh the application web page 1 times
	When I click on "Activity" tab under investment account
	Then I validate "Cancel" button displays
	When I click on the "Cancel" button in Activity tab
	Then I validate following details for cancellation pop-up in Activity tab
		| Are you sure you want to cancel this for ASCGX with $1.00 trade? |
		| Cancel                                                           |
		| Confirm Cancellation                                             |
	And I click on "Cancel" button in pop-up
	And I validate cancel pop-up not displays
	When I click on the "Cancel" button in Activity tab
	Then I click on "Confirm Cancellation" button in pop-up
	When I validate Order was cancelled message
	And I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 1 times
	When I click on the "Select" Account
	And I click on "Activity" tab under investment account
	Then I validate following details for the executed transaction
		| Date Initiated | Executed Date | Investsment | Transaction Type | Status   | Amount |
		| Current date   | Current date  | ASCGX       | Sell             | Canceled | $0.00  |
	When I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 1 times
	When I click on Notification Icon
	Then I validate Cancel notification for "Sell"


@external
Scenario: Verify CANCEL button functionality and cancellation notification after Buy By Amount from Select account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Select" Account
	And I click on "Activity" tab under investment account
	Then I click on cancel button for pending transcations
	When I click on "Current Holdings" tab under investment account
	And I click on TRADE Button
	And I click on BUY Button
	Then I should see both "By Amount" and "By Share" radio buttons
	When I enter 1 dollar amount
	And I click on confirm buy Button
	And I validate success message for buy
	And I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 2 times
	When I click on the "Select" Account
	Then I refresh the application web page 1 times
	When I click on "Activity" tab under investment account
	Then I validate "Cancel" button displays
	When I click on the "Cancel" button in Activity tab
	Then I validate following details for cancellation pop-up in Activity tab
		| Are you sure you want to cancel this for ASCGX with $1.00 trade? |
		| Cancel                                                           |
		| Confirm Cancellation                                             |
	And I click on "Cancel" button in pop-up
	And I validate cancel pop-up not displays
	When I click on the "Cancel" button in Activity tab
	Then I click on "Confirm Cancellation" button in pop-up
	When I validate Order was cancelled message
	And I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 1 times
	When I click on the "Select" Account
	And I click on "Activity" tab under investment account
	Then I validate following details for the executed transaction
		| Date Initiated | Executed Date | Investsment | Transaction Type | Status   | Amount |
		| Current date   | Current date  | ASCGX       | Buy              | Canceled | $0.00  |
	When I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 1 times
	When I click on Notification Icon
	Then I validate Cancel notification for "Buy"


@external
Scenario: Validate Buy with Share on the Choice account and validate from Activity page
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	And I click on "Activity" tab under investment account
	Then I click on cancel button for pending transcations
	When I click on the "Search & Trade" tab in Manage Investments
	And I search for stock symbol "AMZN"
	And I click on BUY Button
	Then I should see both "By Amount" and "By Share" radio buttons
	And I validate that the minimum available to invest should be greater than "$1000"
	When I enter 1 dollar amount
	And I click on confirm buy Button
	And I validate success message for buy
	And I click on "Manage Investment" from the navigation menu
	Then I refresh the application web page 1 times
	When I click on the "Choice" Account
	And I click on "Activity" tab under investment account
	Then I validate following details for the pending transaction
		| Date Initiated | Investsment | Transaction Type | Amount |
		| Current date   | AMZN        | Buy              | $1.00  |

@feature2
Scenario: Validate View Performance Data Link Buy for Choice account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	Then I validate View Performance Data link for all available investments
	And I validate the following options are displayed in View Performance Data
		| Trade               |
		| Buy                 |
		| Sell                |
		| Add To Auto Funding |

@feature2
Scenario: Validate Setup and Suspension of Automated Investing (Auto-Funding) for Choice Account
	Given I am logged in as a user who has an enrolled account
	When I click on "Manage Investment" from the navigation menu
	And I click on the "Choice" Account
	Then I suspend MANAGE AUTOMATED INVESTING if it exists
	And I verify that the "SETUP AUTOMATED INVESTING" link is displayed
	When I click on the "SETUP AUTOMATED INVESTING" link
	Then I should be navigated to the "Auto Funding" page
	And I verify the following options are displayed in Auto Funding:
		| Cancel   |
		| Activate |
	And I click on the "ACTIVATE" button in Auto Funding
	And I click on the "REVIEW" button in Auto Funding
	And I click on the "ACCEPT" button in Auto Funding
	And I verify that the "MANAGE AUTOMATED INVESTING" link is displayed
	And I verify the message "Cash balance funds in excess of $100.00 will automatically be moved to your investments" is shown above the investment list
	When I click on the "MANAGE AUTOMATED INVESTING" link
	Then I should be navigated to the "Auto Funding" page
	And I verify the following options are displayed in Auto Funding:
		| Cancel  |
		| Suspend |
	And I click on the "SUSPEND" button in Auto Funding
	And I verify that the "SETUP AUTOMATED INVESTING" link is displayed





   
   



