#region License GNU GPL
// DlmFixture.cs
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
using System.ComponentModel;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmFixture
    {
        public DlmFixture(DlmMap map)
        {
            Map = map;
        }

        public DlmMap Map
        {
            get;
            set;
        }

        public int FixtureId
        {
            get;
            private set;
        }

        public System.Drawing.Point Offset
        {
            get;
            set;
        }

        public short Rotation
        {
            get;
            set;
        }

        public short ScaleX
        {
            get;
            set;
        }

        public short ScaleY
        {
            get;
            set;
        }

        public int Hue
        {
            get;
            set;
        }

        public byte Alpha
        {
            get;
            set;
        }

        public static DlmFixture ReadFromStream(DlmMap map, IDataReader reader)
        {
            var fixture = new DlmFixture(map);

            fixture.FixtureId = reader.ReadInt();
            fixture.Offset = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            fixture.Rotation = reader.ReadShort();
            fixture.ScaleX = reader.ReadShort();
            fixture.ScaleY = reader.ReadShort();
            fixture.Hue = reader.ReadByte() << 16 | reader.ReadByte() << 8 | reader.ReadByte();
            fixture.Alpha = reader.ReadByte();

            return fixture;
        }
    }
}