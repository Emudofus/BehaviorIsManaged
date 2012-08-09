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
                    var data = source.ReadObject<T>(keys);

                    return data;
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