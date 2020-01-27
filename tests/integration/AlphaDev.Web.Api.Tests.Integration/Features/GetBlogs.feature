@blog
@web
Feature: GetBlogs
	In order to retrieve blog information
	As a developer
	I want to have a service that is able to retrieve blog information

Scenario: Get latest blog
	Given I am an API consumer
	And There are blogs
	When I make a request to get the latest blog
	Then the latest blog is returned

Scenario: Get latest blog when no blogs exist
	Given I am an API consumer
	And There are no blogs
	When I make a request to get the latest blog
	Then I will receive a 404 response

Scenario: Get blog from position sorted by created date
	Given I am an API consumer
	And I have 100 blogs
	When I make a request to get 20 blogs from position 17
	Then those blogs are returned

Scenario: Get specific blog by ID
	Given I am an API consumer
	And There is a blog
	| Content | Title |
	| content | title |
	When I make a request to get that blog
	Then the blog is returned

Scenario: Get specific blog by ID when it doesn't exist
	Given I am an API consumer
	When I make a request to get a blog by the id of 1
	Then I will receive a 404 response