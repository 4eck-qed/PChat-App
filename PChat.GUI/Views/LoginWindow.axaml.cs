using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

// ReSharper disable once CheckNamespace
namespace PChat.GUI
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            while (DataContext is LoginWindowViewModel viewModel)
            {
                if (viewModel.MainWindowOpened)
                {
                    Close();
                    break;
                }
                Thread.Sleep(100);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}