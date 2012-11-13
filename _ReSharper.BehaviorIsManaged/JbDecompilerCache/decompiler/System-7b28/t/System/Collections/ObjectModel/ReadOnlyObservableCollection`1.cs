// Type: System.Collections.ObjectModel.ReadOnlyObservableCollection`1
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
  [TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
  [__DynamicallyInvokable]
  [Serializable]
  public class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    [NonSerialized]
    private NotifyCollectionChangedEventHandler CollectionChanged;
    [NonSerialized]
    private PropertyChangedEventHandler PropertyChanged;

    [__DynamicallyInvokable]
    event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] add
      {
        this.CollectionChanged += value;
      }
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] remove
      {
        this.CollectionChanged -= value;
      }
    }

    [__DynamicallyInvokable]
    protected virtual event NotifyCollectionChangedEventHandler CollectionChanged
    {
      [__DynamicallyInvokable] add
      {
        NotifyCollectionChangedEventHandler changedEventHandler = this.CollectionChanged;
        NotifyCollectionChangedEventHandler comparand;
        do
        {
          comparand = changedEventHandler;
          changedEventHandler = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, comparand + value, comparand);
        }
        while (changedEventHandler != comparand);
      }
      [__DynamicallyInvokable] remove
      {
        NotifyCollectionChangedEventHandler changedEventHandler = this.CollectionChanged;
        NotifyCollectionChangedEventHandler comparand;
        do
        {
          comparand = changedEventHandler;
          changedEventHandler = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, comparand - value, comparand);
        }
        while (changedEventHandler != comparand);
      }
    }

    [__DynamicallyInvokable]
    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] add
      {
        this.PropertyChanged += value;
      }
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] remove
      {
        this.PropertyChanged -= value;
      }
    }

    [__DynamicallyInvokable]
    protected virtual event PropertyChangedEventHandler PropertyChanged
    {
      [__DynamicallyInvokable] add
      {
        PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
        PropertyChangedEventHandler comparand;
        do
        {
          comparand = changedEventHandler;
          changedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, comparand + value, comparand);
        }
        while (changedEventHandler != comparand);
      }
      [__DynamicallyInvokable] remove
      {
        PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
        PropertyChangedEventHandler comparand;
        do
        {
          comparand = changedEventHandler;
          changedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, comparand - value, comparand);
        }
        while (changedEventHandler != comparand);
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyObservableCollection(ObservableCollection<T> list)
      : base((IList<T>) list)
    {
      ((INotifyCollectionChanged) this.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(this.HandleCollectionChanged);
      ((INotifyPropertyChanged) this.Items).PropertyChanged += new PropertyChangedEventHandler(this.HandlePropertyChanged);
    }

    [__DynamicallyInvokable]
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
      if (this.CollectionChanged == null)
        return;
      this.CollectionChanged((object) this, args);
    }

    [__DynamicallyInvokable]
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, args);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.OnCollectionChanged(e);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.OnPropertyChanged(e);
    }
  }
}
