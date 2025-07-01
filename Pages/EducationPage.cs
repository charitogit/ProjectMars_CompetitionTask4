using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_OnboardTask2.Utilities;
using qa_dotnet_cucumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace qa_dotnet_cucumber.Pages
{
    public class EducationPage
    {
        private readonly IWebDriver _driver;

        public EducationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Locators
        // XPath (precise text matching, works despite inner <i> tag)
        private By AddNewEducationButton => By.XPath("//div[@id='account-profile-section']//div[contains(@class,'ui teal button') and contains(.,'Add')]");
        private By UniversityInput => By.XPath("//*[@id='account-profile-section']//input[@name='instituteName']");
        private By CountryDropdown => By.XPath("//*[@id='account-profile-section']//select[@name='country']");
        private By TitleDropdown => By.XPath("//*[@id='account-profile-section']//select[@name='title']");
        private By DegreeInput => By.XPath("//*[@id='account-profile-section']//input[@name='degree']");
        private By YearDropdown => By.XPath("//*[@id='account-profile-section']//select[@name='yearOfGraduation']");
        private By SaveButton => By.XPath("//*[@id='account-profile-section']//input[contains(@class,'ui') and contains(@class,'teal') and contains(@class,'button') and @value='Add']");


        public void AddEducation(Education data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Education data cannot be null.");

            Console.WriteLine($"Adding Education Record: {data.University}, {data.Country}, {data.Title}, {data.Degree}, {data.Year}");

            // Wait for the Education section to be active
            string activeTabContentXPath = "//div[@data-tab='third' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);

            // Click "Add New" button inside the Education section
            string addNewEducationButtonXPath = $"{activeTabContentXPath}//div[normalize-space()='Add New']";
            Wait.WaitToBeClickable(_driver, "XPath", addNewEducationButtonXPath, 10);
            _driver.FindElement(By.XPath(addNewEducationButtonXPath)).Click();

            // Enter College name or University
            Wait.WaitToBeVisible(_driver, "XPath", "//input[@name='instituteName']", 10);
            var universityInput = _driver.FindElement(UniversityInput);
            universityInput.Clear();
            universityInput.SendKeys(data.University);

            // Select Country from dropdown
            SelectByText(CountryDropdown, data.Country);

            // Select Title from dropdown
            SelectByText(TitleDropdown, data.Title);

            // Enter Degree
            var degreeInput = _driver.FindElement(DegreeInput);
            degreeInput.Clear();
            degreeInput.SendKeys(data.Degree);

            // Select Year from dropdown
            SelectByText(YearDropdown, data.Year);


            // Click Add button
            string addButtonXPath = "//input[@value='Add' and @type='button']";
            Wait.WaitToBeClickable(_driver, "XPath", addButtonXPath, 10);
            _driver.FindElement(By.XPath(addButtonXPath)).Click();

        }


        public void EditEducationRecord(Education originalData,Education newData)
        {
            // Click on Edit button for the matching education record to be edited
            // Locate record using original data (before the edit)
            string editButtonRecordXPath = $"//table/tbody/tr[td[1][normalize-space()='{originalData.Country}'] and td[2][normalize-space()='{originalData.University}'] and td[3][normalize-space()='{originalData.Title}'] and td[4][normalize-space()='{originalData.Degree}'] and td[5][normalize-space()='{originalData.Year}']]//i[@class='outline write icon']";  

            var editButton = _driver.FindElement(By.XPath(editButtonRecordXPath));
        
            editButton.Click();

            // Wait for College name or University field, then clear and enter new value
            Wait.WaitToBeVisible(_driver, "XPath", "//input[@name='instituteName']", 10);
            var universityInput = _driver.FindElement(UniversityInput);
            universityInput.Click();
            universityInput.SendKeys(Keys.Control + "a");
            universityInput.SendKeys(Keys.Delete);
            universityInput.SendKeys(newData.University);

            // Select Country from dropdown
            SelectByText(CountryDropdown, newData.Country);

            // Select Title from dropdown
            SelectByText(TitleDropdown, newData.Title);

            // Enter Degree
            var degreeInput = _driver.FindElement(DegreeInput);
            degreeInput.Click();
            degreeInput.SendKeys(Keys.Control + "a");
            degreeInput.SendKeys(Keys.Delete);
            degreeInput.SendKeys(newData.Degree);

            // Select Year from dropdown
            SelectByText(YearDropdown, newData.Year);

            // Click on the Update button within the same editing form
            string updateButtonXpath = "//input[@value='Update' and @type='button']";
            Wait.WaitToBeClickable(_driver, "XPath", updateButtonXpath, 10);
            var updateButton = _driver.FindElement(By.XPath(updateButtonXpath));
            updateButton.Click();
        }



        private void SelectByText(By dropdownLocator, string visibleText)
        {
            var selectElement = new SelectElement(_driver.FindElement(dropdownLocator));
            selectElement.SelectByText(visibleText);
        }

        public string GetActualMessage()
        {

          
                Wait.WaitToBeVisible(_driver, "XPath", "//div[contains(@class, 'ns-show')]", 10);
                string actualMessage = _driver.FindElement(By.XPath("//div[contains(@class, 'ns-show')]")).Text.Trim();
                ClearNotification(); 
                 
            return actualMessage;
          
        }

        public void ClearNotification()
        {
            try
            {
                var closeBtn = _driver.FindElement(By.XPath("//a[contains(@class, 'ns-close')]"));
                if (closeBtn.Displayed)
                {
                    Wait.WaitToBeVisible(_driver, "XPath", "//a[contains(@class, 'ns-close')]", 5);
                    closeBtn.Click();
                }
            }
            catch (NoSuchElementException) { }
        }


        public bool IsRecordPresent(Education data)
        {
            string tableXPath = "//table/tbody"; 
            string recordXPath = $"//table/tbody/tr[td[1][normalize-space()='{data.Country}'] and td[2][normalize-space()='{data.University}'] and td[3][normalize-space()='{data.Title}'] and td[4][normalize-space()='{data.Degree}'] and td[5][normalize-space()='{data.Year}']]";


            try
            {
                // Wait for the table itself to appear (not necessarily the specific record)
                Wait.WaitToBeVisible(_driver, "XPath", tableXPath, 5);

                // After confirming table is visible, search for the specific record
                Wait.WaitToBeVisible(_driver, "XPath", recordXPath, 10);
                return _driver.FindElements(By.XPath(recordXPath)).Count > 0;
            }

            catch (WebDriverTimeoutException)
            {
                // Table never appeared → Treat as no record
                return false; 
            }
       
        }


        public void DeleteAllEducation()
        {
            
            string rowsXPath = "//table[@class='ui fixed table']/tbody/tr";
            var rows = _driver.FindElements(By.XPath(rowsXPath));

            while (rows.Count > 0)
            {
                var deleteButton = rows[0].FindElement(By.XPath(".//i[@class='remove icon']"));
                deleteButton.Click();
                Wait.WaitToBeVisible(_driver, "XPath", "//div[contains(@class,'ns-show')]", 5); // Wait for toast
                //Wait.WaitForElementToDisappear(_driver, "XPath", rowsXPath, 10);
                rows = _driver.FindElements(By.XPath(rowsXPath));
                Console.WriteLine($"Deleting education record at {DateTime.Now}");
            }
        }


        public void DeleteEducationIfExists(Education data)
        {
            try
            {
            
                string activeTabContentXPath = "//div[@data-tab='third' and contains(@class, 'active')]";
                //locate exact specific record as per data parameter
                string deleteButtonXPath = $"{activeTabContentXPath}//table/tbody/tr[td[1][normalize-space()='{data.Country}'] and td[2][normalize-space()='{data.University}'] and td[3][normalize-space()='{data.Title}'] and td[4][normalize-space()='{data.Degree}'] and td[5][normalize-space()='{data.Year}']]//i[@class='remove icon']";

                var deleteButtons = _driver.FindElements(By.XPath(deleteButtonXPath));
                if (deleteButtons.Count > 0)
                {
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);
                    deleteButtons[0].Click();

                    Console.WriteLine($"[Cleanup] Deleted Education data - University: {data.University}, Country: {data.Country}, Title: {data.Title},Degree: {data.Degree}, Year: {data.Year}  ");
                }
                else
                {
                    Console.WriteLine($"[Cleanup] No record found Education data - University: {data.University}, Country: {data.Country}, Title: {data.Title},Degree: {data.Degree}, Year: {data.Year}  ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Cleanup] ERROR deleting education (safe): " + ex.Message);
            }
        }

      


    }

}
