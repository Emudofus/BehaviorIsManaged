using System.Linq;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing.Elements
{
    public class InvokeExpression : IStatement
    {
        public static string Pattern =
            @"(?<assignationtype>[^=(new|throw new)]\w+\s+)?(?:(?<assignationtarget>\w+\.)*(?<assignation>\w+(?:\[\w+\])?\s*(?:=|:\*=)\s*))?(?<stereotype>new|throw new)?\s?(?<target>[^\.]+\.)*(?<name>[_\w]+)\((?<argument>[^,]+,?)*\);";

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

        public string ReturnVariableTypeAssignation
        {
            get;
            set;
        }

        public string ReturnVariableAssignationTarget
        {
            get;
            set;
        }

        public string ReturnVariableAssignation
        {
            get;
            set;
        }

        public string[] Args
        {
            get;
            set;
        }

        public string Preffix
        {
            get;
            set;
        }

        public static InvokeExpression Parse(string str)
        {
            Match match = Regex.Match(str, Pattern, RegexOptions.Multiline);
            InvokeExpression result = null;

            if (match.Success)
            {
                result = new InvokeExpression();

                if (match.Groups["assignationtype"].Value != "")
                    result.ReturnVariableTypeAssignation =
                        match.Groups["assignationtype"].Value.Trim();

                foreach (var capture in match.Groups["assignationtarget"].Captures)
                {
                    result.ReturnVariableAssignationTarget += capture;
                }
                if (result.ReturnVariableAssignationTarget != null &&
                    result.ReturnVariableAssignationTarget.EndsWith("."))
                    result.ReturnVariableAssignationTarget =
                        result.ReturnVariableAssignationTarget.Remove(
                                result.ReturnVariableAssignationTarget.Length - 1, 1); // remove dot

                if (match.Groups["assignation"].Value != "")
                    result.ReturnVariableAssignation = 
						match.Groups["assignation"].Value.Trim().
                            Replace("=", "").
                            Replace(":", "").
                            Replace("*", "").
                            Trim();

                result.Preffix = match.Groups["stereotype"].Value;

                foreach (object capture in match.Groups["target"].Captures)
                {
                    result.Target += capture;
                }
                if (result.Target != null && result.Target.EndsWith("."))
                    result.Target = result.Target.Remove(result.Target.Length - 1, 1);
                        // remove dot

                result.Name = match.Groups["name"].Value;

            	result.Args = (from object capture in match.Groups["argument"].Captures
            	               select capture.ToString().Trim().Replace(",", "")).ToArray();
            }

            return result;
        }
    }
}