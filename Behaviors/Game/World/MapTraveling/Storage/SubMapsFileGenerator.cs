#region License GNU GPL
// SubMapsFileGenerator.cs
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
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors.Data;
using BiM.Protocol.Tools.Dlm;
using NLog;

namespace BiM.Behaviors.Game.World.MapTraveling.Storage
{
    public class SubMapsFileGenerator
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly string m_filePath;
        private readonly ConcurrentDictionary<int, SerializableSubMap[]> m_subMaps = new ConcurrentDictionary<int, SerializableSubMap[]>();
        private Task m_generationTask;
        private int m_counter;

        public event Action<SubMapsFileGenerator> GenerationEnded;

        public SubMapsFileGenerator(string filePath)
        {   
            m_filePath = filePath;
        }

        public string FilePath
        {
            get { return m_filePath; }
        }

        public int GenerationCounter
        {
            get { return m_counter; }
        }

        public int TotalMaps
        {
            get;
            private set;
        }

        private void CountMaps()
        {
            var source = DataProvider.Instance.Sources.OfType<D2PSource>().
                FirstOrDefault(x => x.Reader != null && x.Reader.FileName.Contains("maps"));

            if (source != null)
                TotalMaps = source.Reader.Entries.Count();
        }

        public void BeginGeneration()
        {
            if (m_generationTask != null)
                throw new InvalidOperationException("Generation already started");

            // check we can create the file
            File.Create(FilePath).Close();

            CountMaps();

            m_counter = 0;

            m_generationTask = Task.Factory.StartNew(() =>
            Parallel.ForEach(EnumerateMaps(), map =>
            {
                using (var builder = new SubMapBuilder(map))
                {
                    var submaps = builder.Build();

                    if (!m_subMaps.TryAdd(map.Id, submaps))
                        logger.Error("Cannot analyse map {0} submaps", map.Id);
                }

                Interlocked.Increment(ref m_counter);
            })).ContinueWith(task => EndGeneration());
        }

        private void EndGeneration()
        {
            using (var writer = new BinaryWriter(File.Create(FilePath)))
            {
                // header
                writer.Write("SUBMAPS".ToCharArray());
                writer.Write((int)0); // table offset

                var indexes = new Dictionary<int, int>();

                foreach (var map in m_subMaps)
                {
                    indexes.Add(map.Key, (int) writer.BaseStream.Position);

                    writer.Write(map.Value.Length);
                    foreach (var submap in map.Value)
                    {
                        submap.Serialize(writer);
                    }
                }

                int tableOffset = (int) writer.BaseStream.Position;
                writer.Seek(7, SeekOrigin.Begin);
                writer.Write(tableOffset);
                writer.Seek(tableOffset, SeekOrigin.Begin);

                foreach (var index in indexes)
                {
                    writer.Write(index.Key);
                    writer.Write(index.Value);
                }

                writer.Flush();
            }

            var evnt = GenerationEnded;
            if (evnt != null)
                evnt(this);
        }

        private IEnumerable<Map> EnumerateMaps()
        {
            var maps = DataProvider.Instance.EnumerateAll<DlmMap>(Map.GenericDecryptionKey);

            return maps.Select(Map.CreateDataMapInstance);
        }
    }
}