using System.ComponentModel;
using System.Windows.Media;
using BiM.Protocol.Enums;

namespace BasicPlugin.CharacterSelection
{
    public class CharacterCreationData : INotifyPropertyChanged
    {
        public CharacterCreationData()
        {
            Genders = new SexTypeEnum []
            {
                SexTypeEnum.SEX_MALE,
                SexTypeEnum.SEX_FEMALE
            };
        }

        public PlayableBreedEnum[] EnabledBreeds
        {
            get;
            set;
        }

        public SexTypeEnum[] Genders
        {
            get;
            set;
        }

        public string CharacterName
        {
            get;
            set;
        }

        public PlayableBreedEnum Breed
        {
            get;
            set;
        }

        public SexTypeEnum Sex
        {
            get;
            set;
        }

        public Color Color1
        {
            get;
            set;
        }

        public Color Color2
        {
            get;
            set;
        }
        public Color Color3
        {
            get;
            set;
        }
        public Color Color4
        {
            get;
            set;
        }
        public Color Color5
        {
            get;
            set;
        }

        public bool Color1Used
        {
            get;
            set;
        }

        public bool Color2Used
        {
            get;
            set;
        }

        public bool Color3Used
        {
            get;
            set;
        }

        public bool Color4Used
        {
            get;
            set;
        }

        public bool Color5Used
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}