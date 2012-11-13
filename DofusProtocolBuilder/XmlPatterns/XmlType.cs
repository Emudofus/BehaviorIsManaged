using System.Xml.Serialization;

namespace DofusProtocolBuilder.XmlPatterns
{
    public class XmlType
    {
        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Id
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Heritage
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Namespace
        {
            get;
            set;
        }

        public XmlField[] Fields
        {
            get;
            set;
        }
    }
}