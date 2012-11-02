using System.Windows;
using BiM.Host.UI.ViewModels;

namespace BiM.Host.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView<UIManager>
    {
        public MainWindow()
        {
            DataContext = ViewModel = UIManager.Instance;
            ViewModel.View = this;
            InitializeComponent();
        }

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (UIManager)value; }
        }

        public UIManager ViewModel
        {
            get;
            set;
        }
    }
}
