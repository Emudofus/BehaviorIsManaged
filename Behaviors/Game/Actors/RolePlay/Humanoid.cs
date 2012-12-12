#region License GNU GPL
// Humanoid.cs
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
using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.Movements;
using BiM.Core.Collections;
using BiM.Protocol.Data;
using BiM.Protocol.Types;
using BiM.Core.Extensions;
using GuildInformations = BiM.Behaviors.Game.Guilds.GuildInformations;

namespace BiM.Behaviors.Game.Actors.RolePlay
{
    public abstract class Humanoid : NamedActor
    {
        protected Humanoid()
        {
            
        }

        protected Humanoid(HumanInformations human)
        {
            if (human == null) throw new ArgumentNullException("human");

            Update(human);
        }

        private ObservableCollectionMT<IndexedEntityLook> m_followingCharactersLook = new ObservableCollectionMT<IndexedEntityLook>();

        public ObservableCollectionMT<IndexedEntityLook> FollowingCharactersLook
        {
            get
            {
                return m_followingCharactersLook;
            }
        }

        public Emoticon Emote
        {
            get;
            protected set;
        }

        public DateTime? EmoteStartTime
        {
            get;
            protected set;
        }

        public ActorRestrictionsInformations Restrictions
        {
            get;
            protected set;
        }

        public Title Title
        {
            get;
            protected set;
        }

        public string TitleParam
        {
            get;
            protected set;
        }

        public GuildInformations GuildInformations
        {
            get;
            protected set;
        }

        public override VelocityConfiguration GetAdaptedVelocity(World.Pathfinding.Path path)
        {
            if (Restrictions.forceSlowWalk)
                return MovementBehavior.FantomMovementBehavior;
            else if (Restrictions.cantRun)
                return MovementBehavior.WalkingMovementBehavior;

            return path.MPCost <= 3 ? MovementBehavior.WalkingMovementBehavior : MovementBehavior.RunningMovementBehavior;
        }

        public void Update(HumanInformations human)
        {
            foreach (var option in human.options)
            {
                HandleOption(option);
            }
            Restrictions = human.restrictions;
        }

        private void HandleOption(HumanOption option)
        {
            if (option is HumanOptionEmote)
            {
                var emote = (HumanOptionEmote)option;
                Emote = emote.emoteId > 0 ? ObjectDataManager.Instance.Get<Emoticon>(emote.emoteId) : null;
                EmoteStartTime = emote.emoteStartTime > 0 ? new DateTime?(emote.emoteStartTime.UnixTimestampToDateTime()) : null;
            }
            else if (option is HumanOptionFollowers)
            {
                m_followingCharactersLook = new ObservableCollectionMT<IndexedEntityLook>(( (HumanOptionFollowers)option ).followingCharactersLook);
            }
            else if (option is HumanOptionGuild)
            {
                // todo : guild
            }
            else if (option is HumanOptionOrnament)
            {
                 // todo
            }
            else if (option is HumanOptionTitle)
            {
                // todo
            }
            else
            {
                throw new Exception(string.Format("Unattempt HumanOption type {0}", option.GetType()));
            }
        }

        public override void Dispose()
        {
            m_followingCharactersLook.Clear();
            m_followingCharactersLook = null;

            base.Dispose();
        }
    }
}