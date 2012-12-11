

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KrosmasterInventoryMessage : NetworkMessage
    {
        public const uint Id = 6350;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.KrosmasterFigure[] figures;
        
        public KrosmasterInventoryMessage()
        {
        }
        
        public KrosmasterInventoryMessage(Types.KrosmasterFigure[] figures)
        {
            this.figures = figures;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)figures.Length);
            foreach (var entry in figures)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            figures = new Types.KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++)
            {
                 figures[i] = new Types.KrosmasterFigure();
                 figures[i].Deserialize(reader);
            }
        }
        
    }
    
}