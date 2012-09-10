using System;

namespace BiM.Core.Config
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConfigurableAttribute : Attribute
    {
        public ConfigurableAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
        }

        public ConfigurableAttribute(string name, string comment)
        {
            if (comment == null) throw new ArgumentNullException("comment");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
            Comment = comment;
        }

        public string Name { get; private set; }

        public string Comment
        {
            get;
            set;
        }
    }
}