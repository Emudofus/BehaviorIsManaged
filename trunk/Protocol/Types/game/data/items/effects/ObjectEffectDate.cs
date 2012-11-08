#region License GNU GPL
// ObjectEffectDate.cs
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
    public class ObjectEffectDate : ObjectEffect
    {
        public const short Id = 72;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short year;
        public short month;
        public short day;
        public short hour;
        public short minute;
        
        public ObjectEffectDate()
        {
        }
        
        public ObjectEffectDate(short actionId, short year, short month, short day, short hour, short minute)
         : base(actionId)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(year);
            writer.WriteShort(month);
            writer.WriteShort(day);
            writer.WriteShort(hour);
            writer.WriteShort(minute);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            year = reader.ReadShort();
            if (year < 0)
                throw new Exception("Forbidden value on year = " + year + ", it doesn't respect the following condition : year < 0");
            month = reader.ReadShort();
            if (month < 0)
                throw new Exception("Forbidden value on month = " + month + ", it doesn't respect the following condition : month < 0");
            day = reader.ReadShort();
            if (day < 0)
                throw new Exception("Forbidden value on day = " + day + ", it doesn't respect the following condition : day < 0");
            hour = reader.ReadShort();
            if (hour < 0)
                throw new Exception("Forbidden value on hour = " + hour + ", it doesn't respect the following condition : hour < 0");
            minute = reader.ReadShort();
            if (minute < 0)
                throw new Exception("Forbidden value on minute = " + minute + ", it doesn't respect the following condition : minute < 0");
        }
        
    }
    
}