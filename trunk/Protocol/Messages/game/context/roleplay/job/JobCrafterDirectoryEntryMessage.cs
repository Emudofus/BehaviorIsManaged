

// Generated on 09/23/2012 22:26:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class JobCrafterDirectoryEntryMessage : NetworkMessage
    {
        public const uint Id = 6044;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public Types.JobCrafterDirectoryEntryJobInfo[] jobInfoList;
        public Types.EntityLook playerLook;
        
        public JobCrafterDirectoryEntryMessage()
        {
        }
        
        public JobCrafterDirectoryEntryMessage(Types.JobCrafterDirectoryEntryPlayerInfo playerInfo, Types.JobCrafterDirectoryEntryJobInfo[] jobInfoList, Types.EntityLook playerLook)
        {
            this.playerInfo = playerInfo;
            this.jobInfoList = jobInfoList;
            this.playerLook = playerLook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            playerInfo.Serialize(writer);
            writer.WriteUShort((ushort)jobInfoList.Length);
            foreach (var entry in jobInfoList)
            {
                 entry.Serialize(writer);
            }
            playerLook.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerInfo = new Types.JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            jobInfoList = new Types.JobCrafterDirectoryEntryJobInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobInfoList[i] = new Types.JobCrafterDirectoryEntryJobInfo();
                 jobInfoList[i].Deserialize(reader);
            }
            playerLook = new Types.EntityLook();
            playerLook.Deserialize(reader);
        }
        
    }
    
}