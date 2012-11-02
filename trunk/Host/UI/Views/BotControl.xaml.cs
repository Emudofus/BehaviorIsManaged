using System.Windows;
using System.Windows.Controls;
using BiM.Host.UI.ViewModels;

namespace BiM.Host.UI.Views
{
    /// <summary>
    /// Interaction logic for BotControl.xaml
    /// </summary>
    public partial class BotControl : UserControl, IView<BotViewModel>
    {
        private BotViewModel m_viewModel;

        public BotControl()
        {
            InitializeComponent();
        }

        #region IView<BotViewModel> Members

        public BotViewModel ViewModel
        {
            get { return m_viewModel; }
            set
            {
                m_viewModel = value;
                ( (FrameworkElement)Content ).DataContext = null;
                ((FrameworkElement) Content).DataContext = value;
            }
        }

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BotViewModel) value; }
        }

        #endregion
    }
}