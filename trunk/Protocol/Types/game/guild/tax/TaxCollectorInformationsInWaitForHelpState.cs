#region License GNU GPL
// TaxCollectorInformationsInWaitForHelpState.cs
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
    public class TaxCollectorInformationsInWaitForHelpState : TaxCollectorInformations
    {
        public const short Id = 166;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        
        public TaxCollectorInformationsInWaitForHelpState()
        {
        }
        
        public TaxCollectorInformationsInWaitForHelpState(int uniqueId, short firtNameId, short lastNameId, Types.AdditionalTaxCollectorInformations additionalInfos, short worldX, short worldY, short subAreaId, sbyte state, Types.EntityLook look, int kamas, double experience, int pods, int itemsValue, Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo)
         : base(uniqueId, firtNameId, lastNameId, additionalInfos, worldX, worldY, subAreaId, state, look, kamas, experience, pods, itemsValue)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            waitingForHelpInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
        }
        
    }
    
}