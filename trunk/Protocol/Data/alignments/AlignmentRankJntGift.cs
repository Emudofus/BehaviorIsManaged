

// Generated on 09/23/2012 21:40:19
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentRankJntGift")]
    public class AlignmentRankJntGift : IDataObject
    {
        private const String MODULE = "AlignmentRankJntGift";
        public int id;
        public List<int> gifts;
        public List<int> parameters;
        public List<int> levels;
    }
}