

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightPlacementPossiblePositionsMessage : NetworkMessage
    {
        public const uint Id = 703;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] positionsForChallengers;
        public short[] positionsForDefenders;
        public sbyte teamNumber;
        
        public GameFightPlacementPossiblePositionsMessage()
        {
        }
        
        public GameFightPlacementPossiblePositionsMessage(short[] positionsForChallengers, short[] positionsForDefenders, sbyte teamNumber)
        {
            this.positionsForChallengers = positionsForChallengers;
            this.positionsForDefenders = positionsForDefenders;
            this.teamNumber = teamNumber;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)positionsForChallengers.Length);
            foreach (var entry in positionsForChallengers)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)positionsForDefenders.Length);
            foreach (var entry in positionsForDefenders)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteSByte(teamNumber);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            positionsForChallengers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 positionsForChallengers[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            positionsForDefenders = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 positionsForDefenders[i] = reader.ReadShort();
            }
            teamNumber = reader.ReadSByte();
            if (teamNumber < 0)
                throw new Exception("Forbidden value on teamNumber = " + teamNumber + ", it doesn't respect the following condition : teamNumber < 0");
        }
        
    }
    
}