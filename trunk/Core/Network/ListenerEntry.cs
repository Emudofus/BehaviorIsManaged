using System;

namespace BiM.Core.Network
{
    [Flags]
    public enum ListenerEntry
    {
        Local,
        Client,
        Server,
    }
}