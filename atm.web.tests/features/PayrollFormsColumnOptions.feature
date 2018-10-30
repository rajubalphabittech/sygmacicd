Feature: Payroll Forms Column Options
#Scope of this story is to update the Payroll Forms Page to allows a user to
#customize the columns they see, adjust column order, and column width. 
#These changes will be available the next time a user logs in.

Scenario: User cannot save column customization if no changes have been made
	Given I am an authenticated ATM user on the "Payroll Forms" page
	And I open the Customize Columns window
	Then the 'Save Column Selection' button should be disabled

Scenario: User can save Column Options popup once a change is made
	Given I am an authenticated ATM user on the "Payroll Forms" page
	And I open the Customize Columns window
	When I remove 1 column from the Selected Column list
	Then the 'Save Column Selection' button should be enabled