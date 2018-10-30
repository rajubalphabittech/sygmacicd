Feature: Route Manager
	As an ATM user I should be able to access and use the Route manager page

Scenario: Can load all Stop Markers for a route in Google Maps
	Given I am an authenticated ATM user on the "Route Manager" page
	And I select a random center
	When I select a route with orders from the Left Route section
	Then I should see All Stop Markers Rendered in the Google Maps window

@SYG222
Scenario: Stop Marker Labels should show the stop number they represent
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	When  I select a route with orders from the Left Route section
	Then  each Stop Marker in the route should have the stop number in the label 

@SYG183
Scenario: Stops Table should display when user clicks the expand chevron for that route
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	When  I select a route with orders from the Left Route section
	And   I click the chevron to expand the stops list
	Then  The list of stops for the first selected route should display

@SYG183
Scenario: Stops list should NOT display by default when you select a route
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	When  I select a route with orders from the Left Route section
	Then  I should not see the list of stops displayed

@SYG245
Scenario: Stops list should close when the route is deselected
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	When  I select a route with orders from the Left Route section
	And   I click the chevron to expand the stops list
	When  I deselect the selected route
	Then  I should not see the list of stops displayed

@SYG183
Scenario: Stops list should reload for different routes
	Given I am an authenticated ATM user on the "Route Manager" page
	And   I select 'Columbus' for the Center
	And   I select and display stops for a route in the Left Route section
	When  I select and display stops for a route in the Left Route section
	Then  The list of stops for the second selected route should display