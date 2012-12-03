using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    public interface ICell
    {
        [ProtoMember(1)]
        short Id
        {
            get;
        }

        [ProtoMember(2)]
        short Floor
        {
            get;
        }

        [ProtoMember(3)]
        byte LosMov
        {
            get;
        }

        [ProtoMember(4)]
        byte Speed
        {
            get;
        }

        [ProtoMember(5)]
        byte MapChangeData
        {
            get;
        }

        [ProtoMember(6)]
        byte MoveZone
        {
            get;
        }

        bool Walkable
        {
            get;
        }

        bool LineOfSight
        {
            get;
        }

        bool NonWalkableDuringFight
        {
            get;
        }

        bool FarmCell
        {
            get;
        }

        bool Visible
        {
            get;
        }

        bool NonWalkableDuringRP
        {
            get;
        }
    }
}