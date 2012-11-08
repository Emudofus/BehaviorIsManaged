#region License GNU GPL
// FightOptionsInformations.cs
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
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightOptionsInformations
    {
        public const short Id = 20;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool isSecret;
        public bool isRestrictedToPartyOnly;
        public bool isClosed;
        public bool isAskingForHelp;
        
        public FightOptionsInformations()
        {
        }
        
        public FightOptionsInformations(bool isSecret, bool isRestrictedToPartyOnly, bool isClosed, bool isAskingForHelp)
        {
            this.isSecret = isSecret;
            this.isRestrictedToPartyOnly = isRestrictedToPartyOnly;
            this.isClosed = isClosed;
            this.isAskingForHelp = isAskingForHelp;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, isSecret);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isRestrictedToPartyOnly);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isClosed);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, isAskingForHelp);
            writer.WriteByte(flag1);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            isSecret = BooleanByteWrapper.GetFlag(flag1, 0);
            isRestrictedToPartyOnly = BooleanByteWrapper.GetFlag(flag1, 1);
            isClosed = BooleanByteWrapper.GetFlag(flag1, 2);
            isAskingForHelp = BooleanByteWrapper.GetFlag(flag1, 3);
        }
        
    }
    
}