using System.Windows.Controls;
using BiM.Host.UI.Views;

namespace BasicPlugin.UI
{
    /// <summary>
    /// Interaction logic for ServerSelectionView.xaml
    /// </summary>
    public partial class ServerSelectionView : UserControl, IView<ServerSelectorModelView>
    {
        private ServerSelectorModelView m_viewModel;

        public ServerSelectionView()
        {
            InitializeComponent();
        }

        #region IView<ServerSelectorModelView> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ServerSelectorModelView) value; }
        }

        public ServerSelectorModelView ViewModel
        {
            get { return m_viewModel; }
            set
            {
                m_viewModel = value;
                DataContext = null;
                DataContext = value;
            }
        }

        #endregion
    }
}