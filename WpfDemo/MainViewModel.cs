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
        /// Backfield for "IsSendingEnable" property.
        /// </summary>
        private bool _isSendingEnable;

        /// <summary>
        /// Initialize new instance of <see cref="MainViewModel"/>.
        /// </summary>
        public MainViewModel(TextProcessor? model = null)
        {
            SendText = new DelegateCommand(() => model?.SendTextAsync(MainText)).ObservesCanExecute(() => IsSendingEnable);
            if (model != null)
                model.TextModified += (sender, changedText) => MainTextLabel = changedText;
        }

        /// <summary>
        /// Get or set main text.
        /// </summary>
        public string MainText
        {
            get => _mainText;
            set
            {
                SetProperty(ref _mainText, value, nameof(MainText));
                IsSendingEnable = !string.IsNullOrWhiteSpace(MainText);
            }
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
        public DelegateCommand? SendText { get; }

        /// <summary>
        /// Get or set value is indicating that SendText button is available.
        /// </summary>
        private bool IsSendingEnable
        {
            get => _isSendingEnable;
            set => SetProperty(ref _isSendingEnable, value, nameof(IsSendingEnable));
        }
    }
}
