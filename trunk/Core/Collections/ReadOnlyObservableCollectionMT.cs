using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace BiM.Core.Collections
{
    public class ReadOnlyObservableCollectionMT<T> : ReadOnlyObservableCollection<T>, INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                CollectionChanged += value;
            }
            remove
            {
                CollectionChanged -= value;
            }
        }

        protected override event NotifyCollectionChangedEventHandler CollectionChanged;

        public ReadOnlyObservableCollectionMT(ObservableCollection<T> list)
            : base(list)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
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
                                                            handler, this, args);
                    else // Execute handler as is
                        handler(this, args);
                }
            }
        }
    }
}