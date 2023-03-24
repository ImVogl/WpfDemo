using System.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using WpfDemo.MessageBroker;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<RpcClient>();
            containerRegistry.RegisterSingleton<TextProcessor>();
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <inheritdoc />
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
        }
    }
}
