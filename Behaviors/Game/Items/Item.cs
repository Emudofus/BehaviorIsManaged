#region License GNU GPL
// Item.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.Effects;
using BiM.Core.Collections;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Items
{
    public class Item : ItemBase
    {
        private ReadOnlyObservableCollectionMT<EffectBase> m_readOnlyEffects;
        private ObservableCollectionMT<EffectBase> m_effects;


        public Item(ObjectItem item)
            : base(item.objectGID)
        {
            Guid = item.objectUID;
            m_effects = new ObservableCollectionMT<EffectBase>(item.effects.Select(EffectBase.CreateInstance));
            m_readOnlyEffects = new ReadOnlyObservableCollectionMT<EffectBase>(m_effects);
            Quantity = item.quantity;
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            Position = (CharacterInventoryPositionEnum) item.position;
        }


        public int Guid
        {
            get;
            protected set;
        }

        public ReadOnlyObservableCollectionMT<EffectBase> Effects
        {
            get { return m_readOnlyEffects; }
        }

        public CharacterInventoryPositionEnum Position
        {
            get;
            private set;
        }

        public bool IsUsable
        {
            get { return Template.usable; }
        }

        public int Quantity
        {
            get;
            private set;
        }

        public short PowerRate
        {
            get;
            private set;
        }

        public bool OverMax
        {
            get;
            private set;
        }

        public uint TotalWeight
        {
            get { return (uint) (UnityWeight*Quantity); }
        }

        public bool IsEquipped
        {
            get { return Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED; }
        }

        public void Update(ObjectItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Guid = item.objectUID;
            Template = ObjectDataManager.Instance.Get<Protocol.Data.Item>(item.objectGID);

            m_effects.Clear();
            foreach (EffectBase x in item.effects.Select(EffectBase.CreateInstance))
            {
                m_effects.Add(x);
            }

            Quantity = item.quantity;
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            Position = (CharacterInventoryPositionEnum) item.position;
        }

        public void Update(ObjectMovementMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Position = (CharacterInventoryPositionEnum) msg.position;
        }

        public void Update(ObjectQuantityMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Quantity = msg.quantity;
        }

        public void Update(ObjectItemQuantity msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Quantity = msg.quantity;
        }
    }
}