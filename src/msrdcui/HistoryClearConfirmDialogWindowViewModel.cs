using System.Windows;

namespace rdclauncher
{
    public enum HistoryClearDialogResult
    {
        Cancel,
        DoClear
    }

    public sealed class HistoryClearConfirmDialogWindowViewModel : ViewModelBase
    {
        public HistoryClearConfirmDialogWindowViewModel()
        {
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            DialogResult = HistoryClearDialogResult.Cancel;
        }

        public RelayCommand ClearCommand { get; private set; }

        public HistoryClearDialogResult DialogResult { get; private set;}

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }

        private void ExecuteClearCommand(object obj)
        {
            DialogResult = HistoryClearDialogResult.DoClear;
            Window dialogWindow = obj as Window;
            dialogWindow.Close();
        }
    }
}
