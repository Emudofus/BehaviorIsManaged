namespace DofusProtocolBuilder.Parsing.Elements
{
	public class ForStatement : IStatement
	{
		public string Iterated { get; set; }
		public string Condition { get; set; }

		public string Iterator { get; set; }
	}
}