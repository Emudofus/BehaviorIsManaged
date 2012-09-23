

// Generated on 09/23/2012 22:27:05
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismWorldInformationMessage : NetworkMessage
    {
        public const uint Id = 5854;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int nbSubOwned;
        public int subTotal;
        public int maxSub;
        public Types.PrismSubAreaInformation[] subAreasInformation;
        public int nbConqsOwned;
        public int conqsTotal;
        public Types.VillageConquestPrismInformation[] conquetesInformation;
        
        public PrismWorldInformationMessage()
        {
        }
        
        public PrismWorldInformationMessage(int nbSubOwned, int subTotal, int maxSub, Types.PrismSubAreaInformation[] subAreasInformation, int nbConqsOwned, int conqsTotal, Types.VillageConquestPrismInformation[] conquetesInformation)
        {
            this.nbSubOwned = nbSubOwned;
            this.subTotal = subTotal;
            this.maxSub = maxSub;
            this.subAreasInformation = subAreasInformation;
            this.nbConqsOwned = nbConqsOwned;
            this.conqsTotal = conqsTotal;
            this.conquetesInformation = conquetesInformation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(nbSubOwned);
            writer.WriteInt(subTotal);
            writer.WriteInt(maxSub);
            writer.WriteUShort((ushort)subAreasInformation.Length);
            foreach (var entry in subAreasInformation)
            {
                 entry.Serialize(writer);
            }
            writer.WriteInt(nbConqsOwned);
            writer.WriteInt(conqsTotal);
            writer.WriteUShort((ushort)conquetesInformation.Length);
            foreach (var entry in conquetesInformation)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            nbSubOwned = reader.ReadInt();
            if (nbSubOwned < 0)
                throw new Exception("Forbidden value on nbSubOwned = " + nbSubOwned + ", it doesn't respect the following condition : nbSubOwned < 0");
            subTotal = reader.ReadInt();
            if (subTotal < 0)
                throw new Exception("Forbidden value on subTotal = " + subTotal + ", it doesn't respect the following condition : subTotal < 0");
            maxSub = reader.ReadInt();
            if (maxSub < 0)
                throw new Exception("Forbidden value on maxSub = " + maxSub + ", it doesn't respect the following condition : maxSub < 0");
            var limit = reader.ReadUShort();
            subAreasInformation = new Types.PrismSubAreaInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 subAreasInformation[i] = new Types.PrismSubAreaInformation();
                 subAreasInformation[i].Deserialize(reader);
            }
            nbConqsOwned = reader.ReadInt();
            if (nbConqsOwned < 0)
                throw new Exception("Forbidden value on nbConqsOwned = " + nbConqsOwned + ", it doesn't respect the following condition : nbConqsOwned < 0");
            conqsTotal = reader.ReadInt();
            if (conqsTotal < 0)
                throw new Exception("Forbidden value on conqsTotal = " + conqsTotal + ", it doesn't respect the following condition : conqsTotal < 0");
            limit = reader.ReadUShort();
            conquetesInformation = new Types.VillageConquestPrismInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 conquetesInformation[i] = new Types.VillageConquestPrismInformation();
                 conquetesInformation[i].Deserialize(reader);
            }
        }
        
    }
    
}