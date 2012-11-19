using System.Windows.Controls;
using BiM.Host.UI.Views;

namespace BasicPlugin.CharacterInfo
{
    /// <summary>
    /// Logique d'interaction pour CharacterInfoView.xaml
    /// </summary>
    public partial class CharacterInfoView : UserControl, IView<CharacterInfoViewModel>
    {
        private CharacterInfoViewModel m_viewModel;


        public CharacterInfoView()
        {
            InitializeComponent();
        }

        #region IView<CharacterInfoViewModel> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CharacterInfoViewModel) value; }
        }

        public CharacterInfoViewModel ViewModel
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