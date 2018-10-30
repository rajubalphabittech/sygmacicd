Feature: RouteDetails
	As a Route Tracker user, I want to be able to view the Route Details modal for an
	Added or Removed stop so that I can see details about the Source or Target Route
	without having to search for that route.

@SYG240
Scenario: User can load the Route Details popup for a Removed stop
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select 'Columbus' for the Center
	And   the following Stops columns are selected in Column Options:
         | ColumnName |
         | Comment    |
	When  I try to open the Route Details for a removed stop
