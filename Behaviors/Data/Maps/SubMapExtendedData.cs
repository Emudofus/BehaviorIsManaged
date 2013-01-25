using System.Collections.Generic;

namespace BiM.Behaviors.Data.Maps
{
    public class SubMapExtendedData
    {
        #region Data we want to store for each SubMap
        public SortedSet<int> MobFoundOnTheMap { get; set; }
        public SortedSet<ushort> MobLevels { get; set; }
        public SortedSet<ushort> GroupLevels { get; set; }
        public SortedList<int, int> Ressources { get; set; } // Id ressource => Count
        public bool? AgressiveMobs { get; set; } // Is there any chance to find aggressive mobs on this submap
        public bool RestrictedArea { get; set; } // Area restricted for unsubscribed players
        public int MinLevel { get { if (GroupLevels == null || GroupLevels.Count == 0) return 0; return GroupLevels.Min; } }
        public int MaxLevel { get { if (GroupLevels == null || GroupLevels.Count == 0) return 0; return GroupLevels.Max; } }
        #endregion Data we want to store for each SubMap

        public static SubMapExtendedData LoadData(int SubMapId)
        {
            throw new System.NotImplementedException();
        }

        public static SubMapExtendedData UpdateData(int SubMapId)
        {
            throw new System.NotImplementedException();
        }

        public static SubMapExtendedData ResetData(int? SubMapId)
        {
            throw new System.NotImplementedException();
        }

    }
}
