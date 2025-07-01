using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectMars_OnboardTask2.Pages;
using qa_dotnet_cucumber.Config;
using qa_dotnet_cucumber.Helpers;
using qa_dotnet_cucumber.Models;
using qa_dotnet_cucumber.Pages;
using Reqnroll;
using Reqnroll.BoDi;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
namespace qa_dotnet_cucumber.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly IObjectContainer _objectContainer; 
        private static ExtentSparkReporter? _htmlReporter; 

        private static ExtentReports _extent => ReportSetupNUnit.Extent!;
        private static TestSettings _settings => ReportSetupNUnit.Settings!;

        private ExtentTest? _test;
        private static readonly object _reportLock = new object();

        private readonly ScenarioContext _scenarioContext; 
        public static TestSettings Settings => _settings;

        public TestHooks(IObjectContainer objectContainer,ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext; 
        }

        private void SignInToPortal()
        {
            var driver = _objectContainer.Resolve<IWebDriver>();
            var navigationHelper = _objectContainer.Resolve<NavigationHelper>();
            var signInPage = _objectContainer.Resolve<SignInPage>();

            driver.Navigate().GoToUrl(TestHooks.Settings.Environment.BaseUrl);
            navigationHelper.GoToSignInPage();
            signInPage.SignInSteps("charie_artz@yahoo.com", "P@ssw0rd");

            Console.WriteLine("[Hooks] Successfully signed in.");
        }

        //CLEANUP :  Delete All Education records before scenario starts
        [BeforeScenario("SignInWithEducationCleanup", Order = 1)]
        public void DeleteAllEducationBefore()
        {
            try
            {
                SignInToPortal();
                var navigationHelper = _objectContainer.Resolve<NavigationHelper>();
                var educationPage = _objectContainer.Resolve<EducationPage>();

                navigationHelper.GoToEducationTab();
                var stopwatch = Stopwatch.StartNew();
                educationPage.DeleteAllEducation();
                stopwatch.Stop();

                Console.WriteLine("[BeforeScenario] Education records cleaned up successfully.");
                Console.WriteLine($"Cleanup took: {stopwatch.Elapsed.TotalSeconds} seconds");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[BeforeScenario Education Cleanup ERROR] " + ex.Message);
            }
        }

        //CLEANUP :  Delete All Certification records before scenario starts
        [BeforeScenario("SignInWithCertificationCleanup", Order = 1)]
        public void DeleteAllCertificationBefore()
        {
            try
            {
                SignInToPortal();
                var navigationHelper = _objectContainer.Resolve<NavigationHelper>();
                var certificationPage = _objectContainer.Resolve<CertificationPage>();

                navigationHelper.GoToCertificationsTab();
                var stopwatch = Stopwatch.StartNew();
                certificationPage.DeleteAllCertification();
                stopwatch.Stop();

                Console.WriteLine("[BeforeScenario] Certification records cleaned up successfully.");
                Console.WriteLine($"Cleanup took: {stopwatch.Elapsed.TotalSeconds} seconds");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[BeforeScenario Certification Cleanup ERROR] " + ex.Message);
            }
        }

        //Take screenshots for failed steps
        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepText = scenarioContext.StepContext.StepInfo.Text;

            lock (_reportLock)
            {
                var test = ReportSetupNUnit.CurrentTest;
                if (test == null)
                {
                    Console.WriteLine("[AfterStep] No active test found. Skipping report logging.");
                    return;
                }

                if (scenarioContext.TestError == null)
                {
                    test.Log(Status.Pass, $"{stepType} {stepText}");
                }
                else
                {
                    try
                    {
                        var driver = _objectContainer.Resolve<IWebDriver>();
                        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                        var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"Screenshot_{DateTime.Now.Ticks}_{Thread.CurrentThread.ManagedThreadId}.png");
                        screenshot.SaveAsFile(screenshotPath);
                        test.Log(Status.Fail, $"{stepType} {stepText}", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    }
                    catch (WebDriverException ex)
                    {
                        // Handles the case if WebDriver is already closed/invalid
                        test.Log(Status.Fail, $"{stepType} {stepText} - Screenshot unavailable (Driver error: {ex.Message})");
                    }
                }
            }
        }

        //// CLEANUP: Delete test education data only after test
        [AfterScenario("EducationCleanup", Order = 0)]
        public void DeleteEducationTestDataAfter()
        {
            try
            {
                var driver = _objectContainer.Resolve<IWebDriver>();
                var navigationHelper = _objectContainer.Resolve<NavigationHelper>();
                var educationPage = _objectContainer.Resolve<EducationPage>();

                driver.Navigate().Refresh();
                navigationHelper.GoToEducationTab();

                // Check if table has any rows before continuing
                var rowsXPath = "//div[@data-tab='third' and contains(@class,'active')]//table/tbody/tr";
                var existingRows = driver.FindElements(By.XPath(rowsXPath));

                if (existingRows.Count == 0)
                {
                    Console.WriteLine("[AfterScenario] Education table is already empty. Skipping cleanup.");
                    return;
                }

                if (_scenarioContext.TryGetValue("EducationDataList", out var dataListObj) &&
                    dataListObj is List<Education> educationList)
                {
                    foreach (var education in educationList)
                    {
                        educationPage.DeleteEducationIfExists(education);
                    }

                    Console.WriteLine($"[AfterScenario] Deleted {educationList.Count} education records.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AfterScenario Education Cleanup ERROR] " + ex.Message);
            }
        }

        //// CLEANUP: Delete test certification data only after test
        [AfterScenario("CertificationCleanup", Order = 0)]
        public void DeleteCertificationTestDataAfter()
        {
            try
            {
                var driver = _objectContainer.Resolve<IWebDriver>();
                var navigationHelper = _objectContainer.Resolve<NavigationHelper>();
                var certificationPage = _objectContainer.Resolve<CertificationPage>();

                driver.Navigate().Refresh();
                navigationHelper.GoToCertificationsTab();

                // Check if table has any rows before continuing
                var rowsXPath = "//div[@data-tab='fourth' and contains(@class,'active')]//table/tbody/tr";
                var existingRows = driver.FindElements(By.XPath(rowsXPath));

                if (existingRows.Count == 0)
                {
                    Console.WriteLine("[AfterScenario] Certification table is already empty. Skipping cleanup.");
                    return;
                }

                if (_scenarioContext.TryGetValue("CertificationDataList", out var dataListObj) &&
                    dataListObj is List<Certification> certificationList)
                {
                    foreach (var certification in certificationList)
                    {
                        certificationPage.DeleteCertificationIfExists(certification);
                    }

                    Console.WriteLine($"[AfterScenario] Deleted {certificationList.Count} certification records.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[AfterScenario Certification Cleanup ERROR] " + ex.Message);
            }
        }

 
        [AfterTestRun] //prerequisite is ReportSetupNUNit [OnetimeSetup]
        public static void AfterTestRun()
        {
           
            try
            {
                lock (_reportLock)
                {
                    TestContext.Progress.WriteLine("AfterTestRun executed - Flushing report to: " + _settings.Report.Path + " at " + DateTime.Now);
                    _extent!.Flush();
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($" Error during FlushReport: {ex.Message}");
                throw;
            }
        }
    }
}