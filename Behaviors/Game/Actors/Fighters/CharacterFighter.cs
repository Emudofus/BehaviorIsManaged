#region License GNU GPL
// CharacterFighter.cs
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
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.Alignement;
using BiM.Behaviors.Game.Fights;
using BiM.Behaviors.Game.Stats;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using NLog;

namespace BiM.Behaviors.Game.Actors.Fighters
{
    public class CharacterFighter : Fighter, IPlayer
    {
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected CharacterFighter(Fight fight) : base()
        {
            Fight = fight;
        }

        public CharacterFighter(GameFightCharacterInformations msg, Fight fight) : base (msg, fight)
        {
            Alignment = new AlignmentInformations(msg.alignmentInfos);
            Breed = new Breeds.Breed(ObjectDataManager.Instance.Get<Breed>(msg.breed));            
        }

        public override int Id
        {
            get;
            protected set;
        }

        public virtual AlignmentInformations Alignment
        {
            get;
            protected set;
        }

        public virtual Breeds.Breed Breed
        {
            get;
            protected set;
        }

        public void Update(GameFightCharacterInformations msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Id = msg.contextualId;
            Look = msg.look;
            Map = Fight.Map;
            Update(msg.disposition);
            IsAlive = msg.alive;
            Alignment = new AlignmentInformations(msg.alignmentInfos);

            if (Breed == null || Breed.Id != msg.breed)
                Breed = new Breeds.Breed(ObjectDataManager.Instance.Get<Breed>(msg.breed));

            Stats.Update(msg.stats);
        }

        public override void Update(GameFightFighterInformations informations)
        {
            if (informations == null) throw new ArgumentNullException("informations");

            if (informations is GameFightCharacterInformations)
            {
                Update(informations as GameFightCharacterInformations);
            }
            else
            {
                logger.Error("Cannot update a {0} with a {1} instance", GetType(), informations.GetType()); 
                base.Update(informations);
            }
        }
    }
}