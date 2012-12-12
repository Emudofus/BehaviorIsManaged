#region License GNU GPL
// I18NDataManager.cs
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
using BiM.Core.Config;
using BiM.Core.I18n;
using BiM.Core.Memory;
using BiM.Core.Reflection;
using BiM.Protocol.Tools;
using NLog;

namespace BiM.Behaviors.Data.I18N
{
    public class I18NDataManager : Singleton<I18NDataManager>
    {
        [Configurable("D2IDelayLoading")]
        public static readonly bool DelayLoading = true;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WeakCollection<I18NString> m_links = new WeakCollection<I18NString>();
        private Dictionary<Languages, D2IFile> m_readers = new Dictionary<Languages, D2IFile>();
        private static Dictionary<string, Languages> m_langsShortcuts = new Dictionary<string, Languages>()
        {
            {"fr", Languages.French},
            {"de", Languages.German},
            {"en", Languages.English},
            {"es", Languages.Spanish},
            {"it", Languages.Italian},
            {"ja", Languages.Japanish},
            {"nl", Languages.Dutsh},
            {"pt", Languages.Portugese},
            {"ru", Languages.Russish},
        };

        Languages m_defaultLanguage;
        public Languages DefaultLanguage
        {
            get
            {
                return m_defaultLanguage;
            }
            set
            {
                m_defaultLanguage = value;
                EnsureLanguageIsLoaded(m_defaultLanguage);
            }
        }

        private void EnsureLanguageIsLoaded(Languages language)
        {
            if (m_readers.ContainsKey(language)) return;
            if (string.IsNullOrEmpty(m_d2IPath)) return; // AddReaders not called yet
            foreach (var d2iFile in Directory.EnumerateFiles(m_d2IPath).Where(entry => entry.EndsWith(".d2i")).Where(path => GetLanguageOfFile(path) == language))
            {
                var reader = new D2IFile(d2iFile);
                AddReader(reader, language);
            }
        }
        private string m_d2IPath;
        public void AddReaders(string directory, bool forceLoading = false)
        {
            m_d2IPath = directory;
            if (!DelayLoading || forceLoading)
                foreach (string d2iFile in Directory.EnumerateFiles(directory).Where(entry => entry.EndsWith(".d2i")))
                {
                    var reader = new D2IFile(d2iFile);

                    AddReader(reader);
                }
        }

        private Languages GetLanguageOfFile(string filePath)
        {
            string file = Path.GetFileNameWithoutExtension(filePath);

            if (!file.Contains("_"))
                throw new Exception(string.Format("Cannot found character '_' in file name {0}, cannot deduce the file lang", file));

            string lang = file.Split('_')[1];

            if (!m_langsShortcuts.ContainsKey(lang.ToLower()))
                throw new Exception(string.Format("Unknown lang symbol {0} in file {1}", lang, file));
            return m_langsShortcuts[lang.ToLower()];
        }

        public void AddReader(D2IFile d2iFile)
        {
            Languages language = GetLanguageOfFile(d2iFile.FilePath);

            AddReader(d2iFile, language);
        }

        public void AddReader(D2IFile d2iFile, Languages language)
        {
            m_readers.Add(language, d2iFile);

            logger.Info("File added : {0}", Path.GetFileName(d2iFile.FilePath));
        }

        public string ReadText(uint id, Languages? lang = null)
        {
            return ReadText((int)id);
        }

        public string ReadText(int id, Languages? lang = null)
        {
            if (lang != null)
            {
                EnsureLanguageIsLoaded(lang.Value);
                return m_readers[lang.Value].GetText(id);
            }

            return m_readers[DefaultLanguage].GetText(id);
        }

        public string ReadText(string id, Languages? lang = null)
        {
            if (lang != null)
            {
                EnsureLanguageIsLoaded(lang.Value);
                return m_readers[lang.Value].GetText(id);
            }

            return m_readers[DefaultLanguage].GetText(id);
        }

        public I18NString GetTextLink(uint id, Languages? lang = null)
        {
            return GetTextLink((int)id);
        }

        public I18NString GetTextLink(int id, Languages? lang = null)
        {
            if (lang != null)
            {
                EnsureLanguageIsLoaded(lang.Value);
            }

            return new I18NString(id, this);
        }

        public I18NString GetTextLink(string id, Languages? lang = null)
        {
            if (lang != null)
            {
                EnsureLanguageIsLoaded(lang.Value);
            }

            return new I18NString(id, this);
        }

        public void ChangeLinksLanguage(Languages old, Languages @new)
        {
            foreach (var link in m_links.Where(x => x.Language == old))
            {
                // text refreshed when the language is changed
                link.Language = @new;
            }
        }

        public void RefreshLinks()
        {
            foreach (var link in m_links)
            {
                link.Refresh();
            }
        }
    }
}