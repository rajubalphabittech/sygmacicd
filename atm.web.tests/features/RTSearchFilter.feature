Feature: Route Tracker Search Filter
	As a Route Tracker User, I want to ensure that
	all the search and filter fields perform as expected

Scenario: User can enter a custom date range for routes
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I enter a custom date range
	Then  I should see a list of results returned within that date range

@SYG248
Scenario: Number of Routes count should set to zero if user sets the Center to 'Select Center...'
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I select "Select Center..." from the Center DropDown
	Then I should see the Row Count indicator set to "0"

@SYG263
Scenario: Selecting Today's Date Range should show routes from today through tomorrow
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I set the Date Range to Today
	Then I should see the From Date set to today's date
	And I should see the To Date set to tomorrow's date
	And I should only see Routes displayed which are within the date range displayed
	
@SYG263
Scenario: Selecting Tomorrow's Date Range should show routes from Tomorrow through the following day
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I set the Date Range to Tomorrow
	Then I should see the From Date set to tomorrow's date
	And I should see the To Date set to day after tomorrow's date
	And I should only see Routes displayed which are within the date range displayed

@SYG263
Scenario: Selecting Yesterday's Date Range should show routes from Tomorrow through the following day
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I set the Date Range to Yesterday
	Then I should see the From Date set to yesterday's date
	And I should see the To Date set to today's date
	And I should only see Routes displayed which are within the date range displayed

@SYG253
Scenario: User shouldn't see any Routes if an invalid ShipTo and BillTo are entered
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	And I enter an invalid ShipTo and BillTo number
	When I click the Search button
	Then I should see 0 routes returned

@SYG253
Scenario: User can only enter numeric values for ShipTo
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I search for a ShipTo value of "2+1-"
	Then I should see "21" in the ShipTo field

@SYG253
Scenario: User can only enter numeric values for BillTo
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	When I search for a BillTo value of "2+1-"
	Then I should see "21" in the BillTo field

@SYG279
Scenario: User can filter by routes with Some Orders
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center 
	When  I set the Date Range to Current Week
	When  I set the With Order Option filter to "Partial Orders"
	Then  the routes shown should have at least one stop with an order and at least one stop without an order

@SYG279
Scenario: User can filter by routes with All Orders Only
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center 
	When  I set the Date Range to Current Week
	When  I set the With Order Option filter to "All Orders Only"
	Then  the routes shown should have an order for all stops excluding removed stops
	
@SYG279
Scenario: User can filter by routes with No Orders
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center 
	When  I set the Date Range to Current Week
	When  I set the With Order Option filter to "No Orders"
	Then  the routes shown should have no orders for all stops
	
@SYG133
Scenario: user can see total number of orders versus stops for a route
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   the following Routes columns are selected in Column Options:
         | ColumnName |
         | Route #    |
	When  I set the Date Range to Current Week
	When  I set the With Order Option filter to "Partial Orders"
	Then  I should see the total number of orders versus stops for the routes shown