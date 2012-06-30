using System.Collections.Generic;
using System.Linq;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pDirectory
    {
        public D2pDirectory(string name)
        {
            Name = name;
            FullName = name;
        }

        public string Name
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        private D2pDirectory m_parent;

        public D2pDirectory Parent
        {
            get { return m_parent; }
            set
            {
                m_parent = value; 
                UpdateFullName();
            }
        }

        private List<D2pEntry> m_entries = new List<D2pEntry>();

        public List<D2pEntry> Entries
        {
            get { return m_entries; }
            set { m_entries = value; }
        }

        private List<D2pDirectory> m_directories = new List<D2pDirectory>();

        public List<D2pDirectory> Directories
        {
            get { return m_directories; }
            set { m_directories = value; }
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        private void UpdateFullName()
        {
            var current = this;
            FullName = current.Name;
            while (current.Parent != null)
            {
                FullName = FullName.Insert(0, current.Parent.Name + "\\");
                current = current.Parent;
            }
        }

        public bool HasDirectory(string directory)
        {
            return m_directories.Any(entry => entry.Name == directory);
        }

        public D2pDirectory TryGetDirectory(string directory)
        {
            return m_directories.SingleOrDefault(entry => entry.Name == directory);
        }

        public bool HasEntry(string entryName)
        {
            return m_entries.Any(entry => entry.FullFileName == entryName);
        }

        public D2pEntry TryGetEntry(string entryName)
        {
            return m_entries.SingleOrDefault(entry => entry.FullFileName == entryName);

        }
    }
}