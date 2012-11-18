#region License GNU GPL
// InputNumberDialog.xaml.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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

namespace BasicPlugin.Inventory
{
    /// <summary>
    /// Interaction logic for InputNumberDialog.xaml
    /// </summary>
    public partial class InputNumberDialog : Window, INotifyPropertyChanged
    {
        public InputNumberDialog()
        {
            InitializeComponent();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }

            base.OnKeyDown(e);
        }

        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
