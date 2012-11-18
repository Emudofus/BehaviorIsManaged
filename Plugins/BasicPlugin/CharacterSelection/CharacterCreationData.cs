#region License GNU GPL
// CharacterCreationData.cs
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
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}