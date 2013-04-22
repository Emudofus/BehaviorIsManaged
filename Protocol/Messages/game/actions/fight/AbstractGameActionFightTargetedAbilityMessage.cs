

// Generated on 04/17/2013 22:29:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AbstractGameActionFightTargetedAbilityMessage : AbstractGameActionMessage
    {
        public const uint Id = 6118;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short destinationCellId;
        public sbyte critical;
        public bool silentCast;
        
        public AbstractGameActionFightTargetedAbilityMessage()
        {
        }
        
        public AbstractGameActionFightTargetedAbilityMessage(short actionId, int sourceId, int targetId, short destinationCellId, sbyte critical, bool silentCast)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.destinationCellId = destinationCellId;
            this.critical = critical;
            this.silentCast = silentCast;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(destinationCellId);
            writer.WriteSByte(critical);
            writer.WriteBoolean(silentCast);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            targetId = reader.ReadInt();
            destinationCellId = reader.ReadShort();
            if (destinationCellId < -1 || destinationCellId > 559)
                throw new Exception("Forbidden value on destinationCellId = " + destinationCellId + ", it doesn't respect the following condition : destinationCellId < -1 || destinationCellId > 559");
            critical = reader.ReadSByte();
            if (critical < 0)
                throw new Exception("Forbidden value on critical = " + critical + ", it doesn't respect the following condition : critical < 0");
            silentCast = reader.ReadBoolean();
        }
        
    }
    
}