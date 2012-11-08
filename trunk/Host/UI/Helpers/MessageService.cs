#region License GNU GPL
// MessageService.cs
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
using System.Globalization;
using System.Windows;

namespace BiM.Host.UI.Helpers
{
    public static class MessageService
    {
        private static MessageBoxResult MessageBoxResult
        {
            get { return MessageBoxResult.None; }
        }

        private static MessageBoxOptions MessageBoxOptions
        {
            get { return (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft) ? MessageBoxOptions.RtlReading : MessageBoxOptions.None; }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public static void ShowMessage(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None,
                                MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None,
                                MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// Shows the message as warning.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public static void ShowWarning(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Warning,
                                MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Warning,
                                MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// Shows the message as error.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The message.</param>
        public static void ShowError(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error,
                                MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error,
                                MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// Shows the specified question.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        public static bool? ShowQuestion(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions);
            }

            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }

            return null;
        }

        /// <summary>
        /// Shows the specified yes/no question.
        /// </summary>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        public static bool ShowYesNoQuestion(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNo,
                                         MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNo,
                                         MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions);
            }

            return result == MessageBoxResult.Yes;
        }

    }
}