using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


namespace SeleniumProject.Common
{
    public static class ReportManager
    {
        private static ExtentReports? extent;
        private static ExtentTest? feature;
        private static ExtentTest? scenario;
        private static string? reportPath;

        public static void InitReport()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string reportsDir = Path.Combine(baseDir, "Reports");
            string screenshotsDir = Path.Combine(reportsDir, "Screenshots");

            // 🔥 Delete entire Reports directory (including screenshots and previous reports)
            if (Directory.Exists(reportsDir))
                Directory.Delete(reportsDir, recursive: true);

            // ✅ Recreate Reports and Screenshots directories
            Directory.CreateDirectory(reportsDir);
            Directory.CreateDirectory(screenshotsDir);

            // 🕒 Generate dynamic report filename
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportFileName = $"TestReport_{timestamp}.html";
            reportPath = Path.Combine(reportsDir, reportFileName);

            // 📝 Setup ExtentReports
            var sparkReporter = new ExtentSparkReporter(reportPath)
            {
                Config =
                 {
            DocumentTitle = "Reqnroll Test Report",
            ReportName = "Automation Test Results"
                 }
            };

            extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);
        }


        public static void CreateFeature(string featureName)
        {
            feature = extent.CreateTest(featureName);
        }

        public static void CreateScenario(string scenarioName)
        {
            scenario = feature.CreateNode(scenarioName);
        }

        public static void LogStep(string stepDescription, string status)
        {
            if (scenario == null) return;

            if (status == "Pass")
                scenario.CreateNode(stepDescription).Pass("Passed");
            else
                scenario.CreateNode(stepDescription).Fail("Failed");
        }


        public static void LogStepWithScreenshot(string stepDescription, string status, string base64Image)
        {
            if (scenario == null) return;

            var mediaEntity = MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64Image).Build();

            switch (status.ToLower())
            {
                case "pass":
                    scenario.Pass(stepDescription);
                    break;
                case "fail":
                    scenario.Fail(stepDescription, mediaEntity);
                    break;
                default:
                    scenario.Info(stepDescription);
                    break;
            }
        }

        public static void FlushReport()
        {
            extent.Flush();

            // Optional: Open the report file in the default browser
           /* try
            {
                if (!string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = reportPath,
                        UseShellExecute = true // Required for .NET Core and above
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not open the report file automatically.");
                Console.WriteLine(ex.Message);
            }*/
        }
    }

}
