Feature: Signin

As a user of Project Mars, I want to sign in to my account
so I can manage my skills and languages in my profile
and make them available to viewers and recruiters.

@positive
Scenario Outline: Sign in with valid credentials
	Given I am in the sign in page
	When I enter valid "<email>" and valid "<password>"
	Then I should see my profile page with greeting "<message>"							

	Examples: 
	| testCaseID | email                 | password | message    |
	| TC001      | charie_artz@yahoo.com | P@ssw0rd | Hi Charito |

			