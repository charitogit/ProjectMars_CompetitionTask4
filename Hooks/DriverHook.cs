using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectMars_OnboardTask2.Pages;
using qa_dotnet_cucumber.Helpers;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace qa_dotnet_cucumber.Hooks
{
    [Binding]
    public class DriverHook
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
    

        public DriverHook(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public void InitializeDriver()
        {
            var settings = ReportSetupNUnit.Settings;

            Console.WriteLine($"Starting {_scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
            new DriverManager().SetUpDriver(new ChromeConfig());
            var chromeOptions = new ChromeOptions();

            if (settings.Browser.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }

            var driver = new ChromeDriver(chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.Browser.TimeoutSeconds);
            driver.Manage().Window.Maximize();

            _objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            _objectContainer.RegisterInstanceAs(new NavigationHelper(driver));
            _objectContainer.RegisterInstanceAs(new SignInPage(driver));

            ReportSetupNUnit.CreateScenarioTest(_scenarioContext.ScenarioInfo.Title);
            Console.WriteLine($"Created test: {_scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
           
        }

        [AfterScenario(Order = 100)]
        public void TearDownDriver()
        {
            var driver = _objectContainer.Resolve<IWebDriver>();
            driver?.Quit();
            Console.WriteLine($"Finished scenario on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
        }
    }
}
