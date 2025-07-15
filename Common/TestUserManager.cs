using Newtonsoft.Json;
using SeleniumProject.Model;

namespace SeleniumProject.Common
{
    public static class TestUserManager
    {
        private static TestAccountSet? _testAccountSet;
        private static string? _dataFilePath;

        public static void SetDataFile(string filePath)
        {
            _dataFilePath = filePath;
            _testAccountSet = null; // Reset so Init() reloads from the new file
        }

        public static void Init()
        {
            if (_testAccountSet != null) return;

            if (string.IsNullOrEmpty(_dataFilePath) || !File.Exists(_dataFilePath))
                throw new FileNotFoundException($"Test data file not found: {_dataFilePath}");

            var json = File.ReadAllText(_dataFilePath);
            _testAccountSet = JsonConvert.DeserializeObject<TestAccountSet>(json);
        }

        public static string GetUsername(string role)
        {
            Init();
            if (_testAccountSet!.Users.TryGetValue(role, out var user))
                return user.Username;

            throw new Exception($"User role '{role}' not found.");
        }

        public static string GetDefaultUrl()
        {
            Init();
            return _testAccountSet!.DefaultUrl;
        }
    }
}
