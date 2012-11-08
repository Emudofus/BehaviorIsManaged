#region License GNU GPL
// EleGraphicalData.cs
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
using System.ComponentModel;
using BiM.Core.IO;
using BiM.Protocol.Tools.Ele.Datas;

namespace BiM.Protocol.Tools.Ele
{
    public abstract class EleGraphicalData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EleGraphicalData(EleInstance instance, int id)
        {
            Instance = instance;
            Id = id;
        }

        public EleInstance Instance
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public abstract EleGraphicalElementTypes Type
        {
            get;
        }

        public static EleGraphicalData ReadFromStream(EleInstance instance, BigEndianReader reader)
        {
            var id = reader.ReadInt();
            var type = (EleGraphicalElementTypes)reader.ReadByte();

            switch (type)
            {
                   case EleGraphicalElementTypes.ANIMATED:
                    return AnimatedGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.BLENDED:
                    return BlendedGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.BOUNDING_BOX:
                    return BoundingBoxGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.ENTITY:
                    return EntityGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.NORMAL:
                    return NormalGraphicalElementData.ReadFromStream(instance, id, reader);
                   case EleGraphicalElementTypes.PARTICLES:
                    return ParticlesGraphicalElementData.ReadFromStream(instance, id, reader);
                default:
                    throw new Exception("Unknown graphical data of type " + type);
            }
        }
    }
}