Feature: StopMove
	As a Route Manager user, I should be able to move a 
	Stop between routes. After movving a stop, I should 
	see both the source and target stops reflected in 
	both the Route Manager and Route Tracker.

@SYG225
Scenario: alert should throw if the New Stop Number entered already exists in the target route
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I open the move stop prompt for a stop eligible for stop move
	And   I enter all target route information using a stop number that already exists
	When  I save the stop move prompt
	Then  I should see an alert saying 'NEW STOP NUMBER is already used. Please enter a different Stop Number.'

@SYG225
Scenario: alert should throw if dispatch date time isn't correctly formatted
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I open the move stop prompt for a stop eligible for stop move
	And   I enter all target route information using an incorrectly formatted date time value
	When  I save the stop move prompt
	Then  I should see an alert saying 'New Delivery Date/Time is invalid.'

@SYG106
Scenario: can successfully move a stop
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I open the move stop prompt for a stop eligible for stop move
	And   I enter all target route information
	When  I save the stop move prompt
	Then  the target route should show the added stop in the database

@SYG241
Scenario: Target route section can calculate capacity impacts correctly
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I open the move stop prompt for a stop eligible for stop move
	When  I enter all target route information
	Then  the Stop Move Target Route section should calculate Capacity totals impacts correctly
	
@SYG241
Scenario: Source route section can calculate capacity impacts correctly
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I open the move stop prompt for a stop eligible for stop move
	When  I enter all target route information
	Then  the Stop Move Source Route section should calculate Capacity totals impacts correctly

@SYG379
Scenario: stop move button is disabled and shows correct tooltip for center without move permissions
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Carolina' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I select the route found using the API and display the stops table
	Then  the Stop Move icon is disabled for stops with an order and shows a tooltip of "Center locks stop movement."
	Then  the Stop Move icon is disabled for stops without an order and shows a tooltip of "Can not move a stop without order."

@SYG406
Scenario: User should see Scheduled Delivery time for source stop within the Stop Move popup
	Given I am an authenticated ATM user on the "Route Manager" page	
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	When  I open the move stop prompt for a stop eligible for stop move
	Then  the Scheduled Delivery should appear in the Source Route section of the Stop Move popup

@SYG404
Scenario: user can successfully move a stop using the Quick Stop Move popup
	Given I'm an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I use the API to find a stop which is eligible for stop move
	And   I attempt to move the stop using the Quick Stop Move popup
	Then  the target route should show the added stop in the database

@SYG384
Scenario: alert thrown if dispatched route is selected as Source Route in the stop move popup
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	When  I select a Source Route from the Quick Stop Move popup which has orders and has been dispatched
	Then  I should see an alert saying 'The selected SOURCE ROUTE cannot be moved because it has been dispatched.'

@SYG384
Scenario: alert thrown if dispatched route is selected as Destination Route in the stop move popup
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	When  I select a Destination Route from the Quick Stop Move popup which has orders and has been dispatched
	Then  I should see an alert saying 'The selected DESTINATION ROUTE cannot be moved because it has been dispatched.'