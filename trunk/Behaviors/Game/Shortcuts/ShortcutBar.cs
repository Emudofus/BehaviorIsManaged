using System;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ShortcutBar<T> where T : Shortcut
    {
        private ObservableCollection<T> m_shortcuts; 
        private ReadOnlyObservableCollection<T> m_readOnlyShortcuts; 

        public ShortcutBar(PlayedCharacter character)
        {
            if (character == null) throw new ArgumentNullException("character");
            Character = character;
            m_shortcuts = new ObservableCollection<T>();
            m_readOnlyShortcuts = new ReadOnlyObservableCollection<T>(m_shortcuts);
        }

        public PlayedCharacter Character
        {
            get;
            set;
        }

        public ReadOnlyObservableCollection<T> Shortcuts
        {
            get { return m_readOnlyShortcuts; }
        }

        public T Get(int slot)
        {
            return Shortcuts.FirstOrDefault(x => x.Slot == slot);
        }

        public void Add(T shortcut)
        {
            m_shortcuts.Add(shortcut);
        }

        public bool Remove(T shortcut)
        {
            return m_shortcuts.Remove(shortcut);
        }

        public bool Remove(int slot)
        {
            var item = Get(slot);

            if (item == null)
                return false;

            return m_shortcuts.Remove(item);
        }

        public void Clear()
        {
            m_shortcuts.Clear();
        }
    }
}