

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Documents")]
    public class Document : IDataObject
    {
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        public uint titleId;
        public uint authorId;
        public uint subTitleId;
        public uint contentId;
        public String contentCSS;
    }
}