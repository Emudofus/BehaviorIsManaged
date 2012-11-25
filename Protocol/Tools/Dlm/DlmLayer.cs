#region License GNU GPL
// DlmLayer.cs
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
    public class DlmLayer
    {
        public DlmLayer(DlmMap map)
        {
            Map = map;
        }

        public DlmMap Map
        {
            get;
            set;
        }
        public int LayerId
        {
            get;
            set;
        }

        public DlmCell[] Cells
        {
            get;
            set;
        }

        public static DlmLayer ReadFromStream(DlmMap map, IDataReader reader)
        {
            var layer = new DlmLayer(map);

            layer.LayerId = reader.ReadInt();
            layer.Cells = new DlmCell[reader.ReadShort()];
            for (int i = 0; i < layer.Cells.Length; i++)
            {
                layer.Cells[i] = DlmCell.ReadFromStream(layer, reader);
            }

            return layer;
        }
    }
}