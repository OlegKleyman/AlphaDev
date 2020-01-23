Feature: GetBlogs
	In order to retrieve blog information
	As a developer
	I want to have a service that is able to retrieve blog information

Background:
Given There are blogs

Scenario: Get latest blog
	Given I am an API consumer
	When I make a request to get the latest blog
	Then the latest blog is returned
