using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BasicPlugin.CharacterSelection
{
    /// <summary>
    /// Interaction logic for DeletionDialog.xaml
    /// </summary>
    public partial class DeletionDialog : Window, INotifyPropertyChanged
    {
        public DeletionDialog()
        {
            InitializeComponent();
        }

        public string SecretQuestion
        {
            get;
            set;
        }

        public string SecretAnswer
        {
            get;
            set;
        }

        public string CharacterName
        {
            get;
            set;
        }

        private void ButtonOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
