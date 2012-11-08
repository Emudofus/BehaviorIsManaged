#region License GNU GPL
// KrosmasterInventoryMessage.cs
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
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KrosmasterInventoryMessage : NetworkMessage
    {
        public const uint Id = 6350;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.KrosmasterFigure[] figures;
        
        public KrosmasterInventoryMessage()
        {
        }
        
        public KrosmasterInventoryMessage(Types.KrosmasterFigure[] figures)
        {
            this.figures = figures;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)figures.Length);
            foreach (var entry in figures)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            figures = new Types.KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++)
            {
                 figures[i] = new Types.KrosmasterFigure();
                 figures[i].Deserialize(reader);
            }
        }
        
    }
    
}