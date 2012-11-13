using System.Xml;
using DofusProtocolBuilder.Parsing;

namespace DofusProtocolBuilder.XmlPatterns
{
    public abstract class XmlPatternBuilder
    {
        protected Parser Parser;

        protected XmlPatternBuilder(Parser parser)
        {
            Parser = parser;
        }

        public abstract void WriteToXml(XmlWriter writer);
    }
}