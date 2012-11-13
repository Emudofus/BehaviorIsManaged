#region License GNU GPL
// DlmCell.cs
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
    public class DlmCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DlmCell(DlmLayer layer)
        {
            Layer = layer;
        }

        private DlmLayer m_layer;

        public DlmLayer Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        public short Id
        {
            get;
            set;
        }

        public DlmBasicElement[] Elements
        {
            get;
            set;
        }

        public static DlmCell ReadFromStream(DlmLayer layer, BigEndianReader reader)
        {
            var cell = new DlmCell(layer);

            cell.Id = reader.ReadShort();
            cell.Elements = new DlmBasicElement[reader.ReadShort()];

            for (int i = 0; i < cell.Elements.Length; i++)
            {
                DlmBasicElement element =
                    DlmBasicElement.ReadFromStream(cell, reader);
                cell.Elements[i] = element;
            }

            return cell;
        }
    }
}