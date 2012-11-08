#region License GNU GPL
// NormalGraphicalElementData.cs
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

namespace BiM.Protocol.Tools.Ele.Datas
{
    public class NormalGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NormalGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public int Gfx
        {
            get;
            set;
        }

        public uint Height
        {
            get;
            set;
        }

        public bool HorizontalSymmetry
        {
            get;
            set;
        }

        public System.Drawing.Point Origin
        {
            get;
            set;
        }

        public System.Drawing.Point Size
        {
            get;
            set;
        }

        public override EleGraphicalElementTypes Type
        {
            get { return EleGraphicalElementTypes.NORMAL; }
        }

        public static NormalGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new NormalGraphicalElementData(instance, id);

            data.Gfx = reader.ReadInt();
            data.Height = reader.ReadUInt();
            data.HorizontalSymmetry = reader.ReadBoolean();
            data.Origin = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            data.Size = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());

            return data;
        }
    }
}