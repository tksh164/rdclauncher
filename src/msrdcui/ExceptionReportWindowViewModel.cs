using System;
using System.Threading;
using System.Windows;

namespace rdclauncher
{
    public sealed class ExceptionReportWindowViewModel : ViewModelBase
    {
        public RelayCommand CloseExceptionReportWindowCommand { get; private set; }
        public RelayCommand CopyExceptionReportToClipboardCommand { get; private set; }
        public RelayCommand OpenProjectWebsiteUriCommand { get; private set; }

        public ExceptionReportWindowViewModel(Action windowCloseAction)
        {
            _windowCloseAction = windowCloseAction;
            CopyButtonContentText = CopyButtonContentTextDoCopy;
            CloseExceptionReportWindowCommand = new RelayCommand(ExecuteCloseExceptionReportWindow);
            CopyExceptionReportToClipboardCommand = new RelayCommand(ExecuteCopyExceptionReportToClipboard);
            OpenProjectWebsiteUriCommand = new RelayCommand(ExecuteOpenProjectWebsiteUri);
        }

        private string _exceptionReportText;
        public string ExceptionReportText
        {
            get => _exceptionReportText;
            set => SetProperty(ref _exceptionReportText, value);
        }

        private string _copyButtonContentText;
        public string CopyButtonContentText
        {
            get => _copyButtonContentText;
            set => SetProperty(ref _copyButtonContentText, value);
        }

        private Action _windowCloseAction;
        private void ExecuteCloseExceptionReportWindow(object obj)
        {
            _windowCloseAction?.Invoke();
        }

        private const string CopyButtonContentTextDoCopy = "Copy the exception report to the clipboard";
        private const string CopyButtonContentTextDoneCopy = "Copied!";

        private void ExecuteCopyExceptionReportToClipboard(object obj)
        {
            Clipboard.SetText(ExceptionReportText);

            // Change the button caption and reset it after some seconds elapsed.
            CopyButtonContentText = CopyButtonContentTextDoneCopy;
            var resetTimer = new Timer((state) => {
                CopyButtonContentText = CopyButtonContentTextDoCopy;
                var timer = (Timer)state;
                timer.Dispose();
            });
            const int RestMilliseconds = 5000;
            resetTimer.Change(RestMilliseconds, Timeout.Infinite);
        }

        private void ExecuteOpenProjectWebsiteUri(object obj)
        {
            const string ProjectWebsiteIssuesUri = "https://github.com/tksh164/rdclauncher/issues";
            UriNavigator.Navigate(ProjectWebsiteIssuesUri);
        }
    }
}
