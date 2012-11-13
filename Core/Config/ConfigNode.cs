#region License GNU GPL
// ConfigNode.cs
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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BiM.Core.Config
{
    public class ConfigNode
    {
        public const string NodeName = "Key";
        public const string AttributeName = "name";

        private string m_name;
        private XmlNode m_node;
        private object m_value;

        protected ConfigNode()
        {
        }

        public ConfigNode(XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            Load(node);
        }

        public ConfigNode(string name, object value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            m_name = name;
            m_value = value;
        }

        public virtual string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public bool IsSynchronised
        {
            get;
            protected set;
        }

        public static string GetNodeName(XmlNode node)
        {
            if (node.Attributes[AttributeName] == null)
                throw new Exception(string.Format("Attribute {0} not found", AttributeName));

            return node.Attributes[AttributeName].Value;
        }

        public virtual object GetValue(Type type)
        {
            if (m_node != null)
            {
                m_value = new XmlSerializer(type).Deserialize(new StringReader(m_node.InnerXml));
                m_node = null;
                IsSynchronised = true;
            }

            return m_value;
        }

        public virtual void SetValue(object value)
        {
            m_value = value;
            IsSynchronised = false;
        }

        internal virtual void Save(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            writer.WriteStartElement(NodeName);
            writer.WriteAttributeString(AttributeName, Name);

            if (m_value != null)
                new XmlSerializer(m_value.GetType()).Serialize(writer, m_value);
            IsSynchronised = true;

            writer.WriteEndElement();
        }

        internal virtual void Load(XmlNode node)
        {
            m_name = GetNodeName(node);
            m_node = node;
        }
    }
}