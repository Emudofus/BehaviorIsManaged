#region License GNU GPL
// DataProvider.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Core.Reflection;
using BiM.Protocol.Data;

namespace BiM.Behaviors.Data
{
    public class DataProvider : Singleton<DataProvider>
    {
        private readonly List<IDataSource> m_sources = new List<IDataSource>();

        public ReadOnlyCollection<IDataSource> Sources
        {
            get { return m_sources.AsReadOnly(); }
        }

        public void AddSource(IDataSource source)
        {
            m_sources.Add(source);
        }

        public void RemoveSource(IDataSource source)
        {
            m_sources.Remove(source);
        }

        public IEnumerable<IDataSource> GetSourcesMatching(Type type)
        {
            var sources = m_sources.Where(entry => entry.DoesHandleType(type)).ToArray();

            if (sources.Length < 1)
                throw new InvalidOperationException(string.Format("Type {0} not handled by any data source", type));

            return sources;
        }

        public T Get<T>(params object[] keys) where T : class
        {
            // try each source before throwing an exception
            var exceptions = new List<Exception>();
            var sources = GetSourcesMatching(typeof(T));
            foreach (var source in sources)
            {
                try
                {
                    // lock the source to ensure thread safe context
                    // sacrify some performances
                    lock (source)
                    {
                        var data = source.ReadObject<T>(keys);

                        return data;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(string.Format("Cannot retrieve {0} with these keys {1}", typeof(T), string.Join(",", keys)), exceptions);
        }
    }
}