

// Generated on 12/11/2012 19:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GuildEmblem
    {
        public const short Id = 87;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short symbolShape;
        public int symbolColor;
        public short backgroundShape;
        public int backgroundColor;
        
        public GuildEmblem()
        {
        }
        
        public GuildEmblem(short symbolShape, int symbolColor, short backgroundShape, int backgroundColor)
        {
            this.symbolShape = symbolShape;
            this.symbolColor = symbolColor;
            this.backgroundShape = backgroundShape;
            this.backgroundColor = backgroundColor;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(symbolShape);
            writer.WriteInt(symbolColor);
            writer.WriteShort(backgroundShape);
            writer.WriteInt(backgroundColor);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            symbolShape = reader.ReadShort();
            symbolColor = reader.ReadInt();
            backgroundShape = reader.ReadShort();
            backgroundColor = reader.ReadInt();
        }
        
    }
    
}