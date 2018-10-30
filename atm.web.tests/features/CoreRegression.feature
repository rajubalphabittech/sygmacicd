Feature: Core Regression Scenarios
#These are the core user functions required to determine if the ATM is stable before deploying

@regression @CreatePayrollForm
Scenario: User can view details for a payroll form
	Given I am an authenticated ATM user on the "Payroll Forms" page
	When I try to view details for a payroll form
	Then I should see details for the given payroll form

@regression @SYG289 
Scenario: User should be able to enter a new route without error
	Given I am an authenticated ATM user on the "Manage Routes" page
	When I select a random center
	When I complete the Form to add a new Route
	Then The new route should be saved

@regression
Scenario: User can view employees on the Manage Employees page
	Given I am an authenticated ATM user on the "Manage Employees" page
	When I select a random center
	Then I should see a list of employees returned
