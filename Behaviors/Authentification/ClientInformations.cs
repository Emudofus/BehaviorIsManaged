#region License GNU GPL
// ClientInformations.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Data;
using BiM.Core.Extensions;
using BiM.Core.Reflection;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;
using Version = BiM.Protocol.Types.Version;

namespace BiM.Behaviors.Authentification
{
    public class ClientInformations : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

        public DateTime? BanEndDate
        {
            get;
            set;
        }

        public IdentificationFailureReasonEnum? IdentificationFailureReason
        {
            get;
            set;
        }

        public bool Banned
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

        public PlayableBreedEnum[] VisibleBreeds
        {
            get;
            set;
        }

        public PlayableBreedEnum[] AvailableBreeds
        {
            get;
            set;
        }

        public TimeSpan ServerTimeOffset
        {
            get;
            set;
        }

        private DateTime? m_serverTimeReference;
        private DateTime m_referenceTimeChange;

        public DateTime ServerTimeReference
        {
            get { return m_serverTimeReference ?? DateTime.Now; }
            set { m_serverTimeReference = value;
                m_referenceTimeChange = DateTime.Now; }
        }

        public DateTime ServerTime
        {
            get { return ServerTimeReference + (DateTime.Now - m_referenceTimeChange); }
        }

        public CharactersList CharactersList
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
            Login = msg.login.ToLower(); // always ToLower() for login
            Credentials = msg.credentials.ToArray();
        }

        public void Update(IdentificationFailedBannedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            BanEndDate = msg.banEndDate.UnixTimestampToDateTime();
            Banned = true;

            logger.Warn("F*** I'm banned :( for {0}", BanEndDate - DateTime.Now);
        }

        public void Update(IdentificationFailedMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            IdentificationFailureReason = (IdentificationFailureReasonEnum)msg.reason;

            if (IdentificationFailureReason == IdentificationFailureReasonEnum.BANNED)
                Banned = true;
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
            SelectedServer = ObjectDataManager.Instance.Get<Server>(msg.serverId);
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

        public void Update(AccountCapabilitiesMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            VisibleBreeds = Enum.GetValues(typeof(PlayableBreedEnum)).
                Cast<PlayableBreedEnum>().Where(entry => ( ( msg.breedsVisible >> (int)entry - 1 ) & 1 ) == 1).ToArray();
            AvailableBreeds = Enum.GetValues(typeof(PlayableBreedEnum)).
                Cast<PlayableBreedEnum>().Where(entry => ( ( msg.breedsAvailable >> (int)entry - 1 ) & 1 ) == 1).ToArray();
        }

        public void Update(BasicTimeMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            ServerTimeOffset = new TimeSpan(0, 0, msg.timezoneOffset, 0);
            ServerTimeReference = msg.timestamp.UnixTimestampToDateTime();
        }

        public void Update(CharactersListMessage msg)
        {
            if (msg == null) throw new ArgumentNullException("msg");
            CharactersList = new CharactersList(msg);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}