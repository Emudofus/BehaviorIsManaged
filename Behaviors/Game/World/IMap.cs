using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    public interface IMap
    {
        [ProtoMember(1)]
        int Id
        {
            get;
        }

        [ProtoMember(2)]
        byte Version
        {
            get;
        }

        [ProtoMember(3)]
        bool Encrypted
        {
            get;
        }

        [ProtoMember(4)]
        byte EncryptionVersion
        {
            get;
        }

        [ProtoMember(5)]
        uint RelativeId
        {
            get;
        }

        [ProtoMember(6)]
        byte MapType
        {
            get;
        }

        [ProtoMember(7)]
        int SubAreaId
        {
            get;
        }

        [ProtoMember(8)]
        int BottomNeighbourId
        {
            get;
        }

        [ProtoMember(9)]
        int TopNeighbourId
        {
            get;
        }

        [ProtoMember(10)]
        int LeftNeighbourId
        {
            get;
        }

        [ProtoMember(11)]
        int RightNeighbourId
        {
            get;
        }

        [ProtoMember(12)]
        bool UsingNewMovementSystem
        {
            get;
        }

        [ProtoMember(14)]
        ICellList<ICell> Cells
        {
            get;
        }

        [ProtoMember(15)]
        int X
        {
            get;
        }

        [ProtoMember(16)]
        int Y
        {
            get;
        }

        [ProtoMember(17)]
        int WorldMap
        {
            get;
        }

        [ProtoMember(18)]
        bool Outdoor
        {
            get;
        }
    }
}