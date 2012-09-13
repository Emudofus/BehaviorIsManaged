using System;
using BiM.Behaviors.Game.World;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.Movements
{
    public class TimedPathElement
    {
        public TimedPathElement(Cell currentCell, Cell nextCell, DateTime startTime, DateTime endTime, double velocity, DirectionsEnum direction)
        {
            CurrentCell = currentCell;
            NextCell = nextCell;
            StartTime = startTime;
            EndTime = endTime;
            Velocity = velocity;
            Direction = direction;
        }

        public Cell CurrentCell
        {
            get;
            private set;
        }

        public Cell NextCell
        {
            get;
            private set;
        }

        public DateTime StartTime
        {
            get;
            private set;
        }

        public DateTime EndTime
        {
            get;
            private set;
        }

        public double Velocity
        {
            get;
            private set;
        }

        public DirectionsEnum Direction
        {
            get;
            private set;
        }
    }
}