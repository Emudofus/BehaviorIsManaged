using System.Collections.Generic;
using System.Drawing;
using BiM.Behaviors.Game.Actors;

namespace BiM.Behaviors.Game.World
{
    public interface IContext
    {
        IEnumerable<ContextActor> Actors
        {
            get;
        }

        ContextActor GetContextActor(int id);
        ContextActor[] GetContextActors(Cell cell);
        ContextActor RemoveContextActor(int id);

        void Tick(int dt);
    }
}