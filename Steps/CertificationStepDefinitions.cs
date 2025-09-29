using qa_dotnet_cucumber.Helpers;
using qa_dotnet_cucumber.Models;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System;
using System.Data;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    public class CertificationStepDefinitions
    {
        private readonly NavigationHelper _navigationHelper;
        private readonly CertificationPage _certificationPage;
        private readonly ScenarioContext _scenarioContext;

        private Certification? _certificationData; 

        public CertificationStepDefinitions(NavigationHelper navigationHelper, CertificationPage certificationPage, ScenarioContext scenarioContext)
        {
            _navigationHelper = navigationHelper;
            _certificationPage = certificationPage;
            _scenarioContext = scenarioContext;

        }

        [Given("I am successfully signed in and in Certification tab")]
        public void GivenIAmSuccessfullySignedInAndInCertificationTab()
        {
            //Signin actions handled by BeforeScenario hook

            _navigationHelper.GoToCertificationsTab();
        }

        [When("I add certification record with {string}")]
        public void WhenIAddCertificationRecordWith(string dataKey)
        {
           //fetch the certification data with validcertification
             _certificationData= GetCertificationData(dataKey);

            //add the records using the fetched cetification data
            _certificationPage.AddCertification(_certificationData);

            //store data to scenario context list for later afterscenario hook cleanup
            AddToCertificationList(_certificationData); 

        }

        [When("I succesfully added {string} record")]
        public void WhenISuccesfullyAddedRecord(string dataKey)
        {
            //fetch the certification data with validcertification
            _certificationData = GetCertificationData(dataKey);

            //add the records using the fetched cetification data
            _certificationPage.AddCertification(_certificationData);

            // Store original record for later edit reference
            _scenarioContext["originalCertificationData"] = _certificationData;

            //store data to scenario context list for later afterscenario hook cleanup
            AddToCertificationList(_certificationData);

            Assert.That(_certificationData!.ExpectedMessage, Is.EqualTo(_certificationPage.GetActualMessage()), "Message shown is not as expected");

        }


        [When("I recreated the exact {string} record")]
        public void WhenIRecreatedTheExactRecord(string dataKey)
        {

            _certificationData = GetCertificationData(dataKey);
            _certificationPage.AddCertification(_certificationData);

            // Store in scenario context list for afterscenario cleanup
            //AddToEducationList(_educationData);
        }

        [When("I edit record using {string} record")]
        public void WhenIEditRecordUsingRecord(string dataKey)
        {
            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)

            var originalData = (Certification)_scenarioContext["originalCertificationData"];
            var newData = GetCertificationData(dataKey);
            _certificationData = newData; // So it’s available for next assertions

            //// Store in ScenarioContext list for afterscenario cleanup 
            AddToCertificationList(_certificationData);


            _certificationPage.EditCertificationRecord(originalData, newData);
        }


        [When("I edit the second record using {string} copy of first record")]
        public void WhenIEditTheSecondRecordUsingCopyOfFirstRecord(string dataKey)
        {
            // Cast the stored object from ScenarioContext to the Education model type.
            // (Education is the model class used to store the deserialized test data)

            var originalData = (Certification)_scenarioContext["originalCertificationData"];
            var newData = GetCertificationData(dataKey);
            _certificationData = newData; // So it’s available for next assertions

            //////// Store in ScenarioContext list for afterscenario cleanup 
            ////AddToCertificationList(_certificationData);

            _certificationPage.EditCertificationRecord(originalData, newData);
        }

        [When("I verify record is added  in the list")]
        public void WhenIVerifyRecordIsAddedInTheList()
        {
            Assert.That(_certificationPage.IsRecordPresent(_certificationData!), "Test Failed : Record is not added "); 
        }

        [When("I delete the {string} record")]
        public void WhenIDeleteTheRecord(string dataKey)
        {

            // Cast the stored object from ScenarioContext to the Certification model type.
            // (Certification is the model class used to store the deserialized test data)

            var deleteData = GetCertificationData(dataKey);
            _certificationData = deleteData; // So it’s available for next assertions

            //// Store in ScenarioContext list for afterscenario cleanup 
            AddToCertificationList(deleteData);
            _certificationPage.DeleteCertificationIfExists(deleteData);

        }




        [Then("I should see successful message")]
        public void ThenIShouldSeeSuccessfulMessage()
        {

            Assert.That(_certificationData!.ExpectedMessage, Is.EqualTo(_certificationPage.GetActualMessage())); 
        }
        

        [Then("I should see the record in the list")]
        public void ThenIShouldSeeTheRecordInTheList()
        {
                Assert.That(_certificationPage.IsRecordPresent(_certificationData!), "Test Failed: Record is not added in the certification list");
        }


       

        [Then("I should see error message")]
        public void ThenIShouldSeeErrorMessage()
        {
            Assert.That(_certificationData!.ExpectedMessage, Is.EqualTo(_certificationPage.GetActualMessage()));
        }

        [Then("I should not see the record in list")]
        public void ThenIShouldNotSeeTheRecordInList()
        {
            Assert.That(_certificationPage.IsRecordPresent(_certificationData!),Is.False, "Test Failed: Record should NOT be present, but it was found."); 
        }
 

        [Then("I should not see the updated record in list")]
        public void ThenIShouldNotSeeTheUpdatedRecordInList()
        {
            Assert.That(_certificationPage.IsRecordPresent(_certificationData!), Is.False, "Test Failed: Record should NOT be present, but it was found.");
        }

        [Then("record should be removed in the list")]
        public void ThenRecordShouldBeRemovedInTheList()
        {
            Assert.That(_certificationPage.IsRecordPresent(_certificationData!), Is.False, "Test Failed: Record should NOT be present, but it was found.");
        }



        //Private utility method to centralize fetching of certification data from deserialization
        private Models.Certification GetCertificationData(string dataKey)
        {
            var data = TestDataHelper.GetCertificationData(dataKey);
            Console.WriteLine($"Loaded Certification: {data.Certificate}, {data.CertifiedFrom},{data.Year}");

            if (data == null)
                throw new Exception($"Certification data for key '{dataKey}' is null. Check JSON or TestDataHelper logic.");
            return data;

        }

        //Private utility method to Add to ScenarioContext List for later use in AfterScenario Cleanup Hook 
        private void AddToCertificationList(Certification certification)
        {
            if (!_scenarioContext.ContainsKey("CertificationDataList"))
            {
                _scenarioContext["CertificationDataList"] = new List<Certification>();
            }

            var list = (List<Certification>)_scenarioContext["CertificationDataList"];
            list.Add(certification);
        }
    }
}
