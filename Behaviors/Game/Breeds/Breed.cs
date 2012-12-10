#region License GNU GPL
// Breed.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Stats;

namespace BiM.Behaviors.Game.Breeds
{
    public class Breed : INotifyPropertyChanged
    {
        public Breed(Protocol.Data.Breed breed)
        {
            if (breed == null) throw new ArgumentNullException("breed");
            Id = breed.id;
            ShortName = I18NDataManager.Instance.ReadText(breed.shortNameId);
            LongName = I18NDataManager.Instance.ReadText(breed.longNameId);
            Description = I18NDataManager.Instance.ReadText(breed.descriptionId);
            GameplayDescription = I18NDataManager.Instance.ReadText(breed.gameplayDescriptionId);
            MaleLook = breed.maleLook;
            FemaleLook = breed.femaleLook;
            CreatureBonesId = breed.creatureBonesId;
            MaleArtwork = breed.maleArtwork;
            FemaleArtwork = breed.femaleArtwork;
            StrengthThreshold = ListsToThresholds(breed.statsPointsForStrength);
            AgilityThreshold = ListsToThresholds(breed.statsPointsForAgility);
            VitalityThreshold = ListsToThresholds(breed.statsPointsForVitality);
            WisdomThreshold = ListsToThresholds(breed.statsPointsForWisdom);
            ChanceThreshold = ListsToThresholds(breed.statsPointsForChance);
            IntelligenceThreshold = ListsToThresholds(breed.statsPointsForIntelligence);
            SpellsId = breed.breedSpellsId.ToArray();
            MaleColors = breed.maleColors.ToArray();
            FemaleColors = breed.femaleColors.ToArray();
            AlternativeMaleSkin = breed.alternativeMaleSkin.ToArray();
            AlternativeFemaleSkin = breed.alternativeFemaleSkin.ToArray();
        }

        public int Id
        {
            get;
            private set;
        }

        public string ShortName
        {
            get;
            private set;
        }

        public string LongName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string GameplayDescription
        {
            get;
            private set;
        }

        public string MaleLook
        {
            get;
            private set;
        }

        public string FemaleLook
        {
            get;
            private set;
        }

        public uint CreatureBonesId
        {
            get;
            private set;
        }

        public int MaleArtwork
        {
            get;
            private set;
        }

        public int FemaleArtwork
        {
            get;
            private set;
        }

        public BoostThreshold[] StrengthThreshold
        {
            get;
            private set;
        }
        public BoostThreshold[] VitalityThreshold
        {
            get;
            private set;
        }

        public BoostThreshold[] AgilityThreshold
        {
            get;
            private set;
        }

        public BoostThreshold[] ChanceThreshold
        {
            get;
            private set;
        }

        public BoostThreshold[] IntelligenceThreshold
        {
            get;
            private set;
        }

        public BoostThreshold[] WisdomThreshold
        {
            get;
            private set;
        }

        public uint[] SpellsId
        {
            get;
            private set;
        }

        public uint[] MaleColors
        {
            get;
            private set;
        }

        public uint[] FemaleColors
        {
            get;
            private set;
        }

        public uint[] AlternativeMaleSkin
        {
            get;
            private set;
        }

        public uint[] AlternativeFemaleSkin
        {
            get;
            private set;
        }

        public BoostThreshold[] GetThresholds(BoostableStat stat)
        {
            switch (stat)
            {
                case BoostableStat.Agility:
                    return AgilityThreshold;
                case BoostableStat.Strength:
                    return StrengthThreshold;
                case BoostableStat.Intelligence:
                    return IntelligenceThreshold;
                case BoostableStat.Chance:
                    return ChanceThreshold;
                case BoostableStat.Wisdom:
                    return WisdomThreshold;
                case BoostableStat.Vitality:
                    return VitalityThreshold;
                default:
                    throw new ArgumentException("stat");
            }
        }

        public BoostThreshold GetThreshold(int actualpoints, BoostableStat stat)
        {
            return GetThreshold(actualpoints, GetThresholds(stat));
        }

        public BoostThreshold GetThreshold(int actualpoints, BoostThreshold[] thresholds)
        {
            return thresholds[GetThresholdIndex(actualpoints, thresholds)];
        }

        /// <summary>
        /// Gives the next threshold or null if not found
        /// </summary>
        /// <param name="actualpoints"></param>
        /// <param name="stat"> </param>
        /// <returns></returns>
        public BoostThreshold GetNextThreshold(int actualpoints, BoostableStat stat)
        {
            return GetNextThreshold(actualpoints, GetThresholds(stat));
        }

        /// <summary>
        /// Gives the next threshold or null if not found
        /// </summary>
        /// <param name="actualpoints"></param>
        /// <param name="thresholds"></param>
        /// <returns></returns>
        public BoostThreshold GetNextThreshold(int actualpoints, BoostThreshold[] thresholds)
        {
            var index = GetThresholdIndex(actualpoints, thresholds);
            return thresholds.Length > index + 1 ? thresholds[index + 1] : null;
        }

        private static int GetThresholdIndex(int actualpoints, BoostThreshold[] thresholds)
        {
            for (int i = 0; i < thresholds.Length - 1; i++)
            {
                if (thresholds[i].PointsThreshold <= actualpoints &&
                    thresholds[i + 1].PointsThreshold > actualpoints)
                    return i;
            }

            return thresholds.Length - 1;
        }

        private static BoostThreshold[] ListsToThresholds(List<List<uint>> lists)
        {
            var thresholds = new BoostThreshold[lists.Count];
            for (int i = 0; i < lists.Count; i++)
            {
                thresholds[i] = new BoostThreshold(lists[i]);
            }

            return thresholds;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}