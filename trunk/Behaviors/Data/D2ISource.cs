using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BiM.Core.I18n;
using BiM.Protocol.Data;
using BiM.Protocol.Tools;
using NLog;

namespace BiM.Behaviors.Data
{
    public class D2ISource : IDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

        public D2ISource(Languages defaultLanguage)
        {
            DefaultLanguage = defaultLanguage;
        }

        public Languages DefaultLanguage
        {
            get;
            set;
        }

        private Dictionary<Languages, I18NFile> m_readers = new Dictionary<Languages, I18NFile>(); 

        public void AddReaders(string directory)
        {
            foreach (var d2iFile in Directory.EnumerateFiles(directory).Where(entry => entry.EndsWith(".d2i")))
            {
                var reader = new I18NFile(d2iFile);

                AddReader(reader);
            }
        }

        public void AddReader(I18NFile d2iFile)
        {
            var file = Path.GetFileNameWithoutExtension(d2iFile.FilePath);

            if (!file.Contains("_"))
                throw new Exception(string.Format("Cannot found character '_' in file name {0}, cannot deduce the file lang", file));

            var lang = file.Split('_')[1];

            if (!m_langsShortcuts.ContainsKey(lang.ToLower()))
                throw new Exception(string.Format("Unknown lang symbol {0} in file {1}", lang, file));

            AddReader(d2iFile, m_langsShortcuts[lang.ToLower()]);
        }

        public void AddReader(I18NFile d2iFile, Languages language)
        {
            m_readers.Add(language, d2iFile);

            logger.Info("File added : {0}", Path.GetFileName(d2iFile.FilePath));
        }

        public bool DoesHandleType(Type type)
        {
            return type == typeof(string);
        }

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (keys.Length < 0 || keys.Length > 2 || (!( keys[0] is IConvertible)) || (keys.Length == 2 && !(keys[1] is Languages)) )
                throw new ArgumentException("D2OSource needs a int key or a string key, use ReadObject(int) or ReadObject(string)." +
                    "You can also specify a language ReadObject(int, Languages) or ReadObject(string, Languages)");

            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            var language = keys.Length == 2 ? (Languages)keys[1] : DefaultLanguage;

            if (keys[0] is string)
            {
                string key = keys[0] as string;

                if (!m_readers.ContainsKey(language))
                    throw new Exception(string.Format("I18nFile for {0} is not found", language));

                return m_readers[language].GetText(key) as T;
            }
            else
            {
                int key = Convert.ToInt32(keys[0]);

                if (!m_readers.ContainsKey(language))
                    throw new Exception(string.Format("I18nFile for {0} is not found", language));

                return m_readers[language].GetText(key) as T;

            }
        } 
    }
}