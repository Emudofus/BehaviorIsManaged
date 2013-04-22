

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MonsterGrade")]
    public class MonsterGrade : IDataObject
    {
        public uint grade;
        public int monsterId;
        public uint level;
        public int paDodge;
        public int pmDodge;
        public int wisdom;
        public int earthResistance;
        public int airResistance;
        public int fireResistance;
        public int waterResistance;
        public int neutralResistance;
        public int gradeXp;
        public int lifePoints;
        public int actionPoints;
        public int movementPoints;
    }
}