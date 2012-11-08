#region License GNU GPL
// Config.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using BiM.Core.Extensions;

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

        public void RegisterAttributes()
        {
            RegisterAttributes(Assembly.GetCallingAssembly());
        }

        public void RegisterAttributes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var field in type.GetFields())
                {
                    var attr = field.GetCustomAttribute<ConfigurableAttribute>();

                    if (attr == null)
                        continue;

                    if (Exists(attr.Name))
                        throw new Exception(string.Format("Node with name {0} already used, a node name must be unique", attr.Name));

                    m_nodes.Add(new ConfigVariable(attr, field));
                }

                foreach (var property in type.GetProperties())
                {
                    var attr = property.GetCustomAttribute<ConfigurableAttribute>();

                    if (attr == null)
                        continue; 
                    
                    if (Exists(attr.Name))
                        throw new Exception(string.Format("Node with name {0} already used, a node name must be unique", attr.Name));



                    m_nodes.Add(new ConfigVariable(attr, property));
                }
            }
        }

        public void Load()
        {
            if (!File.Exists(FilePath))
                Save(); // create the config file

            var document = new XmlDocument();
            document.Load(FilePath);

            var navigator = document.CreateNavigator();
            var listedNames = new List<string>();
            foreach (XPathNavigator iterator in navigator.Select("//" + ConfigNode.NodeName + "[@" + ConfigNode.AttributeName + "]"))
            {
                if (!iterator.IsNode)
                    continue;

                var xmlNode = ( (IHasXmlNode)iterator ).GetNode();
                var name = ConfigNode.GetNodeName(xmlNode);
                var node = GetNode(name);

                if (listedNames.Contains(name))
                    throw new Exception(string.Format("Node with name {0} already used, a node name must be unique", name));

                // if the noad already exist we set the value
                if (node != null)
                {
                    node.Load(xmlNode);
                }
                else
                {
                    node = new ConfigNode(xmlNode);
                    m_nodes.Add(node);
                }

                listedNames.Add(name);
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

        public bool Exists(string nodeName)
        {
            return m_nodes.Any(entry => entry.Name == nodeName);
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

            return (T)node.GetValue(typeof(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            var node = GetNode(key);

            if (node == null)
            {
                m_nodes.Add(new ConfigNode(key,  defaultValue));

                return defaultValue;
            }

            return (T)node.GetValue(typeof(T));
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

            return config.Get<T>(key);
        }

        public static T GetStatic<T>(string key, T defaultValue)
        {
            var config = GetAssemblyConfig(Assembly.GetCallingAssembly());

            return config.Get<T>(key, defaultValue);
        }

        public static void SetStatic<T>(string key, T value)
        {
            var config = GetAssemblyConfig(Assembly.GetCallingAssembly());
            config.Set(key, value);
        }
    }
}