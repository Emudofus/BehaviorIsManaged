using BiM.Behaviors.Data;
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

            Spell = DataProvider.Instance.Get<Spell>(msg.spellId);
            SpellLevel = DataProvider.Instance.Get<SpellLevel>((int)Spell.spellLevels[msg.spellLevel - 1]);
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