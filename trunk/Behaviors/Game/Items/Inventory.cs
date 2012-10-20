using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.World;
using BiM.Core.Collections;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Game.Items
{
    public class Inventory : INotifyPropertyChanged
    {
        private readonly Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]> m_itemsPositioningRules
            = new Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]>
          {
              {ItemSuperTypeEnum.SUPERTYPE_AMULET, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET}},
              {ItemSuperTypeEnum.SUPERTYPE_WEAPON, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
              {ItemSuperTypeEnum.SUPERTYPE_WEAPON_7, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
              {ItemSuperTypeEnum.SUPERTYPE_CAPE, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE}},
              {ItemSuperTypeEnum.SUPERTYPE_HAT, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT}},
              {ItemSuperTypeEnum.SUPERTYPE_RING, new [] {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT}},
              {ItemSuperTypeEnum.SUPERTYPE_BOOTS, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS}},
              {ItemSuperTypeEnum.SUPERTYPE_BELT, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT}},
              {ItemSuperTypeEnum.SUPERTYPE_PET, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS}},
              {ItemSuperTypeEnum.SUPERTYPE_DOFUS, new [] {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6}},
              {ItemSuperTypeEnum.SUPERTYPE_SHIELD, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD}},

          };

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ReadOnlyObservableCollectionMT<Item> m_readOnlyItems;
        private ObservableCollectionMT<Item> m_items; 

        public Inventory(PlayedCharacter owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
            m_items = new ObservableCollectionMT<Item>();
            m_readOnlyItems = new ReadOnlyObservableCollectionMT<Item>(m_items);
        }

        public Inventory(PlayedCharacter owner, InventoryContentMessage inventory)
            : this (owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (inventory == null) throw new ArgumentNullException("inventory");
            Kamas = inventory.kamas;

            Update(inventory);
        }

        public PlayedCharacter Owner
        {
            get;
            set;
        }

        public ReadOnlyObservableCollectionMT<Item> Items
        {
            get { return m_readOnlyItems; }
        }

        public int Kamas
        {
            get;
            private set;
        }

        public int Weight
        {
            get;
            private set;
        }

        public int WeightMax
        {
            get;
            private set;
        }

        public bool HasItem(int guid)
        {
            return Items.Any(x => x.Guid == guid);
        }

        public bool HasItem(Item item)
        {
            return HasItem(item.Guid);
        }


        public bool HasItem(CharacterInventoryPositionEnum position)
        {
            return Items.Any(x => x.Position == position);
        }

        public Item GetItem(int guid)
        {
            return Items.FirstOrDefault(x => x.Guid == guid);
        }

        public Item GetItem(CharacterInventoryPositionEnum position)
        {
            return Items.FirstOrDefault(x => x.Position == position);
        }

        public Item[] GetEquipedItems()
        {
            return Items.Where(x => x.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).ToArray();
        }

        public bool CanMove(Item item, CharacterInventoryPositionEnum position, int quantity)
        {
            if (Owner.IsFighting() && Owner.Fight.Phase != FightPhase.Placement)
                return false;

            if (!HasItem(item))
            {
                logger.Error("Cannot move item {0} because the item is not own", item.Name);
                return false;
            }

            if (quantity > item.Quantity)
            {
                logger.Error("Cannot move item {0} because the moved quantity ({1}) is greater than the actual item quantity", item.Name, quantity, item.Quantity);
                return false;
            }

            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && quantity != 1)
            {
                logger.Error("Cannot equip item {0} because the moved quantity > 1", item.Name);
                return false;
            }

            if (m_itemsPositioningRules.ContainsKey(item.SuperType) && !m_itemsPositioningRules[item.SuperType].Contains(position))
            {
                logger.Error("Cannot equip item {0} to {1} because the excepted position is {2}", item.Name, position, m_itemsPositioningRules[item.SuperType][0]);
                return false;
            }

            return true;
        }

        public bool Equip(Item item)
        {
            if (!m_itemsPositioningRules.ContainsKey(item.SuperType))
            {
                logger.Error("Cannot equip item {0} because the super type {1} has no position associated", item.Name, item.SuperType);
                return false;
            }

            var availablePositions = m_itemsPositioningRules[item.SuperType].Where(x => !HasItem(x)).ToArray();
            var position = availablePositions.Length == 0 ? m_itemsPositioningRules[item.SuperType].First() : availablePositions.First();

            return Move(item, position, 1);
        }

        public bool Move(Item item, CharacterInventoryPositionEnum position)
        {
            return Move(item, position, item.Quantity);
        }

        public bool Move(Item item, CharacterInventoryPositionEnum position, int quantity)
        {
            if (!CanMove(item, position, quantity))
                return false;

            Owner.Bot.SendToServer(new ObjectSetPositionMessage(item.Guid, (byte) position, quantity));
            return true;
        }

        public bool Delete(Item item)
        {
            return Delete(item, item.Quantity);
        }

        public bool Delete(Item item, int quantity)
        {
            if (Owner.IsFighting() && Owner.Fight.Phase != FightPhase.Placement)
                return false;

            if (!HasItem(item))
                return false;

            Owner.Bot.SendToServer(new ObjectDeleteMessage(item.Guid, quantity));
            return true;
        }

        public bool Drop(Item item)
        {
            return Drop(item, item.Quantity);
        }

        public bool Drop(Item item, int quantity)
        {
            if (!(Owner.Context is Map))
                return false;

            if (!HasItem(item))
                return false;

            // todo : check if near cells are free

            Owner.Bot.SendToServer(new ObjectDropMessage(item.Guid, quantity));
            return true;
        }

        public bool Use(Item item)
        {
            if (Owner.IsFighting() && Owner.Fight.Phase != FightPhase.Placement)
                return false;

            if (!HasItem(item))
                return false;

            Owner.Bot.SendToServer(new ObjectUseMessage(item.Guid));
            return true;
        }

        internal void AddItem(Item item)
        {
            m_items.Add(item);
        }

        internal bool RemoveItem(int guid)
        {
            var item = GetItem(guid);

            if (item == null)
                return false;

            return m_items.Remove(item);
        }

        public void Update(InventoryContentMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            m_items.Clear();

            foreach (var item in msg.objects.Select(entry => new Item(entry)))
            {
                m_items.Add(item);
            }
        }

        public void Update(InventoryWeightMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Weight = msg.weight;
            WeightMax = msg.weightMax;
        }

        public void Update(ObjectAddedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            AddItem(new Item(msg.@object));
        }

        public void Update(ObjectDeletedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            RemoveItem(msg.objectUID);
        }

        public void Update(ObjectModifiedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            var item = GetItem(msg.@object.objectUID);

            if (item == null)
                logger.Warn("Try to update item {0} but item not found !", msg.@object.objectUID);
            else
                item.Update(msg.@object);
        }

        public void Update(ObjectMovementMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            var item = GetItem(msg.objectUID);

            if (item == null)
                logger.Warn("Try to update item {0} but item not found !", msg.objectUID);
            else
                item.Update(msg);
        }

        public void Update(ObjectQuantityMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            var item = GetItem(msg.objectUID);

            if (item == null)
                logger.Warn("Try to update item {0} but item not found !", msg.objectUID);
            else
                item.Update(msg);
        }

        public void Update(ObjectsAddedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            foreach (var item in msg.@object)
            {
                AddItem(new Item(item));
            }
        }

        public void Update(ObjectsDeletedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            foreach (var item in msg.objectUID)
            {
                RemoveItem(item);
            }
        }

        public void Update(ObjectsQuantityMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            foreach (var obj in msg.objectsUIDAndQty)
            {
                var item = GetItem(obj.objectUID);

                if (item == null)
                    logger.Warn("Try to update item {0} but item not found !", obj.objectUID);
                else
                    item.Update(obj);
            }
        }

        public void Update(ObjectUseMessage msg)
        {

        }

        public void Update(ObjectUseMultipleMessage msg)
        {
        }

        public void Update(ObjectUseOnCellMessage msg)
        {

        }

        public void Update(ObjectUseOnCharacterMessage msg)
        {

        }

        public void Update(SetUpdateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}