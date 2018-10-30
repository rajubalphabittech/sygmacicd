Feature: CreateViewPayRatesAndScalesPage
As a user with access to ‘View Pay Scales’ page
I should be able to see a view only version of the Pay Rates / Scales page. 
This page should have all the same fields as the Pay Rates / Scales page, 
except dropdowns are replaced with text and text boxes are just shown as text.

Scenario: User can view rates for a given center and pay scale
Given I am an authenticated ATM user on the "View Pay Scales" page
And   I select a random center
When  I select a Pay Scale
Then  I should see a Pay Scale Rates table