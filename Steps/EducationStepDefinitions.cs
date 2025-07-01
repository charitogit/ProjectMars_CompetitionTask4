using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using ProjectMars_OnboardTask2.Pages;
using qa_dotnet_cucumber.Helpers;
using qa_dotnet_cucumber.Models;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System;
using System.Data;
using System.Diagnostics;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class EducationStepDefinitions

    {
        private readonly NavigationHelper _navigationHelper;
        private readonly EducationPage _educationPage;
        private readonly ScenarioContext _scenarioContext;

        private Education? _educationData;

        public EducationStepDefinitions(NavigationHelper navigationHelper,EducationPage educationPage, ScenarioContext scenarioContext)
        {
            _navigationHelper = navigationHelper;
            _educationPage = educationPage;
            _scenarioContext = scenarioContext;

        }

  
        [Given("I am successfully signed in and in Education tab")]
        public void GivenIAmSuccessfullySignedInAndInEducationTab()
        {
            //Signin step and Navigation to Education tab is handled by Hook together with the Data cleanup 
        }


        [When("I add education record with {string}")]
        public void WhenIAddEducationRecordWith(string dataKey)
        {

           _educationData = GetEducationData(dataKey);
           _educationPage.AddEducation(_educationData);
    
            // Store in list for  afterscenario cleanup
            AddToEducationList(_educationData);

        }

        [Then("I should see a successful message")]
        public void ThenIShouldSeeASuccessfulMessage()
        {

            Assert.That(_educationData!.ExpectedMessage, Is.EqualTo(_educationPage.GetActualMessage()), "Message shown is not as expected");

        }

     

        [Then("I should see the {string} added in the list")]
        public void ThenIShouldSeeTheAddedInTheList(string dataKey)
        {
      
            Assert.That(_educationPage.IsRecordPresent(_educationData!), "Test Failed: Record is not added");
        }


   


        [Then("I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            Assert.That(_educationData!.ExpectedMessage,Is.EqualTo(_educationPage.GetActualMessage()), "Test Failed: Message is not as expected.");
            
        }

      
        [Then("I should not see the record in the list")]
        public void ThenIShouldNotSeeTheRecordInTheList()
        {
            _navigationHelper.ClickCancelEdit();
            Assert.That(_educationPage.IsRecordPresent(_educationData!), Is.False, "Test Failed: Record should NOT be present, but it was found."); 
        }



        [When("I succesully added {string} record")]
        public void WhenISuccesullyAddedRecord(string dataKey)
        {
            _educationData = GetEducationData(dataKey);
            _educationPage.AddEducation(_educationData);

            // Store original record for later edit reference
            _scenarioContext["originalEducationData"] = _educationData;

            // Store in scenario context list for afterscenario cleanup
            AddToEducationList(_educationData);

            Assert.That(_educationData!.ExpectedMessage, Is.EqualTo(_educationPage.GetActualMessage()), "Message shown is not as expected");
     
             
        }

        [When("I succesully added first {string} record")]
        public void WhenISuccesullyAddedFirstRecord(string dataKey)
        {
            _educationData = GetEducationData(dataKey);
            _educationPage.AddEducation(_educationData);
            // Store in scenario context list for afterscenario cleanup
            AddToEducationList(_educationData);

            Assert.That(_educationData!.ExpectedMessage, Is.EqualTo(_educationPage.GetActualMessage()), "Message shown is not as expected");

        }

        [When("I successfully added second {string} record")]
        public void WhenISuccessfullyAddedSecondRecord(string dataKey)
        {
            _educationData = GetEducationData(dataKey);
            _educationPage.AddEducation(_educationData);
            // Store in scenario context list for afterscenario cleanup
            AddToEducationList(_educationData);

            // Store original record for later edit reference
            _scenarioContext["originalEducationData"] = _educationData;

            Assert.That(_educationData!.ExpectedMessage, Is.EqualTo(_educationPage.GetActualMessage()), "Message shown is not as expected");

        }

       


        [When("I edit the second record with exact {string} of first record")]
        public void WhenIEditTheSecondRecordWithExactOfFirstRecord(string dataKey)
        {
            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)
            var originalData = (Education)_scenarioContext["originalEducationData"]; ;
            var newData = GetEducationData(dataKey);

            _educationData = newData;  
            _educationPage.EditEducationRecord(originalData, newData);

        }




        [When("I recreate the exact {string} record")]
        public void WhenIRecreateTheExactRecord(string dataKey)
        {
            _educationData = GetEducationData(dataKey);
            _educationPage.AddEducation(_educationData);

            // Store in scenario context list for afterscenario cleanup
            //AddToEducationList(_educationData);
        }

        [When("I edit record with valid {string} input")]
        public void WhenIEditRecordWithValidInput(string dataKey)
        {
            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)

            var originalData = (Education)_scenarioContext["originalEducationData"];
            var newData = GetEducationData(dataKey);
            _educationData = newData; // So it’s available for next assertions

            //// Store in ScenarioContext list for afterscenario cleanup 
            AddToEducationList(_educationData);


            _educationPage.EditEducationRecord(originalData, newData);
        }

      
        [When("I edit education record with invalid {string}")]
        public void WhenIEditEducationRecordWithInvalid(string dataKey)
        {
            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)
            var originalData = (Education)_scenarioContext["originalEducationData"];
            var newData = GetEducationData(dataKey);
            _educationData = newData; // So it’s available for next assertions

            _educationPage.EditEducationRecord(originalData, newData);
        }



        [Then("I should see the {string} in the list")]
        public void ThenIShouldSeeTheInTheList(string dataKey)
        {
            Assert.That(_educationPage.IsRecordPresent(_educationData!), "Test Failed: Record is not added");
        }


        [Then("I should not see the update to take effect")]
        public void ThenIShouldNotSeeTheUpdateToTakeEffect()
        {
            Assert.That(_educationPage.IsRecordPresent(_educationData!), Is.False, "Test Failed: Record should NOT be present, but it was found.");
        }



        [When("I verify I see the record added in the list")]
        public void WhenIVerifyISeeTheRecordAddedInTheList()
        {
            Assert.That(_educationPage.IsRecordPresent(_educationData!), "Test Failed: Record is not added");
        }

        
     

        [When("I delete the record {string}")]
        public void WhenIDeleteTheRecord(string dataKey)
        {
            

            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)

            var deleteData = GetEducationData(dataKey);
            _educationData = deleteData; // So it’s available for next assertions

            //// Store in ScenarioContext list for afterscenario cleanup 
            AddToEducationList(_educationData);


            _educationPage.DeleteEducationIfExists(_educationData!);
        }


        [Then("record should be removed from the list")]
        public void ThenRecordShouldBeRemovedFromTheList()
        {
            Assert.That(_educationPage.IsRecordPresent(_educationData!), Is.False, "Test Failed: Record should NOT be present, but it was found.");

        }


        //Private utility method to centralize fetching of education data from deserialization
        private Education GetEducationData(string dataKey)
        {
            var data = TestDataHelper.GetEducationData(dataKey);
            Console.WriteLine($"Loaded Education: {data.Country}, {data.University},{data.Title},{data.Degree},{data.Year}");

            if (data == null)
                throw new Exception($"Education data for key '{dataKey}' is null. Check JSON or TestDataHelper logic.");
            return data;
           

        }

        //Private utility method to Add to ScenarioContext List for later use in AfterScenario Cleanup Hook 
        private void AddToEducationList(Education education)
        {
            if (!_scenarioContext.ContainsKey("EducationDataList"))
            {
                _scenarioContext["EducationDataList"] = new List<Education>();
            }

            var list = (List<Education>)_scenarioContext["EducationDataList"];
            list.Add(education);
        }


    }
}
