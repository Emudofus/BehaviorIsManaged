

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanInformations
    {
        public const short Id = 157;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.EntityLook[] followingCharactersLook;
        public sbyte emoteId;
        public double emoteStartTime;
        public Types.ActorRestrictionsInformations restrictions;
        public short titleId;
        public string titleParam;
        
        public HumanInformations()
        {
        }
        
        public HumanInformations(Types.EntityLook[] followingCharactersLook, sbyte emoteId, double emoteStartTime, Types.ActorRestrictionsInformations restrictions, short titleId, string titleParam)
        {
            this.followingCharactersLook = followingCharactersLook;
            this.emoteId = emoteId;
            this.emoteStartTime = emoteStartTime;
            this.restrictions = restrictions;
            this.titleId = titleId;
            this.titleParam = titleParam;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)followingCharactersLook.Length);
            foreach (var entry in followingCharactersLook)
            {
                 entry.Serialize(writer);
            }
            writer.WriteSByte(emoteId);
            writer.WriteDouble(emoteStartTime);
            restrictions.Serialize(writer);
            writer.WriteShort(titleId);
            writer.WriteUTF(titleParam);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            followingCharactersLook = new Types.EntityLook[limit];
            for (int i = 0; i < limit; i++)
            {
                 followingCharactersLook[i] = new Types.EntityLook();
                 followingCharactersLook[i].Deserialize(reader);
            }
            emoteId = reader.ReadSByte();
            emoteStartTime = reader.ReadDouble();
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            titleId = reader.ReadShort();
            if (titleId < 0)
                throw new Exception("Forbidden value on titleId = " + titleId + ", it doesn't respect the following condition : titleId < 0");
            titleParam = reader.ReadUTF();
        }
        
    }
    
}