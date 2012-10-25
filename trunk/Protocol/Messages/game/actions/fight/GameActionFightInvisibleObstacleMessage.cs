

// Generated on 10/25/2012 10:42:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightInvisibleObstacleMessage : AbstractGameActionMessage
    {
        public const uint Id = 5820;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int sourceSpellId;
        
        public GameActionFightInvisibleObstacleMessage()
        {
        }
        
        public GameActionFightInvisibleObstacleMessage(short actionId, int sourceId, int sourceSpellId)
         : base(actionId, sourceId)
        {
            this.sourceSpellId = sourceSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(sourceSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            sourceSpellId = reader.ReadInt();
            if (sourceSpellId < 0)
                throw new Exception("Forbidden value on sourceSpellId = " + sourceSpellId + ", it doesn't respect the following condition : sourceSpellId < 0");
        }
        
    }
    
}