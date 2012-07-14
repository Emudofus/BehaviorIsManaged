using System;
using BiM.Protocol.Data;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Data
{
    public class DLMSource : IDataSource, IDisposable
    {
        private DlmReader m_reader;

        public DLMSource(DlmReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            m_reader = reader;
        }

        public T ReadObject<T>(int id) where T : class, IDataObject
        {
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            var map = m_reader.ReadMap();

            if (map.Id != id)
                throw new ArgumentException("id != map.id");

            return map as T;
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