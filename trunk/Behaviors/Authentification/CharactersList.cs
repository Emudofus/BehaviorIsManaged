using System;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Authentification
{
    public class CharactersList : ReadOnlyObservableCollection<CharactersListEntry>
    {
        public CharactersListEntry this[ushort id]
        {
            get
            {
                return Items.FirstOrDefault(entry => entry.Id == id);
            }
        }

        public CharactersList(CharactersListMessage msg)
            : base(new ObservableCollection<CharactersListEntry>(msg.characters.Select(entry => new CharactersListEntry(entry))))
        {
        }
    }
}