using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace BiM.Core.Collections
{
    public class ObservableCollectionMT<T> : ObservableCollection<T>
    {
        public ObservableCollectionMT()
        {
        }

        public ObservableCollectionMT(List<T> list)
            : base(list)
        {
        }

        public ObservableCollectionMT(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Be nice - use BlockReentrancy like MSDN said
            using (BlockReentrancy())
            {
                NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
                if (eventHandler != null)
                {
                    Delegate[] delegates = eventHandler.GetInvocationList();
                    // Walk thru invocation list
                    foreach (NotifyCollectionChangedEventHandler handler in delegates)
                    {
                        var dispatcherObject = handler.Target as DispatcherObject;
                        // If the subscriber is a DispatcherObject and different thread
                        if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                            // Invoke handler in the target dispatcher's thread
                            dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind,
                                                               handler, this, e);
                        else // Execute handler as is
                            handler(this, e);
                    }
                }
            }
        }
    }
}