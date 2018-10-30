Feature: RouteNotification
	As an ATM user I want to be able to email 1..n customers from the Route Notification page.

@SYG352 @ignore
Scenario: user can email all customers in a route
	Given I am an authenticated ATM user on the "Route Notification" page
	And   I select all stops for a randomly selected route
	When  I try to send notifications
	Then  I should see confirmation that Notifications are sent successfully