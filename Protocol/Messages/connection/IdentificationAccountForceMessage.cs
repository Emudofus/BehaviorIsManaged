

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IdentificationAccountForceMessage : IdentificationMessage
    {
        public const uint Id = 6119;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string forcedAccountLogin;
        
        public IdentificationAccountForceMessage()
        {
        }
        
        public IdentificationAccountForceMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, sbyte[] credentials, short serverId, string forcedAccountLogin)
         : base(autoconnect, useCertificate, useLoginToken, version, lang, credentials, serverId)
        {
            this.forcedAccountLogin = forcedAccountLogin;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(forcedAccountLogin);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            forcedAccountLogin = reader.ReadUTF();
        }
        
    }
    
}