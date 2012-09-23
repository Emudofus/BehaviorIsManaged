

// Generated on 09/23/2012 22:26:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MapComplementaryInformationsDataMessage : NetworkMessage
    {
        public const uint Id = 226;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public int mapId;
        public sbyte subareaAlignmentSide;
        public Types.HouseInformations[] houses;
        public Types.GameRolePlayActorInformations[] actors;
        public Types.InteractiveElement[] interactiveElements;
        public Types.StatedElement[] statedElements;
        public Types.MapObstacle[] obstacles;
        public Types.FightCommonInformations[] fights;
        
        public MapComplementaryInformationsDataMessage()
        {
        }
        
        public MapComplementaryInformationsDataMessage(short subAreaId, int mapId, sbyte subareaAlignmentSide, Types.HouseInformations[] houses, Types.GameRolePlayActorInformations[] actors, Types.InteractiveElement[] interactiveElements, Types.StatedElement[] statedElements, Types.MapObstacle[] obstacles, Types.FightCommonInformations[] fights)
        {
            this.subAreaId = subAreaId;
            this.mapId = mapId;
            this.subareaAlignmentSide = subareaAlignmentSide;
            this.houses = houses;
            this.actors = actors;
            this.interactiveElements = interactiveElements;
            this.statedElements = statedElements;
            this.obstacles = obstacles;
            this.fights = fights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteInt(mapId);
            writer.WriteSByte(subareaAlignmentSide);
            writer.WriteUShort((ushort)houses.Length);
            foreach (var entry in houses)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)actors.Length);
            foreach (var entry in actors)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)interactiveElements.Length);
            foreach (var entry in interactiveElements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)statedElements.Length);
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)obstacles.Length);
            foreach (var entry in obstacles)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fights.Length);
            foreach (var entry in fights)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            subareaAlignmentSide = reader.ReadSByte();
            var limit = reader.ReadUShort();
            houses = new Types.HouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 houses[i] = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
                 houses[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            actors = new Types.GameRolePlayActorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 actors[i] = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
                 actors[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            interactiveElements = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 interactiveElements[i] = new Types.InteractiveElement();
                 interactiveElements[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            statedElements = new Types.StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 statedElements[i] = new Types.StatedElement();
                 statedElements[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            obstacles = new Types.MapObstacle[limit];
            for (int i = 0; i < limit; i++)
            {
                 obstacles[i] = new Types.MapObstacle();
                 obstacles[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fights = new Types.FightCommonInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 fights[i] = new Types.FightCommonInformations();
                 fights[i].Deserialize(reader);
            }
        }
        
    }
    
}