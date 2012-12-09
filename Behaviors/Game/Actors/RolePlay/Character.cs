#region License GNU GPL
// Character.cs
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
using BiM.Behaviors.Game.Actors.Interfaces;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public class Character : Humanoid, IAlignedActor, IPlayer
    {
        public Character()
        {
            
        }

        public Character(GameRolePlayCharacterInformations characterInformations, Map map)
            : base(characterInformations.humanoidInfo)
        {
            if (characterInformations == null) throw new ArgumentNullException("characterInformations");
            if (map == null) throw new ArgumentNullException("map");

            // do not care about this warnings, this ctor is never called by his inheriter
            Id = characterInformations.contextualId;
            Look = characterInformations.look;
            Map = map;
            Update(characterInformations.disposition);
            Name = characterInformations.name;
            Alignement = new AlignmentInformations(characterInformations.alignmentInfos);
        }

        public AlignmentInformations Alignement
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}]", Name,  Cell);
        }

    }
}