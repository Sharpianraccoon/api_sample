Feature: AverageAge

Calculate the average of a person based on their name

Scenario Outline: Average age of a person is returned
	Given I want to get the average age of the name '<Name>'
	And I expect the average to be '<Age>'
	And I expect the number of people with this name to be '<Count>'
	When I request the average age of a persons name
	Then the expected age is returned
	Examples: 
	| Name | Age | Count |
	| Dave | 67  | 57984 |
	| Enid | 77  | 408   |

Scenario: Expected error is returned when name is not specified
	Given I want to get the average age of the name '<EMPTY>'
	When I request the average age of a persons name without specifying a name
	Then the expected error code and message are returned
