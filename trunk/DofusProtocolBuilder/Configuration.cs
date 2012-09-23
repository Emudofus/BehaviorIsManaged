using System;
using DofusProtocolBuilder.Profiles;

namespace DofusProtocolBuilder
{
    [Serializable]
    public class Configuration
    {
        public string Output
        {
            get;
            set;
        }

        public string SourcePath
        {
            get;
            set;
        }

        public string BaseNamespace
        {
            get;
            set;
        }

        public bool UseIEnumerable
        {
            get;
            set;
        }

        public XmlMessagesProfile XmlMessagesProfile
        {
            get;
            set;
        }

        public XmlTypesProfile XmlTypesProfile
        {
            get;
            set;
        }

        public MessagesProfile MessagesProfile
        {
            get;
            set;
        }

        public TypesProfile TypesProfile
        {
            get;
            set;
        }

        public EnumsProfile EnumsProfile
        {
            get;
            set;
        }

        public DatacenterProfile DatacenterProfile
        {
            get;
            set;
        }

        public void SetDefault()
        {
            Output = "./";

            // decompiled with sothink
            SourcePath = "./sources/";
            BaseNamespace = "BiM.Protocol";
            UseIEnumerable = false;

            XmlMessagesProfile =
                new XmlMessagesProfile
                    {
                        Name = "Xml Messages classes",
                        OutPutPath = "messages_xml/",
                        SourcePath = @"com/ankamagames/dofus/network/messages/",
                        EnableParsing = true,
                    };

            XmlTypesProfile =
                new XmlTypesProfile
                    {
                        Name = "Xml Types classes",
                        OutPutPath = "types_xml/",
                        SourcePath = @"com/ankamagames/dofus/network/types/",
                        EnableParsing = true,
                        
                    };

            MessagesProfile =
                new MessagesProfile
                    {
                        Name = "Messages classes",
                        SourcePath = @"com/ankamagames/dofus/network/messages/",
                        TemplatePath = "./Templates/MessageTemplate.tt",
                        OutPutPath = "messages/",
                        OutPutNamespace = ".Messages",
                    };

            TypesProfile =
                new TypesProfile
                    {
                        Name = "Types classes",
                        SourcePath = @"com/ankamagames/dofus/network/types/",
                        TemplatePath = "./Templates/TypeTemplate.tt",
                        OutPutPath = "types/",
                        OutPutNamespace = ".Types",
                    };

            EnumsProfile =
                new EnumsProfile
                    {
                        Name = "Enums",
                        SourcePath = @"com/ankamagames/dofus/network/enums/",
                        OutPutPath = "enums/",
                        OutPutNamespace = ".Enums",
                        TemplatePath = "./Templates/EnumTemplate.tt",
                        EnableParsing = true,
                    };

            DatacenterProfile =
                new DatacenterProfile
                    {
                        Name = "D2O Data classes",
                        SourcePath = @"com/ankamagames/dofus/datacenter/",
                        OutPutPath = "d2oclasses/",
                        OutPutNamespace = ".Data",
                        TemplatePath = "./Templates/DataCenterTemplate.tt",
                        IgnoreMethods = true,
                        EnableParsing = true,
                    };
        }
    }
}