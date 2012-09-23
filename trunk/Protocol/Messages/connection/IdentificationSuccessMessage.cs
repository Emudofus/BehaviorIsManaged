

// Generated on 09/23/2012 22:26:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IdentificationSuccessMessage : NetworkMessage
    {
        public const uint Id = 22;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hasRights;
        public bool wasAlreadyConnected;
        public string login;
        public string nickname;
        public int accountId;
        public sbyte communityId;
        public string secretQuestion;
        public double subscriptionEndDate;
        public double accountCreation;
        
        public IdentificationSuccessMessage()
        {
        }
        
        public IdentificationSuccessMessage(bool hasRights, bool wasAlreadyConnected, string login, string nickname, int accountId, sbyte communityId, string secretQuestion, double subscriptionEndDate, double accountCreation)
        {
            this.hasRights = hasRights;
            this.wasAlreadyConnected = wasAlreadyConnected;
            this.login = login;
            this.nickname = nickname;
            this.accountId = accountId;
            this.communityId = communityId;
            this.secretQuestion = secretQuestion;
            this.subscriptionEndDate = subscriptionEndDate;
            this.accountCreation = accountCreation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, hasRights);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, wasAlreadyConnected);
            writer.WriteByte(flag1);
            writer.WriteUTF(login);
            writer.WriteUTF(nickname);
            writer.WriteInt(accountId);
            writer.WriteSByte(communityId);
            writer.WriteUTF(secretQuestion);
            writer.WriteDouble(subscriptionEndDate);
            writer.WriteDouble(accountCreation);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            hasRights = BooleanByteWrapper.GetFlag(flag1, 0);
            wasAlreadyConnected = BooleanByteWrapper.GetFlag(flag1, 1);
            login = reader.ReadUTF();
            nickname = reader.ReadUTF();
            accountId = reader.ReadInt();
            if (accountId < 0)
                throw new Exception("Forbidden value on accountId = " + accountId + ", it doesn't respect the following condition : accountId < 0");
            communityId = reader.ReadSByte();
            if (communityId < 0)
                throw new Exception("Forbidden value on communityId = " + communityId + ", it doesn't respect the following condition : communityId < 0");
            secretQuestion = reader.ReadUTF();
            subscriptionEndDate = reader.ReadDouble();
            if (subscriptionEndDate < 0)
                throw new Exception("Forbidden value on subscriptionEndDate = " + subscriptionEndDate + ", it doesn't respect the following condition : subscriptionEndDate < 0");
            accountCreation = reader.ReadDouble();
            if (accountCreation < 0)
                throw new Exception("Forbidden value on accountCreation = " + accountCreation + ", it doesn't respect the following condition : accountCreation < 0");
        }
        
    }
    
}