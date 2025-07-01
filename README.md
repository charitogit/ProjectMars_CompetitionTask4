# ProjectMars_CompetitionTask4


BDD-style Test Automation Framework for [Project Mars](http://localhost:5003/) web application using:

- Reqnroll (.NET-compatible BDD)
- Selenium WebDriver
- Extent Reports for rich test reports
- JSON-based test data (for Education and Certification features)
- Page Object Model (POM)
- Clean Dependency Injection
- .NET 8

---

## 📁 Project Structure

ProjectMars_OnboardTask2/
├── Features/           # Gherkin feature files
├── Steps/              # C# step implementations
├── Pages/              # Page Object Model classes
├── Models/             # Data models (Education, Certification)
├── Helpers/            # Navigation utilities, test data helper for deserialization
├── Hooks/              # Hooks for cleanup, WebDriver, reporting
├── TestData/           # JSON files for test inputs
├── Config/             # Configuration classes
├── Tests/              # NUnit test runner
├── Utilitties/         # Wait
├── settings.json       # Config (browser, URL, timeout, report path)
├── TestReport.html     # Test result report under bin folder

## JSON Data Structure (sample)
Education test data is stored in `TestData/education.json`:

{
  "validEducation": {
    "Country": "New Zealand",
    "University": "AUT",
    "Title": "B.Sc",
    "Degree": "Computer Science",
    "Year": "2022",
    "ExpectedMessage": "Education has been added"
  }
}
It is mapped to the Education model class:

public class Education
{
    public string Country { get; set; }
    public string University { get; set; }
    public string Title { get; set; }
    public string Degree { get; set; }
    public string Year { get; set; }
    public string ExpectedMessage { get; set; }
}
 
### ⚙️ Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Google Chrome installed (WebDriver managed via [WebDriverManager](https://github.com/rosolko/WebDriverManager.Net))

###  Setup & Run Tests

```bash
dotnet restore
dotnet test

# After execution, open the test report:
TestReport.html (under bin folder)

# Features Covered
Module	            Coverage
Sign In		    Valid login 
Education	    Add, Edit, Delete, Duplicate checks (Sign In is handled by Hook)
Certification	    Add, Edit, Delete, Empty field scenarios (Sign In is handled by Hook)

### Tags & Hooks
Tag				Purpose
@SignInWithEducationCleanup	Logs in and clears Education tab before test
@SignInWithCertificationCleanup	Logs in and clears Certification tab
@EducationCleanup		Deletes test-added education records after
@CertificationCleanup		Deletes test-added certification records

📸 Extent Reports  (generated to folder ** bin\TestReport.html)
✅ Pass/Fail steps
📷 Screenshots on failure (generated to folder ** bin\Debug\net8.0)
📊 Test duration, system info, metadata

Sample	        System Info Logged
Name		Value
Environment	http://localhost:5003/
Browser		Chrome


⚙️ Config File: settings.json
json
Copy
Edit
{
  "Browser": {
    "Type": "Chrome",
    "Headless": false,
    "TimeoutSeconds": 5
  },
  "Report": {
    "Path": "TestReport.html",
    "Title": "Test Automation Report"
  },
  "Environment": {
    "BaseUrl": "http://localhost:5003/"
  }
}
##Credential/s for Automation
This is a dummy credential used only for automation testing:

email: charie_artz@yahoo.com
password: P@ssw0rd
