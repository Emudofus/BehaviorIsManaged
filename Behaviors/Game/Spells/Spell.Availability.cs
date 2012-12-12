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

        private uint _nbCastAllowed;
        private uint _nbTurnToWait;
        private Dictionary<int, int> _targeted;

        public void StartFight()
        {           
            _nbTurnToWait = LevelTemplate.initialCooldown;
            if (_nbTurnToWait == 0)
                _nbCastAllowed = LevelTemplate.maxCastPerTurn;
            else
                _nbCastAllowed = 0;
            _targeted = null;
        }

        public void EndTurn()
        {
            if (_nbTurnToWait > 0)
                _nbTurnToWait--;
            if (_nbTurnToWait == 0)
                _nbCastAllowed = LevelTemplate.maxCastPerTurn;
            _targeted = null; // Reset targeted counts
        }

        public void CastAt(int idTarget)
        {
            // Count for limited usage per target
            if (LevelTemplate.maxCastPerTarget > 0)
            {
                int targetCount = 1;
                if (_targeted == null)
                    _targeted = new Dictionary<int, int>();
                if (!_targeted.TryGetValue(idTarget, out targetCount))
                    targetCount = 1;
                else
                    targetCount++;
                _targeted[idTarget] = targetCount;
                Debug.Assert(targetCount <= LevelTemplate.maxCastPerTarget);
            }
            Debug.Assert(_nbCastAllowed > 0 || LevelTemplate.maxCastPerTurn <= 0);
            if (LevelTemplate.minCastInterval > 0)
                _nbTurnToWait = (uint) LevelTemplate.minCastInterval + 1;

            if (_nbCastAllowed > 0)
                _nbCastAllowed--;
        }

        public string AvailabilityExplainString(int? idTarget)
        {
            string res = string.Empty;
            if (LevelTemplate.minCastInterval != 0 ||  _nbTurnToWait != 0)
                res += string.Format("turns to wait : {0}/{1}, ", _nbTurnToWait, LevelTemplate.minCastInterval);
            if (_nbCastAllowed != 0 || LevelTemplate.maxCastPerTurn != 0)
                res += string.Format("cast allowed : {0}/{1}, ", _nbCastAllowed, LevelTemplate.maxCastPerTurn);
            if (idTarget != null && LevelTemplate.maxCastPerTarget != 0)
            {
                int targetCount = 0;
                if (_targeted!=null && !_targeted.TryGetValue(idTarget.Value, out targetCount))
                    targetCount = 0;
                res += string.Format("cast allowed on target {2}: {0}/{1}, ", targetCount, LevelTemplate.maxCastPerTarget, idTarget);
            }
            return res;
            //private uint _nbCastAllowed;
            //private uint _nbTurnToWait;
            
        }

        public bool IsAvailable(int? idTarget, Spells.Spell.SpellCategory? category=null)
        {
            if (_nbTurnToWait > 0) 
                return false;

            // Limit on usage per turn not reached
            if (LevelTemplate.maxCastPerTurn > 0 && _nbCastAllowed == 0) 
                return false;

            if (!HasCategory(category)) 
                return false;

            // No restriction per target => available
            if (LevelTemplate.maxCastPerTarget <= 0 || _nbCastAllowed > 0) return true;
            
            // No target identified
            if (idTarget == null) return true;

            if (_targeted != null)
            {
                int targetCount = 0;
                if (!_targeted.TryGetValue(idTarget.Value, out targetCount))
                    targetCount = 0;
                if (targetCount >= LevelTemplate.maxCastPerTarget) 
                    return false;
            }

            return true;
        }

        #endregion Stuff to control availability of the spell
    }
}
