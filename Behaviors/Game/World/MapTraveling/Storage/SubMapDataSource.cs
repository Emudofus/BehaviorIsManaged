#region License GNU GPL
// SubMapRetriever.cs
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using BiM.Behaviors.Data;
using BiM.Core.IO;

namespace BiM.Behaviors.Game.World.MapTraveling.Storage
{
    /// <summary>
    /// Thread safe
    /// </summary>
    public class SubMapDataSource : IDataSource
    {
        private readonly string m_filePath;
        private ConcurrentDictionary<int, int> m_indexes = new ConcurrentDictionary<int, int>();
        private ConcurrentDictionary<int, SerializableSubMap[]> m_cache = new ConcurrentDictionary<int, SerializableSubMap[]>();
        private int m_tableOffset;
        private byte[] m_fileData;

        public SubMapDataSource(string filePath)
        {
            m_filePath = filePath;
        }

        public string FilePath
        {
            get { return m_filePath; }
        }

        public void Initialize()
        {
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("Not found", FilePath);

            m_fileData = File.ReadAllBytes(FilePath);

            var reader = new FastLittleEndianReader(m_fileData);
            
            if (reader.ReadUTFBytes(7) != "SUBMAPS")
                throw new InvalidOperationException(string.Format("Corrupted file, header 'SUBMAPS' not found in file {0}", FilePath));

            m_tableOffset = reader.ReadInt();

            reader.Seek(m_tableOffset, SeekOrigin.Begin);
            while (reader.BytesAvailable > 0)
            {
                var index = reader.ReadInt();
                var offset = reader.ReadInt();

                m_indexes.TryAdd(index, offset);
            }
            
        }

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (keys.Length != 1 || !( keys[0] is IConvertible ))
                throw new ArgumentException("SubMapDataSource needs a int key, use ReadObject(int)");

            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            int id = Convert.ToInt32(keys[0]);

            if (!m_indexes.ContainsKey(id))
                throw new ArgumentException("SubMapDataSource cannot find map id : " + id);

            var offset = m_indexes[id];
            SerializableSubMap[] result;
            if (m_cache.TryGetValue(id, out result))
                return result as T;

            var reader = new BinaryReader(new MemoryStream(m_fileData));
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            int len = reader.ReadInt32();
            result = new SerializableSubMap[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = new SerializableSubMap();
                result[i].Deserialize(reader);
            }

            m_cache.TryAdd(id, result);

            return result as T;
        }

        public IEnumerable<T> EnumerateObjects<T>(params object[] keys) where T : class
        {
            // copy
            var indexes = new Dictionary<int, int>(m_indexes);

            foreach (var subMaps in m_cache)
            {
                indexes.Remove(subMaps.Key);
                yield return subMaps.Value as T;
            }

            foreach (var index in indexes)
            {
                var offset = index.Value;

                var reader = new BinaryReader(new MemoryStream(m_fileData));
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                int len = reader.ReadInt32();
                var result = new SerializableSubMap[len];
                for (int i = 0; i < len; i++)
                {
                    result[i] = new SerializableSubMap();
                    result[i].Deserialize(reader);
                }

                m_cache.TryAdd(index.Key, result);
            }
        }

        public bool DoesHandleType(Type type)
        {
            return type == typeof(SerializableSubMap[]);
        }
    }
}