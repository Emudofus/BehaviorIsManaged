using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Items
{
    public class Inventory : INotifyPropertyChanged
    {
        public Inventory(PlayedCharacter owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
            Items = new ObservableCollection<Item>();
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

        public ObservableCollection<Item> Items
        {
            get;
            set;
        }

        public int Kamas
        {
            get;
            set;
        }

        public bool HasItem(int guid)
        {
            return false;
        }

        public Item GetItem(int guid)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(CharacterInventoryPositionEnum position)
        {
            throw new NotImplementedException();
        }

        public Item[] GetEquipedItems()
        {
            throw new NotImplementedException();
        }

        public bool Equip(Item item)
        {
            throw new NotImplementedException();
        }

        public bool Move(Item item, CharacterInventoryPositionEnum position)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Item item)
        {
            throw new NotImplementedException();
        }

        public bool Drop(Item item)
        {
            throw new NotImplementedException();
        }

        public bool Use(Item item)
        {
            throw new NotImplementedException();
        }

        public void Update(InventoryContentMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Items.Clear();

            foreach (var item in msg.objects.Select(entry => new Item(entry)))
            {
                Items.Add(item);
            }
        }

        public void Update(SetUpdateMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            // todo
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}