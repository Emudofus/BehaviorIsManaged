

// Generated on 04/17/2013 22:30:03
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TitlesAndOrnamentsListMessage : NetworkMessage
    {
        public const uint Id = 6367;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] titles;
        public short[] ornaments;
        public short activeTitle;
        public short activeOrnament;
        
        public TitlesAndOrnamentsListMessage()
        {
        }
        
        public TitlesAndOrnamentsListMessage(short[] titles, short[] ornaments, short activeTitle, short activeOrnament)
        {
            this.titles = titles;
            this.ornaments = ornaments;
            this.activeTitle = activeTitle;
            this.activeOrnament = activeOrnament;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)titles.Length);
            foreach (var entry in titles)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)ornaments.Length);
            foreach (var entry in ornaments)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteShort(activeTitle);
            writer.WriteShort(activeOrnament);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            titles = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 titles[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            ornaments = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 ornaments[i] = reader.ReadShort();
            }
            activeTitle = reader.ReadShort();
            if (activeTitle < 0)
                throw new Exception("Forbidden value on activeTitle = " + activeTitle + ", it doesn't respect the following condition : activeTitle < 0");
            activeOrnament = reader.ReadShort();
            if (activeOrnament < 0)
                throw new Exception("Forbidden value on activeOrnament = " + activeOrnament + ", it doesn't respect the following condition : activeOrnament < 0");
        }
        
    }
    
}