# ProjectMars Competition Task - Education and Certification
BDD-style Test Automation Framework for Project Mars web application using:

- Reqnroll (.NET-compatible BDD)
- Selenium WebDriver
- ExtentReports for rich test reports
- JSON-based test data (for Education and Certification features)
- Page Object Model (POM)
- Clean Dependency Injection
- .NET 8

---

## Project Structure

```
ProjectMars_OnboardTask2/
├── Features/          # Gherkin feature files
├── Steps/             # C# step implementations
├── Pages/             # Page Object Model classes
├── Models/            # Data models (Education, Certification)
├── Helpers/           # Navigation utilities, test data helper for deserialization
├── Hooks/             # Hooks for cleanup, WebDriver, reporting
├── TestData/          # JSON files for test inputs
├── Config/            # Configuration classes
├── Tests/             # NUnit test runner (if applicable)
├── Utilities/         # Waits and custom helpers
├── settings.json      # Config for browser, URL, timeout, report path
└── TestReport.html    # Test result report (generated under bin folder)
```

---

## JSON Data Structure (Sample)

Education test data is stored in `TestData/education.json`:

```json
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
```

This maps to the following model class:

```csharp
public class Education
{
    public string Country { get; set; }
    public string University { get; set; }
    public string Title { get; set; }
    public string Degree { get; set; }
    public string Year { get; set; }
    public string ExpectedMessage { get; set; }
}
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- Google Chrome installed (WebDriver managed via WebDriverManager)

### Setup & Run Tests

```bash
dotnet restore
dotnet test
```

After execution, open the test report:

- `TestReport.html` (under the `bin` folder)

---

## Features Covered

| Module    	| Coverage										 |
| -------------	| -------------------------------------------------------------------------------------- |
| Sign In   	| Valid login                        							 |
| Education 	| Add, Edit, Delete, Duplicate checks, Empty field scenarios,*(Sign In handled by Hook)* |
| Certification | Add, Edit, Delete, Duplicate checks, Empty field scenarios,*(Sign In handled by Hook)* |

---

## Tags & Hooks

| Tag                             | Purpose                                                 |
| ------------------------------- | ------------------------------------------------------- |
| @SignInWithEducationCleanup     | Logs in and clears Education tab before test            |
| @SignInWithCertificationCleanup | Logs in and clears Certification tab before test        |
| @EducationCleanup               | Deletes test-added education records after scenario     |
| @CertificationCleanup           | Deletes test-added certification records after scenario |

---

## Extent Reports

- Generated to: `bin/TestReport.html`
- Includes:
  - Pass/Fail steps
  - Screenshots on failure (`bin/Debug/net8.0`)
  - Test duration, system info, metadata

### Sample System Info Logged

| Name        | Value                                            |
| ----------- | ------------------------------------------------ |
| Environment | [http://localhost:5003/](http://localhost:5003/) |
| Browser     | Chrome                                           |

---

## Configuration File: `settings.json`

```json
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
```

---

## Credentials for Automation Testing

> Dummy credentials used for automation test execution only:

- **Email**: [charie_artz@yahoo.com]
- **Password**: P@ssw0rd

