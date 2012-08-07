using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace BiM.Core.Config
{
    public class Config
    {
        private readonly string m_filePath;
        private readonly List<ConfigNode> m_nodes = new List<ConfigNode>();
        private static readonly List<Tuple<Assembly, Config>> m_boundsAssemblies = new List<Tuple<Assembly, Config>>();

        public Config(string filePath)
        {
            m_filePath = filePath;
        }

        public string FilePath
        {
            get { return m_filePath; }
        }

        public List<ConfigNode> Nodes
        {
            get { return m_nodes; }
        }

        public void Load()
        {
            if (!File.Exists(FilePath))
                Save();

            var document = new XmlDocument();
            document.Load(FilePath);

            var navigator = document.CreateNavigator();
            m_nodes.Clear();
            foreach (XPathNavigator iterator in navigator.Select("//" + ConfigNode.NodeName + "[@" + ConfigNode.AttributeName + "]"))
            {
                if (!iterator.IsNode)
                    continue;

                var node = new ConfigNode(( (IHasXmlNode)iterator ).GetNode());

                m_nodes.Add(node);
            }
        }

        public void Save()
        {
            var writer = XmlWriter.Create(FilePath, new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = " ",
                Encoding = Encoding.UTF8
            });

            writer.WriteStartDocument();
            writer.WriteStartElement("Configuration");

            foreach (var node in m_nodes)
            {
                node.Save(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }

        public ConfigNode GetNode(string name)
        {
            var nodes = m_nodes.Where(entry => entry.Name == name).ToArray();

            if (nodes.Length > 1)
                throw new Exception(string.Format("Found {0} nodes with name {1}, a node name must be unique", nodes.Length, name));

            return nodes.SingleOrDefault();
        }

        public T Get<T>(string key)
        {
            var node = GetNode(key);

            if (node == null)
                throw new KeyNotFoundException(string.Format("Node {0} not found", key));

            return node.GetValue<T>();
        }

        public T Get<T>(string key, T defaultValue)
        {
            var node = GetNode(key);

            if (node == null)
            {
                m_nodes.Add(new ConfigNode(key,  defaultValue));

                return defaultValue;
            }

            return node.GetValue<T>();
        }

        public void Set<T>(string key, T value)
        {
            var node = GetNode(key);

            if (node == null)
                throw new KeyNotFoundException(string.Format("Node {0} not found", key));

            node.SetValue(value);
        }

        public void BindAssembly(Assembly assembly)
        {
            m_boundsAssemblies.Add(Tuple.Create(assembly, this));
        }

        public void UnBindAssembly(Assembly assembly)
        {
            m_boundsAssemblies.RemoveAll(entry => entry.Item1 == assembly);
        }

        public static Config GetAssemblyConfig(Assembly assembly)
        {
            var configs = m_boundsAssemblies.Where(entry => entry.Item1 == assembly).ToArray();

            if (configs.Length > 1)
                throw new Exception(string.Format("Found {0} configs bound to assembly {1}", configs.Length, assembly));

            if (configs.Length <= 0)
                throw new Exception(string.Format("No config bound to assembly {0}", assembly));

            return configs.Single().Item2;
        }


        public static Config GetCurrentClassConfig()
        {
            var assembly = Assembly.GetCallingAssembly();

            return GetAssemblyConfig(assembly);
        }

        public static T GetStatic<T>(string key)
        {
            var config = GetAssemblyConfig(Assembly.GetCallingAssembly());
            var node = config.GetNode(key);

            if (node == null)
                throw new KeyNotFoundException(string.Format("Node {0} not found", key));

            return node.GetValue<T>();
        }

        public static T GetStatic<T>(string key, T defaultValue)
        {
            var config = GetAssemblyConfig(Assembly.GetCallingAssembly());
            var node = config.GetNode(key);

            if (node == null)
            {
                config.Nodes.Add(new ConfigNode(key, defaultValue));

                return defaultValue;
            }

            return node.GetValue<T>();
        }

        public static void SetStatic<T>(string key, T value)
        {
            var config = GetAssemblyConfig(Assembly.GetCallingAssembly());
            var node = config.GetNode(key);

            if (node == null)
                throw new KeyNotFoundException(string.Format("Node {0} not found", key));

            node.SetValue(value);
        }
    }
}