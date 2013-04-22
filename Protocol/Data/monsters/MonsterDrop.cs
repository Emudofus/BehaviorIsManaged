

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MonsterDrop")]
    public class MonsterDrop : IDataObject
    {
        public uint dropId;
        public int monsterId;
        public int objectId;
        public float percentDropForGrade1;
        public float percentDropForGrade2;
        public float percentDropForGrade3;
        public float percentDropForGrade4;
        public float percentDropForGrade5;
        public int count;
        public int findCeil;
        public Boolean hasCriteria;
    }
}