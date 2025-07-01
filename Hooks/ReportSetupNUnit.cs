using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using qa_dotnet_cucumber.Config;
using System.Text.Json;

namespace qa_dotnet_cucumber.Hooks
{
    [SetUpFixture]
    public class ReportSetupNUnit
    {
        public static ExtentReports Extent;
        public static ExtentSparkReporter HtmlReporter;
        public static TestSettings Settings;

        private static readonly object _lock = new();
        public static ExtentTest? CurrentTest { get; private set; }

        [OneTimeSetUp]
        public void InitReport()
        {
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                string settingsPath = Path.Combine(currentDir, "settings.json");

                if (!File.Exists(settingsPath))
                    throw new FileNotFoundException($"settings.json not found at: {settingsPath}");

                string json = File.ReadAllText(settingsPath);
                Settings = JsonSerializer.Deserialize<TestSettings>(json)
                    ?? throw new Exception("Failed to deserialize settings.json");

                // Prepare ExtentReport
                string projectRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
                string reportFileName = Settings.Report.Path.TrimStart('/');
                string reportPath = Path.Combine(projectRoot, reportFileName);

                HtmlReporter = new ExtentSparkReporter(reportPath);
                Extent = new ExtentReports();
                Extent.AttachReporter(HtmlReporter);

                Extent.AddSystemInfo("Environment", Settings.Environment.BaseUrl);
                Extent.AddSystemInfo("Browser", Settings.Browser.Type);

                Console.WriteLine($"[ReportHooks Init] Report initialized at {DateTime.Now}, path: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during InitReport: {ex.Message}");
                throw;
            }
        }

        // Used inside BeforeScenario to register test node
        public static ExtentTest CreateScenarioTest(string scenarioTitle)
        {
            lock (_lock)
            {
                CurrentTest = Extent.CreateTest(scenarioTitle);
                Console.WriteLine($"[Report] Created test: {scenarioTitle} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                return CurrentTest;
            }
        }

     
        //[OneTimeTearDown] - Flush Report is handled by Reqnroll Hook - Hooks.cs - [AfterTestRun] to ensure run after test


        /*  Static constructor to eagerly trigger the config load before any scenarios run
         This ensures settings are initialized before Reqnroll test hooks use them */
        static ReportSetupNUnit()
        {
            if (Settings == null)
                new ReportSetupNUnit().InitReport(); // Eagerly trigger configuration + report init
        }
    }
}
