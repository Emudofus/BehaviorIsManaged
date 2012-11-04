using System.ComponentModel;
using BiM.Protocol.Enums;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Authentification
{
    public class CharactersListEntry : INotifyPropertyChanged
    {
        public CharactersListEntry(CharacterBaseInformations entry)
        {
            Id = entry.id;
            Name = entry.name;
            Level = entry.level;
            Look = entry.entityLook;
            Breed = (PlayableBreedEnum)entry.breed;
            Sex = entry.sex;
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }

        public EntityLook Look
        {
            get;
            private set;
        }

        public PlayableBreedEnum Breed
        {
            get;
            private set;
        }

        public bool Sex
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}