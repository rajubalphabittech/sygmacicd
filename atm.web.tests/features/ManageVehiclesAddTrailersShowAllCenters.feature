Feature: MangeVehiclesAndTrailersCanShowAllCenters
	As A user, I should be able to view
	Routes for all centers at once. 
	Bug 713 was causing an error that blocked this

Scenario: Can load vehicles and trailers for all centers
	Given I am an authenticated ATM user on the "Manage Vehicles And Trailers" page
	When I select "All" from the center search field
	Then I should see a list of all Routes returned