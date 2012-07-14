using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Items
{
    public class ItemToSellInNpcShop: ItemBase
    {
        public ItemToSellInNpcShop(ObjectItemToSellInNpcShop item)
        {
            Template = DataProvider.Instance.GetObjectData<Protocol.Data.Item>(item.objectGID);
            Effects = new ObservableCollection<ObjectEffect>(item.effects);
            PowerRate = item.powerRate;
            OverMax = item.overMax;
            ObjectPrice = item.objectPrice;
            BuyCriterion = item.buyCriterion;
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

        public string BuyCriterion
        {
            get;
            set;
        }
    }
}