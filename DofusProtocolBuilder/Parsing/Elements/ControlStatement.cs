using System;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    public class ControlStatementEnd : IStatement
    {

    }

    public class ControlStatement : IStatement
    {
        public static string Pattern =
            @"\b(?<type>if|else if|else|while|break|return);?\s*(?<condition>\(?\s*[^;]*\s*\)?)?";

        public ControlType ControlType
        {
            get;
            set;
        }

        public string Condition
        {
            get;
            set;
        }

        public static ControlStatement Parse(string str)
        {
            Match match = Regex.Match(str, Pattern, RegexOptions.Multiline);
            ControlStatement result = null;

            if (match.Success)
            {
                result = new ControlStatement();

                if (match.Groups["type"].Value != "")
					result.ControlType =
                        (ControlType)
                        Enum.Parse(typeof (ControlType), match.Groups["type"].Value.Replace(" ", ""), true);

                if (match.Groups["condition"].Value != "")
                {
                    // remove the ( at the begin and the ) at the end
                    if (match.Groups["condition"].Value.StartsWith("(") &&
                        match.Groups["condition"].Value.EndsWith(")"))
                        result.Condition = match.Groups["condition"].Value.
                            Remove(match.Groups["condition"].Value.Length - 1, 1).
                            Remove(0, 1).
                            Trim();
                    else
                        result.Condition = match.Groups["condition"].Value.Trim();
                }
            }

            return result;
        }
    }
}