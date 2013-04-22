

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Breeds")]
    public class Breed : IDataObject
    {
        public const String MODULE = "Breeds";
        public int id;
        public uint shortNameId;
        public uint longNameId;
        public uint descriptionId;
        public uint gameplayDescriptionId;
        public String maleLook;
        public String femaleLook;
        public uint creatureBonesId;
        public int maleArtwork;
        public int femaleArtwork;
        public List<List<uint>> statsPointsForStrength;
        public List<List<uint>> statsPointsForIntelligence;
        public List<List<uint>> statsPointsForChance;
        public List<List<uint>> statsPointsForAgility;
        public List<List<uint>> statsPointsForVitality;
        public List<List<uint>> statsPointsForWisdom;
        public List<uint> breedSpellsId;
        public List<uint> maleColors;
        public List<uint> femaleColors;
    }
}