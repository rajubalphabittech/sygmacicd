Feature: RT and RM Activity Log
//As a user I should be able to view, add, and edit the activity log from either the Route Tracker
//or Route Manager pages

@SYG301
Scenario: User should see most recent Activity log comment for a stop in the comment column of the Route Tracker
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I select a random center
	And I view Stops for the first route listed
	When I create an activity log comment with a category of Customer Communication for a random stop
	Then I should see the comment column show the activity log comment I entered

@SYG301
Scenario: A new Activity Log comment is created when the User saves a comment in the Route Tracker
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view Stops for the first route listed
	And   I enter a comment for a random stop
	And   I Submit the changes to the route stops
	When  I view the activity log for the selected stop
	Then  I should see an activity log comment created for the comment I just entered

@SYG301
Scenario: the RT comment column should NOT show the latest Activity Log comment if its' type is 'Driver Communication'
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I select a random center
	And I view Stops for the first route listed
	When I create an activity log comment with a category of Driver Communication for a random stop
	Then I should not see the comment column show the activity log comment I entered

@SYG301
Scenario: the RT comment column should NOT show the latest Activity Log comment if its' type is 'Stop Adjustment'
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view Stops for the first route listed
	When  I create an activity log comment with a category of Stop Adjustment for a random stop
	Then  I should not see the comment column show the activity log comment I entered

@SYG283
Scenario: User can see the Call Logging button when landing on the Activity Log
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	When  I load the activity log for a randomly selected route and stop
	Then  I should see a Call Logging button

@SYG410
Scenario: Route Activity Log Aggregator should be able to display for any route
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	When  I open the Activity Log Aggregator for a random route
	Then  I should see the Activity Log popup loaded for the that route

@SYG407	
Scenario: RT can create Activity Log comment in stop 0 for a route using Quick Add screen
	Given I'm an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I set the Date Range to Current Week
	When  I use the Quick Add activity log to create a comment with a category of Customer Communication for stop number "0" and random route
	Then  I should see the comment I just created in the Aggregate Activity Viewer for the randomly selected route

@SYG407	
Scenario: RM can create Activity Log comment in stop 0 for a route using Quick Add screen
	Given I'm an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	When  I use the Quick Add activity log to create a comment with a category of Customer Communication for a random stop number and route
	Then  I should see that the Quick Add Activity Log comment saved correctly

@SYG407	
Scenario: Save button should be disabled when no Route selected in the Quick Add Activity Log popup
	Given I'm an authenticated ATM user on the "Route Manager" page
	And   I select a random center
	When  I open the Quick Add Activity Log Comment popup
	Then  the 'Activity Log Save Comment' button should be disabled

@SYG417 @SYG455
Scenario: a Time Adjustment comment is created in the activity log when stop adjusted delivery is changed
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I update the Adjusted Delivery value for a random route and stop number
	And   I Submit the changes to the route stops
	When  I open the Activity Log Aggregator for the selected route
	Then  I should see a Time Adjustment comment for the updated stop which shows the Adjusted Delivery time change

@SYG417 @SYG455
Scenario: a Time Adjustment comment is created in the activity log when stop Scheduled Delivery is changed
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I update the Scheduled Delivery value for a random route and stop number
	And   I Submit the changes to the route stops
	When  I open the Activity Log Aggregator for the selected route
	Then  I should see a Time Adjustment comment for the updated stop which shows the Scheduled Delivery time change