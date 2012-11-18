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
        uint m_nbCastAllowed;
        uint m_nbTurnToWait;
        Dictionary<int, int> m_targeted;
        public void StartFight()
        {
            m_nbTurnToWait = LevelTemplate.initialCooldown;
            if (m_nbTurnToWait == 0)
                m_nbCastAllowed = LevelTemplate.maxCastPerTurn;
            else
                m_nbCastAllowed = 0;
            if (LevelTemplate.maxCastPerTarget > 0)
                m_targeted = new Dictionary<int, int>();
            else
                m_targeted = null;
        }

        public void EndTurn()
        {
            if (m_nbTurnToWait > 0)
                m_nbTurnToWait--;
            if (m_nbTurnToWait == 0)
                m_nbCastAllowed = LevelTemplate.maxCastPerTurn;

            m_targeted = new Dictionary<int, int>();  // Reset targeted counts
        }

        public void CastAt(int idTarget)
        {
            // Count for limited usage per target
            if (LevelTemplate.maxCastPerTarget > 0)
            {
                int targetCount = 1;
                if (!m_targeted.TryGetValue(idTarget, out targetCount))
                    targetCount = 1;
                else
                    targetCount++;
                m_targeted[idTarget] = targetCount;
                Debug.Assert(targetCount <= LevelTemplate.maxCastPerTarget);
            }
            Debug.Assert(m_nbCastAllowed > 0 || LevelTemplate.maxCastPerTarget <= 0);
            m_nbTurnToWait = (uint)LevelTemplate.globalCooldown;

            if (m_nbCastAllowed > 0)
                m_nbCastAllowed--;
        }

        public bool IsAvailable(int? idTarget)
        {
            if (m_nbTurnToWait > 0) return false;

            // Limit on usage per turn not reached
            if (LevelTemplate.maxCastPerTurn > 0 && m_nbCastAllowed == 0) return false;

            // No restriction per target => available
            if (LevelTemplate.maxCastPerTarget <= 0 || m_nbCastAllowed > 0) return true;

            // No target identified
            if (idTarget == null) return true;

            int targetCount = 0;
            if (!m_targeted.TryGetValue(idTarget.Value, out targetCount))
                targetCount = 0;
            if (targetCount >= LevelTemplate.maxCastPerTarget) return false;

            return true;
        }
        #endregion Stuff to control availability of the spell
    }
}
