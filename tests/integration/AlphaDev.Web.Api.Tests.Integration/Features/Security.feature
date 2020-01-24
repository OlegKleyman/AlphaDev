Feature: Security
	To be able to identify myself
	As a developer
	I want a service for security related operations

Scenario: Authentication successful
	Given I am an existing user
	When I request a token
	Then I will receive a security token
