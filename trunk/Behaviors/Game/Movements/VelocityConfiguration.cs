namespace BiM.Behaviors.Game.Movements
{
    public class VelocityConfiguration
    {
        public VelocityConfiguration(double horizontalVelocity, double verticalVelocity, double linearVelocity)
        {
            HorizontalVelocity = horizontalVelocity;
            VerticalVelocity = verticalVelocity;
            LinearVelocity = linearVelocity;
        }

        public double HorizontalVelocity
        {
            get;
            private set;
        }

        public double VerticalVelocity
        {
            get;
            private set;
        }

        public double LinearVelocity
        {
            get;
            private set;
        }

    }
}