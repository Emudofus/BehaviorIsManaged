using BiM.Behaviors;

namespace BiM.MITM.Network
{
    public class BotMITM : Bot
    {
        public BotMITM(ConnectionMITM connection, NetworkMessageDispatcher dispatcher)
             : base (dispatcher)
        {
            Connection = connection;   
        }

        public ConnectionMITM Connection
        {
            get;
            private set;
        }

        public bool ExpectedDisconnection
        {
            get;
            set;
        }

        public bool Identified
        {
            get;
            set;
        }

        public void ChangeConnection(ConnectionMITM connection)
        {
            Connection = connection;
        }

        public override void Stop()
        {
            if (!Running)
                return;

            if (Connection != null)
                Connection.Disconnect();

            base.Stop();
        }

        public override void Dispose()
        {
            if (Connection != null)
                Connection.Disconnect();

            base.Dispose();
        }
    }
}