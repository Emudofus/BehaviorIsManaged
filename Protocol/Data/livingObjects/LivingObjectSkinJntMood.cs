

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("LivingObjectSkinJntMood")]
    public class LivingObjectSkinJntMood : IDataObject
    {
        public const String MODULE = "LivingObjectSkinJntMood";
        public int skinId;
        public List<List<int>> moods;
    }
}