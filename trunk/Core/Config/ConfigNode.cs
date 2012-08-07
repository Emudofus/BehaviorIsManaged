using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BiM.Core.Config
{
    public class ConfigNode
    {
        public const string NodeName = "Key";
        public const string AttributeName = "name";

        private XmlNode m_node;
        private object m_value;
        private string m_name;

        public ConfigNode(XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            m_node = node;

            if (node.Attributes[AttributeName] == null)
                throw new Exception(string.Format("Attribute {0} not found", AttributeName));
        }

        public ConfigNode(string name, object value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            m_name = name;
            m_value = value;
        }

        public string Name
        {
            get
            {
                if (m_node == null)
                    return m_name;
                else
                    return m_node.Attributes[AttributeName].Value;
            }
            set
            {
                if (m_node == null)
                    m_name = value;
                else
                    m_node.Attributes[AttributeName].Value = value;
            }
        }

        public T GetValue<T>()
        {
            if (m_value != null)
                return (T)m_value;

            return (T)(m_value = new XmlSerializer(typeof(T)).Deserialize(new StringReader(m_node.InnerXml)));
        }

        public void SetValue<T>(T value)
        {
            m_value = value;
        }

        internal void Save(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            writer.WriteStartElement(NodeName);
            writer.WriteAttributeString(AttributeName, Name);

            if (m_value != null)
                new XmlSerializer(m_value.GetType()).Serialize(writer, m_value);
            else if (m_node == null)
                throw new Exception("m_node and m_value are null, something went wrong");
            else
                m_node.WriteContentTo(writer);


            writer.WriteEndElement();
        }
    }
}