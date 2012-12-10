#region License GNU GPL
// SpellCast.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Game.Fights
{
    public class SpellCast
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SpellCast()
        {
            
        }

        public SpellCast(Fight fight, GameActionFightSpellCastMessage msg)
        {
            Caster = fight.GetFighter(msg.sourceId);

            if (Caster == null)
                logger.Error("Fighter {0} not found as he casted spell {1}", msg.sourceId, msg.spellId);

            Spell = ObjectDataManager.Instance.Get<Spell>(msg.spellId);
            SpellLevel = ObjectDataManager.Instance.Get<SpellLevel>((int)Spell.spellLevels[msg.spellLevel - 1]);
            Target = fight.Map.Cells[msg.destinationCellId];
            RoundCast = fight.Round;
            Critical = (FightSpellCastCriticalEnum) msg.critical;
            SilentCast = msg.silentCast;
            TargetedFighter = fight.GetFighter(msg.targetId);
        }

        public Fighter Caster
        {
            get;
            set;
        }

        public Spell Spell
        {
            get;
            set;
        }

        public SpellLevel SpellLevel
        {
            get;
            set;
        }

        public Cell Target
        {
            get;
            set;
        }

        public Fighter TargetedFighter
        {
            get;
            set;
        }

        public int RoundCast
        {
            get;
            set;
        }

        public FightSpellCastCriticalEnum Critical
        {
            get;
            set;
        }

        public bool SilentCast
        {
            get;
            set;
        }
    }
}