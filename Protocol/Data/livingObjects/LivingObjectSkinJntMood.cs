

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("LivingObjectSkinJntMood")]
    public class LivingObjectSkinJntMood : IDataObject
    {
        private const String MODULE = "LivingObjectSkinJntMood";
        public int skinId;
        public List<List<int>> moods;
    }
}