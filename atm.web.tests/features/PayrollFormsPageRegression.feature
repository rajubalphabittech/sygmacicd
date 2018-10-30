Feature: PayrollFormsPageRegression
	As a user I want to verify that basic core features on the Payroll
	Forms page are working as expected

@regression
Scenario: User should be able to create a new payroll form
	Given I am an authenticated ATM user on the "Payroll Forms" page
	And I open a blank payroll form
	When I enter valid information for a Regular form with a current weekending date
	And I try to create the payroll form
	Then I should see the newly created form