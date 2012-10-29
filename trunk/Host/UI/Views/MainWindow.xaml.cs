using System.Windows;

namespace BiM.Host.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = UIManager.Instance;
            InitializeComponent();
        }
    }
}
