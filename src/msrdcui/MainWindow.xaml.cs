using System.Windows;

namespace rdclauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = GetInitializedViewModel();
        }

        private static MainWindowViewModel GetInitializedViewModel()
        {
            var vm = new MainWindowViewModel()
            {
                DefaultPortNumber = Properties.Settings.Default.DefaultPortNumber,
                IsFitSessionToWindowEnabled = Properties.Settings.Default.DefaultFitSessionToWindowEnabled,
                IsUpdateResolutionOnResizeEnabled = Properties.Settings.Default.DefaultUpdateResolutionOnResizeEnabled,
                IsFullScreenEnabled = Properties.Settings.Default.DefaultFullScreenEnabled,
            };

            var remoteComputerHistory = PersistentUserSettings.ReadRemoteComputerHistory();
            foreach (var historyItem in remoteComputerHistory)
            {
                vm.RemoteComputerHistory.Add(historyItem);
            }
            vm.RemoteComputer = remoteComputerHistory.Count > 0 ? remoteComputerHistory[0] : "";

            var rdcWindowTitleHistory = PersistentUserSettings.ReadRdcWindowTitleHistory();
            foreach (var historyItem in rdcWindowTitleHistory)
            {
                vm.RdcWindowTitleHistory.Add(historyItem);
            }

            return vm;
        }
    }
}
