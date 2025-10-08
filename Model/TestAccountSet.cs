using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumProject.Model
{
    // CLASS: TestAccountUser
    // PURPOSE: Represents a single test user account with authentication details
    // FLOW: Used by TestAccountSet to store individual user information
    // CONNECTS TO: TestAccountSet (via Users dictionary) → Test data loading
    public class TestAccountUser
    {
        // PROPERTY: Username - Test user's login username
        // PURPOSE: Stores the username for authentication during test execution
        // FLOW: Loaded from JSON → Used in login steps → Authenticates user
        public string Username { get; set; } = string.Empty;
    }

    // CLASS: TestAccountSet
    // PURPOSE: Container for test configuration and user account data
    // FLOW: Loaded from JSON files → Provides test data to step definitions
    // CONNECTS TO: Test data files (JSON) → Step definitions (via data binding)
    public class TestAccountSet
    {
        // PROPERTY: DefaultUrl - Base URL for the application under test
        // PURPOSE: Stores the target application URL for test execution
        // FLOW: Loaded from JSON → Used in test setup → Navigates to application
        public string DefaultUrl { get; set; } = string.Empty;
        
        // PROPERTY: Users - Dictionary of test users keyed by user role/type
        // PURPOSE: Stores multiple test user accounts for different test scenarios
        // FLOW: Loaded from JSON → Accessed by step definitions → Used for login
        // EXAMPLE: Users["admin"] = TestAccountUser, Users["user"] = TestAccountUser
        public Dictionary<string, TestAccountUser> Users { get; set; } = new();
    }

}
