

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class PartyGuestInformations
    {
        public const short Id = 374;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int guestId;
        public int hostId;
        public string name;
        public Types.EntityLook guestLook;
        public sbyte breed;
        public bool sex;
        public Types.PlayerStatus status;
        
        public PartyGuestInformations()
        {
        }
        
        public PartyGuestInformations(int guestId, int hostId, string name, Types.EntityLook guestLook, sbyte breed, bool sex, Types.PlayerStatus status)
        {
            this.guestId = guestId;
            this.hostId = hostId;
            this.name = name;
            this.guestLook = guestLook;
            this.breed = breed;
            this.sex = sex;
            this.status = status;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guestId);
            writer.WriteInt(hostId);
            writer.WriteUTF(name);
            guestLook.Serialize(writer);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            guestId = reader.ReadInt();
            if (guestId < 0)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0");
            hostId = reader.ReadInt();
            if (hostId < 0)
                throw new Exception("Forbidden value on hostId = " + hostId + ", it doesn't respect the following condition : hostId < 0");
            name = reader.ReadUTF();
            guestLook = new Types.EntityLook();
            guestLook.Deserialize(reader);
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
        }
        
    }
    
}