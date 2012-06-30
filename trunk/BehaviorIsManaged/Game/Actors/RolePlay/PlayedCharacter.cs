using System;
using BiM.Game.Guilds;
using BiM.Game.Items;
using BiM.Game.Spells;

namespace BiM.Game.Actors.RolePlay
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