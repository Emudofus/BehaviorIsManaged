

// Generated on 12/11/2012 19:44:36
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