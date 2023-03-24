using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfDemo.MessageBroker;

namespace WpfDemo
{
    /// <summary>
    /// Model for main window.
    /// </summary>
    internal class TextProcessor
    {
        /// <summary>
        /// <see cref="RpcClient"/>.
        /// </summary>
        private readonly RpcClient _client;

        /// <summary>
        /// UI dispatcher.
        /// </summary>
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Initialize new instance of <see cref="TextProcessor"/>.
        /// </summary>
        /// <param name="client"><see cref="RpcClient"/>.</param>
        public TextProcessor(RpcClient client)
        {
            _client = client;
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler<string> TextModified;

        /// <summary>
        /// Sending text.
        /// </summary>
        /// <param name="text">Sent text.</param>
        public async Task SendTextAsync(string text)
        {
            var changedText = await _client.ModifyTextAsync(text);
            var handler = TextModified;
            _dispatcher.Invoke(() => handler?.Invoke(this, changedText));
        }
    }
}
