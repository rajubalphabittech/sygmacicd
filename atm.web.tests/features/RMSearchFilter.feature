Feature: RouteManagerSearchAndFilter
	As a Route Manager User, I want to ensure that
	all the search and filter fields perform as expected

@SYG253
Scenario: User shouldn't see any results if an invalid ShipTo and BillTo are entered
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	And   I view the left routes panel
	When  I filter using a invalid ShipTo and BillTo number in the left routes panel
	Then  I should NOT see any routes returned

@SYG253
Scenario: User can filter routes by valid BillTo and ShipTo values	
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	And   I view the left routes panel
	When  I filter using a valid ShipTo and BillTo number in the left routes panel
	Then  I should only see routes which have contain the BillTo and ShipTo values entered