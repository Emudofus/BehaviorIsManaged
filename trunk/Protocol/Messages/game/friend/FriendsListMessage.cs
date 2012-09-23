

// Generated on 09/23/2012 22:26:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FriendsListMessage : NetworkMessage
    {
        public const uint Id = 4002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FriendInformations[] friendsList;
        
        public FriendsListMessage()
        {
        }
        
        public FriendsListMessage(Types.FriendInformations[] friendsList)
        {
            this.friendsList = friendsList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)friendsList.Length);
            foreach (var entry in friendsList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            friendsList = new Types.FriendInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 friendsList[i] = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
                 friendsList[i].Deserialize(reader);
            }
        }
        
    }
    
}