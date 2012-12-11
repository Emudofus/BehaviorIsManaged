

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapRunningFightDetailsMessage : NetworkMessage
    {
        public const uint Id = 5751;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public string[] names;
        public short[] levels;
        public sbyte teamSwap;
        public bool[] alives;
        
        public MapRunningFightDetailsMessage()
        {
        }
        
        public MapRunningFightDetailsMessage(int fightId, string[] names, short[] levels, sbyte teamSwap, bool[] alives)
        {
            this.fightId = fightId;
            this.names = names;
            this.levels = levels;
            this.teamSwap = teamSwap;
            this.alives = alives;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteUShort((ushort)names.Length);
            foreach (var entry in names)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)levels.Length);
            foreach (var entry in levels)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteSByte(teamSwap);
            writer.WriteUShort((ushort)alives.Length);
            foreach (var entry in alives)
            {
                 writer.WriteBoolean(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            names = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 names[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            levels = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 levels[i] = reader.ReadShort();
            }
            teamSwap = reader.ReadSByte();
            if (teamSwap < 0)
                throw new Exception("Forbidden value on teamSwap = " + teamSwap + ", it doesn't respect the following condition : teamSwap < 0");
            limit = reader.ReadUShort();
            alives = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 alives[i] = reader.ReadBoolean();
            }
        }
        
    }
    
}