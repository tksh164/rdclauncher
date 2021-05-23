using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace rdclauncher
{
    internal static class ExceptionReportBuilder
    {
        public static string GetReportText(Exception exception)
        {
            var reportText = new StringBuilder();
            reportText.AppendLine(@"**** ENVIRONMENT ****");

            // App
            var appVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            var processArch = RuntimeInformation.ProcessArchitecture.ToString();
            var dotNet = RuntimeInformation.FrameworkDescription;
            reportText.AppendFormat(@"App version: {0}", appVersion);
            reportText.AppendLine();
            reportText.AppendFormat(@"Process architecture: {0}", processArch);
            reportText.AppendLine();
            reportText.AppendFormat(@".NET: {0}", dotNet);
            reportText.AppendLine();

            // OS
            var productName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "n/a").ToString();
            var os = Environment.OSVersion;
            var ubr = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "UBR", 0).ToString();
            var currentVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "n/a").ToString();
            var releaseId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "n/a").ToString();
            var displayVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "n/a").ToString();
            var osArch = RuntimeInformation.OSArchitecture.ToString();
            reportText.AppendFormat(@"OS: {0} {1}.{2}.{3}.{4} ({5}, {6}, {7}) {8}", productName, os.Version.Major, os.Version.Minor, os.Version.Build, ubr, displayVersion, releaseId, currentVersion, osArch);
            reportText.AppendLine();

            // Stack trace
            int nestLevel = 0;
            var ex = exception;
            while (ex != null)
            {
                reportText.AppendLine();
                reportText.AppendFormat(@"**** EXCEPTION (Level:{0}) ****", nestLevel);
                reportText.AppendLine();
                reportText.AppendLine(ex.Message);
                reportText.AppendFormat(@"Exception: {0}", ex.GetType().FullName);
                reportText.AppendLine();
                reportText.AppendLine(@"**** STACK TRACE ****");
                reportText.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
                nestLevel++;
            }

            return reportText.ToString();
        }
    }
}
