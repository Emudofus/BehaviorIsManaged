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

using System.Collections.Generic;
using System.Diagnostics;

namespace BiM.Behaviors.Game.Spells
{
    public partial class Spell
    {
        #region Stuff to control availability of the spell

        private uint m_nbCastAllowed;
        private uint m_nbTurnToWait;
        private Dictionary<int, int> m_targeted;

        public void StartFight()
        {           
            m_nbTurnToWait = LevelTemplate.initialCooldown;
            if (m_nbTurnToWait == 0)
                m_nbCastAllowed = LevelTemplate.maxCastPerTurn;
            else
                m_nbCastAllowed = 0;
            m_targeted = null;
        }

        public void EndTurn()
        {
            if (m_nbTurnToWait > 0)
                m_nbTurnToWait--;
            if (m_nbTurnToWait == 0)
                m_nbCastAllowed = LevelTemplate.maxCastPerTurn;
            m_targeted = null; // Reset targeted counts
        }

        public void CastAt(int idTarget)
        {
            // Count for limited usage per target
            if (LevelTemplate.maxCastPerTarget > 0)
            {
                int targetCount = 1;
                if (m_targeted == null)
                    m_targeted = new Dictionary<int, int>();
                if (!m_targeted.TryGetValue(idTarget, out targetCount))
                    targetCount = 1;
                else
                    targetCount++;
                m_targeted[idTarget] = targetCount;
                Debug.Assert(targetCount <= LevelTemplate.maxCastPerTarget);
            }
            Debug.Assert(m_nbCastAllowed > 0 || LevelTemplate.maxCastPerTurn <= 0);
            m_nbTurnToWait = (uint) LevelTemplate.minCastInterval;

            if (m_nbCastAllowed > 0)
                m_nbCastAllowed--;
        }

        public bool IsAvailable(int? idTarget, Spells.Spell.SpellCategory? category=null)
        {
            if (m_nbTurnToWait > 0) return false;

            // Limit on usage per turn not reached
            if (LevelTemplate.maxCastPerTurn > 0 && m_nbCastAllowed == 0) return false;

            // No restriction per target => available
            if (LevelTemplate.maxCastPerTarget <= 0 || m_nbCastAllowed > 0) return true;

            if (!HasCategory(category)) return false;

            // No target identified
            if (idTarget == null) return true;

            if (m_targeted != null)
            {
                int targetCount = 0;
                if (m_targeted!=null)
                    if (!m_targeted.TryGetValue(idTarget.Value, out targetCount))
                        targetCount = 0;
                if (targetCount >= LevelTemplate.maxCastPerTarget) return false;
            }

            return true;
        }

        #endregion Stuff to control availability of the spell
    }
}
