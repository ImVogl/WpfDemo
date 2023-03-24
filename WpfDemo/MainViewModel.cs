using Prism.Commands;
using Prism.Mvvm;

namespace WpfDemo
{
    /// <summary>
    /// ViewModel for MainWindow.
    /// </summary>
    internal class MainViewModel : BindableBase
    {
        /// <summary>
        /// Backfield for main text.
        /// </summary>
        private string _mainText = string.Empty;

        /// <summary>
        /// Backfield for main text label.
        /// </summary>
        private string _mainTextLabel = string.Empty;

        /// <summary>
        /// Initialize new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel(MainModel? model = null)
        {
            SendText = new DelegateCommand(() => model?.SendTextSync(MainText));
        }

        /// <summary>
        /// Get or set main text.
        /// </summary>
        public string MainText
        {
            get => _mainText;
            set => SetProperty(ref _mainText, value, nameof(MainText));
        }

        /// <summary>
        /// Get or set main text label.
        /// </summary>
        public string MainTextLabel
        {
            get => _mainTextLabel;
            set => SetProperty(ref _mainTextLabel, value, nameof(MainTextLabel));
        }

        /// <summary>
        /// Get or set for command sending.
        /// </summary>
        public DelegateCommand? SendText { get;}
    }
}
