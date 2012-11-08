#region License GNU GPL
// InventoryView.xaml.cs
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BiM.Host.UI.Views;

namespace BasicPlugin.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl, IView<InventoryViewModel>
    {
        public InventoryView()
        {
            InitializeComponent();
        }

        #region IView<InventoryViewModel> Members

        object IView.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (InventoryViewModel) value;

                DataContext = null;
                DataContext = value;
            }
        }

        public InventoryViewModel ViewModel
        {
            get;
            set;
        }

        #endregion

        private void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnContextMenuLoaded(object sender, RoutedEventArgs e)
        {
            NameScope.SetNameScope((ContextMenu)sender, NameScope.GetNameScope(this));
        }
    }
}