namespace BiM.Behaviors.Frames
{
    public interface IFrame
    {
        Bot Bot
        {
            get;
        }

        void OnAttached();
        void OnDetached();
    }
}