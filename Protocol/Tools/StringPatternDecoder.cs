#region License GNU GPL
// StringPatternDecoder.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BiM.Protocol.Tools
{
    public class StringPatternDecoder
    {
        public StringPatternDecoder(string encodedString, object[] arguments)
        {
            EncodedString = encodedString;
            Arguments = arguments;
        }

        public string EncodedString
        {
            get;
            private set;
        }

        public object[] Arguments
        {
            get;
            private set;
        }

        public int? CheckValidity(bool canThrow = true)
        {
            var parentheses = new Stack<int>();
            int bracketDeep = 0;
            int parentheseDeep = 0;

            for (int i = 0; i < EncodedString.Length; i++)
            {
                var chr = EncodedString[i];

                if (chr == '#' || chr == '~')
                {
                    if (i + 1 >= EncodedString.Length || !char.IsDigit(EncodedString[i + 1]))
                        if (canThrow) throw new InvalidPatternException("Attempt a digit after '" + chr + "'", i);
                        else return i;
                }


                if (chr == '{')
                    bracketDeep++;
                else if (chr == '}')
                    bracketDeep--;

                if (chr == '[')
                {
                    parentheseDeep++;
                    parentheses.Push(i);
                }
                else if (chr == ']')
                {
                    parentheseDeep--;
                    int last = parentheses.Pop();

                    var sub = EncodedString.Substring(i + 1, last - 1 - i);

                    if (string.IsNullOrEmpty(sub) || !sub.All(char.IsDigit))
                        if (canThrow) throw new InvalidPatternException("Attempt a number between [ and ]", last);
                        else return i;
                }
            }

            if (bracketDeep != 0)
                if (canThrow) throw new InvalidPatternException("'{' not closed");
                else return 0;

            if (parentheseDeep != 0)
                if (canThrow) throw new InvalidPatternException("'[' not closed");
                else return 0;

            return null;
        }

        public string Decode()
        {
            CheckValidity();

            return DecodeInternal(EncodedString, Arguments);
        }

        private string DecodeInternal(string str, object[] args)
        {
            var stringBuilder = new StringBuilder();

            var parentheses = new Stack<int>();
            var brackets = new Stack<int>();

            for (int i = 0; i < str.Length; i++)
            {
                var chr = str[i];

                if (chr == '#')
                {
                    var digit = int.Parse(str[i + 1].ToString());

                    if (args.Length > digit - 1)
                        stringBuilder.Append(args[digit - 1]);

                    i++;
                }

                else if (chr == '~')
                {
                    var digit = int.Parse(str[i + 1].ToString());

                    if (args.Length <= digit - 1)
                        return stringBuilder.ToString();

                    i++;
                }

                else if (chr == '{')
                {
                    int depth = 1;
                    int j = i + 1;
                    for (; depth > 0 && j < str.Length; j++)
                    {
                        if (str[j] == '{')
                            depth++;
                        else if (str[j] == '}')
                            depth--;
                    }

                    stringBuilder.Append(DecodeInternal(str.Substring(i + 1, j - 2 - i), args));
                    i = j - 1;
                }

                else if (chr == '[')
                {
                    int depth = 1;
                    int j = i + 1;
                    for (; depth > 0 && j < str.Length; j++)
                    {
                        if (str[j] == '[')
                            depth++;
                        else if (str[j] == ']')
                            depth--;
                    }

                    var sub = str.Substring(i + 1, j - 2 - i);
                    var number = int.Parse(sub);

                    if (args.Length > number - 1)
                        stringBuilder.Append(args[number - 1]);

                    i = j - 1;
                }

                else
                    stringBuilder.Append(chr);
            }

            return stringBuilder.ToString();
        }
    }

    [Serializable]
    public class InvalidPatternException : Exception
    {
        public int Index
        {
            get;
            set;
        }

        public InvalidPatternException(string message)
            : base(message)
        {
        }

        public InvalidPatternException(string message, int index)
            : base(message)
        {
            Index = index;
        }

        public InvalidPatternException(string message, Exception inner, int index)
            : base(message, inner)
        {
            Index = index;
        }

        protected InvalidPatternException(
            SerializationInfo info,
            StreamingContext context, int index)
            : base(info, context)
        {
            Index = index;
        }
    }
}