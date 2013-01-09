// 
// Copyright (c) 2004-2011 Jaroslaw Kowalski <jaak@jkowalski.net>
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using NLog.Config;

#if !NET_CF && !MONO && !SILVERLIGHT

namespace NLog.Targets
{
    [Target("WpfRichTextBox")]
    public sealed class WpfRichTextBoxTarget : TargetWithLayout
    {
        private static readonly TypeConverter colorConverter = new ColorConverter();
        private readonly List<RichTextBox> m_targetRichTextBoxs = new List<RichTextBox>();
        private int lineCount;

        static WpfRichTextBoxTarget()
        {
            var rules = new List<WpfRichTextBoxRowColoringRule>
                            {
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Fatal", "White", "Red", FontStyles.Normal, FontWeights.Bold),
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Error", "Red", "Empty", FontStyles.Italic, FontWeights.Bold),
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Warn", "Orange", "Empty"),
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Info", "Black", "Empty"),
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Debug", "Gray", "Empty"),
                                new WpfRichTextBoxRowColoringRule("level == LogLevel.Trace", "DarkGray", "Empty", FontStyles.Italic, FontWeights.Normal),
                            };

            DefaultRowColoringRules = rules.AsReadOnly();
        }

        public WpfRichTextBoxTarget()
        {
            WordColoringRules = new List<WpfRichTextBoxWordColoringRule>();
            RowColoringRules = new List<WpfRichTextBoxRowColoringRule>();
            ToolWindow = true;
        }

        public static ReadOnlyCollection<WpfRichTextBoxRowColoringRule> DefaultRowColoringRules
        {
            get;
            private set;
        }

        public string ControlName
        {
            get;
            set;
        }

        public string FormName
        {
            get;
            set;
        }

        [DefaultValue(false)]
        public bool UseDefaultRowColoringRules
        {
            get;
            set;
        }

        [ArrayParameter(typeof (WpfRichTextBoxRowColoringRule), "row-coloring")]
        public IList<WpfRichTextBoxRowColoringRule> RowColoringRules
        {
            get;
            private set;
        }

        [ArrayParameter(typeof (WpfRichTextBoxWordColoringRule), "word-coloring")]
        public IList<WpfRichTextBoxWordColoringRule> WordColoringRules
        {
            get;
            private set;
        }

        [DefaultValue(true)]
        public bool ToolWindow
        {
            get;
            set;
        }

        public bool ShowMinimized
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public bool AutoScroll
        {
            get;
            set;
        }

        public bool BoldTime
        {
            get;
            set;
            }

        public int MaxLines
        {
            get;
            set;
        }

        protected override void InitializeTarget()
        {
            Application.Current.Dispatcher.Invoke(
                new Action(() =>
                               {
                                   foreach (Window window in Application.Current.Windows)
                                   {
                                       if (window.GetType().Name.Equals(FormName, StringComparison.OrdinalIgnoreCase))
                                       {
                                           object element = window.FindName(ControlName);

                                           if (element is RichTextBox)
                                               m_targetRichTextBoxs.Add(element as RichTextBox);
                                       }
                                   }
                               }));
        }

        protected override void CloseTarget()
        {
        }

        protected override void Write(LogEventInfo logEvent)
        {
            WpfRichTextBoxRowColoringRule matchingRule = null;

            foreach (WpfRichTextBoxRowColoringRule rr in RowColoringRules)
            {
                if (rr.CheckCondition(logEvent))
                {
                    matchingRule = rr;
                    break;
                }
            }

            if (UseDefaultRowColoringRules && matchingRule == null)
            {
                foreach (WpfRichTextBoxRowColoringRule rr in DefaultRowColoringRules)
                {
                    if (rr.CheckCondition(logEvent))
                    {
                        matchingRule = rr;
                        break;
                    }
                }
            }

            if (matchingRule == null)
            {
                matchingRule = WpfRichTextBoxRowColoringRule.Default;
            }

            string logMessage = Layout.Render(logEvent);

            if (Application.Current != null)
                Application.Current.Dispatcher.Invoke(new Action(() => SendTheMessageToRichTextBox(logMessage, matchingRule)));
        }

        private void SendTheMessageToRichTextBox(string logMessage, WpfRichTextBoxRowColoringRule rule)
        {
            foreach (RichTextBox richTextBox in m_targetRichTextBoxs)
            {
                var tr = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd) {Text = logMessage + "\n"};

                if (rule.FontColor != "Empty")
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush((Color) colorConverter.ConvertFromString(rule.FontColor)));

                if (rule.BackgroundColor != "Empty")
                    tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush((Color) colorConverter.ConvertFromString(rule.BackgroundColor)));

                tr.ApplyPropertyValue(TextElement.FontStyleProperty, rule.Style);
                tr.ApplyPropertyValue(TextElement.FontWeightProperty, rule.Weight);

                if (MaxLines > 0)
                {
                    lineCount++;
                    if (lineCount > MaxLines)
                    {
                        tr = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        tr.Text.Remove(0, tr.Text.IndexOf('\n'));
                        lineCount--;
                    }
                }

                if (AutoScroll)
                {
                    richTextBox.ScrollToEnd();
                }
            }
        }

        #region Nested type: DelSendTheMessageToRichTextBox

        private delegate void DelSendTheMessageToRichTextBox(string logMessage, WpfRichTextBoxRowColoringRule rule);

        #endregion

        #region Nested type: FormCloseDelegate

        private delegate void FormCloseDelegate();

        #endregion
    }
}

#endif