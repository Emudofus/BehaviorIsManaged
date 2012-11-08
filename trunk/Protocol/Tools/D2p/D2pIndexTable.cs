#region License GNU GPL
// D2pIndexTable.cs
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
using System.ComponentModel;
using System.IO;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pIndexTable : INotifyPropertyChanged
    {
        public const int TableOffset = -24;
        public const SeekOrigin TableSeekOrigin = SeekOrigin.End;

        public D2pIndexTable(D2pFile container)
        {
            Container = container;
        }

        public D2pFile Container
        {
            get;
            private set;
        }

        public int OffsetBase
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public int EntriesDefinitionOffset
        {
            get;
            set;
        }

        public int EntriesCount
        {
            get;
            set;
        }

        public int PropertiesOffset
        {
            get;
            set;
        }

        public int PropertiesCount
        {
            get;
            set;
        }

        public void ReadTable(IDataReader reader)
        {
            OffsetBase = reader.ReadInt();
            Size = reader.ReadInt();
            EntriesDefinitionOffset = reader.ReadInt();
            EntriesCount = reader.ReadInt();
            PropertiesOffset = reader.ReadInt();
            PropertiesCount = reader.ReadInt();
        }

        public void WriteTable(IDataWriter writer)
        {
            writer.WriteInt(OffsetBase);
            writer.WriteInt(Size);
            writer.WriteInt(EntriesDefinitionOffset);
            writer.WriteInt(EntriesCount);
            writer.WriteInt(PropertiesOffset);
            writer.WriteInt(PropertiesCount);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}