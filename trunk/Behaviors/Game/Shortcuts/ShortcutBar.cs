using System;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Collections;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class ShortcutBar<T>
        where T : Shortcut
    {
        private ObservableCollectionMT<T> m_shortcuts; 
        private ReadOnlyObservableCollectionMT<T> m_readOnlyShortcuts; 

        public ShortcutBar(PlayedCharacter character)
        {
            if (character == null) throw new ArgumentNullException("character");
            Character = character;
            m_shortcuts = new ObservableCollectionMT<T>();
            m_readOnlyShortcuts = new ReadOnlyObservableCollectionMT<T>(m_shortcuts);
        }

        public PlayedCharacter Character
        {
            get;
            protected set;
        }

        public abstract ShortcutBarEnum BarType
        {
            get;
        }

        public ReadOnlyObservableCollectionMT<T> Shortcuts
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

        public abstract void Update(ShortcutBarContentMessage content);
        public abstract void Update(ShortcutBarRefreshMessage message);
    }
}