

// Generated on 12/11/2012 19:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightVanishMessage : AbstractGameActionMessage
    {
        public const uint Id = 6217;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public GameActionFightVanishMessage()
        {
        }
        
        public GameActionFightVanishMessage(short actionId, int sourceId, int targetId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
        }
        
    }
    
}