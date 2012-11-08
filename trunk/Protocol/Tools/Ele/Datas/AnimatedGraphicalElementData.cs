#region License GNU GPL
// AnimatedGraphicalElementData.cs
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
    public class AnimatedGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public AnimatedGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type
        {
            get
            {
                return EleGraphicalElementTypes.ANIMATED;
            }
        }

        public uint MinDelay
        {
            get;
            set;
        }

        public uint MaxDelay
        {
            get;
            set;
        }

        public static new AnimatedGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new AnimatedGraphicalElementData(instance, id);

            data.Gfx = reader.ReadInt();
            data.Height = reader.ReadUInt();
            data.HorizontalSymmetry = reader.ReadBoolean();
            data.Origin = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());
            data.Size = new System.Drawing.Point(reader.ReadShort(), reader.ReadShort());

            if (instance.Version == 4)
            {
                data.MinDelay = reader.ReadUInt();
                data.MaxDelay = reader.ReadUInt();
            }

            return data;
        }
    }
}