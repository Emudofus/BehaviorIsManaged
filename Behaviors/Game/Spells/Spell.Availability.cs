#region License GNU GPL
// Spell.cs
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

// Author : FastFrench - antispam@laposte.net
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BiM.Behaviors.Game.Spells
{
    public partial class Spell
    {

        #region Stuff to control availability of the spell
        uint NbCastAllowed;
        uint NbTurnToWait;
        Dictionary<int, int> Targeted;
        public void StartFight()
        {
            NbTurnToWait = LevelTemplate.initialCooldown;
            if (NbTurnToWait == 0)
                NbCastAllowed = LevelTemplate.maxCastPerTurn;
            else
                NbCastAllowed = 0;
            if (LevelTemplate.maxCastPerTarget > 0)
                Targeted = new Dictionary<int, int>();
            else
                Targeted = null;
        }

        public void EndTurn()
        {
            if (NbTurnToWait > 0)
                NbTurnToWait--;
            if (NbTurnToWait == 0)
                NbCastAllowed = LevelTemplate.maxCastPerTurn;

            Targeted = new Dictionary<int, int>();  // Reset targeted counts
        }

        public void CastAt(int idTarget)
        {
            // Count for limited usage per target
            if (LevelTemplate.maxCastPerTarget > 0)
            {
                int TargetCount = 1;
                if (!Targeted.TryGetValue(idTarget, out TargetCount))
                    TargetCount = 1;
                else
                    TargetCount++;
                Targeted[idTarget] = TargetCount;
                Debug.Assert(TargetCount <= LevelTemplate.maxCastPerTarget);
            }
            Debug.Assert(NbCastAllowed > 0 || LevelTemplate.maxCastPerTarget <= 0);
            NbTurnToWait = (uint)LevelTemplate.globalCooldown;

            if (NbCastAllowed > 0)
                NbCastAllowed--;
        }

        public bool IsAvailable(int? idTarget)
        {
            if (NbTurnToWait > 0) return false;

            // Limit on usage per turn not reached
            if (LevelTemplate.maxCastPerTurn > 0 && NbCastAllowed == 0) return false;

            // No restriction per target => available
            if (LevelTemplate.maxCastPerTarget <= 0 || NbCastAllowed > 0) return true;

            // No target identified
            if (idTarget == null) return true;

            int TargetCount = 0;
            if (!Targeted.TryGetValue(idTarget.Value, out TargetCount))
                TargetCount = 0;
            if (TargetCount >= LevelTemplate.maxCastPerTarget) return false;

            return true;
        }
        #endregion Stuff to control availability of the spell
    }
}
