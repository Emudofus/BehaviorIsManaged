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
using BiM.Protocol.Enums;

namespace BasicPlugin.CharacterSelection
{
    /// <summary>
    /// Interaction logic for CharacterCreationDialog.xaml
    /// </summary>
    public partial class CharacterCreationDialog : Window, INotifyPropertyChanged
    {
        public CharacterCreationDialog()
        {
            InitializeComponent();
            Data = new CharacterCreationData();
        }

        private CharacterCreationData m_data;

        public CharacterCreationData Data
        {
            get { return m_data; }
            set
            {
                m_data = value; DataContext = null; DataContext = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ButtonOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
