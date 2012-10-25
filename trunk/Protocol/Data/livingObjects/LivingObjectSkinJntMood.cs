

// Generated on 10/25/2012 10:43:01
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