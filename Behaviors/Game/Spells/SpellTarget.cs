using System;
using BiM.Behaviors.Game.World;

namespace BiM.Behaviors.Game.Spells
{
    public class SpellTarget
    {
        public SpellTarget(double efficiency = 0, Cell source = null, Cell target = null, Spell spell = null)
        {
            Efficiency = efficiency;
            FromCell = source;
            TargetCell = target;
            Spell = spell;
            cast = spell == null;
        }

        public double Efficiency { get; set; }
        public Cell FromCell { get; set; }
        public Cell TargetCell { get; set; }
        public Spell Spell { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public bool cast { get; set; }
        public string Comment { get; set; }
    }
}
