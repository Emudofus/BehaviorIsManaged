

// Generated on 12/11/2012 19:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildFightPlayersEnemiesListMessage : NetworkMessage
    {
        public const uint Id = 5928;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public Types.CharacterMinimalPlusLookInformations[] playerInfo;
        
        public GuildFightPlayersEnemiesListMessage()
        {
        }
        
        public GuildFightPlayersEnemiesListMessage(double fightId, Types.CharacterMinimalPlusLookInformations[] playerInfo)
        {
            this.fightId = fightId;
            this.playerInfo = playerInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteUShort((ushort)playerInfo.Length);
            foreach (var entry in playerInfo)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            playerInfo = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 playerInfo[i] = new Types.CharacterMinimalPlusLookInformations();
                 playerInfo[i].Deserialize(reader);
            }
        }
        
    }
    
}