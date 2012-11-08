#region License GNU GPL
// D2OClassDefinition.cs
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
using System.Diagnostics;
using System.Linq;

namespace BiM.Protocol.Tools
{
    [DebuggerDisplay("Name = {Name}")]
    public class D2OClassDefinition
    {
        public D2OClassDefinition(int id, string classname, string packagename, Type classType, IEnumerable<D2OFieldDefinition> fields, long offset)
        {
            Id = id;
            Name = classname;
            PackageName = packagename;
            ClassType = classType;
            Fields = fields.ToDictionary(entry => entry.Name);
            Offset = offset;
        }

        public Dictionary<string, D2OFieldDefinition> Fields
        {
            get;
            private set;
        }

        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string PackageName
        {
            get;
            private set;
        }

        public Type ClassType
        {
            get;
            private set;
        }

        internal long Offset
        {
            get;
            set;
        }
    }
}