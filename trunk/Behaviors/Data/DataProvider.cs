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

        public IDataSource GetSourceMatching(Type type)
        {
            var sources = m_sources.Where(entry => entry.DoesHandleType(type)).ToArray();

            if (sources.Length > 1)
                throw new InvalidOperationException("Type {0} handle by more than 1 source");

            if (sources.Length < 1)
                throw new InvalidOperationException("Type {0} not handled by any data source");

            return sources.Single();
        }

        public T Get<T>(int id)
            where T : class, IDataObject
        {
            var source = GetSourceMatching(typeof(T));

            return source.ReadObject<T>(id);
        }

        public T GetObjectDataOrDefault<T>(int id)
            where T : class, IDataObject
        {
            var source = GetSourceMatching(typeof(T));

            return source.ReadObject<T>(id);
        }
    }
}