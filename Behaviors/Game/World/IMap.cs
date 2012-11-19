#region License GNU GPL
// IMap.cs
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

using System.Drawing;
using BiM.Protocol.Tools.Dlm;

namespace BiM.Behaviors.Game.World
{
    public interface IMap
    {
        CellList Cells
        {
            get;
        }

        int Id
        {
            get;
        }

        byte Version
        {
            get;
        }

        bool Encrypted
        {
            get;
        }

        byte EncryptionVersion
        {
            get;
        }

        uint RelativeId
        {
            get;
        }

        byte MapType
        {
            get;
        }

        int SubAreaId
        {
            get;
        }

        int BottomNeighbourId
        {
            get;
        }

        int LeftNeighbourId
        {
            get;
        }

        int RightNeighbourId
        {
            get;
        }

        int ShadowBonusOnEntities
        {
            get;
        }

        Color BackgroundColor
        {
            get;
        }

        ushort ZoomScale
        {
            get;
        }

        short ZoomOffsetX
        {
            get;
        }

        short ZoomOffsetY
        {
            get;
        }

        bool UseLowPassFilter
        {
            get;
        }

        bool UseReverb
        {
            get;
        }

        int PresetId
        {
            get;
        }

        DlmFixture[] BackgroudFixtures
        {
            get;
        }

        int TopNeighbourId
        {
            get;
        }

        DlmFixture[] ForegroundFixtures
        {
            get;
        }

        int GroundCRC
        {
            get;
        }

        DlmLayer[] Layers
        {
            get;
        }

        bool UsingNewMovementSystem
        {
            get;
        }
    }
}