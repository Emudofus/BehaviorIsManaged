

// Generated on 12/11/2012 19:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class UpdateMountBoostMessage : NetworkMessage
    {
        public const uint Id = 6179;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double rideId;
        public Types.UpdateMountBoost[] boostToUpdateList;
        
        public UpdateMountBoostMessage()
        {
        }
        
        public UpdateMountBoostMessage(double rideId, Types.UpdateMountBoost[] boostToUpdateList)
        {
            this.rideId = rideId;
            this.boostToUpdateList = boostToUpdateList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(rideId);
            writer.WriteUShort((ushort)boostToUpdateList.Length);
            foreach (var entry in boostToUpdateList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            rideId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            boostToUpdateList = new Types.UpdateMountBoost[limit];
            for (int i = 0; i < limit; i++)
            {
                 boostToUpdateList[i] = Types.ProtocolTypeManager.GetInstance<Types.UpdateMountBoost>(reader.ReadShort());
                 boostToUpdateList[i].Deserialize(reader);
            }
        }
        
    }
    
}