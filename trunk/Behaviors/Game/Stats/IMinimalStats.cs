using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Stats
{
    public interface IMinimalStats
    {

        int Initiative
        {
            get;
        }

        int Health
        {
            get;
        }

        int MaxHealth
        {
            get;
        }

        int MaxHealthBase
        {
            get;
        }

        int CurrentAP
        {
            get;
        }

        int CurrentMP
        {
            get;
        }

        int MaxAP
        {
            get;
        }

        int MaxMP
        {
            get;
        }

        int PermanentDamagePercent
        {
            get;
        }

        int TackleBlock
        {
            get;
        }

        int TackleEvade
        {
            get;
        }

        int DodgeAPProbability
        {
            get;
        }

        int DodgeMPProbability
        {
            get;
        }

        int NeutralResistPercent
        {
            get;
        }

        int EarthResistPercent
        {
            get;
        }

        int WaterResistPercent
        {
            get;
        }

        int AirResistPercent
        {
            get;
        }

        int FireResistPercent
        {
            get;
        }

        int NeutralElementReduction
        {
            get;
        }

        int EarthElementReduction
        {
            get;
        }

        int WaterElementReduction
        {
            get;
        }

        int AirElementReduction
        {
            get;
        }

        int FireElementReduction
        {
            get;
        }
        void Update(GameFightMinimalStats stats);
    }
}