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
        private D2pFile m_reader;

        public D2PSource(D2pFile reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            m_reader = reader;

            BindTypeWithExtension(typeof(DlmMap), ".dlm");
        }

        public void BindTypeWithExtension(Type type, string ext)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (ext == null) throw new ArgumentNullException("ext");
            m_types.Add(type, m_reader.Entries.
                Where(entry => Path.GetExtension(entry.FileName) == ext).
                Select(entry => entry).ToArray());
        }

        public T ReadObject<T>(params object[] keys) where T : class, IDataObject
        {
            // we don't check the others keys, they may be used by the others sources
            if (( !( keys[0] is IConvertible ) ))
                throw new ArgumentException("D2PSource needs a int key, use ReadObject(int)");

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

        private IDataSource CreateDataSource(D2pEntry entry, Type type)
        {
            if (type == typeof(DlmMap))
                return new DLMSource(new DlmReader(new MemoryStream(m_reader.ReadFile(entry))));

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