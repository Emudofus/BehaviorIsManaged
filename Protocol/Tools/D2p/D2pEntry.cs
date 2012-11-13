#region License GNU GPL
// D2pEntry.cs
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
using System.ComponentModel;
using System.IO;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pEntry : INotifyPropertyChanged
    {
        private D2pDirectory[] m_directories;
        private string m_fullFileName;
        private byte[] m_newData;

        private D2pEntry(D2pFile container)
        {
            Container = container;
            Index = -1;
        }

        public D2pEntry(D2pFile container, string fileName)
        {
            Container = container;
            FullFileName = fileName;
            Index = -1;
        }

        public D2pEntry(D2pFile container, string fileName, byte[] data)
        {
            Container = container;
            FullFileName = fileName;
            m_newData = data;
            State = D2pEntryState.Added;
            Size = data.Length;
            Index = -1;
        }

        public D2pFile Container
        {
            get;
            set;
        }

        public string FileName
        {
            get { return Path.GetFileName(m_fullFileName); }
        }

        public string FullFileName
        {
            get { return m_fullFileName; }
            private set
            {
                m_fullFileName = value;
            }
        }

        public D2pDirectory Directory
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public D2pEntryState State
        {
            get;
            set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public static D2pEntry CreateEntryDefinition(D2pFile container, IDataReader reader)
        {
            var entry = new D2pEntry(container);
            entry.ReadEntryDefinition(reader);

            return entry;
        }

        public void ReadEntryDefinition(IDataReader reader)
        {
            FullFileName = reader.ReadUTF();
            Index = reader.ReadInt();
            Size = reader.ReadInt();
        }

        public void WriteEntryDefinition(IDataWriter writer)
        {
            if (Index == -1)
                throw new InvalidOperationException("Invalid entry, index = -1");

            writer.WriteUTF(FullFileName);
            writer.WriteInt(Index);
            writer.WriteInt(Size);
        }

        public byte[] ReadEntry(IDataReader reader)
        {
            if (State == D2pEntryState.Removed)
                throw new InvalidOperationException("Cannot read a deleted entry");

            if (State == D2pEntryState.Dirty || State == D2pEntryState.Added)
                return m_newData;

            return reader.ReadBytes(Size);
        }

        public void ModifyEntry(byte[] data)
        {
            m_newData = data;
            Size = data.Length;
            State = D2pEntryState.Dirty;
        }

        public string[] GetDirectoriesName()
        {
            return Path.GetDirectoryName(FullFileName).Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}