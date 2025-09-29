
@SignInWithEducationCleanup @EducationCleanup
Feature: Education
As a user I would like to add,edit and delete Education I have in a list
so I can showcase them under my profile page.
 
@positive 
  Scenario: Add valid education record
    Given  I am successfully signed in and in Education tab
    When I add education record with "validEducation" 
    Then I should see a successful message
    And I should see the "validEducation" added in the list   

 @negative 
  Scenario Outline: Add education with an empty field
  Given  I am successfully signed in and in Education tab
    When I add education record with "<dataKey>" 
    Then I should see an error message
    And I should not see the record in the list

    Examples: 
 |testCaseID  | dataKey             |   
 |TC003		  | emptyUniversity     |   
 |TC004		  | noSelectionCountry  |  
 |TC005		  | noSelectionTitle    |  
 |TC007		  | emptyDegree         |  
 |TC008		  | noSelectionYear     |  

 @negative 
  Scenario: Add exact duplicate education record
  Given  I am successfully signed in and in Education tab
    When I succesully added "validEducation" record
    And  I recreate the exact "duplicateEducation" record
    Then I should see an error message

 @positive 
  Scenario: Edit education record with valid inputs
  Given  I am successfully signed in and in Education tab
    When  I succesully added "validEducation" record 
    And I edit record with valid "editEducation" input
    Then I should see a successful message
    And I should see the "editEducation" in the list   

  @negative 
  Scenario: Edit exact duplicate education record
  Given  I am successfully signed in and in Education tab
    When I succesully added first "validEducation" record
    And I successfully added second "validEducation2" record
    And  I edit the second record with exact "duplicateEducation" of first record 
    Then I should see an error message 

  @negative
  Scenario Outline: Edit education with an empty  field
  Given  I am successfully signed in and in Education tab
   When I succesully added "validEducation" record
    And I edit education record with invalid "<dataKey>"  
    Then I should see an error message
    And I should not see the update to take effect 

    Examples: 
 |testCaseID  | dataKey             |   
 |TC018		  | emptyUniversity     |   
 |TC019		  | noSelectionCountry  |  
 |TC020		  | noSelectionTitle    |  
 |TC021		  | emptyDegree         |  
 |TC022		  | noSelectionYear     |  

 
Scenario: Delete education record
Given  I am successfully signed in and in Education tab
When I succesully added "validEducation" record
And I verify I see the record added in the list 
When  I delete the record "deleteEducation"
Then  I should see a successful message
And   record should be removed from the list 

 