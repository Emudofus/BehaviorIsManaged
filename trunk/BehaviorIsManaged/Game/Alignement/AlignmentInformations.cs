using System;
using System.ComponentModel;
using BiM.Protocol.Types;

namespace BiM.Game.Alignement
{
    public class AlignmentInformations : INotifyPropertyChanged
    {
        public AlignmentInformations()
        {
            
        }

        public AlignmentInformations(ActorAlignmentInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");
            AlignmentSide = informations.alignmentSide;
            AlignmentValue = informations.alignmentValue;
            AlignmentGrade = informations.alignmentGrade;
            Dishonor = informations.dishonor;
            CharacterPower = informations.characterPower;
        }

        public AlignmentInformations(ActorExtendedAlignmentInformations informations)
            : this((ActorAlignmentInformations)informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");
            Extended = true;
            Honor = informations.honor;
            HonorGradeFloor = informations.honorGradeFloor;
            HonorNextGradeFloor = informations.honorNextGradeFloor;
            PvpEnabled = informations.pvpEnabled;
        }

        public sbyte AlignmentSide
        {
            get;
            set;
        }

        public sbyte AlignmentValue
        {
            get;
            set;
        }

        public sbyte AlignmentGrade
        {
            get;
            set;
        }

        public ushort Dishonor
        {
            get;
            set;
        }

        public int CharacterPower
        {
            get;
            set;
        }

        public bool Extended
        {
            get;
            set;
        }

        public ushort Honor
        {
            get;
            set;
        }

        public ushort HonorGradeFloor
        {
            get;
            set;
        }

        public ushort HonorNextGradeFloor
        {
            get;
            set;
        }

        public bool PvpEnabled
        {
            get;
            set;
        }

        public void TogglePvpEnabled()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}