

@SignInWithCertificationCleanup @CertificationCleanup
Feature: Certification

As a user I would like to add,edit and delete Certifications I hold in a list
so I can showcase them under my profile page.


@positive   
  Scenario: Add valid certification record
  Given  I am successfully signed in and in Certification tab
    When I add certification record with "validCertification" 
    Then  I should see successful message
    And I should see the record in the list

@negative
 Scenario Outline: Add certification with empty field
  Given  I am successfully signed in and in Certification tab
    When I add certification record with "<dataKey>" 
    Then  I should see error message
    And I should not see the record in list

    Examples:
   | testCaseID | dataKey        |
   |TCO29       |emptyCertificate|
   |TCO30       |emptyCertifiedFrom|
   |TCO31       |noSelectionYear|

    @negative 
  Scenario: Add exact duplicate certification record
    When I succesfully added "validCertification" record
    And  I recreated the exact "duplicateCertification" record
     Then  I should see error message

      @positive 
  Scenario: Edit certification record with valid inputs
    When   I succesfully added "validCertification" record
    And I edit record using "editCertification" record 
    Then  I should see successful message
    And I should see the record in the list

      @negative 
  Scenario: Edit exact duplicate certification record
   When I succesfully added "validCertification" record
   And  I succesfully added "validCertification2" record
    And  I edit the second record using "duplicateCertification" copy of first record 
    Then  I should see error message

  @negative
  Scenario Outline: Edit education with an empty  field
   When I succesfully added "validCertification" record
    And I edit record using "<dataKey>" record 
    Then  I should see error message
      And I should not see the updated record in list

    Examples: 
 |testCaseID  | dataKey             |   
 |TC042		  | emptyCertificate     |   
 |TC043		  | emptyCertifiedFrom  |  
 |TC044		  | noSelectionYear    |  

 
 
Scenario: Delete certification record
When I succesfully added "validCertification" record
And  I verify record is added  in the list 
When I delete the "deleteCertification" record
Then I should see successful message
And record should be removed in the list 