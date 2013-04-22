

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentRankJntGift")]
    public class AlignmentRankJntGift : IDataObject
    {
        public const String MODULE = "AlignmentRankJntGift";
        public int id;
        public List<int> gifts;
        public List<int> parameters;
        public List<int> levels;
    }
}