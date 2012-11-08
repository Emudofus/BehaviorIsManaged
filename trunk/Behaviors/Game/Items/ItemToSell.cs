#region License GNU GPL
// ItemToSell.cs
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
using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
using BiM.Core.Collections;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Items
{
    public class ItemToSell : ItemBase
    {
        public ItemToSell(ObjectItemToSell item)
            : base(item.objectGID)
        {
            Guid = item.objectUID;
            Effects = new ObservableCollectionMT<ObjectEffect>(item.effects);
            Quantity = item.quantity;
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            ObjectPrice = item.objectPrice;
        }

        public int Guid
        {
            get;
            protected set;
        }


        public ObservableCollection<ObjectEffect> Effects
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public short PowerRate
        {
            get;
            set;
        }

        public bool OverMax
        {
            get;
            set;
        }

        public int ObjectPrice
        {
            get;
            set;
        }
    }
}