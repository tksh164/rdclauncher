using System.Diagnostics;

namespace rdclauncher
{
    internal static class UriNavigator
    {
        public static void Navigate(string uri)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = uri,
                UseShellExecute = true,
            });
        }
    }
}
