Feature: GETPlace

Scenario: Verify GET Place API endPoint
	Given I generate a 'GET' verb request for an action called 'GET_PLACEINFO'
	And I setup the following query parameters
		| Field   | Value                            |
		| key     | qaclick123                       |
		| placeID | f52b08756606b29045a96eedd33b90b7 |
	When I send the generated request
	Then I validate the Response Status Code is '200'
	Then I validate the following response is generated
		| Field                                                                                                                                                                                                                                                          |
		| {"location":{"latitude":"-58.387494","longitude":"53.827362"},"accuracy":"50","name":"Frontline house 123","phone_number":"(+91) 983 893 3937","address":"70 Summer walk, UK","types":"shoe park,shop","website":"http:\/\/google.com","language":"French-IN"} |