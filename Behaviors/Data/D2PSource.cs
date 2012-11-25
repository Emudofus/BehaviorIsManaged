﻿#region License GNU GPL
// D2PSource.cs
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
using System.IO;
using System.Linq;
using BiM.Protocol.Data;
using BiM.Protocol.Tools.D2p;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Data
{
    public class D2PSource : IDataSource, IDisposable
    {
        private Dictionary<Type, D2pEntry[]> m_types = new Dictionary<Type, D2pEntry[]>();
        private Dictionary<int, D2pEntry> m_entriesLinks = new Dictionary<int, D2pEntry>(); 
        private Dictionary<D2pEntry, IDataSource> m_sources = new Dictionary<D2pEntry, IDataSource>(); 
        private readonly D2pFile m_reader;

        public D2PSource(D2pFile reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            m_reader = reader;

            BindTypeWithExtension(typeof(DlmMap), ".dlm");
        }

        public D2pFile Reader
        {
            get { return m_reader; }
        }

        public void BindTypeWithExtension(Type type, string ext)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (ext == null) throw new ArgumentNullException("ext");
            m_types.Add(type, m_reader.Entries.
                Where(entry => Path.GetExtension(entry.FileName) == ext).
                Select(entry => entry).ToArray());
        }

        public T ReadObject<T>(params object[] keys) where T : class
        {
            // we don't check the others keys, they may be used by the others sources
            if (( !( keys[0] is IConvertible ) ))
                throw new ArgumentException("D2PSource needs a int key, use ReadObject(int, args[])");

            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException(string.Format("type {0} not handled", typeof(T)));

            int id = Convert.ToInt32(keys[0]);
            

            // retrieve the bound entry to the key or find it in the d2p file
            D2pEntry entry;
            if (!m_entriesLinks.TryGetValue(id, out entry))
            {
                var idStr = id.ToString();
                entry = m_types[typeof(T)].FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.FileName) == idStr);

                if (entry == null)
                    throw new ArgumentException(string.Format("id {0} not found", id));

                m_entriesLinks.Add(id, entry);
            }

            // then retrieve the data source bound to the entry or create it
            IDataSource source;
            if (!m_sources.TryGetValue(entry, out source))
            {
                source = CreateDataSource(entry, typeof(T));

                m_sources.Add(entry, source);
            }

            return source.ReadObject<T>(keys);
        }

        public IEnumerable<T> EnumerateObjects<T>(params object[] keys) where T : class
        {
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException(string.Format("type {0} not handled", typeof(T)));

            var entries = m_types[typeof(T)];

            foreach (D2pEntry entry in entries)
            {
                IDataSource source = CreateDataSource(entry, typeof (T));
                IEnumerator<T> enumerator = source.EnumerateObjects<T>(keys).GetEnumerator();

                bool hasNext = true;
                while (hasNext)
                {
                    hasNext = enumerator.MoveNext();

                    if (hasNext)
                        yield return enumerator.Current;
                }
            }
        }

        private IDataSource CreateDataSource(D2pEntry entry, Type type)
        {
            if (type == typeof(DlmMap))
                return new DLMSource(new DlmReader(m_reader.ReadFile(entry)));

            throw new ArgumentException(string.Format("type {0} not handled", type));
        }

        public bool DoesHandleType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return m_types.ContainsKey(type);
        }

        public void Dispose()
        {
            if (m_reader != null)
                m_reader.Dispose();
        }
    }
}