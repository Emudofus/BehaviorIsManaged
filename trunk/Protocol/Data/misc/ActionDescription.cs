

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ActionDescription")]
    public class ActionDescription : IDataObject
    {
        public const String MODULE = "ActionDescriptions";
        public uint id;
        public uint typeId;
        public String name;
        public uint descriptionId;
        public Boolean trusted;
        public Boolean needInteraction;
        public uint maxUsePerFrame;
        public uint minimalUseInterval;
        public Boolean needConfirmation;
    }
}