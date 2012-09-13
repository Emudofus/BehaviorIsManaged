namespace BiM.Behaviors.Game.World.Pathfinding
{
    public interface IOpenList
    {
        void Push(Cell cell);
        Cell Pop();

        int Count
        {
            get;
        }
    }
}