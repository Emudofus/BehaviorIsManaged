using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using System.Xml.Serialization;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Profiles;
using DofusProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;


namespace DofusProtocolBuilder
{
    public class Program
    {
        public static Configuration Configuration = new Configuration();

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var serializer = new XmlSerializer(typeof(Configuration));
            Configuration.SetDefault();

            string configPath = "./config.xml";
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-config":
                        if (args.Length < i + 2)
                            Shutdown("Value of -config is not defined like : -config {configPath}");

                        configPath = args[i + 1];
                        break;
                    case "-createconfig":
                        var writer = XmlWriter.Create(configPath, new XmlWriterSettings()
                        {
                            Indent = true
                        });
                        serializer.Serialize(writer, Configuration);
                        writer.Close();
                        Shutdown("Config created. Please restart");
                        break;
                }
            }

            if (!File.Exists(configPath))
            {
                var writer = XmlWriter.Create(configPath, new XmlWriterSettings()
                                                              {
                                                                  Indent = true
                                                              });
                serializer.Serialize(writer, Configuration);
                writer.Close();
                Shutdown("Config created. Please restart");
            }
            else
            {
                var reader = XmlReader.Create(configPath, new XmlReaderSettings());
                Configuration = serializer.Deserialize(reader) as Configuration;
                reader.Close();
            }

            var profiles =
            new ParsingProfile[]
                {
                    Configuration.XmlMessagesProfile,
                    Configuration.XmlTypesProfile,
                    Configuration.MessagesProfile,
                    Configuration.TypesProfile,
                    Configuration.DatacenterProfile,
                    Configuration.EnumsProfile,
                };

            foreach (ParsingProfile parsingProfile in profiles)
            {
                Console.WriteLine("Executing profile \'{0}\' ... ", parsingProfile.Name);

                if (parsingProfile.OutPutNamespace != null)
                    parsingProfile.OutPutNamespace = parsingProfile.OutPutNamespace.Insert(0, Configuration.BaseNamespace);

                if (!Directory.Exists(Configuration.Output))
                    Directory.CreateDirectory(Configuration.Output);

                
                if (Directory.Exists(Path.Combine(Configuration.Output, parsingProfile.OutPutPath)))
                {
                    DeleteDirectory(Path.Combine(Configuration.Output, parsingProfile.OutPutPath));
                }

                Directory.CreateDirectory(Path.Combine(Configuration.Output, parsingProfile.OutPutPath));

                IEnumerable<string> files = Directory.EnumerateFiles(
                    Path.Combine(Configuration.SourcePath, parsingProfile.SourcePath), "*.as",
                    SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string relativePath = parsingProfile.GetRelativePath(file);

                    if (!Directory.Exists(Path.Combine(Configuration.Output, parsingProfile.OutPutPath, relativePath)))
                        Directory.CreateDirectory(Path.Combine(Configuration.Output, parsingProfile.OutPutPath, relativePath));

                    var parser = new Parser(file, parsingProfile.BeforeParsingReplacementRules,
                                            parsingProfile.IgnoredLines)
                        {IgnoreMethods = parsingProfile.IgnoreMethods};

                    try
                    {
                        if (parsingProfile.EnableParsing)
                            parser.ParseFile();
                    }
                    catch (InvalidCodeFileException)
                    {
                        Console.WriteLine("File {0} not parsed correctly", Path.GetFileName(file));
                        continue;
                    }

                    parsingProfile.ExecuteProfile(parser);
                }

                Console.WriteLine("Done !");
            }
        }

        private static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Shutdown("Unhandled Exception : " + e.ExceptionObject);
        }

        public static void Shutdown(string reason = "")
        {
            Console.WriteLine("The program is shutting down {0}", (reason != "" ? ": " + reason : ""));

            Console.WriteLine("Press any key to exit...");
            Console.Read();

            Environment.Exit(0);
        }
    }
}