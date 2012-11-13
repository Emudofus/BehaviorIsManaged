#region License GNU GPL
// ConfigurableAttribute.cs
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

namespace BiM.Core.Config
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConfigurableAttribute : Attribute
    {
        public ConfigurableAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
        }

        public ConfigurableAttribute(string name, string comment)
        {
            if (comment == null) throw new ArgumentNullException("comment");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            Name = name;
            Comment = comment;
        }

        public string Name { get; private set; }

        public string Comment
        {
            get;
            set;
        }
    }
}