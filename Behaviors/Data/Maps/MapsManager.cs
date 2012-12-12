#region License GNU GPL
// MapsManager.cs
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
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Data;
using BiM.Core.Config;
using BiM.Core.Reflection;
using BiM.Core.UI;
using BiM.Protocol.Data;
using BiM.Protocol.Tools.D2p;
using BiM.Protocol.Tools.Dlm;
using NLog;
using ProtoBuf;

namespace BiM.Behaviors.Data.Maps
{
    public class MapsManager : Singleton<MapsManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<int, D2pEntry> m_entriesLinks = new Dictionary<int, D2pEntry>();
        private D2pFile m_reader;

        #region Map Data
        private const int HeaderSize = 14;
        public const ushort CurrentMapsFileVersion = 2;

        [Configurable("MapsDataFile")]
        public static readonly string MapsDataFile = "./maps.dat";

        private readonly ConcurrentDictionary<int, int> m_offsetsTable = new ConcurrentDictionary<int, int>();
        private ProgressionCounter m_progression;
        private BinaryWriter m_writer;

        private ushort m_fileVersion;
        private uint m_tableOffset;
        private uint m_length;

        #endregion

        public int MapsCount
        {
            get { return m_reader.Entries.Count(); }
        }

        public bool FillLinks()
        {
            foreach (var entry in m_reader.Entries)
                if (!m_entriesLinks.ContainsKey(entry.Index))
                    m_entriesLinks.Add(entry.Index, entry);
            return m_entriesLinks.Count > 0;
        }

        public DlmMap GetDlmMap(int id, string decryptionKey)
        {
            // retrieve the bound entry to the key or find it in the d2p file
            D2pEntry entry;
            if (!m_entriesLinks.TryGetValue(id, out entry))
            {
                var idStr = id.ToString();
                entry = m_reader.Entries.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.FileName) == idStr);

                if (entry == null)
                    throw new ArgumentException(string.Format("Map id {0} not found", id));

                m_entriesLinks.Add(id, entry);
            }

            // then retrieve the data source bound to the entry or create it
            var dlmReader = new DlmReader(m_reader.ReadFile(entry));
            dlmReader.DecryptionKey = decryptionKey;

            return dlmReader.ReadMap();
        }

        public MapData GetMapData(int id)
        {
            if (!m_offsetsTable.ContainsKey(id))
                throw new Exception(string.Format("Map id {0} not found", id));

            var stream = File.OpenRead(MapsDataFile);
            stream.Seek(m_offsetsTable[id], SeekOrigin.Begin);

            return Serializer.DeserializeWithLengthPrefix<MapData>(stream, PrefixStyle.Fixed32);
        }

        public IEnumerable<MapData> EnumerateMaps()
        {
            var stream = File.OpenRead(MapsDataFile);
            stream.Seek(HeaderSize, SeekOrigin.Begin);

            while (stream.Position < m_length + HeaderSize)
            {
                yield return Serializer.DeserializeWithLengthPrefix<MapData>(stream, PrefixStyle.Fixed32);
            }

            stream.Dispose();
        }


        public IEnumerable<DlmMap> EnumerateClientMaps(string key)
        {

            foreach (var entry in m_entriesLinks)
            {
                int id = int.Parse(Path.GetFileNameWithoutExtension(entry.Value.FileName));

                // then retrieve the data source bound to the entry or create it
                var dlmReader = new DlmReader(m_reader.ReadFile(entry.Value));
                dlmReader.DecryptionKey = key;

                yield return dlmReader.ReadMap();
            }
        }

        public IEnumerable<DlmMap> EnumerateClientMaps(DlmReader.KeyProvider decryptionKeyProvider)
        {

            foreach (var entry in m_entriesLinks)
            {
                int id = int.Parse(Path.GetFileNameWithoutExtension(entry.Value.FileName));

                // then retrieve the data source bound to the entry or create it
                var dlmReader = new DlmReader(m_reader.ReadFile(entry.Value));
                dlmReader.DecryptionKeyProvider = decryptionKeyProvider;

                yield return dlmReader.ReadMap();
            }
        }

        public ProgressionCounter Initialize(string d2pFile)
        {
            m_reader = new D2pFile(d2pFile);

            if (!File.Exists(MapsDataFile))
                return BeginFileCreation();

            var stream = File.OpenRead(MapsDataFile);
            var reader = new BinaryReader(stream);

            if (new string(reader.ReadChars(4)) != "MAPS")
            {
                throw new Exception(string.Format("File {0} corrupted, delete it manually", MapsDataFile));
            }

            m_fileVersion = reader.ReadUInt16();

            if (m_fileVersion != CurrentMapsFileVersion)
            {
                logger.Info("{0} outdated (file version :{1}, expected version {2})", MapsDataFile, m_fileVersion, CurrentMapsFileVersion);
                reader.Dispose();
                return BeginFileCreation();
            }

            m_tableOffset = reader.ReadUInt32();
            m_length = reader.ReadUInt32();

            reader.BaseStream.Seek(m_tableOffset, SeekOrigin.Begin);

            while (stream.Position < stream.Length)
            {
                m_offsetsTable.TryAdd(reader.ReadInt32(), reader.ReadInt32());
            }

            reader.Dispose();

            return null;
        }

        public ProgressionCounter BeginFileCreation()
        {
            m_writer = new BinaryWriter(File.Create(MapsDataFile));

            m_writer.Write("MAPS".ToCharArray());
            m_writer.Write(CurrentMapsFileVersion); // version
            m_writer.Write(0); // table offset
            m_writer.Write(0); // total length
            if (m_entriesLinks.Count < 100)
                FillLinks();
            m_progression = new ProgressionCounter(MapsCount);
            IEnumerable<DlmMap> maps = EnumerateClientMaps(Map.GenericDecryptionKey);
            int counter = 0;
            Task.Factory.StartNew(() =>
            {
                foreach (DlmMap map in maps)
                {
                    var position = ObjectDataManager.Instance.GetOrDefault<MapPosition>(map.Id);
                    m_offsetsTable.TryAdd(map.Id, (int)m_writer.BaseStream.Position);
                    Serializer.SerializeWithLengthPrefix(m_writer.BaseStream, new MapData(map, position), PrefixStyle.Fixed32);

                    m_progression.UpdateValue(counter++);
                }
            }).ContinueWith((task) => EndFileCreation());

            return m_progression;
        }

        private void EndFileCreation()
        {
            m_tableOffset = (uint)m_writer.BaseStream.Position;
            m_length = (uint)( m_writer.BaseStream.Position - HeaderSize ); // substracts the header

            foreach (var entry in m_offsetsTable)
            {
                m_writer.Write(entry.Key);
                m_writer.Write(entry.Value);
            }

            m_writer.Seek(6, SeekOrigin.Begin);
            m_writer.Write(m_tableOffset);
            m_writer.Write(m_length);

            m_writer.Flush();
            m_writer.Close();
            m_writer.Dispose();
            m_writer = null;

            m_progression.NotifyEnded();
        }

    }
}