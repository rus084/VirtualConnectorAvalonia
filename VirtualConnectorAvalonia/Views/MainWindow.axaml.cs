using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VirtualConnectorAvalonia.ViewModels;

namespace VirtualConnectorAvalonia
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
           // Networks = new List<DataRouter>();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
