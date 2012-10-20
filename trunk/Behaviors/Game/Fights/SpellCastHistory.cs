using System.Collections.Generic;
using System.Collections.ObjectModel;
using BiM.Behaviors.Game.Actors.Fighters;
using BiM.Core.Collections;

namespace BiM.Behaviors.Game.Fights
{
    public class SpellCastHistory
    {
        private ObservableCollectionMT<SpellCast> m_casts = new ObservableCollectionMT<SpellCast>();
        private ReadOnlyObservableCollectionMT<SpellCast> m_readOnlyCasts;

        public SpellCastHistory(Fighter fighter)
        {
            Fighter = fighter;
            m_readOnlyCasts = new ReadOnlyObservableCollectionMT<SpellCast>(m_casts);
        }

        public Fighter Fighter
        {
            get;
            private set;
        }

        public ReadOnlyObservableCollectionMT<SpellCast> Casts
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