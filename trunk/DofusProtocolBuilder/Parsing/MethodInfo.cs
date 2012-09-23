using System;
using System.Collections.Generic;
using DofusProtocolBuilder.Parsing.Elements;

namespace DofusProtocolBuilder.Parsing
{
	public struct Argument
	{
		public string Name;
		public string Type;

	    public Argument(string name, string type, string defaultValue = default(string))
	    {
	        Name = name;
	        Type = type;
	        DefaultValue = defaultValue;
	    }

	    public string DefaultValue;
	}

    public class MethodInfo
    {
        #region MethodModifiers enum

        public enum MethodModifiers
        {
            None,
            Abstract,
            Constant,
            Static,
            New,
            Override,
            Virtual
        } ;

        #endregion

        public MethodInfo()
        {
            Modifiers = new List<MethodModifiers>();
        }

        public List<MethodModifiers> Modifiers
        {
            get;
            set;
        }

		public List<IStatement> Statements
		{
			get; set;
		}

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

		public Argument[] Arguments
        {
            get;
            set;
        }

        public string CustomLine
        {
            get;
            set;
        }
    }
}