using System;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Core.Extensions;
using BiM.Protocol.Data;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using Version = BiM.Protocol.Types.Version;

namespace BiM.Behaviors.Authentification
{
    public class ClientInformations : INotifyPropertyChanged
    {
        public string Salt
        {
            get;
            set;
        }

        public sbyte[] PublicRSAKey
        {
            get;
            set;
        }

        public string Login
        {
            get;
            set;
        }

        public sbyte[] Credentials
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public int AccountId
        {
            get;
            set;
        }

        public sbyte CommunityId
        {
            get;
            set;
        }

        public string SecretQuestion
        {
            get;
            set;
        }

        public DateTime? SubscriptionEndDate
        {
            get;
            set;
        }

        public DateTime AccountCreation
        {
            get;
            set;
        }

        public DateTime BanEndDate
        {
            get;
            set;
        }

        public string ConnectionTicket
        {
            get;
            set;
        }

        public string Lang
        {
            get;
            set;
        }

        public VersionExtended Version
        {
            get;
            set;
        }

        public Version RequieredVersion
        {
            get;
            set;
        }

        public ServersList ServersList
        {
            get;
            set;
        }

        public Server SelectedServer
        {
            get;
            set;
        }

        public string ServerAddress
        {
            get;
            set;
        }

        public ushort ServerPort
        {
            get;
            set;
        }

        public bool CanCreateNewCharacter
        {
            get;
            set;
        }

        public bool HasAdminRights
        {
            get;
            set;
        }

        public bool IsSubscribed()
        {
            return SubscriptionEndDate.HasValue && SubscriptionEndDate > DateTime.Now;
        }

        public bool IsBanned()
        {
            return BanEndDate > DateTime.Now;
        }

        public bool IsClientUpToDate()
        {
            // if RequieredVersion is null then it means that the server accept the client version
            return RequieredVersion == null || (Version != null &&
                   RequieredVersion.major == Version.major &&
                   RequieredVersion.minor == Version.minor &&
                   RequieredVersion.release == Version.release &&
                   RequieredVersion.revision == Version.revision &&
                   RequieredVersion.patch == Version.patch);
        }

        public bool CanLogin()
        {
            return IsClientUpToDate() && !IsBanned();
        }

        public void Update(HelloConnectMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Salt = msg.salt;
            PublicRSAKey = msg.key.ToArray();
        }

        public void Update(IdentificationMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            Version = msg.version;
            Lang = msg.lang;
            Login = msg.login;
            Credentials = msg.credentials.ToArray();
        }

        public void Update(IdentificationFailedBannedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            BanEndDate = msg.banEndDate.UnixTimestampToDateTime();
        }

        public void Update(IdentificationSuccessMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            HasAdminRights = msg.hasRights;
            Nickname = msg.nickname;
            AccountId = msg.accountId;
            CommunityId = msg.communityId;
            SecretQuestion = msg.secretQuestion;
            if (msg.subscriptionEndDate > 0)
                SubscriptionEndDate = msg.subscriptionEndDate.UnixTimestampToDateTime();

            AccountCreation = msg.subscriptionEndDate.UnixTimestampToDateTime();
        }

        public void Update(IdentificationFailedForBadVersionMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            RequieredVersion = msg.requiredVersion;
        }

        public void Update(SelectedServerDataMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            SelectedServer = DataProvider.Instance.Get<Server>(msg.serverId);
            ConnectionTicket = msg.ticket;
            ServerAddress = msg.address;
            ServerPort = msg.port;
            CanCreateNewCharacter = msg.canCreateNewCharacter;
        }

        public void Update(ServersListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            ServersList = new ServersList(msg);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}