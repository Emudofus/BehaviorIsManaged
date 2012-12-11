

// Generated on 12/11/2012 19:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightExchangePositionsMessage : AbstractGameActionMessage
    {
        public const uint Id = 5527;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short casterCellId;
        public short targetCellId;
        
        public GameActionFightExchangePositionsMessage()
        {
        }
        
        public GameActionFightExchangePositionsMessage(short actionId, int sourceId, int targetId, short casterCellId, short targetCellId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.casterCellId = casterCellId;
            this.targetCellId = targetCellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(casterCellId);
            writer.WriteShort(targetCellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            casterCellId = reader.ReadShort();
            if (casterCellId < -1 || casterCellId > 559)
                throw new Exception("Forbidden value on casterCellId = " + casterCellId + ", it doesn't respect the following condition : casterCellId < -1 || casterCellId > 559");
            targetCellId = reader.ReadShort();
            if (targetCellId < -1 || targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : targetCellId < -1 || targetCellId > 559");
        }
        
    }
    
}