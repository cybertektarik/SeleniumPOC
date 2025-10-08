using Newtonsoft.Json;
using SeleniumProject.Model;

namespace SeleniumProject.Common
{
    // CLASS: TestUserManager
    // PURPOSE: Manages test data loading and user account access from JSON files
    // FLOW: Loads JSON files → Deserializes to TestAccountSet → Provides user data to step definitions
    // CONNECTS TO: JSON files (via file paths) → TestAccountSet (via deserialization) → Step definitions (via GetUsername)
    public static class TestUserManager
    {
        // FIELD: _testAccountSet - Cached test account data from JSON
        // PURPOSE: Stores deserialized test data to avoid repeated file reads
        // FLOW: Loaded once → Cached for performance → Accessed by GetUsername/GetDefaultUrl
        private static TestAccountSet? _testAccountSet;
        
        // FIELD: _dataFilePath - Path to current test data file
        // PURPOSE: Tracks which JSON file is currently loaded
        // FLOW: Set by SetDataFile → Used by Init → Determines which JSON to load
        private static string? _dataFilePath;

        // METHOD: SetDataFile
        // PURPOSE: Selects which JSON test data file to use
        // FLOW: Called by TestHooks → Sets file path → Clears cache → Forces reload
        // DRIVER FLOW: No driver involved - file path configuration only
        public static void SetDataFile(string filePath)
        {
            _dataFilePath = filePath;        // Store the file path
            _testAccountSet = null;          // Clear cache to force reload from new file
        }
        
        // METHOD: Init
        // PURPOSE: Loads and deserializes JSON test data file
        // FLOW: Checks if data loaded → Reads JSON file → Deserializes to TestAccountSet
        // DRIVER FLOW: No driver involved - JSON file processing only
        public static void Init()
        {
            // STEP 1: Check if data already loaded
            if (_testAccountSet != null) return;

            // STEP 2: Validate file path and existence
            if (string.IsNullOrEmpty(_dataFilePath) || !File.Exists(_dataFilePath))
                throw new FileNotFoundException($"Test data file not found: {_dataFilePath}");

            // STEP 3: Read and deserialize JSON file
            var json = File.ReadAllText(_dataFilePath);                    // Read JSON file content
            _testAccountSet = JsonConvert.DeserializeObject<TestAccountSet>(json); // Convert to TestAccountSet object
        }

        // METHOD: GetUsername
        // PURPOSE: Retrieves username for specified user role
        // FLOW: Calls Init → Searches Users dictionary → Returns username
        // DRIVER FLOW: No driver involved - data retrieval only
        // USAGE: Called by step definitions to get login credentials
        public static string GetUsername(string role)
        {
            Init(); // Ensure JSON data is loaded
            
            // STEP 1: Look up user role in Users dictionary
            if (_testAccountSet!.Users.TryGetValue(role, out var user))
                return user.Username; // Return username for the role

            // STEP 2: Throw exception if role not found
            throw new Exception($"User role '{role}' not found.");
        }

        // METHOD: GetDefaultUrl
        // PURPOSE: Retrieves default application URL from test data
        // FLOW: Calls Init → Returns DefaultUrl from TestAccountSet
        // DRIVER FLOW: No driver involved - URL retrieval only
        // USAGE: Called by TestHooks to navigate to application
        public static string GetDefaultUrl()
        {
            Init(); // Ensure JSON data is loaded
            return _testAccountSet!.DefaultUrl; // Return the default URL
        }
    }
}
