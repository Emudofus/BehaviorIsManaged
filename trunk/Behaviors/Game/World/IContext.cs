using System.Drawing;
using BiM.Behaviors.Game.Actors;

namespace BiM.Behaviors.Game.World
{
    public interface IContext
    {
        ContextActor[] GetActors(Cell cell);
        ContextActor RemoveActor(int id);
    }
}