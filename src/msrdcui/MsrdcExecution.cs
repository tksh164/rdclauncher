using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace rdclauncher
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
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = GetMsrdcExeFilePath(),
                    Arguments = BuildMsrdcArguments(tempRdpFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                });
                Thread.Sleep(1000);
            }
            finally
            {
                File.Delete(tempRdpFilePath);
            }
        }

        private static string CreateTempRdpFile(MsrdcLaunchSettings settings)
        {
            var msrdcWindowTitle = GetMsrdcWindowTitle(settings.WindowTitle, settings.RemoteComputer, settings.RemotePort);
            var tempRdpFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".rdp");
            using (var stream = new FileStream(tempRdpFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                // The name or IP address of the remote computer that you want to connect to.
                writer.WriteLine("full address:s:{0}:{1}", settings.RemoteComputer, settings.RemotePort);

                // 0: The RDP client will not prompt for credentials when connecting to a server that does not support server authentication.
                // 1: The RDP client will prompt for credentials when connecting to a server that does not support server authentication.
                writer.WriteLine("prompt for credentials:i:1");

                // 0: When connecting to the remote computer, do not use the administrative session.
                // 1: When connecting to the remote computer, use the administrative session.
                writer.WriteLine("administrative session:i:1");

                // The window title of the RDP client.
                writer.WriteLine("remotedesktopname:s:{0}", msrdcWindowTitle);

                // 0: The local window content won't scale when resized.
                // 1: The local window content will scale when resized.
                writer.WriteLine("smart sizing:i:{0}", settings.IsFitSessionToWindowEnabled ? "1" : "0");

                // 0: Session resolution remains static for the duration of the session.
                // 1: Session resolution updates as the local window resizes.
                writer.WriteLine("dynamic resolution:i:{0}", settings.IsUpdateResolutionOnResizeEnabled ? "1" : "0");

                // 1: The remote session will appear in a window.
                // 2: The remote session will appear full screen.
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

        private static string BuildMsrdcArguments(string tempRdpFilePath)
        {
            var options = new List<string>
            {
                WrapWithDoubleQuote(tempRdpFilePath),
                string.Format("/p:{0}", Process.GetCurrentProcess().Id),
            };
            return string.Join(" ", options);
        }

        private static string WrapWithDoubleQuote(string str)
        {
            return @"""" + str + @"""";
        }
    }
}
