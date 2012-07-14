using System;

namespace BiM.Core.Network
{
    [Flags]
    public enum ListenerEntry
    {
        Undefined = 0,
        Local = 1,
        Client = 2,
        Server = 4,
    }
}