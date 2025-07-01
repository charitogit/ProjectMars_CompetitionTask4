using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V131.DOM;
using ProjectMars_OnboardTask2.Utilities;

namespace qa_dotnet_cucumber.Helpers
{
    public class NavigationHelper
    {
        private readonly IWebDriver _driver;

        public NavigationHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateTo(string urlPath)
        {
            _driver.Navigate().GoToUrl(Hooks.TestHooks.Settings.Environment.BaseUrl + urlPath);
        }

        public void GoToSignInPage()
        {
          
            _driver.FindElement(By.XPath("//a[contains(text(), 'Sign In')]")).Click();

        }

        public void GoToEducationTab()
        {
            // Wait and click the "Education" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Education')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Education section to be active
            string activeTabContentXPath = "//div[@data-tab='third' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);


        }

        public void GoToCertificationsTab()
        {
            // Wait and click the "Certifications" tab
            string tabXPath = "//*[@id='account-profile-section']//a[contains(text(),'Certifications')]";
            Wait.WaitToBeVisible(_driver, "XPath", tabXPath, 10);
            _driver.FindElement(By.XPath(tabXPath)).Click();

            // Wait for the Certifications section to be active
            string activeTabContentXPath = "//div[@data-tab='fourth' and contains(@class, 'active')]";
            Wait.WaitToBeVisible(_driver, "XPath", activeTabContentXPath, 10);

        
        }

        public void ClickCancelEdit()
        {
            // Click on the Cancel button 
            string cancelButtonXpath = "//input[@value='Cancel' and @type='button']";
            Wait.WaitToBeClickable(_driver, "XPath", cancelButtonXpath, 10);
            var cancelButton = _driver.FindElement(By.XPath(cancelButtonXpath));
            cancelButton.Click();

        }

     


    }
}