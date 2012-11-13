#region License GNU GPL
// D2OClassAttribute.cs
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

namespace BiM.Protocol.Tools
{
    public class D2OClassAttribute : Attribute
    {
        public D2OClassAttribute(string name, bool autoBuild = true)
        {
            Name = name;
            AutoBuild = autoBuild;
        }

        public D2OClassAttribute(string name, string packageName, bool autoBuild = true)
        {
            Name = name;
            PackageName = packageName;
            AutoBuild = autoBuild;
        }

        public string Name
        {
            get;
            set;
        }

        public string PackageName
        {
            get;
            set;
        }

        public bool AutoBuild
        {
            get;
            set;
        }
    }
}