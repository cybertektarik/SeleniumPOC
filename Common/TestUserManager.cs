using Newtonsoft.Json;
using SeleniumProject.Model;

namespace SeleniumProject.Common
{
    public static class TestUserManager
    {
        private static TestAccountSet? _testAccountSet;

        public static void Init(string filePath = "Data/UserRoles_Set1.json")
        {
            if (_testAccountSet != null) return;

            var json = File.ReadAllText(filePath);
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
