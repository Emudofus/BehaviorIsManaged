#region License GNU GPL
// AlignmentInformations.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.ComponentModel;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Alignement
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

        public void Update(ActorAlignmentInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");
            AlignmentSide = informations.alignmentSide;
            AlignmentValue = informations.alignmentValue;
            AlignmentGrade = informations.alignmentGrade;
            Dishonor = informations.dishonor;
            CharacterPower = informations.characterPower;
        }
    }
}