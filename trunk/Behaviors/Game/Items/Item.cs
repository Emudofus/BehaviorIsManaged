using System;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Data;
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
        private string m_name;
        private ReadOnlyObservableCollectionMT<EffectBase> m_readOnlyEffects;
        private ObservableCollectionMT<EffectBase> m_effects;
        private string m_description;
        private ItemType m_type;

        public Item(ObjectItem item)
        {
            Guid = item.objectUID;
            Template = DataProvider.Instance.Get<Protocol.Data.Item>(item.objectGID);
            m_effects = new ObservableCollectionMT<EffectBase>(item.effects.Select(EffectBase.CreateInstance));
            m_readOnlyEffects = new ReadOnlyObservableCollectionMT<EffectBase>(m_effects);
            Quantity = item.quantity;
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            Position = (CharacterInventoryPositionEnum) item.position;
        }

        public Protocol.Data.Item Template
        {
            get;
            private set;
        }

        public ItemType Type
        {
            get
            {
                return m_type ?? ( m_type = DataProvider.Instance.Get<ItemType>(Template.typeId) );
            }
        }

        public string Name
        {
            get { return m_name ?? (m_name = DataProvider.Instance.Get<string>(Template.nameId)); }
        }

        public string Description
        {
            get
            {
                return m_description ?? ( m_description = DataProvider.Instance.Get<string>(Template.descriptionId) );
            }
        }

        public ItemSuperTypeEnum SuperType
        {
            get { return (ItemSuperTypeEnum) Type.superTypeId; }
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

        public bool IsEquiped()
        {
            return Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        public void Update(ObjectItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Guid = item.objectUID;
            Template = DataProvider.Instance.Get<Protocol.Data.Item>(item.objectGID);

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