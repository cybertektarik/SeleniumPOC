# 📊 JSON Data Files Flow Documentation

## 🎯 Overview
This document explains how the JSON data files (`UserRoles_Set1.json` and `UserRoles_Set2.json`) work together with your Selenium framework to provide test data and user account management.

## 🔄 Complete Data Flow Diagram

```
TestHooks.BeforeScenario
    ↓
SetTestDataFileBasedOnTags() (Tag Selection)
    ↓
TestUserManager.SetDataFile() (File Selection)
    ↓
JSON File Loading (UserRoles_Set1.json or Set2.json)
    ↓
TestUserManager.Init() (JSON Deserialization)
    ↓
TestAccountSet Object (Cached Data)
    ↓
Step Definitions (GetUsername/GetDefaultUrl)
    ↓
Test Execution (Login/Authentication)
```

## 🏗️ Architecture Components

### 1. **JSON Data Files**
- **UserRoles_Set1.json**: Feature2 environment data
- **UserRoles_Set2.json**: External environment data
- **Structure**: DefaultUrl + Users dictionary
- **Purpose**: Store test user accounts and application URLs

### 2. **TestUserManager Class**
- **Location**: `Common/TestUserManager.cs`
- **Purpose**: Manages JSON file loading and data access
- **Key Methods**: `SetDataFile()`, `Init()`, `GetUsername()`, `GetDefaultUrl()`

### 3. **TestAccountSet Model**
- **Location**: `Model/TestAccountSet.cs`
- **Purpose**: Data model for deserialized JSON content
- **Structure**: DefaultUrl + Users dictionary

### 4. **TestHooks Integration**
- **Location**: `Hooks/TestHooks.cs`
- **Purpose**: Selects appropriate data file based on scenario tags
- **Method**: `SetTestDataFileBasedOnTags()`

## 🔗 Data Flow Sequence

### Step 1: TestHooks.BeforeScenario()
```csharp
// 1. Check scenario tags
SetTestDataFileBasedOnTags(_scenarioContext.ScenarioInfo.Tags);

// 2. Select data file based on tags
if (tags.Contains("feature2"))
    testDataPath = "Data/UserRoles_Set1.json";
else if (tags.Contains("external"))
    testDataPath = "Data/UserRoles_Set2.json";

// 3. Configure TestUserManager
TestUserManager.SetDataFile(testDataPath);
```

### Step 2: TestUserManager.SetDataFile()
```csharp
// 1. Store file path
_dataFilePath = filePath;

// 2. Clear cache to force reload
_testAccountSet = null;
```

### Step 3: TestUserManager.Init() (Lazy Loading)
```csharp
// 1. Check if already loaded
if (_testAccountSet != null) return;

// 2. Read JSON file
var json = File.ReadAllText(_dataFilePath);

// 3. Deserialize to TestAccountSet
_testAccountSet = JsonConvert.DeserializeObject<TestAccountSet>(json);
```

### Step 4: Step Definitions Access Data
```csharp
// Get username for login
string username = TestUserManager.GetUsername("EnrolledUser");

// Get URL for navigation
string url = TestUserManager.GetDefaultUrl();
```

## 📋 JSON File Structure

### UserRoles_Set1.json (Feature2 Environment)
```json
{
  "DefaultUrl": "https://employee-feature2.live-test-domain.com/#/auth/login?returnUrl=%2F",
  "Users": {
    "LimitedAccountUser": { "Username": "Feature2HSABTester005" },
    "EnrolledUser": { "Username": "Feature2HSABTester023" },
    "PreEnrolledUser": { "Username": "Feature2HSABTester019" },
    "DefaultUser": { "Username": "Feature2HSABTester041" }
  }
}
```

### UserRoles_Set2.json (External Environment)
```json
{
  "DefaultUrl": "https://employee-external.live-test-domain.com/#/auth/login?returnUrl=%2F",
  "Users": {
    "LimitedAccountUser": { "Username": "ExternalDev005" },
    "EnrolledUser": { "Username": "ExternalDev001" },
    "PreEnrolledUser": { "Username": "ExternalDev006" },
    "DefaultUser": { "Username": "ExternalDev014" }
  }
}
```

## 🎯 User Role Types

### 1. **LimitedAccountUser**
- **Purpose**: Tests with restricted account access
- **Feature2**: `Feature2HSABTester005`
- **External**: `ExternalDev005`

### 2. **EnrolledUser**
- **Purpose**: Tests with full account access
- **Feature2**: `Feature2HSABTester023`
- **External**: `ExternalDev001`

### 3. **PreEnrolledUser**
- **Purpose**: Tests for users not yet fully enrolled
- **Feature2**: `Feature2HSABTester019`
- **External**: `ExternalDev006`

### 4. **DefaultUser**
- **Purpose**: Tests with standard user permissions
- **Feature2**: `Feature2HSABTester041`
- **External**: `ExternalDev014`

## 🔧 Tag-Based File Selection

### TestHooks.SetTestDataFileBasedOnTags()
```csharp
// Default selection
string testDataPath = "Data/UserRoles_Set1.json";

// Tag-based selection
if (tags.Contains("feature2"))
    testDataPath = "Data/UserRoles_Set1.json";
else if (tags.Contains("external"))
    testDataPath = "Data/UserRoles_Set2.json";
```

### Scenario Tag Examples
```gherkin
@feature2
Scenario: Test in Feature2 environment
    # Uses UserRoles_Set1.json

@external
Scenario: Test in External environment
    # Uses UserRoles_Set2.json
```

## 💡 Key Benefits

### 1. **Environment Separation**
- Different URLs for different environments
- Separate user accounts for each environment
- Easy switching between test environments

### 2. **User Role Management**
- Centralized user account management
- Role-based test data access
- Easy addition of new user types

### 3. **Performance Optimization**
- Lazy loading of JSON data
- Caching to avoid repeated file reads
- Efficient data access patterns

### 4. **Maintainability**
- Easy to modify test data without code changes
- Clear separation of concerns
- Type-safe data access

## 🔍 Usage Examples

### In Step Definitions
```csharp
[Given(@"I am logged in as a user who has an enrolled account")]
public void GivenIAmLoggedInAsEnrolledUser()
{
    // Get username from JSON data
    string username = TestUserManager.GetUsername("EnrolledUser");
    
    // Use username for login
    Pages.LoginPage.UsernameField.SendKeys(username);
}
```

### In TestHooks
```csharp
[BeforeScenario]
public void BeforeScenario()
{
    // Select data file based on tags
    SetTestDataFileBasedOnTags(_scenarioContext.ScenarioInfo.Tags);
    
    // Navigate to URL from JSON data
    NavigateToDefaultUrl(TestUserManager.GetDefaultUrl());
}
```

## 🚨 Important Notes

1. **File Path**: JSON files must be in `Data/` folder
2. **Tag Selection**: Scenario tags determine which file to use
3. **Lazy Loading**: JSON is loaded only when needed
4. **Caching**: Data is cached for performance
5. **Error Handling**: File not found exceptions are handled

## 🔧 Troubleshooting

### Common Issues
1. **File Not Found**: Check file path and existence
2. **Role Not Found**: Verify user role exists in JSON
3. **Wrong Environment**: Check scenario tags
4. **Data Not Loading**: Verify JSON format is correct

### Debug Tips
1. Check console output for "Using Test Data File: ..."
2. Verify scenario tags match expected values
3. Ensure JSON files are in correct location
4. Check JSON format for syntax errors

---

**💡 This system provides flexible, environment-aware test data management for your Selenium framework!**
