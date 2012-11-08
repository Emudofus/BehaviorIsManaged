#region License GNU GPL
// Version.cs
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
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class Version
    {
        public const short Id = 11;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte major;
        public sbyte minor;
        public sbyte release;
        public int revision;
        public sbyte patch;
        public sbyte buildType;
        
        public Version()
        {
        }
        
        public Version(sbyte major, sbyte minor, sbyte release, int revision, sbyte patch, sbyte buildType)
        {
            this.major = major;
            this.minor = minor;
            this.release = release;
            this.revision = revision;
            this.patch = patch;
            this.buildType = buildType;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(major);
            writer.WriteSByte(minor);
            writer.WriteSByte(release);
            writer.WriteInt(revision);
            writer.WriteSByte(patch);
            writer.WriteSByte(buildType);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            major = reader.ReadSByte();
            if (major < 0)
                throw new Exception("Forbidden value on major = " + major + ", it doesn't respect the following condition : major < 0");
            minor = reader.ReadSByte();
            if (minor < 0)
                throw new Exception("Forbidden value on minor = " + minor + ", it doesn't respect the following condition : minor < 0");
            release = reader.ReadSByte();
            if (release < 0)
                throw new Exception("Forbidden value on release = " + release + ", it doesn't respect the following condition : release < 0");
            revision = reader.ReadInt();
            if (revision < 0)
                throw new Exception("Forbidden value on revision = " + revision + ", it doesn't respect the following condition : revision < 0");
            patch = reader.ReadSByte();
            if (patch < 0)
                throw new Exception("Forbidden value on patch = " + patch + ", it doesn't respect the following condition : patch < 0");
            buildType = reader.ReadSByte();
            if (buildType < 0)
                throw new Exception("Forbidden value on buildType = " + buildType + ", it doesn't respect the following condition : buildType < 0");
        }
        
    }
    
}