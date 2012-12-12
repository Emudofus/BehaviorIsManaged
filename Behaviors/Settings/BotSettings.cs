﻿#region License GNU GPL
// BotSettings.cs
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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using BiM.Core.Config;

namespace BiM.Behaviors.Settings
{
    /// <summary>
    /// Settings related to a bot instance
    /// </summary>
    public class BotSettings
    {
        public const string NodeName = "Settings";
        public const string AttributeName = "name";


        private bool m_loaded;
        private List<SettingsEntry> m_entries = new List<SettingsEntry>();
        private List<XmlNode> m_loadedNodes = new List<XmlNode>(); 

        public BotSettings(string path)
        {
            FilePath = path;
        }

        public string FilePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the settings entry or create it with default constructor if it does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOrAddEntry<T>()
            where T : SettingsEntry, new()
        {
            var entries = m_entries.OfType<T>().ToArray();

            if (entries.Length > 1)
                throw new Exception(string.Format("Found {0} settings entries of type {1}, 1 or 0 expected", entries.Length, typeof(T)));

            if (entries.Length == 0)
            {
                var entry = new T();
                var node = GetEntryNode(entry.EntryName);

                if (node != null)
                {
                    var deserializedEntry = (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(node.InnerXml));

                    m_entries.Add(deserializedEntry);
                    m_loadedNodes.Remove(node);

                    return deserializedEntry;
                }

                m_entries.Add(entry);
                return entry;
            }

            return entries.Single();
        }

        public bool EntryExists(Type entryType)
        {
            return m_entries.Any(entryType.IsInstanceOfType);
        }

        public bool EntryExists<T>()
        {
            return m_entries.Any(x => x is T);
        }

        /// <summary>
        /// Returns false if the entry already exists
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool AddEntry(SettingsEntry entry)
        {
            var type = entry.GetType();
            if (EntryExists(type))
                return false;

           m_entries.Add(entry);
           return true;
        }

        public bool AddOrSetEntry(SettingsEntry entry)
        {
             var type = entry.GetType();
            if (EntryExists(type))
                if (!RemoveEntry(type))
                    throw new Exception(string.Format("Entry {0} already exists but cannot be removed for an unknow reason", type));


            m_entries.Add(entry);
            return true;
        }

        public bool RemoveEntry(Type entryType)
        {
            return m_entries.RemoveAll(entryType.IsInstanceOfType) > 0;
        }

        public bool RemoveEntry<T>()
        {
            return m_entries.RemoveAll(x => x is T) > 0;
        }

        private XmlNode GetEntryNode(string name)
        {
            return m_loadedNodes.SingleOrDefault(entry => entry.Attributes[AttributeName].Value == name);
        }

        public void Load()
        {
            if (m_loaded)
                return;

            if(!File.Exists(FilePath))
            {
                m_loaded = true;
                return;
            }

            var document = new XmlDocument();
            document.Load(FilePath);

            var navigator = document.CreateNavigator();
            foreach (XPathNavigator iterator in navigator.Select("//" + NodeName + "[@" + AttributeName + "]"))
            {
                if (!iterator.IsNode)
                    continue;

                var xmlNode = ( (IHasXmlNode)iterator ).GetNode();

                if (xmlNode.Attributes == null)
                    throw new Exception("xmlNode.Attributes == null");

                if (GetEntryNode(xmlNode.Attributes[AttributeName].Value) != null)
                    throw new Exception(string.Format("Found at least 2 entries with name {0}. Expected 1 or 0. File {1} corrupted",
                        xmlNode.Attributes[AttributeName].Value, FilePath));

                m_loadedNodes.Add(xmlNode);
            }

            m_loaded = true;
        }

        public void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            var writer = XmlWriter.Create(FilePath, new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = " ",
                Encoding = Encoding.UTF8
            });

            writer.WriteStartDocument();
            writer.WriteStartElement("BotSettings");

            foreach (var entry in m_entries)
            {
                writer.WriteStartElement(NodeName);
                writer.WriteAttributeString(AttributeName, entry.EntryName);
                new XmlSerializer(entry.GetType()).Serialize(writer, entry);
                writer.WriteEndElement();
            }

            foreach (var node in m_loadedNodes)
            {
                writer.WriteNode(node.CreateNavigator(), true);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }
    }
}