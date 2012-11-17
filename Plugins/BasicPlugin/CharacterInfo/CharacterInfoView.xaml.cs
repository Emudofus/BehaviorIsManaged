using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            set { ViewModel = (CharacterInfoViewModel)value; }
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
