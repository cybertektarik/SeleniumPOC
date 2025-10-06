using Newtonsoft.Json;
using SeleniumProject.Model;

namespace SeleniumProject.Common
{
    public static class TestUserManager
    {
        private static TestAccountSet? _testAccountSet;
        private static string? _dataFilePath;

        //SetDataFile only chooses the file; the actual JSON parsing happens inside Init()
        //which GetDefaultUrl() triggers via Init().
        public static void SetDataFile(string filePath) //saves the path and clears the cached _testAccountSet
        {
            _dataFilePath = filePath;
            _testAccountSet = null; // Reset so Init() reloads from the new file
        }
        // init() reads the JSON file (File.ReadAllText(_dataFilePath))
        // and JsonConvert.DeserializeObject<TestAccountSet>(json) to populate _testAccountSet
        public static void Init()
        {
            if (_testAccountSet != null) return;

            if (string.IsNullOrEmpty(_dataFilePath) || !File.Exists(_dataFilePath))
                throw new FileNotFoundException($"Test data file not found: {_dataFilePath}");

            var json = File.ReadAllText(_dataFilePath);
            _testAccountSet = JsonConvert.DeserializeObject<TestAccountSet>(json);
        }

        //GetUsername method calls Init() and returns the username for the requested role
        public static string GetUsername(string role)
        {
            Init();
            if (_testAccountSet!.Users.TryGetValue(role, out var user))
                return user.Username;

            throw new Exception($"User role '{role}' not found.");
        }

        public static string GetDefaultUrl()
        {
            Init();//to ensure JSON is loaded
            return _testAccountSet!.DefaultUrl;
        }
    }
}
