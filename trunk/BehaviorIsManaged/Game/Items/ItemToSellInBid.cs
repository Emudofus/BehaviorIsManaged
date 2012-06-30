using BiM.Protocol.Types;

namespace BiM.Game.Items
{
    public class ItemToSellInBid : ItemToSell
    {
        public ItemToSellInBid(ObjectItemToSellInBid item)
            : base(item)
        {
            UnsoldDelay = item.unsoldDelay;
        }

        public short UnsoldDelay
        {
            get;
            set;
        }
    }
}