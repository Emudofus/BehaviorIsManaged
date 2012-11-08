#region License GNU GPL
// KeySenderTest.cs
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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace WindowManager
{
    public class KeySenderTest
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content.StartsWith(".send"))
            {
                message.BlockNetworkSend();

                var detector = bot.GetFrame<WindowDetector>();

                if (detector == null)
                {
                    bot.Character.SendMessage("Frame WindowDetector not found");
                    return;
                }

                var splits = message.content.Split(' ');
                if (splits.Length != 3)
                {
                    bot.Character.SendMessage("syntax : .send [System.Windows.Forms.Keys] [delay]");
                    return;
                }

                Keys key;
                try
                {
                    key = (Keys)Enum.Parse(typeof(Keys), splits[1]);
                }
                catch (Exception)
                {
                    bot.Character.SendMessage("syntax : .send [System.Windows.Forms.Keys] [delay]");
                    return;
                }

                int delay;
                if (!int.TryParse(splits[2], out delay))
                {
                    bot.Character.SendMessage("syntax : .send [System.Windows.Forms.Keys] [delay]");
                    return;
                }

                bot.CallDelayed(delay, () => detector.SendKey(key));
            }
        }

        // --------------------------------------------------------------------------------
        /// <summary>
        /// Converts a C# literal string into a normal string.
        /// </summary>
        /// <param name="source">Source C# literal string.</param>
        /// <returns>
        /// Normal string representation.
        /// </returns>
        // --------------------------------------------------------------------------------
        public static string StringFromCSharpLiteral(string source)
        {
            StringBuilder sb = new StringBuilder(source.Length);
            int pos = 0;
            while (pos < source.Length)
            {
                char c = source[pos];
                if (c == '\\')
                {
                    // --- Handle escape sequences
                    pos++;
                    if (pos >= source.Length) throw new ArgumentException("Missing escape sequence");
                    switch (source[pos])
                    {
                        // --- Simple character escapes
                        case '\'': c = '\''; break;
                        case '\"': c = '\"'; break;
                        case '\\': c = '\\'; break;
                        case '0': c = '\0'; break;
                        case 'a': c = '\a'; break;
                        case 'b': c = '\b'; break;
                        case 'f': c = '\f'; break;
                        case 'n': c = ' '; break;
                        case 'r': c = ' '; break;
                        case 't': c = '\t'; break;
                        case 'v': c = '\v'; break;
                        case 'x':
                            // --- Hexa escape (1-4 digits)
                            StringBuilder hexa = new StringBuilder(10);
                            pos++;
                            if (pos >= source.Length)
                                throw new ArgumentException("Missing escape sequence");
                            c = source[pos];
                            if (Char.IsDigit(c) || ( c >= 'a' && c <= 'f' ) || ( c >= 'A' && c <= 'F' ))
                            {
                                hexa.Append(c);
                                pos++;
                                if (pos < source.Length)
                                {
                                    c = source[pos];
                                    if (Char.IsDigit(c) || ( c >= 'a' && c <= 'f' ) || ( c >= 'A' && c <= 'F' ))
                                    {
                                        hexa.Append(c);
                                        pos++;
                                        if (pos < source.Length)
                                        {
                                            c = source[pos];
                                            if (Char.IsDigit(c) || ( c >= 'a' && c <= 'f' ) ||
                                              ( c >= 'A' && c <= 'F' ))
                                            {
                                                hexa.Append(c);
                                                pos++;
                                                if (pos < source.Length)
                                                {
                                                    c = source[pos];
                                                    if (Char.IsDigit(c) || ( c >= 'a' && c <= 'f' ) ||
                                                      ( c >= 'A' && c <= 'F' ))
                                                    {
                                                        hexa.Append(c);
                                                        pos++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            c = (char)Int32.Parse(hexa.ToString(), NumberStyles.HexNumber);
                            pos--;
                            break;
                        case 'u':
                            // Unicode hexa escape (exactly 4 digits)
                            pos++;
                            if (pos + 3 >= source.Length)
                                throw new ArgumentException("Unrecognized escape sequence");
                            try
                            {
                                uint charValue = UInt32.Parse(source.Substring(pos, 4),
                                  NumberStyles.HexNumber);
                                c = (char)charValue;
                                pos += 3;
                            }
                            catch (SystemException)
                            {
                                throw new ArgumentException("Unrecognized escape sequence");
                            }
                            break;
                        case 'U':
                            // Unicode hexa escape (exactly 8 digits, first four must be 0000)
                            pos++;
                            if (pos + 7 >= source.Length)
                                throw new ArgumentException("Unrecognized escape sequence");
                            try
                            {
                                uint charValue = UInt32.Parse(source.Substring(pos, 8),
                                  NumberStyles.HexNumber);
                                if (charValue > 0xffff)
                                    throw new ArgumentException("Unrecognized escape sequence");
                                c = (char)charValue;
                                pos += 7;
                            }
                            catch (SystemException)
                            {
                                throw new ArgumentException("Unrecognized escape sequence");
                            }
                            break;
                        default:
                            throw new ArgumentException("Unrecognized escape sequence");
                    }
                }
                pos++;
                sb.Append(c);
            }
            return sb.ToString();
        }

        // --------------------------------------------------------------------------------
        /// <summary>
        /// Converts a C# verbatim literal string into a normal string.
        /// </summary>
        /// <param name="source">Source C# literal string.</param>
        /// <returns>
        /// Normal string representation.
        /// </returns>
        // --------------------------------------------------------------------------------
        public static string StringFromVerbatimLiteral(string source)
        {
            StringBuilder sb = new StringBuilder(source.Length);
            int pos = 0;
            while (pos < source.Length)
            {
                char c = source[pos];
                if (c == '\"')
                {
                    // --- Handle escape sequences
                    pos++;
                    if (pos >= source.Length) throw new ArgumentException("Missing escape sequence");
                    if (source[pos] == '\"') c = '\"';
                    else throw new ArgumentException("Unrecognized escape sequence");
                }
                pos++;
                sb.Append(c);
            }
            return sb.ToString();
        }

        // --------------------------------------------------------------------------------
        /// <summary>
        /// Converts a C# literal string into a normal character..
        /// </summary>
        /// <param name="source">Source C# literal string.</param>
        /// <returns>
        /// Normal char representation.
        /// </returns>
        // --------------------------------------------------------------------------------
        public static char CharFromCSharpLiteral(string source)
        {
            string result = StringFromCSharpLiteral(source);
            if (result.Length != 1)
                throw new ArgumentException("Invalid char literal");
            return result[0];
        }
    }
}