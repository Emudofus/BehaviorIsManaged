#region License GNU GPL
// DLMSource.cs
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
using BiM.Protocol.Data;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Data
{
    public class DLMSource : IDataSource, IDisposable
    {
        private readonly DlmReader m_reader;
        private DlmMap m_map;

        public DLMSource(DlmReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            m_reader = reader;
        }

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            if (m_map != null)
                return m_map as T;

            if (keys.Length <= 0 || keys.Length > 2 || !( keys[0] is IConvertible ) || ( keys.Length == 2 && !( keys[1] is string ) ))
                throw new ArgumentException("DLMSource needs a int key and can have a decryptionKey as string, use ReadObject(int) or ReadObject(int, string)");


            int id = Convert.ToInt32(keys[0]);

            if (keys[1] is string)
                m_reader.DecryptionKey = (string)keys[1];
            
            m_map = m_reader.ReadMap();

            if (m_map.Id != id)
                throw new ArgumentException("id != map.id");

            return m_map as T;
        }

        public bool DoesHandleType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return type == typeof(DlmMap);
        }

        public void Dispose()
        {
            if (m_reader != null)
                m_reader.Dispose();
        }
    }
}