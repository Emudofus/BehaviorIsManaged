#region License GNU GPL

// MapDataSource.cs
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
using System.Linq;
using System.Threading.Tasks;
using BiM.Behaviors.Data;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.UI;
using BiM.Protocol.Tools.Dlm;
using Db4objects.Db4o;
using NLog;
using ProtoBuf;

namespace BiM.Behaviors.Game.World.Data
{
    public class MapDataSource : IDataSource
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const int HeaderSize = 12;

        [Configurable("MapsDataFile")]
        public static readonly string MapsDataFile = "./maps.dat";

        private readonly ConcurrentDictionary<int, int> m_offsetsTable = new ConcurrentDictionary<int, int>();
        private ProgressionCounter m_progression;
        private BinaryWriter m_writer;

        private int m_tableOffset;
        private int m_length;

        #region IDataSource Members

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            if (keys.Length != 1)
                throw new ArgumentException("keys.Length != 1");

            int id = Convert.ToInt32(keys[0]);

            if (!m_offsetsTable.ContainsKey(id))
                throw new Exception(string.Format("Map id {0} not found", id));

            var stream = File.OpenRead(MapsDataFile);
            stream.Seek(m_offsetsTable[id], SeekOrigin.Begin);

            return Serializer.DeserializeWithLengthPrefix<MapData>(stream, PrefixStyle.Fixed32) as T;
        }

        public IEnumerable<T> EnumerateObjects<T>(params object[] keys) where T : class
        {
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            var stream = File.OpenRead(MapsDataFile);
            stream.Seek(HeaderSize, SeekOrigin.Begin);

            while (stream.Position < m_length + HeaderSize)
            {
                yield return Serializer.DeserializeWithLengthPrefix<MapData>(stream, PrefixStyle.Fixed32) as T;
            }

            stream.Dispose();
        }

        public bool DoesHandleType(Type type)
        {
            return type == typeof (MapData);
        }

        #endregion

        public ProgressionCounter Initialize()
        {
            if (!File.Exists(MapsDataFile))
                return BeginFileCreation();

            var stream = File.OpenRead(MapsDataFile);
            var reader = new BinaryReader(stream);

            if (new string(reader.ReadChars(4)) != "MAPS")
            {
                throw new Exception(string.Format("File {0} corrupted, delete it manually", MapsDataFile));
            }

            m_tableOffset = reader.ReadInt32();
            m_length = reader.ReadInt32();

            reader.BaseStream.Seek(m_tableOffset, SeekOrigin.Begin);

            while (stream.Position < stream.Length)
            {
                m_offsetsTable.TryAdd(reader.ReadInt32(), reader.ReadInt32());
            }

            return null;
        }

        public ProgressionCounter BeginFileCreation()
        {
            m_writer = new BinaryWriter(File.Create(MapsDataFile));

            m_writer.Write("MAPS".ToCharArray());
            m_writer.Write(0); // table offset
            m_writer.Write(0); // total length

            m_progression = new ProgressionCounter(CountMaps());
            IEnumerable<DlmMap> maps = DataProvider.Instance.EnumerateAll<DlmMap>(Map.GenericDecryptionKey);
            int counter = 0;
            Task.Factory.StartNew(() =>
                {
                    foreach (DlmMap map in maps)
                    {
                        m_offsetsTable.TryAdd(map.Id, (int) m_writer.BaseStream.Position);
                        Serializer.SerializeWithLengthPrefix(m_writer.BaseStream, new MapData(map), PrefixStyle.Fixed32);

                        m_progression.UpdateValue(counter++);
                    }
                }).ContinueWith((task) => EndFileCreation());

            return m_progression;
        }

        private void EndFileCreation()
        {
            m_tableOffset = (int) m_writer.BaseStream.Position;
            m_length = (int) (m_writer.BaseStream.Position - HeaderSize); // substracts the header

            foreach (var entry in m_offsetsTable)
            {
                m_writer.Write(entry.Key);
                m_writer.Write(entry.Value);
            }

            m_writer.Seek(4, SeekOrigin.Begin);
            m_writer.Write(m_tableOffset);
            m_writer.Write(m_length);

            m_writer.Flush();
            m_writer.Close();
            m_writer.Dispose();
            m_writer = null;

            m_progression.NotifyEnded();
        }

        private int CountMaps()
        {
            D2PSource source = DataProvider.Instance.Sources.OfType<D2PSource>().
                FirstOrDefault(x => x.Reader != null && x.Reader.FileName.Contains("maps"));

            if (source != null)
                return source.Reader.Entries.Count();

            return 0;
        }
    }
}