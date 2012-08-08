using System;
using BiM.Protocol.Data;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Data
{
    public class DLMSource : IDataSource, IDisposable
    {
        private readonly DlmReader m_reader;

        public DLMSource(DlmReader reader)
        {
            if (m_reader == null) throw new ArgumentNullException("reader");
            m_reader = reader;
        }

        public T ReadObject<T>(params object[] keys) where T : class, IDataObject
        {
            if (keys.Length <= 0 || keys.Length > 2 || !( keys[0] is IConvertible ) || ( keys.Length == 2 && !( keys[1] is string ) ))
                throw new ArgumentException("DLMSource needs a int key and can have a decryptionKey as string, use ReadObject(int) or ReadObject(int, string)");

            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            int id = Convert.ToInt32(keys[0]);

            if (keys[1] is string)
                m_reader.DecryptionKey = (string)keys[1];

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