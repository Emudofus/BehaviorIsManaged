

// Generated on 10/25/2012 10:42:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage
    {
        public const uint Id = 6116;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int weaponGenericId;
        
        public GameActionFightCloseCombatMessage()
        {
        }
        
        public GameActionFightCloseCombatMessage(short actionId, int sourceId, int targetId, short destinationCellId, sbyte critical, bool silentCast, int weaponGenericId)
         : base(actionId, sourceId, targetId, destinationCellId, critical, silentCast)
        {
            this.weaponGenericId = weaponGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(weaponGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponGenericId = reader.ReadInt();
            if (weaponGenericId < 0)
                throw new Exception("Forbidden value on weaponGenericId = " + weaponGenericId + ", it doesn't respect the following condition : weaponGenericId < 0");
        }
        
    }
    
}