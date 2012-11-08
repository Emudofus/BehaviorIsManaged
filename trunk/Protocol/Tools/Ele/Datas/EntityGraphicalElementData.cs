#region License GNU GPL
// EntityGraphicalElementData.cs
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
    public class EntityGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EntityGraphicalElementData(EleInstance instance, int id)
            : base(instance, id)
        {
        }

        public string EntityLook
        {
            get;
            set;
        }

        public bool HorizontalSymmetry
        {
            get;
            set;
        }

        public bool PlayAnimation
        {
            get;
            set;
        }

        public bool PlayAnimStatic
        {
            get;
            set;
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

        public override EleGraphicalElementTypes Type
        {
            get { return EleGraphicalElementTypes.ENTITY; }
        }

        public static EntityGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new EntityGraphicalElementData(instance, id);

            data.EntityLook = reader.ReadUTF7BitLength();
            data.HorizontalSymmetry = reader.ReadBoolean();

            if (instance.Version >= 7)
            {
                data.PlayAnimation = reader.ReadBoolean();
            }

            if (instance.Version >= 6)
            {
                data.PlayAnimStatic = reader.ReadBoolean();
            }

            if (instance.Version >= 5)
            {
                data.MinDelay = reader.ReadUInt();
                data.MaxDelay = reader.ReadUInt();
            }

            return data;
        }
    }
}