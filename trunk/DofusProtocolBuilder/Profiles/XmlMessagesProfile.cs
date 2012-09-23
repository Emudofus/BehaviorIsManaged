using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.XmlPatterns;

namespace DofusProtocolBuilder.Profiles
{
    public class XmlMessagesProfile : ParsingProfile
    {
        public XmlMessagesProfile()
        {
            BeforeParsingReplacementRules =
                new SerializableDictionary<string, string>
                    {
                        {@"param(\d+)", @"arg$1"},
                        // Sothink -> Trillix variable synthax
                        {@"_loc_(\d+)", @"loc$1"},
                        {@"(var\s|this\.)base(?![\w\d])", @"$1@base"},
                        // add '@' on variable name that are keyword in c#,
                        {@"(var\s|this\.)object(?![\w\d])", @"$1@object"},
                        {@"(var\s|this\.)operator(?![\w\d])", @"$1@operator"},
                        {@"this\.", string.Empty},
                        // delete this, it's useless

                        {@"\bNetworkMessage", @"Message"},
                        // renaming to correspond with DofusProtocol names
                        {@"\bIDataOutput", @"BigEndianWriter"},
                        {@"\bIDataInput", @"BigEndianReader"},
                        {@"\bread(?!y)", @"Read"},
                        // rename read/write methods to a Pascal name
                        {@"\bwrite", @"Write"},
                        {@"(Write|Read)Unsigned([^B])", @"$1U$2"},
                        // convert "unsigned" from read/write methods to "u" like in ReadUInt
                        {@"ReadByte", @"ReadSByte"},
                        // a AS3 byte is signed but the .net byte is not, so we have to convert it to sbyte
                        {@"ReadUnsignedByte", @"ReadByte"},
                        // convert method ReadUnsingedByte to ReadByte (default is a byte unsigned)

                        {@"flash\.geom\.", string.Empty},
                        // delete "flash.geom" 

                        {@"Vector\.([\w_\d]+) = new ([\w_\d]+)();", "$1 = new List<$2>();"},
                        // convert "Vector." to List (C#) (and its props)
                        {@"new Vector\.<([\d\w]+)>\((\d+), (true|false)\)", "new List<$1>($2)"},
                        {@"new Vector\.<([\d\w]+)>", "new List<$1>()"},
                        {@"(__AS3__\.vec\.)?Vector\.", "List"},
                        {@"\.push\(", @".Add("},
                        {@"\.length", @".Count"},
                        {@"super\.", @"base."},
                        // convert super keyword to C# base keyword

                        // add a cast and preffix Enums before each enum
                        {@"\b(\w+)Enum\.", "(byte)Enums.$1Enum."},
                        // another manual fix (for RawDataMessage.as 2.3.5)
                        {@"arg1\.ReadBytes\(content\)", "content = arg1.ReadBytes()"}
                    };
            IgnoredLines =
                new[]
                    {
                        // just a fix of a shit of sothink
                        @".*arguments = [\w\d]+\.ReadUShort.*",
                    };
        }

        public XmlMessage SearchXmlPattern(string classname)
        {
            string[] results = Directory.GetFiles(Path.Combine(Program.Configuration.Output, OutPutPath), classname + ".xml", SearchOption.AllDirectories);

            if (results.Length != 1)
                return null;

            var deserializer = new XmlSerializer(typeof (XmlMessage));

            return (XmlMessage) deserializer.Deserialize(XmlReader.Create(results[0]));
        }

        public override void ExecuteProfile(Parser parser)
        {
            string relativePath = GetRelativePath(parser.Filename);

            string xmlfile = Path.Combine(Program.Configuration.Output, OutPutPath, relativePath, Path.GetFileNameWithoutExtension(parser.Filename)) + ".xml";

            var builder = new XmlMessageBuilder(parser);

            XmlWriter writer = XmlWriter.Create(xmlfile, new XmlWriterSettings
                                                             {
                                                                 OmitXmlDeclaration = true,
                                                                 Indent = true,
                                                                 IndentChars = "\t",
                                                                 NamespaceHandling = NamespaceHandling.OmitDuplicates,
                                                             });
            builder.WriteToXml(writer);
            writer.Close();

            Console.WriteLine("Wrote {0}", xmlfile);
        }
    }
}