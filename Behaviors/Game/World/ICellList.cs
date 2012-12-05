using System.Collections.Generic;
using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World
{
    public interface ICellList<out T> : IEnumerable<T>
        where T : ICell
    {
        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T this[int id]
        {
            get;
        }
    }
}