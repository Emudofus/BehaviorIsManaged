using System.Drawing;
using BiM.Behaviors.Game.Actors;

namespace BiM.Behaviors.Game.World
{
    public interface IContext
    {
        ContextActor GetContextActor(int id);
        ContextActor[] GetContextActors(Cell cell);
        ContextActor RemoveContextActor(int id);
    }
}