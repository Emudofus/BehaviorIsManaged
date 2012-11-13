#region License GNU GPL
// D2pProperty.cs
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
using BiM.Core.IO;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pProperty : INotifyPropertyChanged
    {
        public D2pProperty()
        {
            
        }

        public D2pProperty(string key, string property)
        {
            Key = key;
            Value = property;
        }

        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public void ReadProperty(IDataReader reader)
        {
            Key = reader.ReadUTF();
            Value = reader.ReadUTF();
        }

        public void WriteProperty(IDataWriter writer)
        {
            writer.WriteUTF(Key);
            writer.WriteUTF(Value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}