Feature: Route Tracker Column Options
	
@SYG314
Scenario: User can't move Columns in the Routes - Selected Columns table beyond the top of the list
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	And I select 3 column(s) from the Routes - Selected Columns table
	When I try to move the Selected Routes columns past the top of the list
	Then I should still see the moved columns in the list in Routes Selected List

@SYG314
Scenario: User can't move Columns in the Routes - Selected Columns table beyond the bottom of the list
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	And I select 4 column(s) from the Routes - Selected Columns table
	When I try to move the Selected Routes columns past the bottom of the list
	Then I should still see the moved columns in the list in Routes Selected List
	
@SYG314
Scenario: User can't move Columns in the Stops - Selected Columns table beyond the top of the list
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	And I select 4 column(s) from the Stops - Selected Columns table
	When I try to move the Selected Stops columns past the top of the list
	Then I should still see the moved columns in the list in Stops Selected List

@SYG314
Scenario: User can't move Columns in the Stops - Selected Columns table beyond the bottom of the list
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	And I select 5 column(s) from the Stops - Selected Columns table
	When I try to move the Selected Stops columns past the bottom of the list
	Then I should still see the moved columns in the list in Stops Selected List

@SYG268
Scenario: user can't move Scheduled Delivery column to Available list by double clicking
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	When I double left click on the 'Scheduled Delivery' column
	Then I should not see the 'Scheduled Delivery' column in the Stops Available list
	
@SYG268
Scenario: user can't move Adjusted Delivery column to Available list by double clicking
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	When I double left click on the 'Adjusted Delivery' column
	Then I should not see the 'Adjusted Delivery' column in the Stops Available list
		
@SYG268 @SYG455
Scenario Outline: user can't move Offset Hours columns to Available list by double clicking
	Given I am an authenticated ATM user on the "Route Tracker" page
	And I view the Column Options popup
	When I double left click on the '<Offset Hours Column Name>' column
	Then I should not see the '<Offset Hours Column Name>' column in the Stops Available list
	Examples: 
	| example description           | Offset Hours Column Name         |
	| Adjusted Offset Hours Column  | Adjusted Offset Hours / Cascade  |
	| Scheduled Offset Hours Column | Scheduled Offset Hours / Cascade |

@SYG453
Scenario: Routes Selected Columns shows correct option after clicking Reset To Default
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view the Column Options popup
	When  I click the 'Reset To Default' button
	Then  I should see the following options in the 'Routes Selected Columns' select list
	| Column                  |
	| Route #                 |
	| Route Name              |
	| Stops                   |
	| Concepts                |
	| Scheduled Dispatch Time |
	| Adjusted Dispatch Time  |
	| Primary Driver          |
	| Secondary Driver        |
	| Team Driver/Helper      |

@SYG453
Scenario: Routes Available Columns shows correct option after clicking Reset To Default
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view the Column Options popup
	When  I click the 'Reset To Default' button
	Then  I should see the following options in the 'Routes Available Columns' select list
	| Column                |
	| Cases                 |
	| Cooler Cases          |
	| Cooler Cubes          |
	| Cooler LBs            |
	| Cubes                 |
	| Dry Cases             |
	| Dry Cubes             |
	| Dry LBs               |
	| Freezer Cases         |
	| Freezer Cubes         |
	| Freezer LBs           |
	| LBs                   |
	| Trailer Id            |
	| Truck Id              |

@SYG453
Scenario: Stops Selected Columns shows correct option after clicking Reset To Default
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view the Column Options popup
	When  I click the 'Reset To Default' button
	Then  I should see the following options in the 'Stops Selected Columns' select list
	| Column                           |
	| Stop                             |
	| Status                           |
	| Bill To                          |
	| Ship To                          |
	| Customer                         |
	| Concept                          |
	| LBs                              |
	| Cases                            |
	| Planned Delivery                 |
	| Scheduled Delivery               |
	| Scheduled Offset Hours / Cascade |
	| Adjusted Delivery                |
	| Adjusted Offset Hours / Cascade  |
	| Actual Arrival Time              |
	| Comment                          |

@SYG453
Scenario: Stops Available Columns shows correct option after clicking Reset To Default
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   I view the Column Options popup
	When  I click the 'Reset To Default' button
	Then  I should see the following options in the 'Stops Available Columns' select list
	| Column              |
	| City                |
	| Cooler Cases        |
	| Cooler Cubes        |
	| Cooler LBs          |
	| Cubes               |
	| Current Driver Name |
	| Dry Cases           |
	| Dry Cubes           |
	| Dry LBs             |
	| Email               |
	| Freezer Cases       |
	| Freezer Cubes       |
	| Freezer LBs         |
	| Order Status        |
	| Phone               |
	| State               |
	| Stop Status         |
	| Street              |
	| Zip                 |

@SYG449
Scenario: User can load stops table after selecting all stops columns
	Given I am an authenticated ATM user on the "Route Tracker" page
	And   I select a random center
	And   All Routes and Stops columns are selected in Column Options
	When  I view the Stops Table for a random route
	Then  I should see the stops table load successfully