using System.Threading.Tasks;

namespace BiM.Core.Messages
{
    public class DispatcherTask
    {
        public MessageDispatcher Dispatcher { get; private set; }

        public bool Running
        {
            get;
            private set;
        }

        public object Processor { get; set; }

        public DispatcherTask(MessageDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            Processor = this;
        }

        public DispatcherTask(MessageDispatcher dispatcher, object processor)
        {
            Dispatcher = dispatcher;
            Processor = processor;
        }

        public void Start()
        {
            Running = true;
            Task.Factory.StartNew(Process);
        }

        public void Stop()
        {
            Running = false;
        }

        private void Process()
        {
            while (Running)
            {
                Dispatcher.Wait();

                if (Running)
                    Dispatcher.ProcessDispatching(Processor);
            }
        }
    }
}