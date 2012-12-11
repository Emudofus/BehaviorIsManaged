

// Generated on 12/11/2012 19:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorListMessage : NetworkMessage
    {
        public const uint Id = 5930;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte nbcollectorMax;
        public short taxCollectorHireCost;
        public Types.TaxCollectorInformations[] informations;
        public Types.TaxCollectorFightersInformation[] fightersInformations;
        
        public TaxCollectorListMessage()
        {
        }
        
        public TaxCollectorListMessage(sbyte nbcollectorMax, short taxCollectorHireCost, Types.TaxCollectorInformations[] informations, Types.TaxCollectorFightersInformation[] fightersInformations)
        {
            this.nbcollectorMax = nbcollectorMax;
            this.taxCollectorHireCost = taxCollectorHireCost;
            this.informations = informations;
            this.fightersInformations = fightersInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(nbcollectorMax);
            writer.WriteShort(taxCollectorHireCost);
            writer.WriteUShort((ushort)informations.Length);
            foreach (var entry in informations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fightersInformations.Length);
            foreach (var entry in fightersInformations)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbcollectorMax = reader.ReadSByte();
            if (nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            taxCollectorHireCost = reader.ReadShort();
            if (taxCollectorHireCost < 0)
                throw new Exception("Forbidden value on taxCollectorHireCost = " + taxCollectorHireCost + ", it doesn't respect the following condition : taxCollectorHireCost < 0");
            var limit = reader.ReadUShort();
            informations = new Types.TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 informations[i] = Types.ProtocolTypeManager.GetInstance<Types.TaxCollectorInformations>(reader.ReadShort());
                 informations[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fightersInformations = new Types.TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightersInformations[i] = new Types.TaxCollectorFightersInformation();
                 fightersInformations[i].Deserialize(reader);
            }
        }
        
    }
    
}