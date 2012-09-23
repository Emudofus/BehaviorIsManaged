using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    /// <summary>
    /// </summary>
    /// <remarks>
    ///   Only work if the value is a variable or a hard coded value
    /// </remarks>
    public class AssignationStatement : IStatement
    {
        public static string Pattern =
            @"^(?<var>var\s)?(?<variable>[^:=]+):?(?<type>[^=]+)?\s*=\s*(?<value>[^;]+);$";

        public string Name
        {
            get;
            set;
        }

        public string Target
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string TypeDeclaration
        {
            get;
            set;
        }

        public static AssignationStatement Parse(string str)
        {
            Match match = Regex.Match(str, Pattern, RegexOptions.Multiline);
            AssignationStatement result = null;

            if (match.Success)
            {
                result = new AssignationStatement();

                if (match.Groups["var"].Value != "")
                {
                    result.TypeDeclaration = match.Groups["type"].Value.Trim();
                }

                if (match.Groups["variable"].Value != "")
                {
                    Match variableMatch = Regex.Match(match.Groups["variable"].Value, @"^(?<target>[^\.]+\.)*(?<name>.+)", RegexOptions.Multiline);

                    result.Target = variableMatch.Groups["target"].Value.Trim().TrimEnd('.');
                    result.Name = variableMatch.Groups["name"].Value.Trim();
                }    

                result.Value = match.Groups["value"].Value.Trim();

                if (!result.Value.Contains("\""))
                {
                    if (result.Value.Contains("<"))
                    {
                        string generictype = result.Value.Split('<').Last().Split('>').First().Split('.').Last();
                        string defaulttype = result.Value.Split('<').Last().Split('>').First();

                        result.Value = result.Value.Replace(defaulttype, generictype);
                    }

                    result.Value = result.Value;
                }
            }

            return result;
        }
    }
}