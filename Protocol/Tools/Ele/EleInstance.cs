#region License GNU GPL
// EleInstance.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Ele
{
    public class EleInstance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EleInstance()
        {
            GraphicalDatas = new Dictionary<int, EleGraphicalData>();
            GfxJpgMap = new Dictionary<int, bool>();
        }

        public byte Version
        {
            get;
            set;
        }

        public Dictionary<int, EleGraphicalData> GraphicalDatas
        {
            get;
            set;
        }

        public Dictionary<int, bool> GfxJpgMap
        {
            get;
            set;
        }

        public static EleInstance ReadFromStream(BigEndianReader reader)
        {
            var instance = new EleInstance();

            instance.Version = reader.ReadByte();

            var count = reader.ReadUInt();
            for (int i = 0; i < count; i++)
            {
                var elem = EleGraphicalData.ReadFromStream(instance, reader);
                instance.GraphicalDatas.Add(elem.Id, elem);
            }

            if (instance.Version >= 8)
            {
                var gfxCount = reader.ReadInt();
                for (int i = 0; i < gfxCount; i++)
                {
                    instance.GfxJpgMap.Add(reader.ReadInt(), true);
                }
            }

            return instance;
        }
    }
}