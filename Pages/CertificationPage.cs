using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjectMars_OnboardTask2.Utilities;
using qa_dotnet_cucumber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qa_dotnet_cucumber.Pages
{
    public class CertificationPage
    {
        private readonly IWebDriver _driver;

        public CertificationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Locators
        // XPath (precise text matching, works despite inner <i> tag)
        private By AddNewCertificationButton => By.XPath("//div[@id='account-profile-section']//div[contains(@class,'ui teal button') and contains(.,'Add')]");
        private By CertificateInput => By.XPath("//*[@id='account-profile-section']//input[@name='certificationName']");
        private By CertifiedFromInput => By.XPath("//*[@id='account-profile-section']//input[@name='certificationFrom']");
        private By YearDropdown => By.XPath("//*[@id='account-profile-section']//select[@name='certificationYear']");   
        private By SaveButton => By.XPath("//*[@id='account-profile-section']//input[contains(@class,'ui') and contains(@class,'teal') and contains(@class,'button') and @value='Add']");

        public object ScreenshotImageFormat { get; private set; }

        public void AddCertification(Certification data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Certification data cannot be null.");

            Console.WriteLine($"Adding Certification Record: {data.Certificate}, {data.CertifiedFrom}, {data.Year}");

            // Wait for the Certification section to be active
            string activeTabContentXPath = "//div[@data-tab='fourth' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);

            // Click "Add New" button inside the Certification section
            string addNewCertificationButtonXPath = $"{activeTabContentXPath}//div[normalize-space()='Add New']";
            Wait.WaitToBeClickable(_driver, "XPath", addNewCertificationButtonXPath, 10);
            _driver.FindElement(By.XPath(addNewCertificationButtonXPath)).Click();

            // Enter Certificate or Award
            Wait.WaitToBeVisible(_driver, "XPath", "//input[@name='certificationName']", 10);
            var certificateInput = _driver.FindElement(CertificateInput);
            certificateInput.Clear();
            certificateInput.SendKeys(data.Certificate);

          
            // Enter Certified From
            var certifiedFromInput = _driver.FindElement(CertifiedFromInput);
            certifiedFromInput.Clear();
            certifiedFromInput.SendKeys(data.CertifiedFrom);

            // Select Year from dropdown
            SelectByText(YearDropdown, data.Year);


            // Click Add button
            string addButtonXPath = "//input[@value='Add' and @type='button']";
            Wait.WaitToBeClickable(_driver, "XPath", addButtonXPath, 10);
            _driver.FindElement(By.XPath(addButtonXPath)).Click();

        }

        public void EditCertificationRecord(Certification originalData, Certification newData)
        {
            // Click on Edit button for the matching certification record to be edited
            // Locate record using original data (before the edit)
            string editButtonRecordXPath = $"//table/tbody/tr[td[1][normalize-space()='{originalData.Certificate}'] and td[2][normalize-space()='{originalData.CertifiedFrom}'] and td[3][normalize-space()='{originalData.Year}']]//i[@class='outline write icon']";

            var editButton = _driver.FindElement(By.XPath(editButtonRecordXPath));

            editButton.Click();

            // Wait for Certificate or Award field, then clear and enter new value
            Wait.WaitToBeVisible(_driver, "XPath", "//input[@name='certificationName']", 10);
            var certificateInput = _driver.FindElement(CertificateInput);
            certificateInput.Click();
            certificateInput.SendKeys(Keys.Control + "a");
            certificateInput.SendKeys(Keys.Delete);
            certificateInput.SendKeys(newData.Certificate);

          
            // Enter CertifiedFrom
            var certifiedFromInput = _driver.FindElement(CertifiedFromInput);
            certifiedFromInput.Click();
            certifiedFromInput.SendKeys(Keys.Control + "a");
            certifiedFromInput.SendKeys(Keys.Delete);
            certifiedFromInput.SendKeys(newData.CertifiedFrom);

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


        public bool IsRecordPresent(Certification data)
        {
           
            string tableXPath = "//div[@data-tab='fourth' and contains(@class,'active')]//table/tbody";
            string recordXPath = $"//div[@data-tab='fourth' and contains(@class,'active')]//table/tbody/tr[td[1][normalize-space()='{data.Certificate}'] and td[2][normalize-space()='{data.CertifiedFrom}'] and td[3][normalize-space()='{data.Year}']]";


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


        public void DeleteAllCertification()
        {
            //  Scoped to the fourth tab (Certification tab)
            string rowsXPath = "//div[@data-tab='fourth' and contains(@class,'active')]//table[@class='ui fixed table']/tbody/tr";
            var rows = _driver.FindElements(By.XPath(rowsXPath));

            while (rows.Count > 0)
            {
                // Target the delete icon only within that same scoped fourth tab row
                var deleteButton = rows[0].FindElement(By.XPath(".//i[@class='remove icon']"));
                deleteButton.Click();

                // Wait for toast to confirm deletion
                Wait.WaitToBeVisible(_driver, "XPath", "//div[contains(@class,'ns-show')]", 5);

                // Re-fetch rows after deletion
                rows = _driver.FindElements(By.XPath(rowsXPath));
                Console.WriteLine($"Deleting certification record at {DateTime.Now}");
            }
        }






        public void DeleteCertificationIfExists(Certification data)
        {
            try
            {

                string activeTabContentXPath = "//div[@data-tab='fourth' and contains(@class, 'active')]";
                //locate exact specific record as per data parameter
                string deleteButtonXPath = $"{activeTabContentXPath}//table/tbody/tr[td[1][normalize-space()='{data.Certificate}'] and td[2][normalize-space()='{data.CertifiedFrom}'] and td[3][normalize-space()='{data.Year}']]//i[@class='remove icon']";

                var deleteButtons = _driver.FindElements(By.XPath(deleteButtonXPath));
                if (deleteButtons.Count > 0)
                {
                    Wait.WaitToBeClickable(_driver, "XPath", deleteButtonXPath, 5);
                    deleteButtons[0].Click();

                    Console.WriteLine($"[Cleanup] Deleted Certification data - Certificate: {data.Certificate}, Certififed From: {data.CertifiedFrom}, Year: {data.Year}");
                }
                else
                {
                    Console.WriteLine($"[Cleanup] No record found Certification data - Certificate: {data.Certificate}, Certififed From: {data.CertifiedFrom}, Year: {data.Year}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Cleanup] ERROR deleting Certification (safe): " + ex.Message);
            }
        }





    }
}
