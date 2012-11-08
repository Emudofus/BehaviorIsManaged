#region License GNU GPL
// InventoryViewModel.cs
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
using System.ComponentModel;
using System.Windows;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Items;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BasicPlugin.Inventory
{
    internal class InventoryViewModelRegister
    {
        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.AddFrame(new InventoryViewModel(bot));
        }
    }

    public class InventoryViewModel : Frame<InventoryViewModel>, IViewModel<InventoryView>
    {
        private InputNumberDialog m_dialog;

        public InventoryViewModel(Bot bot)
            : base(bot)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return View; }
            set { View = (InventoryView)value; }
        }

        public InventoryView View
        {
            get;
            set;
        }


        #region EquipItemCommand

        private DelegateCommand m_equipItemCommand;

        public DelegateCommand EquipItemCommand
        {
            get { return m_equipItemCommand ?? (m_equipItemCommand = new DelegateCommand(OnEquipItem, CanEquipItem)); }
        }

        private bool CanEquipItem(object parameter)
        {
            return parameter is Item && Bot.Character.Inventory.CanEquip(parameter as Item);
        }

        private void OnEquipItem(object parameter)
        {
            if (parameter == null || !CanEquipItem(parameter))
                return;

            Bot.Character.Inventory.Equip(parameter as Item);
        }

        #endregion



        #region UnEquipCommand

        private DelegateCommand m_unEquipCommand;

        public DelegateCommand UnEquipCommand
        {
            get { return m_unEquipCommand ?? (m_unEquipCommand = new DelegateCommand(OnUnEquip, CanUnEquip)); }
        }

        private bool CanUnEquip(object parameter)
        {
            return parameter is Item && (parameter as Item).IsEquipped;
        }

        private void OnUnEquip(object parameter)
        {
            if (parameter == null || !CanUnEquip(parameter))
                return;

            var item = parameter as Item;

            Bot.Character.Inventory.Move(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
        }

        #endregion

        private int? InputNumber(int max)
        {
            if (m_dialog != null && m_dialog.Visibility == Visibility.Visible)
                m_dialog.Close();

            if (max == 1)
                return 1;

            m_dialog = new InputNumberDialog()
            {
                Min = 1,
                Max = max,
                Value = max,
            };

            if (m_dialog.ShowDialog() == true)
                return m_dialog.Value;
            return null;
        }

        #region RemoveItemCommand

        private DelegateCommand m_removeItemCommand;

        public DelegateCommand RemoveItemCommand
        {
            get { return m_removeItemCommand ?? (m_removeItemCommand = new DelegateCommand(OnRemoveItem, CanRemoveItem)); }
        }

        private bool CanRemoveItem(object parameter)
        {
            return parameter is Item && Bot.Character.Inventory.CanDelete(parameter as Item);
        }

        private void OnRemoveItem(object parameter)
        {
            if (parameter == null || !CanRemoveItem(parameter))
                return;

            var item = parameter as Item;

            var quantity = InputNumber(item.Quantity);
            if (quantity != null)
                Bot.Character.Inventory.Delete(item, quantity.Value);
        }

        #endregion


        #region DropItemCommand

        private DelegateCommand m_dropItemCommand;

        public DelegateCommand DropItemCommand
        {
            get { return m_dropItemCommand ?? (m_dropItemCommand = new DelegateCommand(OnDropItem, CanDropItem)); }
        }

        private bool CanDropItem(object parameter)
        {
            return parameter is Item && Bot.Character.Inventory.CanDrop(parameter as Item);
        }

        private void OnDropItem(object parameter)
        {
            if (parameter == null || !CanDropItem(parameter))
                return; 
            
            var item = parameter as Item;

            var quantity = InputNumber(item.Quantity);
            if (quantity != null)
                Bot.Character.Inventory.Drop(item, quantity.Value);
        }

        #endregion


        #region UseCommand

        private DelegateCommand m_useCommand;

        public DelegateCommand UseCommand
        {
            get { return m_useCommand ?? (m_useCommand = new DelegateCommand(OnUse, CanUse)); }
        }

        private bool CanUse(object parameter)
        {
            return parameter is Item && (parameter as Item).IsUsable && Bot.Character.Inventory.CanUse(parameter as Item);
        }

        private void OnUse(object parameter)
        {
            if (parameter == null || !CanUse(parameter))
                return;

            var item = parameter as Item;

            Bot.Character.Inventory.Use(item);
        }

        #endregion

        public override void OnAttached()
        {
            base.OnAttached();

            var viewModel = Bot.GetViewModel();
            var layout = viewModel.AddDocument(this, () => new InventoryView());
            layout.Title = "Inventory";
            layout.CanClose = false;
        }

        public override void OnDetached()
        {
            base.OnDetached();

            if (m_dialog != null && m_dialog.Visibility == Visibility.Visible)
                View.Dispatcher.Invoke(new Action(m_dialog.Close));
        }
    }
}