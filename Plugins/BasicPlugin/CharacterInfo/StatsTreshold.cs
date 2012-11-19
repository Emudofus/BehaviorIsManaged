using System;
using System.Collections.Generic;
using BiM.Protocol.Data;

namespace BasicPlugin.CharacterInfo
{
    class StatsTreshold
    {
        static public List<List<uint>> GetThresholds(short statsid,Breed breed)
        {
            switch (statsid)
            {
                case 14:
                    return breed.statsPointsForAgility;
                case 13:
                    return breed.statsPointsForChance;
                case 15:
                    return breed.statsPointsForIntelligence;
                case 10:
                    return breed.statsPointsForStrength;
                case 12:
                    return breed.statsPointsForWisdom;
                case 11:
                    return breed.statsPointsForVitality;
                default:
                    throw new ArgumentException("statsid");
            }
        }

        static public List<uint> GetThreshold(short actualpoints, short statsid, Breed breed)
        {
            List<List<uint>> thresholds = GetThresholds(statsid, breed);
            return thresholds[GetThresholdIndex(actualpoints, thresholds)];
        }

        static public int GetThresholdIndex(int actualpoints, List<List<uint>> thresholds)
        {
            for (int i = 0; i < thresholds.Count - 1; i++)
            {
                if (thresholds[i][0] <= actualpoints &&
                    thresholds[i + 1][0] > actualpoints)
                    return i;
            }

            return thresholds.Count - 1;
        }
    }
}
