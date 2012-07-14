using BiM.Behaviors.Game.Guilds;
using BiM.Behaviors.Game.Items;
using BiM.Behaviors.Game.Spells;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class PlayedCharacter : Character
    {
        public PlayedCharacter()
        {
        }

        public Stats.Stats Stats
        {
            get;
            set;
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public SpellsBook SpellsBook
        {
            get;
            set;
        }

        public Guild Guild
        {
            get;
            set;
        }
    }
}