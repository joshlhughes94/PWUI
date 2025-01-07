Feature: TestWebsiteTests

@browser-firefox
Scenario: Test Website Firefox
	Given I have navigated to the test url
	When I have completed the Contact Form

@browser-chromium
Scenario: Test Website Chrome
	Given I have navigated to the test url
	When I have completed the Contact Form

@browser-webkit
Scenario: Test Website Webkit
	Given I have navigated to the test url
	When I have completed the Contact Form
