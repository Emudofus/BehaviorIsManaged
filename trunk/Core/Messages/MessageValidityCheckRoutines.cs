using System;

namespace BiM.Core.Messages
{
    // note : not sure if i keep that
    [Flags]
    public enum MessageValidityCheckRoutines
    {
        None = 0,
        BotNotNull = 1,
        CharacterNotNull = 3,
        CharacterFighting = 7,
    }
}