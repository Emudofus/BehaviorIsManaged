using System.Collections.Generic;
using System.IO;
using BiM.Core.IO;

namespace BiM.Protocol.Tools
{
    public class I18NFile
    {
        private readonly string m_uri;
        private readonly Dictionary<string, string> m_textIndexes = new Dictionary<string, string>();
        private readonly Dictionary<int, string> m_indexes = new Dictionary<int, string>();

        public I18NFile(string uri)
        {
            m_uri = uri;
            Init();
        }

        private void Init()
        {
            using (var reader = new BigEndianReader(new StreamReader(m_uri).BaseStream))
            {
                var indexPos = reader.ReadInt();
                reader.Seek(indexPos, SeekOrigin.Begin);
                var indexLen = reader.ReadInt();

                for (int i = 0; i < indexLen; i += 8)
                {
                    var key = reader.ReadInt();
                    var dataPos = reader.ReadInt();
                    var pos = (int)reader.BaseStream.Position;
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    m_indexes.Add(key, reader.ReadUTF());
                    reader.Seek(pos, SeekOrigin.Begin);
                }

                while (reader.BytesAvailable > 0)
                {
                    var key = reader.ReadUTF();
                    var dataPos = reader.ReadInt();
                    var pos = (int)reader.BaseStream.Position;
                    reader.Seek(dataPos, SeekOrigin.Begin);
                    m_textIndexes.Add(key, reader.ReadUTF());
                    reader.Seek(pos, SeekOrigin.Begin);
                }
            }
        }

        public string GetText(int id)
        {
            if (m_indexes.ContainsKey(id))
            {
                return m_indexes[id];
            }
            return "{null}";
        }

        public string GetText(string id)
        {
            if (m_textIndexes.ContainsKey(id))
            {
                return m_textIndexes[id];
            }
            return "{null}";
        }

        public void SetText(int id, string value)
        {
            if (m_indexes.ContainsKey(id))
                m_indexes[id] = value;
            else
                m_indexes.Add(id, value);
        }

        public void SetText(string id, string value)
        {
            if (m_textIndexes.ContainsKey(id))
                m_textIndexes[id] = value;
            else
                m_textIndexes.Add(id, value);
        }

        public Dictionary<int, string> GetAllText()
        {
            return m_indexes;
        }

        public Dictionary<string, string> GetAllUiText()
        {
            return m_textIndexes;
        }

        public void Save()
        {
            Save(m_uri);
        }

        public void Save(string uri)
        {
            using (var writer = new BigEndianWriter(new StreamWriter(uri).BaseStream))
            {
                var indexTable = new BigEndianWriter();
                writer.Seek(4, SeekOrigin.Begin);

                foreach (var index in m_indexes)
                {
                    indexTable.WriteInt(index.Key);
                    indexTable.WriteInt((int)writer.Position);
                    writer.WriteUTF(index.Value);
                }

                var indexLen = (int)indexTable.Position;

                foreach (var index in m_textIndexes)
                {
                    indexTable.WriteUTF(index.Key);
                    indexTable.WriteInt((int)writer.Position);
                    writer.WriteUTF(index.Value);
                }

                var indexPos = (int)writer.Position;

                /* write index at end */
                var indexData = indexTable.Data;
                writer.WriteInt(indexLen);
                writer.WriteBytes(indexData);

                /* write index pos at begin */
                writer.Seek(0, SeekOrigin.Begin);
                writer.WriteInt(indexPos);
            }
        }
    }
}
