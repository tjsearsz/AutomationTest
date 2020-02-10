Feature: AutomationTest

@myTest
Scenario: Take five displayed shoes of PUMA from Ebay
	Given User enters on Ebay
	When User searches for shoes
	And User clicks on More filters... option
	And User selects brand "PUMA"
	And User selects size 10
	And User applies filters
	And User orders the results by price in ascendant order
	Then The order of shoes gets displayed correctly
	When User orders the results by name in ascendant order
	And User orders the results by price in descendant order
