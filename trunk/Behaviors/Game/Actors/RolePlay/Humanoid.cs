using System;
using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
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

        protected Humanoid(HumanWithGuildInformations human)                         
            : this((HumanInformations)human)
        {
            GuildInformations = new GuildInformations(human.guildInformations);
        }

        private ObservableCollectionMT<EntityLook> m_followingCharactersLook = new ObservableCollectionMT<EntityLook>();

        public ObservableCollectionMT<EntityLook> FollowingCharactersLook
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
            m_followingCharactersLook = new ObservableCollectionMT<EntityLook>(human.followingCharactersLook);
            Emote = human.emoteId > 0 ?
                DataProvider.Instance.Get<Emoticon>(human.emoteId) : null;
            EmoteStartTime = human.emoteStartTime > 0 ?
                new DateTime?(human.emoteStartTime.UnixTimestampToDateTime()) : null;
            Restrictions = human.restrictions;
            Title = human.titleId > 0 ?
                DataProvider.Instance.Get<Title>(human.titleId) : null;
            TitleParam = human.titleParam;
        }

        public override void Dispose()
        {
            m_followingCharactersLook.Clear();
            m_followingCharactersLook = null;

            base.Dispose();
        }
    }
}