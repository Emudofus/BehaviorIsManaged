

// Generated on 10/25/2012 10:42:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FriendsListWithSpouseMessage : FriendsListMessage
    {
        public const uint Id = 5931;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FriendSpouseInformations spouse;
        
        public FriendsListWithSpouseMessage()
        {
        }
        
        public FriendsListWithSpouseMessage(Types.FriendInformations[] friendsList, Types.FriendSpouseInformations spouse)
         : base(friendsList)
        {
            this.spouse = spouse;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(spouse.TypeId);
            spouse.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spouse = Types.ProtocolTypeManager.GetInstance<Types.FriendSpouseInformations>(reader.ReadShort());
            spouse.Deserialize(reader);
        }
        
    }
    
}