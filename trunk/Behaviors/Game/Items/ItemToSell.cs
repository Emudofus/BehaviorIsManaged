using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Items
{
    public class ItemToSell : ItemBase
    {
        public ItemToSell(ObjectItemToSell item)
        {
            Guid = item.objectUID;
            Template = DataProvider.Instance.GetObjectData<Protocol.Data.Item>(item.objectGID);
            Effects = new ObservableCollection<ObjectEffect>(item.effects);
            Quantity = item.quantity;
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            ObjectPrice = item.objectPrice;
        }

        public Protocol.Data.Item Template
        {
            get;
            set;
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