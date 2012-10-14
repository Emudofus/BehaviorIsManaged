using System.Collections.Generic;
using System.Collections.ObjectModel;
using BiM.Behaviors.Game.Actors.Fighters;

namespace BiM.Behaviors.Game.Fights
{
    public class SpellCastHistory
    {
        private ObservableCollection<SpellCast> m_casts = new ObservableCollection<SpellCast>();
        private ReadOnlyObservableCollection<SpellCast> m_readOnlyCasts;

        public SpellCastHistory(Fighter fighter)
        {
            Fighter = fighter;
            m_readOnlyCasts = new ReadOnlyObservableCollection<SpellCast>(m_casts);
        }

        public Fighter Fighter
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollection<SpellCast> Casts
        {
            get
            {
                return m_readOnlyCasts;
            }
        }

        public void AddSpellCast(SpellCast cast)
        {
            m_casts.Add(cast);
        }
    }
}