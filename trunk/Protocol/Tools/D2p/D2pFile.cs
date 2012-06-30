using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.D2p
{
    public class D2pFile : INotifyPropertyChanged, IDisposable
    {
        public event Action<D2pFile, int> ExtractPercentProgress;

        private void OnExtractPercentProgress(int percent)
        {
            Action<D2pFile, int> handler = ExtractPercentProgress;
            if (handler != null) handler(this, percent);
        }

        private readonly Dictionary<string, D2pEntry> m_entries = new Dictionary<string, D2pEntry>();
        private readonly List<D2pFile> m_links = new List<D2pFile>();

        private readonly Queue<D2pFile> m_linksToSave = new Queue<D2pFile>();
        private readonly List<D2pProperty> m_properties = new List<D2pProperty>();

        private readonly List<D2pDirectory>  m_rootDirectories = new List<D2pDirectory>();

        private bool m_isDisposed;
        private BigEndianReader m_reader;

        public D2pFile()
        {
            IndexTable = new D2pIndexTable(this);
            m_reader = new BigEndianReader(new byte[0]);
        }

        public D2pFile(string file)
        {
            FilePath = file;
            m_reader = new BigEndianReader(File.OpenRead(file));

            InternalOpen();
        }

        public Stream Stream
        {
            get { return m_reader != null ? m_reader.BaseStream : null; }
        }

        public D2pIndexTable IndexTable
        {
            get;
            private set;
        }

        public ReadOnlyCollection<D2pProperty> Properties
        {
            get { return m_properties.AsReadOnly(); }
        }

        public IEnumerable<D2pEntry> Entries
        {
            get { return m_entries.Values; }
        }

        public ReadOnlyCollection<D2pFile> Links
        {
            get { return m_links.AsReadOnly(); }
        }

        public ReadOnlyCollection<D2pDirectory> RootDirectories
        {
            get { return m_rootDirectories.AsReadOnly(); }
        }

        public string FilePath
        {
            get;
            private set;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_isDisposed)
                return;

            m_isDisposed = true;

            if (m_reader != null)
                m_reader.Dispose();

            if (m_links != null)
            foreach (var link in m_links)
            {
                link.Dispose();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public bool HasFilePath()
        {
            return !string.IsNullOrEmpty(FilePath);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propd = PropertyChanged;
            if (propd != null)
                propd(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Initialize

        public D2pEntry this[string fileName]
        {
            get { return m_entries[fileName]; }
        }

        private void InternalOpen()
        {
            if (!Stream.CanSeek)
                m_reader = m_reader.ReadBytesInNewBigEndianReader((int) m_reader.BytesAvailable);

            if (m_reader.ReadByte() != 2 || m_reader.ReadByte() != 1)
                throw new FileLoadException("Corrupted d2p header");

            ReadTable();
            ReadProperties();
            ReadEntriesDefinitions();
        }

        private void ReadTable()
        {
            m_reader.Seek(D2pIndexTable.TableOffset, D2pIndexTable.TableSeekOrigin);
            IndexTable = new D2pIndexTable(this);
            IndexTable.ReadTable(m_reader);
        }

        private void ReadProperties()
        {
            m_reader.Seek(IndexTable.PropertiesOffset, SeekOrigin.Begin);
            for (int i = 0; i < IndexTable.PropertiesCount; i++)
            {
                var property = new D2pProperty();
                property.ReadProperty(m_reader);

                if (property.Key == "link")
                {
                    InternalAddLink(property.Value);
                }

                m_properties.Add(property);
            }
        }

        private void ReadEntriesDefinitions()
        {
            m_reader.Seek(IndexTable.EntriesDefinitionOffset, SeekOrigin.Begin);
            for (int i = 0; i < IndexTable.EntriesCount; i++)
            {
                var entry = D2pEntry.CreateEntryDefinition(this, m_reader);

                InternalAddEntry(entry);
            }
        }

        public void AddProperty(D2pProperty property)
        {
            if (property.Key == "link")
            {
                InternalAddLink(property.Value);
            }

            InternalAddProperty(property);
        }

        public bool RemoveProperty(D2pProperty property)
        {
            if (property.Key == "link")
            {
                D2pFile link = m_links.FirstOrDefault(entry =>
                                                      Path.GetFullPath(GetLinkFileAbsolutePath(property.Value)) ==
                                                      Path.GetFullPath(entry.FilePath));

                if (link == null || !InternalRemoveLink(link))
                    throw new Exception(string.Format("Cannot remove the associated link {0} to this property",
                                                      property.Value));
            }

            if (m_properties.Remove(property))
            {
                OnPropertyChanged("Properties");
                IndexTable.PropertiesCount--;
                return true;
            }

            return false;
        }

        private void InternalAddProperty(D2pProperty property)
        {
            m_properties.Add(property);
            OnPropertyChanged("Properties");
            IndexTable.PropertiesCount++;
        }

        public void AddLink(string linkFile)
        {
            InternalAddLink(linkFile);
            InternalAddProperty(new D2pProperty
                                    {
                                        Key = "link",
                                        Value = linkFile
                                    });
        }

        private void InternalAddLink(string linkFile)
        {
            string path = GetLinkFileAbsolutePath(linkFile);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(linkFile);
            }

            var link = new D2pFile(path);
            foreach (D2pEntry entry in link.Entries)
            {
                InternalAddEntry(entry);
            }

            m_links.Add(link);
            OnPropertyChanged("Links");
        }

        private string GetLinkFileAbsolutePath(string linkFile)
        {
            if (File.Exists(linkFile))
            {
                return linkFile;
            }

            if (HasFilePath())
            {
                string absolutePath = Path.Combine(Path.GetDirectoryName(FilePath), linkFile);

                if (File.Exists(absolutePath))
                {
                    return absolutePath;
                }
            }

            return linkFile;
        }

        public bool RemoveLink(D2pFile file)
        {
            D2pProperty property =
                m_properties.FirstOrDefault(entry =>
                                            Path.GetFullPath(GetLinkFileAbsolutePath(entry.Value)) ==
                                            Path.GetFullPath(file.FilePath));

            if (property == null)
                return false;

            bool result = InternalRemoveLink(file) && m_properties.Remove(property);

            if (result)
                OnPropertyChanged("Properties");

            return result;
        }

        private bool InternalRemoveLink(D2pFile link)
        {
            if (m_links.Remove(link))
            {
                OnPropertyChanged("Links");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Ignore entries of linked files
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public D2pEntry[] GetEntriesOfInstanceOnly()
        {
            return m_entries.Values.Where(entry => entry.Container == this).ToArray();
        }

        public D2pEntry GetEntry(string fileName)
        {
            return m_entries[fileName];
        }

        public D2pEntry TryGetEntry(string fileName)
        {
            D2pEntry entry;
            if (m_entries.TryGetValue(fileName, out entry))
                return entry;

            return null;
        }

        public string[] GetFilesName()
        {
            return m_entries.Keys.ToArray();
        }

        public void AddEntry(D2pEntry entry)
        {
            entry.State = D2pEntryState.Added;
            InternalAddEntry(entry);
            IndexTable.EntriesCount++;
            OnPropertyChanged("Entries");
        }

        private void InternalAddEntry(D2pEntry entry)
        {
            m_entries.Add(entry.FullFileName, entry);
            InternalAddDirectories(entry);
        }

        private void InternalAddDirectories(D2pEntry entry)
        {
            var directories = entry.GetDirectoriesName();

            if (directories.Length == 0)
                return;

            D2pDirectory current = null;
            if (!HasDirectory(directories[0]))
            {
                current = new D2pDirectory(directories[0]);

                m_rootDirectories.Add(current);
            }
            else
            {
                current = TryGetDirectory(directories[0]);
            }

            current.Entries.Add(entry);

            foreach (var directory in directories.Skip(1))
            {
                if (!current.HasDirectory(directory))
                {
                    var dir = new D2pDirectory(directory)
                    {
                        Parent = current
                    };
                    current.Directories.Add(dir);

                    current = dir;
                }
                else
                {
                    current = current.TryGetDirectory(directory);
                }

                current.Entries.Add(entry);
            }

            entry.Directory = current;
        }

        public bool RemoveEntry(D2pEntry entry)
        {
            if (entry.Container != this)
            {
                if (!entry.Container.RemoveEntry(entry))
                    return false;

                if (!m_linksToSave.Contains(entry.Container))
                    m_linksToSave.Enqueue(entry.Container);
            }

            if (m_entries.Remove(entry.FullFileName))
            {
                entry.State = D2pEntryState.Removed;
                InternalRemoveDirectories(entry);
                OnPropertyChanged("Entries");

                if (entry.Container == this)
                    IndexTable.EntriesCount--;

                return true;
            }

            return false;
        }

        private void InternalRemoveDirectories(D2pEntry entry)
        {
            D2pDirectory current = entry.Directory;
            while (current != null)
            {
                current.Entries.Remove(entry);

                if (current.Parent != null && current.Entries.Count == 0)
                    current.Parent.Directories.Remove(current);
                else if (current.IsRoot && current.Entries.Count == 0)
                    m_rootDirectories.Remove(current);

                current = current.Parent;
            }
        }

        #endregion

        #region Read

        public bool Exists(string fileName)
        {
            return m_entries.ContainsKey(fileName);
        }

        public Dictionary<D2pEntry, byte[]> ReadAllFiles()
        {
            var result = new Dictionary<D2pEntry, byte[]>();

            foreach (var entry in m_entries)
            {
                result.Add(entry.Value, ReadFile(entry.Value));
            }

            return result;
        }

        public byte[] ReadFile(D2pEntry entry)
        {
            if (entry.Container != this)
                return entry.Container.ReadFile(entry);

            if (entry.Index >= 0 && IndexTable.OffsetBase + entry.Index >= 0)
                m_reader.Seek(IndexTable.OffsetBase + entry.Index, SeekOrigin.Begin);

            byte[] data = entry.ReadEntry(m_reader);

            return data;
        }

        public byte[] ReadFile(string fileName)
        {
            if (!Exists(fileName))
                throw new FileNotFoundException(fileName);

            D2pEntry entry = GetEntry(fileName);

            return ReadFile(entry);
        }

        public void ExtractFile(string fileName, bool overwrite = false)
        {                          
            if (!Exists(fileName))
                throw new FileNotFoundException(fileName);

            D2pEntry entry = GetEntry(fileName);

            var dest = Path.Combine("./", entry.FullFileName);
                
            if (!Directory.Exists(Path.GetDirectoryName(dest)))
                Directory.CreateDirectory(dest);

            ExtractFile(fileName, dest, overwrite);
        }

        public void ExtractFile(string fileName, string destination, bool overwrite = false)
        {
            byte[] bytes = ReadFile(fileName);
            FileAttributes attr = File.GetAttributes(destination);

            if (( attr & FileAttributes.Directory ) == FileAttributes.Directory)
            {
                destination = Path.Combine(destination, Path.GetFileName(fileName));
            }

            if (File.Exists(destination) && !overwrite)
                throw new InvalidOperationException(string.Format("Cannot overwrite {0}", destination));

            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                Directory.CreateDirectory(Path.GetDirectoryName(destination));

            File.WriteAllBytes(destination, bytes);
        }

        public void ExtractDirectory(string directoryName, string destination)
        {
            if (!HasDirectory(directoryName))
                throw new InvalidOperationException(string.Format("Directory {0} does not exist", directoryName));

            var directory = TryGetDirectory(directoryName);

            if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(destination, directory.FullName))))
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(destination, directory.FullName)));

            foreach (var entry in directory.Entries)
            {
                ExtractFile(entry.FullFileName, Path.Combine(destination, entry.FullFileName));
            }

            foreach (D2pDirectory pDirectory in directory.Directories)
            {
                ExtractDirectory(pDirectory.FullName, destination);
            }
        }

        public void ExtractAllFiles(string destination, bool overwrite = false, bool progress = false)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destination)))
                Directory.CreateDirectory(destination);

            //create dirs
            foreach (var dir in m_entries.Select(entry => entry.Value.GetDirectoriesName()).Distinct())
            {
                var dest = Path.Combine(Path.GetFullPath(destination), Path.Combine(dir));

                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
            }

            double i = 0;
            int progressPercent = 0;
            foreach (var entry in m_entries)
            {
                if (File.Exists(Path.GetFullPath(destination)) && !overwrite)
                    throw new InvalidOperationException(string.Format("Cannot overwrite {0}", destination));

                var dest = Path.Combine(Path.GetFullPath(destination), entry.Value.FullFileName);

                File.WriteAllBytes(dest, ReadFile(entry.Value));
                i++;

                if (progress)
                {
                    if ((int) (( i / m_entries.Count ) * 100) != progressPercent)
                        OnExtractPercentProgress(progressPercent = (int) (( i / m_entries.Count ) * 100));
                }
            }
        }

        #endregion

        #region Write

        public D2pEntry AddFile(string file)
        {
            byte[] bytes = File.ReadAllBytes(file);

            var dest = file;

            if (HasFilePath())
                dest = GetRelativePath(file, Path.GetDirectoryName(FilePath));

            return AddFile(dest, bytes);
        }

        public D2pEntry AddFile(string fileName, byte[] data)
        {
            var entry = new D2pEntry(this, fileName, data);

            AddEntry(entry);

            return entry;
        }

        public bool RemoveFile(string file)
        {
            D2pEntry entry = TryGetEntry(file);

            return entry != null && RemoveEntry(entry);
        }

        public bool ModifyFile(string file, byte[] data)
        {
            D2pEntry entry = TryGetEntry(file);

            if (entry == null)
                return false;

            entry.ModifyEntry(data);

            if (entry.Container != this &&
                !m_linksToSave.Contains(entry.Container))
            {
                m_linksToSave.Enqueue(entry.Container);
            }

            return true;
        }

        private string GetRelativePath(string file, string directory)
        {
            var uri = new Uri(Path.GetFullPath(file));
            var currentUri = new Uri(Path.GetFullPath(directory));

            return currentUri.MakeRelativeUri(uri).ToString();
        }

        public void Save()
        {
            if (HasFilePath())
                SaveAs(FilePath);
            else
            {
                throw new InvalidOperationException("Cannot perform Save : No path defined, use SaveAs instead");
            }
        }

        public void SaveAs(string destination, bool overwrite = true)
        {
            // save the links before
            while (m_linksToSave.Count > 0)
            {
                D2pFile link = m_linksToSave.Dequeue();

                // theorically the path is defined
                link.Save();
            }

            Stream stream;
            if (!File.Exists(destination))
                stream = File.Create(destination);
            else if (!overwrite)
                throw new InvalidOperationException("Cannot perform SaveAs : file already exist, notify overwrite parameter to true");
            else
                stream = File.OpenWrite(destination);

            using (var writer = new BigEndianWriter(stream))
            {
                // header
                writer.WriteByte(2);
                writer.WriteByte(1);

                D2pEntry[] entries = GetEntriesOfInstanceOnly();
                // avoid the header
                int offset = 2;

                foreach (D2pEntry entry in entries)
                {
                    byte[] data = ReadFile(entry);

                    entry.Index = (int) writer.Position - offset;
                    writer.WriteBytes(data);
                }

                var entriesDefOffset = (int) writer.Position;

                foreach (D2pEntry entry in entries)
                {
                    entry.WriteEntryDefinition(writer);
                }

                var propertiesOffset = (int) writer.Position;

                foreach (D2pProperty property in m_properties)
                {
                    property.WriteProperty(writer);
                }

                IndexTable.OffsetBase = offset;
                IndexTable.EntriesCount = entries.Length;
                IndexTable.EntriesDefinitionOffset = entriesDefOffset;
                IndexTable.PropertiesCount = m_properties.Count;
                IndexTable.PropertiesOffset = propertiesOffset;
                IndexTable.Size = IndexTable.EntriesDefinitionOffset - IndexTable.OffsetBase;

                IndexTable.WriteTable(writer);
            }
        }

        #endregion

        #region Explore
        public bool HasDirectory(string directory)
        {
            var directoriesName = directory.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (directoriesName.Length == 0)
                return false;

            var current = m_rootDirectories.SingleOrDefault(entry => entry.Name == directoriesName[0]);

            if (current == null)
                return false;

            foreach (var dir in directoriesName.Skip(1))
            {
                if (!current.HasDirectory(dir))
                    return false;

                current = current.TryGetDirectory(dir);
            }

            return true;
        }

        public D2pDirectory TryGetDirectory(string directory)
        {
            var directoriesName = directory.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (directoriesName.Length == 0)
                return null;

            var current = m_rootDirectories.SingleOrDefault(entry => entry.Name == directoriesName[0]);

            if (current == null)
                return null;

            foreach (var dir in directoriesName.Skip(1))
            {
                if (!current.HasDirectory(dir))
                    return null;

                current = current.TryGetDirectory(dir);
            }

            return current;
        }

        public D2pDirectory[] GetDirectoriesTree(string directory)
        {
            var result = new List<D2pDirectory>();
            var directoriesName = directory.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (directoriesName.Length == 0)
                return new D2pDirectory[0];

            var current = m_rootDirectories.SingleOrDefault(entry => entry.Name == directoriesName[0]);

            if (current == null)
                return new D2pDirectory[0];

            result.Add(current);

            foreach (var dir in directoriesName.Skip(1))
            {
                if (!current.HasDirectory(dir))
                    return result.ToArray();

                current = current.TryGetDirectory(dir);
                result.Add(current);
            }

            return result.ToArray();
        }


        #endregion
    }
}