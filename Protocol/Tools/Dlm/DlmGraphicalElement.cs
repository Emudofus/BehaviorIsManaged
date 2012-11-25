#region License GNU GPL
// DlmGraphicalElement.cs
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
    public class DlmGraphicalElement : DlmBasicElement
    {
        public const float CELL_HALF_WIDTH = 43;
        public const float CELL_HALF_HEIGHT = 21.5f;

        private int m_altitude;
        private uint m_elementId;
        private ColorMultiplicator m_finalTeint;
        private ColorMultiplicator m_hue;
        private uint m_identifier;
        private System.Drawing.Point m_offset;
        private System.Drawing.Point m_pixelOffset;
        private ColorMultiplicator m_shadow;

        public DlmGraphicalElement(DlmCell cell)
            : base(cell)
        {

        }

        public ElementTypesEnum ElementType
        {
            get { return DlmBasicElement.ElementTypesEnum.Graphical; }
        }

        public ColorMultiplicator ColorMultiplicator
        {
            get { return m_finalTeint; }
        }

        public int Altitude
        {
            get { return m_altitude; }
            set { m_altitude = value; }
        }

        public uint ElementId
        {
            get { return m_elementId; }
            set { m_elementId = value; }
        }

        public ColorMultiplicator FinalTeint
        {
            get { return m_finalTeint; }
            set { m_finalTeint = value; }
        }

        public ColorMultiplicator Hue
        {
            get { return m_hue; }
            set { m_hue = value; }
        }

        public uint Identifier
        {
            get { return m_identifier; }
            set { m_identifier = value; }
        }

        public System.Drawing.Point Offset
        {
            get { return m_offset; }
            set { m_offset = value; }
        }

        public System.Drawing.Point PixelOffset
        {
            get { return m_pixelOffset; }
            set { m_pixelOffset = value; }
        }

        public ColorMultiplicator Shadow
        {
            get { return m_shadow; }
            set { m_shadow = value; }
        }

        public new static DlmGraphicalElement ReadFromStream(DlmCell cell, IDataReader reader)
        {
            var element = new DlmGraphicalElement(cell);

            element.m_elementId = reader.ReadUInt();
            element.m_hue = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), false);
            element.m_shadow = new ColorMultiplicator(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), false);

            if (cell.Layer.Map.Version <= 4)
            {
                element.m_offset.X = reader.ReadByte();
                element.m_offset.Y = reader.ReadByte();
                element.m_pixelOffset.X = (int) (element.m_offset.X * CELL_HALF_WIDTH);
                element.m_pixelOffset.Y = (int) (element.m_offset.Y * CELL_HALF_HEIGHT);
            }
            else
            {
                element.m_pixelOffset.X = reader.ReadShort();
                element.m_pixelOffset.Y = reader.ReadShort();
                element.m_offset.X = (int)( element.m_pixelOffset.X / CELL_HALF_WIDTH );
                element.m_offset.Y = (int)( element.m_pixelOffset.Y / CELL_HALF_HEIGHT );
            }

            element.m_altitude = reader.ReadByte();
            element.m_identifier = reader.ReadUInt();

            // we don't care
            //element.CalculateFinalTeint();

            return element;
        }

        public void CalculateFinalTeint()
        {
            var loc1 = m_hue.Red + m_shadow.Red;
            var loc2 = m_hue.Green + m_shadow.Green;
            var loc3 = m_hue.Blue + m_shadow.Blue;

            loc1 = ColorMultiplicator.Clamp((loc1 + 128)*2, 0, 512);
            loc2 = ColorMultiplicator.Clamp((loc2 + 128)*2, 0, 512);
            loc3 = ColorMultiplicator.Clamp((loc3 + 128)*2, 0, 512);

            m_finalTeint = new ColorMultiplicator(loc1, loc2, loc3, true);
        }
    }
}