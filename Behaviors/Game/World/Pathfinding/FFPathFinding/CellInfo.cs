#region License GNU GPL
// Spell.cs
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

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
  public class CellInfo
  {
    public const int MAP_SIZE = 14;
    public const int NB_CELL = 560;
    public const short CELL_ERROR = -1;
    public const int DEFAULT_DISTANCE = 999;
    public static readonly Color DefaultColor = Color.Green;
    public static readonly Color PathColor = Color.LightGreen;
    public static readonly Color ForestColor = Color.Brown;
    public int weight { get; set; }
    public const int maxWeight = 50;
    public const int defaultWeight = 5;
    private int _distanceSteps = DEFAULT_DISTANCE;
    public int distanceSteps { get { return _distanceSteps; } set { Debug.Assert(value >= 0); _distanceSteps = value; } }
    public byte subMapId { get; set; } // 0 = undefined
    public Cell cell {get; set; }
    public bool isDiagonal { get; set; } // Used to put an higher price on diagonals in PathFinding algorithm 

    public CellInfo()
    {
      x = -1;
      y = -1;
      cellId = -1;
      color = Color.Empty;
      isInPath1 = false;
      isInPath2 = false;
      isWalkable = false;
      isCombatWalkable = false;
      allowLOS = false;
      speed = 1;
      distanceSteps = DEFAULT_DISTANCE;
      subMapId = 0;
    }

    public CellInfo(Cell _cell)
    {
        cell = _cell;
        x = _cell.X;
        y = _cell.Y;
        _cellid = _cell.Id;
        color = DefaultColor;
        isInPath1 = false;
        isInPath2 = false;
        isWalkable = _cell.Walkable;
        isCombatWalkable = !_cell.NonWalkableDuringFight && isWalkable;
        allowLOS = _cell.LineOfSight;
        speed = _cell.Speed;
        distanceSteps = DEFAULT_DISTANCE;
        subMapId = 0;
    }
    public CellInfo(short cellId, bool isWalkable = true, bool isCombatWalkable = true, bool allowLOS = true, bool drawable = true, int speed = 1)
    {
      this.x = x;
      this.y = y;
      this.cellId = cellId;
      this.color = DefaultColor;
      this.isInPath1 = false;
      this.isInPath2 = false;
      this.isWalkable = isWalkable;
      this.isCombatWalkable = isCombatWalkable;
      this.allowLOS = allowLOS;
      this.speed = speed;
    }


    static public short CellIdFromPos(int x, int y)
    {
      int LowPart = (y + (x - y) / 2);
      int HighPart = x - y;
      if (LowPart < 0 || LowPart >= MAP_SIZE) return CELL_ERROR;
      if (HighPart < 0 || HighPart > 39) return CELL_ERROR;
      int result = HighPart * MAP_SIZE + LowPart;
      if (result >= NB_CELL || result < 0) return CELL_ERROR;
      return (short)( (x - y) * MAP_SIZE + y + (x - y) / 2);
    }

    public short getNeighbourCell(int dx, int dy)
    {
      return CellIdFromPos(x + dx, y + dy);
    }

    public int x { get; set; }
    public int y { get; set; }

    private short _cellid;
    public short cellId
    {
      get
      {
        return _cellid;
      }
      set
      {
        _cellid = value;
        //(cellId Mod 14) + cellId / 28
        if (value < 0) return;

        //x = (value % 14) + (int)((double)value  / 14.0); // Faux
        y = (value % 14) - (value - value % 28) / 28; // OK
        x = (short)(0.5 + value / 14.5 + y * 13.5 / 14.5); // OK.... even if I don't know why

        //Debug.Assert(_cellid == CellIdFromPos(x, y));
        //if (_cellid != CellIdFromPos(x, y))
        //  Debug.Print("CellId:{0} => [{1},{2}] => {3}", _cellid, x, y, CellIdFromPos(x, y));
      }
    }
    public string label { get; set; }
    public bool isInPath1 { get; set; }
    public bool isInPath2 { get; set; }
    public bool isWalkable { get; set; }
    public bool isCombatWalkable { get; set; }
    public bool allowLOS { get; set; }
    public Color color { get; set; }
    public uint mapLink { get; set; }
    public int gfxCount { get; set; }
    public uint firstGfx { get; set; }
    private int _speed;

    public int speed
    {
      get
      {
        return _speed;
      }
      set
      {
        _speed = value;
        if (value == 0)
        {
          weight = defaultWeight;
          color = DefaultColor;
        }
        else
        {
          if (speed < 0)
          {
            weight = defaultWeight * (1 - speed);
            color = ForestColor;
            if (weight > maxWeight)
              weight = maxWeight;
          }
          else
            if (speed > 0)
            {
              color = PathColor;
              weight = defaultWeight / (1 + speed);
              if (weight < 1) weight = 1;
            }
        }
      }
    }

    public enum FieldNames { Nothing, Label, isInPath, isWalkable, isCombatWalkable, allowLOS, color, speed, coordinates, PathFindingInformations, mapLink, cellID, gfxCount, firstGfx };

    static public Array FillCombo()
    {
      return Enum.GetValues(typeof(FieldNames));      
    }

    public string getValue(FieldNames whichElement)
    {
      switch (whichElement)
      {
        case FieldNames.Nothing:
          return "";
        case FieldNames.Label:
          return label;
        case FieldNames.isInPath:
          return isInPath1.ToString();
        case FieldNames.isWalkable:
          return isWalkable.ToString();
        case FieldNames.isCombatWalkable:
          return isCombatWalkable.ToString();
        case FieldNames.allowLOS:
          return allowLOS.ToString();
        case FieldNames.color:
          return color.ToString();
        case FieldNames.speed:
          return speed.ToString();
        case FieldNames.coordinates:
          return String.Format("{0},{1}", x, y);
        case FieldNames.PathFindingInformations:
          return distanceSteps.ToString();
        case FieldNames.mapLink:
          return mapLink.ToString();
        case FieldNames.cellID:
          return this.cellId.ToString();
        case FieldNames.firstGfx:
          return this.firstGfx.ToString();
        case FieldNames.gfxCount:
          return this.gfxCount.ToString();
        default:
          return "???";
      }

    }
  }
}
