using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BiM.Host.UI.Views;
using Xceed.Wpf.Toolkit;

namespace SnifferPlugin
{
    /// <summary>
    /// Interaction logic for SnifferView.xaml
    /// </summary>
    public partial class SnifferView : UserControl, IView<SnifferViewModel>
    {
        private SnifferViewModel m_viewModel;



        public SnifferView()
        {
            InitializeComponent();
        }

        #region IView<SnifferViewModel> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SnifferViewModel)value; }
        }

        public SnifferViewModel ViewModel
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

        private void OnSearchTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var expr = ((WatermarkTextBox)sender).GetBindingExpression(TextBox.TextProperty);
            if (expr != null)
                expr.UpdateSource();
        }

        private void OnSearchTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var expr = ((WatermarkTextBox)sender).GetBindingExpression(TextBox.TextProperty);
            if (expr != null)
                expr.UpdateSource();
        }

        private void checkBoxOnTheFly_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}