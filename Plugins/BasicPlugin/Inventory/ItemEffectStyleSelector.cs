#region License GNU GPL
// ItemEffectStyleSelector.cs
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
using System.Windows.Media;

namespace BasicPlugin.Inventory
{
    public class ItemEffectStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item,
                                          DependencyObject container)
        {
            var st = new Style {TargetType = typeof (ListBoxItem)};
            var backGroundSetter = new Setter {Property = Control.BackgroundProperty};
            var listBox = ItemsControl.ItemsControlFromItemContainer(container) as ListBox;
            int index = listBox.ItemContainerGenerator.IndexFromContainer(container);

            backGroundSetter.Value = index%2 == 0 ? Brushes.LightYellow : Brushes.Transparent;
            st.Setters.Add(backGroundSetter);

            return st;
        }
    }
}