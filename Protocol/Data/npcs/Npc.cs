

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Npcs")]
    public class Npc : IDataObject
    {
        public const String MODULE = "Npcs";
        public int id;
        public uint nameId;
        public List<List<int>> dialogMessages;
        public List<List<int>> dialogReplies;
        public List<uint> actions;
        public uint gender;
        public String look;
        public int tokenShop;
        public List<AnimFunNpcData> animFunList;
    }
}