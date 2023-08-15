Feature: AverageAge

A short summary of the feature

Scenario Outline: Average Age Test
	Given I want to get the average age of the name '<Name>'
	And I expect the average to be '<Age>'
	When I request the average age of a persons name
	Then the expected age is returned
	Examples: 
	| Name | Age |
	| Dave | 67  |
	| Enid | 77  |
