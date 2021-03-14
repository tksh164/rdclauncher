using System.Reflection;

namespace rdclauncher
{
    internal static class WindowTitleBuilder
    {
        public static string GetWindowTitle()
        {
            const string WindowTitle = "Remote Desktop Client Launcher";
            var version = Assembly.GetEntryAssembly().GetName().Version;
            string versionText = string.Format("v{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            return string.Format("{0} {1}", WindowTitle, versionText);
        }
    }
}
