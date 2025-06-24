using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;


namespace SeleniumProject.Common
{
    public static class ReportManager
    {
        private static ExtentReports _extent;
        private static string _reportPath;

        private static ThreadLocal<ExtentTest> _feature = new ThreadLocal<ExtentTest>();
        private static ThreadLocal<ExtentTest> _scenario = new ThreadLocal<ExtentTest>();

        public static void InitReport(bool isParallel)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string reportsDir = Path.Combine(baseDir, "Reports");
            string screenshotsDir = Path.Combine(reportsDir, "Screenshots");

            if (Directory.Exists(reportsDir))
                Directory.Delete(reportsDir, true);

            Directory.CreateDirectory(reportsDir);
            Directory.CreateDirectory(screenshotsDir);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportFileName = $"TestReport_{timestamp}.html";
            _reportPath = Path.Combine(reportsDir, reportFileName);

            var sparkReporter = new ExtentSparkReporter(_reportPath);
            sparkReporter.Config.Theme = Theme.Standard;
            sparkReporter.Config.DocumentTitle = "Reqnroll Test Report";
            sparkReporter.Config.ReportName = isParallel
                ? "Automation Test Results (Parallel)"
                : "Automation Test Results (Sequential)";

            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);
        }


        public static void CreateFeature(string featureName)
        {
            _feature.Value = _extent.CreateTest(featureName);
        }

        public static void CreateScenario(string scenarioName)
        {
            if (_feature.Value == null)
            {
                // Optional fallback for safety
                _feature.Value = _extent.CreateTest("Unnamed Feature");
            }

            _scenario.Value = _feature.Value.CreateNode(scenarioName);
        }

        public static void LogStep(string stepDescription, string status)
        {
            if (_scenario.Value == null) return;

            if (status.Equals("Pass", StringComparison.OrdinalIgnoreCase))
            {
                _scenario.Value.Pass(stepDescription);
            }
            else
            {
                _scenario.Value.Fail(stepDescription);
            }
        }

        public static void LogStepWithScreenshot(string stepDescription, string status, string base64Image)
        {
            if (_scenario.Value == null) return;

            var mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64Image).Build();

            switch (status.ToLower())
            {
                case "pass":
                    _scenario.Value.Pass(stepDescription, mediaEntity);
                    break;
                case "fail":
                    _scenario.Value.Fail(stepDescription, mediaEntity);
                    break;
                default:
                    _scenario.Value.Info(stepDescription);
                    break;
            }
        }

        public static void FlushReport()
        {
            _extent.Flush();

            // Optional: auto-open the report

            if (!string.IsNullOrEmpty(_reportPath) && File.Exists(_reportPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _reportPath,
                    UseShellExecute = true
                });
            }

        }
    }


}