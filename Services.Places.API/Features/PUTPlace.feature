Feature: PUTPlace

Scenario: Verify PUT Place API endPoint
	Given I generate a 'PUT' verb request for an action called 'PUT_PLACEINFO'
	And I setup the following query parameters
		| Field   | Value                            |
		| key     | qaclick123                       |
		| placeID | 2a7a1ffcb8644541a10f96876a40c4a2 |
	And I setup the 'putPlace' json file with the following parameters
		| Field    | value                            |
		| place_id | 2a7a1ffcb8644541a10f96876a40c4a2 |
		| address  | 70 winter walk, USA              |
		| key      | qaclick123                       |
	When I send the generated request
	Then I validate the Response Status Code is '200'
	Then I validate the following response is generated
		| Field                                                                                                                                                                                                                                                            |
		| {"location":{"latitude":"-58.387494","longitude":"53.827362"},"accuracy":"50","name":"Frontline house 123","phone_number":"(+91) 983 893 3937","address":"70 Summer walk, UK","types":"shoe park,shop","website":"http:\\/\\/google.com","language":"French-IN"} |