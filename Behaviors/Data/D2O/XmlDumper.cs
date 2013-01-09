using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using BiM.Behaviors.Data.I18N;
using BiM.Behaviors.Game.Effects;
using BiM.Behaviors.Game.Spells.Shapes;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Tools;

namespace BiM.Behaviors.Data.D2O
{
    static public class XmlDumper
    {
        static Assembly asm = Assembly.GetAssembly(typeof(D2OClassAttribute));
        static void cleanXML(string xmlFileName, bool spells)
        {

            try
            {
                string contents;
                using (StreamReader streamReader = File.OpenText(xmlFileName))
                    // Now, read the entire file into a strin
                    contents = streamReader.ReadToEnd();

                // Write the modification into the same file
                using (StreamWriter streamWriter = File.CreateText(xmlFileName))
                {
                    string result = new StringBuilder(contents).Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").ToString();
                    string pattern1 = @"<descriptionId>(\d+)</descriptionId>"; // replace descriptionId by corresponding string
                    string pattern2 = @"<nameId>(\d+)</nameId>"; // replace nameId by corresponding string
                    string pattern3 = @"<spellLevels>.+</spellLevels>"; // remove full spellLevels struct
                    string pattern4 = @"<targetId>(\d+)</targetId>"; // replace targetId by corresponding enum
                    string pattern5 = @"<rawZone>(.+)</rawZone>"; // complete targetId with corresponding surface
                    string pattern6 = @"<effectId>(\d+)</effectId>"; // complete effectId with corresponding surface
                    string pattern7 = @"<longNameId>(\d+)</longNameId>"; // replace longNameId by corresponding string
                    string pattern8 = @"<shortNameId>(\d+)</shortNameId>"; // replace shortNameId by corresponding string
                    result = Regex.Replace(
                                Regex.Replace(
                                    Regex.Replace(
                                        Regex.Replace(result,
                                        pattern1, match => "<description>" + I18NDataManager.Instance.ReadText(int.Parse(match.Groups[1].Value)) + "</description>"),
                                        pattern7, match => "<longName>" + I18NDataManager.Instance.ReadText(int.Parse(match.Groups[1].Value)) + "</longName>"),
                                        pattern8, match => "<shortName>" + I18NDataManager.Instance.ReadText(int.Parse(match.Groups[1].Value)) + "</shortName>"),
                                        pattern2, match => "<name>" + I18NDataManager.Instance.ReadText(int.Parse(match.Groups[1].Value)) + "</name>");
                    if (spells)
                    {
                        result =
                            Regex.Replace(
                                Regex.Replace(
                                Regex.Replace(
                                Regex.Replace(result, pattern3, ""),
                                    pattern4, match => "<targetId>" + ((SpellTargetType)int.Parse(match.Groups[1].Value)).ToString() + "</targetId>"),
                                    pattern5, match => "<rawZone>" + string.Format("{0} ({1} cells)", match.Groups[1].Value, new Zone(match.Groups[1].Value).Surface) + "</rawZone>"),
                                    pattern6, match =>
                                        {
                                            int effectId = int.Parse(match.Groups[1].Value);
                                            Effect template = ObjectDataManager.Instance.Get<Effect>(effectId);
                                            return "<effectId>" + ((EffectsEnum)effectId).ToString() + "</effectId>" + " <description>" + I18NDataManager.Instance.ReadText(template.descriptionId) + "</description>";
                                        });
                    }
                    streamWriter.Write(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static public bool SimpleDumper(string XMLFileName, Type type)
        {
            using (XmlTextWriter writer = new XmlTextWriter(XMLFileName, null))
            {
                Type[] types = { typeof(EffectInstanceDice) };// asm.GetTypes().Where(entry => entry.Namespace != null && entry.GetConstructor(System.Type.EmptyTypes) != null && entry.Namespace.StartsWith("BiM.Protocol.Data")).ToArray<Type>();
                try
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");

                    XmlSerializer d2oSerializer = new XmlSerializer(type, types);
                    writer.WriteStartElement(type.Name + "_root");
                    // write
                    //using (var stream = File.Create(XMLFileName))
                    {
                        foreach (object obj in ObjectDataManager.Instance.EnumerateObjects(type))
                        {
                            d2oSerializer.Serialize(writer, obj); // your instance
                        }
                    }
                    writer.WriteEndElement();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            // Open a file for reading
            cleanXML(XMLFileName, false);
            return true;
        }

        static public bool DumpAll()
        {
            Directory.CreateDirectory("XML");
            foreach (Type type in ObjectDataManager.Instance.GetAllTypes())
            {
                SimpleDumper("XML/" + type.Name + ".xml", type);
            }
            return true;
        }

        static public bool SpellsDumper(string XMLFileName)
        {
            Directory.CreateDirectory("XML/Spells");
            foreach (SpellType type in ObjectDataManager.Instance.EnumerateObjects<SpellType>())
                    
            //foreach (BreedEnum breed in Enum.GetValues(typeof(BreedEnum)))
            {
                string typeName = I18NDataManager.Instance.ReadText(type.longNameId);
                string breedFileName = "XML/Spells/"+ typeName + "_" + XMLFileName;
                using (XmlTextWriter writer = new XmlTextWriter(breedFileName, null))
                {
                    Type D2oClass = typeof(Spell);
                    Type[] types = { typeof(SpellLevel), typeof(EffectInstanceDice) };// asm.GetTypes().Where(entry => entry.Namespace != null && entry.GetConstructor(System.Type.EmptyTypes) != null && entry.Namespace.StartsWith("BiM.Protocol.Data")).ToArray<Type>();
                    XmlSerializer d2oSerializer = new XmlSerializer(typeof(Spell), types);
                    XmlSerializer d2oSerializerLv = new XmlSerializer(typeof(SpellLevel), types);

                    //Use indenting for readability.
                    writer.Formatting = Formatting.Indented;

                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                    writer.WriteStartElement("Spells_root");

                    // write
                    foreach (Spell obj in ObjectDataManager.Instance.EnumerateObjects<Spell>())
                        if (obj.spellLevels.Count == 0 || obj.typeId == (uint)type.id)
                        {
                            //writer.WriteStartElement("Spell");
                            //writer.WriteAttributeString("Name", I18NDataManager.Instance.ReadText(obj.nameId));
                            //writer.WriteAttributeString("Description", I18NDataManager.Instance.ReadText(obj.descriptionId));
                            d2oSerializer.Serialize(writer, obj); // your instance
                            foreach (var lv in obj.spellLevels)
                            {
                                SpellLevel spellLv = ObjectDataManager.Instance.Get<SpellLevel>(lv);
                                //writer.WriteStartElement("SpellLevel");                            
                                d2oSerializerLv.Serialize(writer, spellLv); // your instance
                                //writer.WriteEndElement();                    

                            }
                            //writer.WriteEndElement();
                        }
                    writer.WriteEndElement();
                }
                // Open a file for reading
                cleanXML(breedFileName, true);
            }
            return true;

        }




    }
}
