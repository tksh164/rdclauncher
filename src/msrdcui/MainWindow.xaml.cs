using System.Windows;

namespace msrdcui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel()
            {
                PortNumber = Properties.Settings.Default.PortNumber,
                IsFitSessionToWindowEnabled = Properties.Settings.Default.DefaultFitSessionToWindowEnabled,
                IsUpdateResolutionOnResizeEnabled = Properties.Settings.Default.DefaultUpdateResolutionOnResizeEnabled,
                IsFullScreenEnabled = Properties.Settings.Default.DefaultFullScreenEnabled,
            };
            var remoteComputerHistory = PersistentUserSettings.ReadRemoteComputerHistory();
            foreach (var historyItem in remoteComputerHistory)
            {
                vm.RemoteComputerHistory.Add(historyItem);
            }
            vm.RemoteComputer = remoteComputerHistory[0];
            this.DataContext = vm;
        }
    }
}
