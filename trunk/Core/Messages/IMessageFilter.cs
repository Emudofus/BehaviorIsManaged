#region License GNU GPL
// IMessageFilter.cs
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

namespace BiM.Core.Messages
{
    public interface IMessageFilter
    {
        /// <summary>
        /// Method called to allow or not a message handling.
        /// </summary>
        /// <param name="message">The handled message</param>
        /// <param name="sender">The message sender</param>
        /// <param name="handlerContainer">The container type of the handler</param>
        /// <param name="container">The container of the handler (null if static)</param>
        /// <returns>True to allow the message handling</returns>
        bool Handle(Message message, object sender, Type handlerContainer, object container);
    }
}