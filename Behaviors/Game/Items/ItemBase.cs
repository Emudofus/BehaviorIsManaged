#region License GNU GPL
// ItemBase.cs
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
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Items.Icons;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Game.Items
{
    public abstract class ItemBase : INotifyPropertyChanged
    {
        private string m_description;
        private string m_name;
        private ItemType m_type;
        private ItemIcon m_icon;

        protected ItemBase(int templateId)
        {
            Template = DataProvider.Instance.Get<Protocol.Data.Item>(templateId);
        }


        public Protocol.Data.Item Template
        {
            get;
            protected set;
        }

        public ItemType Type
        {
            get { return m_type ?? (m_type = DataProvider.Instance.Get<ItemType>(Template.typeId)); }
        }

        public string Name
        {
            get { return m_name ?? (m_name = DataProvider.Instance.Get<string>(Template.nameId)); }
        }

        public string Description
        {
            get { return m_description ?? (m_description = DataProvider.Instance.Get<string>(Template.descriptionId)); }
        }

        public ItemSuperTypeEnum SuperType
        {
            get { return (ItemSuperTypeEnum) Type.superTypeId; }
        }

        public uint Level
        {
            get { return Template.level; }
        }

        public uint UnityWeight
        {
            get { return Template.realWeight; }
        }

        public ItemIcon Icon
        {
            get { return m_icon ?? (m_icon = DataProvider.Instance.Get<ItemIcon>(Template.iconId)); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion
    }
}