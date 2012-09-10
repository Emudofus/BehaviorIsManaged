using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace BiM.Core.Config
{
    public class ConfigVariable : ConfigNode
    {
        public const string CommentAttribute = "comment";
        public ConfigVariable(ConfigurableAttribute attribute, FieldInfo field)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (field == null) throw new ArgumentNullException("field");
            Attribute = attribute;
            Field = field;
        }

        public ConfigVariable(ConfigurableAttribute attribute, PropertyInfo property)
        {
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (property == null) throw new ArgumentNullException("property");
            Attribute = attribute;
            Property = property;
        }

        public override string Name
        {
            get { return Attribute.Name; }
            set { throw new InvalidOperationException("Cannot set an attribute name"); }
        }

        public ConfigurableAttribute Attribute
        {
            get;
            private set;
        }

        public FieldInfo Field
        {
            get;
            private set;
        }

        public PropertyInfo Property
        {
            get;
            private set;
        }

        public Type VariableType
        {
            get { return Field != null ? Field.FieldType : Property.PropertyType; }
        }

        public override void SetValue(object value)
        {
            if (Field != null)
            {
                if (!Field.IsStatic)
                    throw new Exception(
                        string.Format("Cannot assign a non-static field ({0}) in a static context (variable={1})",
                                      Field.Name, Attribute.Name));

                Field.SetValue(null, value);
            }
            else if (Property != null)
            {
                MethodInfo method = Property.GetSetMethod(true);

                if (method == null)
                    throw new Exception(string.Format("Property {0} has no set method (variable={1})", Property.Name,
                                                      Attribute.Name));

                if (!method.IsStatic)
                    throw new Exception(
                        string.Format("Cannot assign a non-static property ({0}) in a static context (variable={1})",
                                      Property.Name, Attribute.Name));

                Property.SetValue(null, value, new object[0]);
            }
            else
            {
                throw new Exception(string.Format("No property and no field bound to the variable {0}", Attribute.Name));
            }

            IsSynchronised = false;
        }

        public override object GetValue(Type type)
        {
            return GetValue();
        }

        public object GetValue()
        {
            if (Field != null)
            {
                if (!Field.IsStatic)
                    throw new Exception(
                        string.Format("Cannot get a non-static field ({0}) in a static context (variable={1})",
                                      Field.Name, Attribute.Name));

                return Field.GetValue(null);
            }
            else if (Property != null)
            {
                MethodInfo method = Property.GetGetMethod(true);

                if (method == null)
                    throw new Exception(string.Format("Property {0} has no set method (variable={1})", Property.Name,
                                                      Attribute.Name));

                if (!method.IsStatic)
                    throw new Exception(
                        string.Format("Cannot get a non-static property ({0}) in a static context (variable={1})",
                                      Property.Name, Attribute.Name));

                return Property.GetValue(null, new object[0]);
            }

            throw new Exception(string.Format("No property and no field bound to the variable {0}", Attribute.Name));
        }

        public void SetValue(object value, object instance)
        {
            if (Field != null)
            {
                if (Field.IsStatic)
                    throw new Exception(
                        string.Format("Cannot assign a static field ({0}) in a non-static context (variable={1})",
                                      Field.Name, Attribute.Name));

                Field.SetValue(instance, value);
            }
            else if (Property != null)
            {
                MethodInfo method = Property.GetSetMethod(true);

                if (method == null)
                    throw new Exception(string.Format("Property {0} has no set method (variable={1})", Property.Name,
                                                      Attribute.Name));

                if (method.IsStatic)
                    throw new Exception(
                        string.Format("Cannot assign a static property ({0}) in a non-static context (variable={1})",
                                      Property.Name, Attribute.Name));

                Property.SetValue(instance, value, new object[0]);
            }
            else
            {
                throw new Exception(string.Format("No property and no field bound to the variable {0}", Attribute.Name));
            }

            IsSynchronised = false;
        }

        public object GetValue(object instance)
        {
            if (Field != null)
            {
                if (Field.IsStatic)
                    throw new Exception(
                        string.Format("Cannot get a static field ({0}) in a non-static context (variable={1})",
                                      Field.Name, Attribute.Name));

                return Field.GetValue(instance);
            }
            else if (Property != null)
            {
                MethodInfo method = Property.GetSetMethod(true);

                if (method == null)
                    throw new Exception(string.Format("Property {0} has no set method (variable={1})", Property.Name,
                                                      Attribute.Name));

                if (method.IsStatic)
                    throw new Exception(
                        string.Format("Cannot get a static property ({0}) in a non-static context (variable={1})",
                                      Property.Name, Attribute.Name));

                return Property.GetValue(instance, new object[0]);
            }
            else
            {
                throw new Exception(string.Format("No property and no field bound to the variable {0}", Attribute.Name));
            }
        }


        internal override void Save(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            writer.WriteStartElement(NodeName);
            writer.WriteAttributeString(AttributeName, Name);

            if (!string.IsNullOrEmpty(Attribute.Comment))
                writer.WriteAttributeString(CommentAttribute, Attribute.Comment);

            object value = GetValue();
            new XmlSerializer(value.GetType()).Serialize(writer, value);

            IsSynchronised = true;

            writer.WriteEndElement();
        }

        internal override void Load(XmlNode node)
        {
            SetValue(new XmlSerializer(VariableType).Deserialize(new StringReader(node.InnerXml)));
            IsSynchronised = true;
        }
    }
}