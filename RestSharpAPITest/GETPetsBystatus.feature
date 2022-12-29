Feature: GETPetsBystatus
GET the Pet info

Scenario:GET PET by Status
	Given I generate the request with the verb 'GET' for the enpoint called 'getPetsByStatus'
	And I setup the query paramaters
	|Field   | Value     |
	| status | available |
	When I send the API Request
	Then I validate the Response is 'OK'
	#Then I verify the fields in the response


	
