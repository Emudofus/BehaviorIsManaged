

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SoundUi")]
    public class SoundUi : IDataObject
    {
        public uint id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public List<SoundUiElement> subElements;
        public String MODULE = "SoundUi";
    }
}