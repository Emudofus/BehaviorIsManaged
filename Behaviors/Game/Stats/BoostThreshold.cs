#region License GNU GPL
// BoostThreshold.cs
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

namespace BiM.Behaviors.Game.Stats
{
    public class BoostThreshold
    {
        public BoostThreshold(uint pointsThreshold, uint pointsPerBoost, uint boostPerPoints = 1)
        {
            PointsThreshold = pointsThreshold;
            PointsPerBoost = pointsPerBoost;
            BoostPerPoints = boostPerPoints;
        }

        public BoostThreshold(List<uint> threshold)
        {
            if (threshold.Count != 3 && threshold.Count != 2)
                throw new ArgumentException("threshold.Count != 3 && threshold.Count != 2");

            PointsThreshold = threshold[0];
            PointsPerBoost = threshold[1];
            BoostPerPoints = threshold.Count > 2 ? threshold[2] : 1;
        }

        public uint PointsThreshold
        {
            get;
            set;
        }

        public uint PointsPerBoost
        {
            get;
            set;
        }

        public uint BoostPerPoints
        {
            get;
            set;
        }
    }
}