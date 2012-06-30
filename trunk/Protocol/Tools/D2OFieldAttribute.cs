using System;

namespace BiM.Protocol.Tools
{
    public class D2OFieldAttribute : Attribute
    {
        public string FieldName
        {
            get;
            set;
        }

        public D2OFieldAttribute()
        {
        }

        public D2OFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}