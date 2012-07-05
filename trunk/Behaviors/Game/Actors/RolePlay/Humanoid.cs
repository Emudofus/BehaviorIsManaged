using System;
using System.Collections.ObjectModel;
using BiM.Behaviors.Data;
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

            m_followingCharactersLook = new ObservableCollection<EntityLook>(human.followingCharactersLook);
            Emote = human.emoteId > 0 ? 
                DataProvider.Instance.GetObjectDataOrDefault<Emoticon>(human.emoteId) : null;
            EmoteStartTime = human.emoteStartTime > 0 ? 
                new DateTime?(human.emoteStartTime.UnixTimestampToDateTime()) : null;
            Restrictions = human.restrictions;
            Title = human.titleId > 0 ?
                DataProvider.Instance.GetObjectDataOrDefault<Title>(human.titleId) : null;
            TitleParam = human.titleParam;
        }

        protected Humanoid(HumanWithGuildInformations human)
            : this((HumanInformations)human)
        {
            GuildInformations = new GuildInformations(human.guildInformations);
        }

        private ObservableCollection<EntityLook> m_followingCharactersLook = new ObservableCollection<EntityLook>();

        public ObservableCollection<EntityLook> FollowingCharactersLook
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

        public override void Dispose()
        {
            m_followingCharactersLook.Clear();
            m_followingCharactersLook = null;

            base.Dispose();
        }
    }
}