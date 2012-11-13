using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;

namespace DofusProtocolBuilder.Profiles
{
    public class EnumsProfile : ParsingProfile
    {
        public override void ExecuteProfile(Parser parser)
        {
            string file = Path.Combine(Program.Configuration.Output, OutPutPath, GetRelativePath(parser.Filename), Path.GetFileNameWithoutExtension(parser.Filename));

            var engine = new Engine();
            var host = new TemplateHost(TemplatePath);
            host.Session["Parser"] = parser;
            host.Session["Profile"] = this;
            var output = engine.ProcessTemplate(File.ReadAllText(TemplatePath), host);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ErrorText);
            }

            if (host.Errors.Count > 0)
                Program.Shutdown();

            File.WriteAllText(file + host.FileExtension, output);

            Console.WriteLine("Wrote {0}", file + host.FileExtension);
        }
    }
}