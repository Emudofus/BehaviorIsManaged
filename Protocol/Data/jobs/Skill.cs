

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Skills")]
    public class Skill : IDataObject
    {
        private const String MODULE = "Skills";
        public int id;
        public uint nameId;
        public int parentJobId;
        public Boolean isForgemagus;
        public int modifiableItemType;
        public int gatheredRessourceItem;
        public List<int> craftableItemIds;
        public int interactiveId;
        public String useAnimation;
        public Boolean isRepair;
        public int cursor;
        public Boolean availableInHouse;
        public uint levelMin;
    }
}