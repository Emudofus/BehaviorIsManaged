#region License GNU GPL
// TimedPath.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Pathfinding;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Movements
{
    public class TimedPath
    {
        private readonly TimedPathElement[] m_path;

        public TimedPath(TimedPathElement[] path)
        {
            m_path = path;
            CheckValidity();
        }

        private void CheckValidity()
        {
            for (int i = 0; i < m_path.Length; i++)
            {
                if (i > 0 && m_path[i - 1].EndTime > m_path[i].EndTime)
                    throw new Exception("Incorrect timed path, the dates are not correctly sorted");
            }
        }

        public ReadOnlyCollection<TimedPathElement> Elements
        {
            get { return Array.AsReadOnly(m_path); }
        }

        public TimedPathElement GetCurrentElement()
        {
            for (int i = 0; i < m_path.Length; i++)
            {
                if (m_path[i].EndTime > DateTime.Now)
                {
                    if (i == 0)
                        return m_path[0];

                    return m_path[i];
                }
            }

            return m_path[m_path.Length - 1];
        }

        public Cell GetCurrentCell()
        {
            return GetCurrentElement().CurrentCell;
        }

        public double GetCurrentVelocity()
        {
            return GetCurrentElement().Velocity;
        }

        public double GetBaryCenter(DateTime now)
        {
            var elem = GetCurrentElement();

            return elem.Velocity * ( now - elem.StartTime ).TotalMilliseconds;
        }

        public override string ToString()
        {
            return string.Join(", ", m_path.Select(entry => entry.CurrentCell.Id + "@" + entry.StartTime.ToString("mm:ss.FFFF")));
        }

        public static TimedPath Create(Path path, double hVelocity, double vVelocity, double lVelocity, DateTime referenceDate)
        {
            var cells = path.Cells;
            var result = new List<TimedPathElement>();
                    
            var lastCellTime = referenceDate;
            for (int i = 0; i < cells.Length; i++)
            {
                int direction;

                if (i + 1 < cells.Length)
                    direction = (int)cells[i].OrientationToAdjacent(cells[i + 1]);
                else
                    direction = (int)cells[i - 1].OrientationToAdjacent(cells[i]);

                double velocity;
                if (direction % 4 == 0)
                    velocity = hVelocity;
                else if (direction % 2 == 0)
                    velocity = vVelocity;
                else
                    velocity = lVelocity;

                var end = lastCellTime + TimeSpan.FromMilliseconds(1 / velocity);

                result.Add(new TimedPathElement(cells[i], i + 1 < cells.Length ? cells[i + 1] : null, lastCellTime, end, velocity, (DirectionsEnum)direction));

                lastCellTime = end;
            }

            return new TimedPath(result.ToArray());
        }

    }
}