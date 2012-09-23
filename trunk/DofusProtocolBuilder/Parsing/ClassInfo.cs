using System.Collections.Generic;

namespace DofusProtocolBuilder.Parsing
{
    public class ClassInfo
    {
    	public ClassInfo()
    	{
    		Implementations = new List<string>();
            CustomAttribute = new List<string>();
    	}

        #region ClassModifiers enum

        public enum ClassModifiers
        {
            None,
            Abstract
        } ;

        #endregion

        public AccessModifiers AccessModifier
        {
            get;
            set;
        }

        public ClassModifiers ClassModifier
        {
            get;
            set;
        }

        public string Namespace
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Heritage
        {
            get;
            set;
        }

		public List<string> Implementations
		{
			get;
            set; 
        }

        public List<string> CustomAttribute
        {
            get;
            set;
        }
    }
}