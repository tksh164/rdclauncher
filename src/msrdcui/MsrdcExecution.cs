using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace msrdcui
{
    internal sealed class MsrdcLaunchSettings
    { 
        public string RemoteComputer { get; set; }
        public uint RemotePort { get; set; }
        public string WindowTitle { get; set; }
        public bool IsFitSessionToWindowEnabled { get; set; }
        public bool IsUpdateResolutionOnResizeEnabled { get; set; }
        public bool IsFullScreenEnabled { get; set; }
    }

    internal static class MsrdcExecution
    {
        public static void LaunchMsrdc(MsrdcLaunchSettings settings)
        {
            var tempRdpFilePath = CreateTempRdpFile(settings);
            var msrdcWindowTitle = GetMsrdcWindowTitle(settings.WindowTitle, settings.RemoteComputer, settings.RemotePort);

            try
            {
                using (var process = Process.Start(new ProcessStartInfo()
                {
                    FileName = GetMsrdcExeFilePath(),
                    Arguments = BuildMsrdcArguments(tempRdpFilePath, msrdcWindowTitle),
                    UseShellExecute = false,
                }))
                {
                    process.WaitForExit();
                }
            }
            finally
            {
                File.Delete(tempRdpFilePath);
            }
        }

        private static string CreateTempRdpFile(MsrdcLaunchSettings settings)
        {
            var tempRdpFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".rdp");
            using (var stream = new FileStream(tempRdpFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine("full address:s:{0}:{1}", settings.RemoteComputer, settings.RemotePort);
                writer.WriteLine("prompt for credentials:i:1");
                writer.WriteLine("administrative session:i:1");
                writer.WriteLine("smart sizing:i:{0}", settings.IsFitSessionToWindowEnabled ? "1" : "0");
                writer.WriteLine("dynamic resolution:i:{0}", settings.IsUpdateResolutionOnResizeEnabled ? "1" : "0");
                writer.WriteLine("screen mode id:i:{0}", settings.IsFullScreenEnabled ? "2" : "1");
            }
            return tempRdpFilePath;
        }

        private static string GetMsrdcWindowTitle(string windowTitle, string remoteComputer, uint port)
        {
            if (string.IsNullOrEmpty(windowTitle))
            {
                return string.Format("{0}:{1}", remoteComputer, port);
            }
            else
            {
                return string.Format("{0} - {1}:{2}", windowTitle, remoteComputer, port);
            }
        }

        private static string GetMsrdcExeFilePath()
        {
            const string MsrdcExeSystemInstallPath = @"C:\Program Files\Remote Desktop\msrdc.exe";
            if (File.Exists(MsrdcExeSystemInstallPath)) return MsrdcExeSystemInstallPath;

            const string MsrdcExePerUserInstallPath = @"%LocalAppData%\Apps\Remote Desktop\msrdc.exe";
            var expandedMsrdcExePerUserInstallPath = Environment.ExpandEnvironmentVariables(MsrdcExePerUserInstallPath);
            if (File.Exists(expandedMsrdcExePerUserInstallPath)) return expandedMsrdcExePerUserInstallPath;

            throw new FileNotFoundException("msrdc.exe does not exists.");
        }

        private static string BuildMsrdcArguments(string tempRdpFilePath, string windowTitle)
        {
            var options = new List<string>
            {
                WrapWithDoubleQuote(tempRdpFilePath),
                string.Format("/n:{0}", WrapWithDoubleQuote(windowTitle)),
            };
            return string.Join(" ", options);
        }

        private static string WrapWithDoubleQuote(string str)
        {
            return @"""" + str + @"""";
        }
    }
}
