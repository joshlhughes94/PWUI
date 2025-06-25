Feature: TestWebsiteTests

Scenario Outline: Buy a backpack successfully on <browser>
	Given I run the test on "<browser>"
	And I have navigated to the test url, logged in successfully
	And I add a backpack to my basket
	When I go to the basket and select Checkout 
	And I complete the required fields to complete my checkout
	Then I see a message confirming that my order has been successful
	Examples: 
	| browser          |
	| browser-firefox  |
	| browser-webkit   |
	| browser-chromium |
