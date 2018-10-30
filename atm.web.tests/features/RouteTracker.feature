Feature: RouteTrackerPage

@SYG228
Scenario: User can update and save Adjusted Delivery Time for a stop that has not been dispatched
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I select a random center
	And I view stops for a route which hasn't been dispatched
	And I increase the Adjusted Delivery value by "30" minutes for a random stop
	When I Submit the changes to the route stops
	Then the Adjusted Delivery value should have changed to the entered value

@SYG214
Scenario: Offset Hours value for a stop will reset to 0 if user clears the input
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I select a random center
	And I view Stops for the first route listed
	And I set the Adjusted Offset Hours to "" for a random stop
	When I Submit the changes to the route stops
	Then I should see the Adjusted Delivery value equal to the Scheduled Delivery

@SYG288
Scenario: When Scheduled Delivery changed then the Adjusted Delivery should use that value
	Given I am an authenticated ATM user on the "Route Tracker" page
	And  I select a random center
	And  I view Stops for the first route listed
	When I change the Scheduled Delivery time for a random stop
	Then I should see the Adjusted Delivery value equal to the Scheduled Delivery

@SYG335
Scenario: clicking Adjusted Delivery Cascade button will apply the offset hours value to all stops below the one clicked
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I select a random center
	And I view Stops for the first route listed
	And I set the Adjusted Offset Hours to "1.0" for a random stop
	When I click the Adjusted Cascade button for the above selected stop
	Then I should see the Adjusted Offset Hours field set to the value entered for every stop below

@SYG381
Scenario: updating adjusted delivery time for stop zero should persist on reload
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view Stops for the first route listed
    And   I increase the Adjusted Delivery value by "30" minutes for Stop "0"
	And   I Submit the changes to the route stops
	When  I reload the Stops Table for the selected route
	Then  the Adjusted Delivery value for Stop "0" should have been updated

@SYG215
Scenario: Actual Arrival Time should be blank for stops which have not been delivered
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I set the Date Range to Tomorrow
	And the following Stops columns are selected in Column Options:
	| ColumnName          |
	| Actual Arrival Time |
	When  I view Stops for the first route listed
	Then  the Arrival Delivery Date column should be empty for all stops that haven't been delivered

Scenario: removed stop will not allow user to add a comment in the Route Tracker stops table
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I set the Date Range to Current Week
	And   I set the Route Modification Types filter to "Removed"
	And   I view Stops for the first route listed
	Then  I should see the Comment textbox disabled for all removed stops and shows a tooltip of "Use Activity Log to add comment."
	
Scenario: Route Capacity should match sum of stops capactity
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   the following Routes columns are selected in Column Options:
	| ColumnName |
	| Cases      |
	| Cubes      |
	| LBs        |
	When  I select a random center
	Then  the Route capacity should match the sum of stops capacity for all routes shown 

Scenario: All stops on a random route should contain an Adjusted Delivery time
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	When  I view the Stops Table for a random route
	Then  Every stop in the current route should have an Adjusted Delivery time

@SYG452
Scenario: Actual Arrival Time should show the correct tooltip
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   the following Stops columns are selected in Column Options:
	| ColumnName          |
	| Actual Arrival Time |
	When  I view Stops for the first route listed
	Then  the "Actual Arrival Time" column header should have a tooltip which says "Actual Arrival Delivery Time from Telogis."

@SYG405
Scenario: Changing Adjusted Delivery for stop 0 should update the Adjusted Dispatch Time
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   the following Routes columns are selected in Column Options:
	| ColumnName             |
	| Adjusted Dispatch Time |
	And   I view the Stops Table for a random route
	And   I increase the Adjusted Delivery value by "30" minutes for Stop "0"
	Then  I should see the Adjusted Dispatch value match the Adjusted Delivery value for Stop 0

@SYG455
Scenario: Cascading Scheduled Delivery changes should update Scheduled Offset and Adjusted Offset for all stops below the one clicked
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view Stops for the first route listed
	And   I set the Scheduled Offset Hours to "1.0" for a random stop
	When  I click the Scheduled Cascade button for the above selected stop
	Then  I should see the Scheduled Offset Hours field set to the value entered for every stop below