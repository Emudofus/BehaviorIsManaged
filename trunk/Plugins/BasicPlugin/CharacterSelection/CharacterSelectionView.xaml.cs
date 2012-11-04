using System.Windows.Controls;
using BiM.Host.UI.Views;

namespace BasicPlugin.CharacterSelection
{
    /// <summary>
    /// Interaction logic for CharacterSelectionView.xaml
    /// </summary>
    public partial class CharacterSelectionView : UserControl, IView<CharacterSelectionViewModel>
    {
        private CharacterSelectionViewModel m_viewModel;

        public CharacterSelectionView()
        {
            InitializeComponent();
        }

        #region IView<CharacterSelectionViewModel> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CharacterSelectionViewModel) value; }
        }

        public CharacterSelectionViewModel ViewModel
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