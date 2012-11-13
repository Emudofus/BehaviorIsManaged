namespace DofusProtocolBuilder.Parsing
{
    public class FieldInfo
    {
        public AccessModifiers Modifiers
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public bool IsStatic
        {
            get;
            set;
        }

		public bool IsConst
		{
			get; set;
		}
    }
}