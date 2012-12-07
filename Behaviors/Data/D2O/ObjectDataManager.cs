#region License GNU GPL
// ObjectDataManager.cs
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
using BiM.Core.Reflection;
using BiM.Protocol.Tools;
using NLog;

namespace BiM.Behaviors.Data
{
    /// <summary>
    /// Retrieves D2O objects. Thread safe
    /// </summary>
    public class ObjectDataManager : Singleton<ObjectDataManager>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Type, D2OReader> m_readers = new Dictionary<Type, D2OReader>();
        private List<Type> m_ignoredTyes = new List<Type>();

        public void AddReaders(string directory)
        {
            foreach (var d2oFile in Directory.EnumerateFiles(directory).Where(entry => entry.EndsWith(".d2o")))
            {
                var reader = new D2OReader(d2oFile);

                AddReader(reader);
            }
        }

        public void AddReader(D2OReader d2oFile)
        {
            var classes = d2oFile.Classes;

            foreach (var @class in classes)
            {
                if (m_ignoredTyes.Contains(@class.Value.ClassType))
                    continue;

                if (m_readers.ContainsKey(@class.Value.ClassType))
                {
                    // this classes are not bound to a single file, so we ignore them
                    m_ignoredTyes.Add(@class.Value.ClassType);
                    m_readers.Remove(@class.Value.ClassType);
                }
                else
                {
                    m_readers.Add(@class.Value.ClassType, d2oFile);
                }
            }

            logger.Info("File added : {0}", Path.GetFileName(d2oFile.FilePath));

        }

        public T Get<T>(uint key)
            where T : class
        {
            return Get<T>((int)key);
        }

        public T Get<T>(int key)
            where T : class
        {
            if (!m_readers.ContainsKey(typeof(T)))
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = m_readers[typeof(T)];

            return reader.ReadObject<T>(key, true);
        }

        public T GetOrDefault<T>(uint key)
            where T : class
        {
            return GetOrDefault<T>((int)key);
        }

        public T GetOrDefault<T>(int key)
            where T : class
        {
            try
            {
                return Get<T>(key);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> EnumerateObjects<T>(params object[] keys) where T : class
        {
            if (!m_readers.ContainsKey(typeof(T)))
                throw new ArgumentException("Cannot find data corresponding to type : " + typeof(T));

            var reader = m_readers[typeof(T)];

            return reader.Indexes.Select(index => reader.ReadObject(index.Key, true)).OfType<T>().Select(obj => obj);
        }
    }
}