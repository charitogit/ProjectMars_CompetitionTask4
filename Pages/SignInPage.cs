using NUnit.Framework;
using OpenQA.Selenium;
using ProjectMars_OnboardTask2.Utilities;
using System.Globalization;

namespace ProjectMars_OnboardTask2.Pages
{
    public class SignInPage
    {

        private readonly IWebDriver _driver;

        public SignInPage(IWebDriver driver)
        {
            _driver = driver;
        }

        //XPath locators
        private IWebElement EmailInput => _driver.FindElement(By.XPath("//input[@name='email']"));
        private IWebElement PasswordInput => _driver.FindElement(By.XPath("//input[@name='password']"));
        private IWebElement LoginButton => _driver.FindElement(By.XPath("//button[contains(text(),'Login') or contains(text(),'Login')]"));

      
        public void SignInSteps (string email, string password)
        {
            
      
            // enter email
            Wait.WaitToBeVisible(_driver, "XPath", "//input[contains(@placeholder, 'Email')]", 10);
            EmailInput.Clear();
            EmailInput.SendKeys(email);

            // enter password
            Wait.WaitToBeVisible(_driver, "XPath", "//input[@name='password' or @type='password' or contains(@placeholder, 'Password')]", 10);
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);

            // click login button
            LoginButton.Click();

        }

        public void AssertSuccessfulLogIn(string message)
        {
            
            Wait.WaitToBeVisible(_driver, "XPath", "//div[@id='account-profile-section']//span[contains(text(), 'Hi')]\r\n", 10);
            string greetingText = _driver.FindElement(By.XPath("//div[@id='account-profile-section']//span[contains(text(), 'Hi')]\r\n")).Text;
            Assert.That(greetingText == message, "Failed to login.");
        }

      

        public void AssertFailedLogIn( string message)
        {
            //Check if logged in successfully        
            Wait.WaitToBeVisible(_driver, "XPath", $"//*[contains(text(), '{message}')]", 10);
            string errorText = _driver.FindElement(By.XPath($"//*[contains(text(), '{message}')]")).Text;
            Assert.That(errorText == message, "Test Failed");

        }

        public bool IsSignInFormVisible()
        {
            return EmailInput.Displayed && PasswordInput.Displayed;
        }



    }
}
