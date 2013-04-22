

// Generated on 04/17/2013 22:30:01
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class InventoryPresetSaveCustomMessage : NetworkMessage
    {
        public const uint Id = 6329;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public byte[] itemsPositions;
        public int[] itemsUids;
        
        public InventoryPresetSaveCustomMessage()
        {
        }
        
        public InventoryPresetSaveCustomMessage(sbyte presetId, sbyte symbolId, byte[] itemsPositions, int[] itemsUids)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.itemsPositions = itemsPositions;
            this.itemsUids = itemsUids;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            writer.WriteUShort((ushort)itemsPositions.Length);
            foreach (var entry in itemsPositions)
            {
                 writer.WriteByte(entry);
            }
            writer.WriteUShort((ushort)itemsUids.Length);
            foreach (var entry in itemsUids)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            var limit = reader.ReadUShort();
            itemsPositions = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemsPositions[i] = reader.ReadByte();
            }
            limit = reader.ReadUShort();
            itemsUids = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemsUids[i] = reader.ReadInt();
            }
        }
        
    }
    
}