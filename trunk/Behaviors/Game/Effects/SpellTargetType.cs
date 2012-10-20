using System;

namespace BiM.Behaviors.Game.Effects
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF = 0x1,
        ALLY_1 = 0x2,
        ALLY_2 = 0x4,
        ALLY_SUMMONS = 0x8,
        ALLY_STATIC_SUMMONS = 0x10,
        ALLY_3 = 0x20,
        ALLY_4 = 0x40,
        ALLY_5 = 0x80,
        ENNEMY_1 = 0x100,
        ENNEMY_2 = 0x200,
        ENNEMY_SUMMONS = 0x400,
        ENNEMY_STATIC_SUMMONS = 0x800,
        ENNEMY_3 = 0x1000,
        ENNEMY_4 = 0x2000,
        ENNEMY_5 = 0x4000,
        ALL = 0x7FFF,
        ONLY_SELF = 0x8000,
    }
}