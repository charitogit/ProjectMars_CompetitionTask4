using qa_dotnet_cucumber.Models;
using System.Text.Json;

namespace qa_dotnet_cucumber.Helpers
{
    public static class TestDataHelper
    {
        // Dictionary to hold all education data loaded from JSON, cached in memory
        private static Dictionary<string, Models.Education> _cachedEducationData;

        // Dictionary to hold all certification data loaded from JSON, cached in memory
        private static Dictionary<string, Certification> _cachedCertificationData;

        // Static constructor - runs ONCE when TestDataHelper is first used
        static TestDataHelper()
        {
            // Load education data
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "educationData.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Test data file not found at: {filePath}");

            var json = File.ReadAllText(filePath);
            _cachedEducationData = JsonSerializer.Deserialize<Dictionary<string, Education>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new Exception("Failed to load education test data.");

            // Load certification data
            var certPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "certificationData.json");
            if (!File.Exists(certPath))
                throw new FileNotFoundException($"Certification test data file not found at: {certPath}");

            var certJson = File.ReadAllText(certPath);
            _cachedCertificationData = JsonSerializer.Deserialize<Dictionary<string, Certification>>(certJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new Exception("Failed to load certification test data.");
        }

        /// <summary>
        /// Retrieves a single Education object by its key from the cached data.
        /// </summary>
        /// <param name="key">The key in the JSON, e.g., 'validEducation'</param>
        /// <returns>Education object</returns>
        public static Models.Education GetEducationData(string key)
        {
            if (_cachedEducationData.TryGetValue(key, out var data))
                return data;

            throw new Exception($"Education test data with key '{key}' not found.");
        }

        public static Models.Certification GetCertificationData(string key)
        {
            if (_cachedCertificationData.TryGetValue(key, out var data))
                return data;

            throw new Exception($"Certification  test data with key '{key}' not found.");
        }
    }
}
