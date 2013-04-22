

// Generated on 04/17/2013 22:29:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightInvisibleDetectedMessage : AbstractGameActionMessage
    {
        public const uint Id = 6320;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short cellId;
        
        public GameActionFightInvisibleDetectedMessage()
        {
        }
        
        public GameActionFightInvisibleDetectedMessage(short actionId, int sourceId, int targetId, short cellId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            cellId = reader.ReadShort();
            if (cellId < -1 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
        }
        
    }
    
}