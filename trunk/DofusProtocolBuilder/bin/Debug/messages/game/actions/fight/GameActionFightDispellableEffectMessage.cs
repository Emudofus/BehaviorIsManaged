

// Generated on 10/25/2012 10:42:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightDispellableEffectMessage : AbstractGameActionMessage
    {
        public const uint Id = 6070;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AbstractFightDispellableEffect effect;
        
        public GameActionFightDispellableEffectMessage()
        {
        }
        
        public GameActionFightDispellableEffectMessage(short actionId, int sourceId, Types.AbstractFightDispellableEffect effect)
         : base(actionId, sourceId)
        {
            this.effect = effect;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(effect.TypeId);
            effect.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            effect = Types.ProtocolTypeManager.GetInstance<Types.AbstractFightDispellableEffect>(reader.ReadShort());
            effect.Deserialize(reader);
        }
        
    }
    
}