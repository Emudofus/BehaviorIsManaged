namespace DofusProtocolBuilder.Parsing.Elements
{
    public class ForEachStatement : IStatement
    {
        public string Iterated
        {
            get;
            set;
        }

        public string Iterator
        {
            get;
            set;
        }
    }
}