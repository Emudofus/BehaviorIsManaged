// Type: System.Collections.ObjectModel.ObservableCollection`1
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
  [TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
  [Serializable]
  public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    private ObservableCollection<T>.SimpleMonitor _monitor = new ObservableCollection<T>.SimpleMonitor();
    private const string CountString = "Count";
    private const string IndexerName = "Item[]";
    [NonSerialized]
    private NotifyCollectionChangedEventHandler CollectionChanged;
    [NonSerialized]
    private PropertyChangedEventHandler PropertyChanged;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
      add
      {
        this.PropertyChanged += value;
      }
      remove
      {
        this.PropertyChanged -= value;
      }
    }

    public virtual event NotifyCollectionChangedEventHandler CollectionChanged
    {
      add
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
      remove
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

    protected virtual event PropertyChangedEventHandler PropertyChanged
    {
      add
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
      remove
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

    public ObservableCollection()
    {
    }

    public ObservableCollection(List<T> list)
      : base(list != null ? (IList<T>) new List<T>(list.Count) : (IList<T>) list)
    {
      this.CopyFrom((IEnumerable<T>) list);
    }

    public ObservableCollection(IEnumerable<T> collection)
    {
      if (collection == null)
        throw new ArgumentNullException("collection");
      this.CopyFrom(collection);
    }

    private void CopyFrom(IEnumerable<T> collection)
    {
      IList<T> items = this.Items;
      if (collection == null || items == null)
        return;
      foreach (T obj in collection)
        items.Add(obj);
    }

    public void Move(int oldIndex, int newIndex)
    {
      this.MoveItem(oldIndex, newIndex);
    }

    protected override void ClearItems()
    {
      this.CheckReentrancy();
      base.ClearItems();
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionReset();
    }

    protected override void RemoveItem(int index)
    {
      this.CheckReentrancy();
      T obj = this[index];
      base.RemoveItem(index);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, (object) obj, index);
    }

    protected override void InsertItem(int index, T item)
    {
      this.CheckReentrancy();
      base.InsertItem(index, item);
      this.OnPropertyChanged("Count");
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Add, (object) item, index);
    }

    protected override void SetItem(int index, T item)
    {
      this.CheckReentrancy();
      T obj = this[index];
      base.SetItem(index, item);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, (object) obj, (object) item, index);
    }

    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
      this.CheckReentrancy();
      T obj = this[oldIndex];
      base.RemoveItem(oldIndex);
      base.InsertItem(newIndex, obj);
      this.OnPropertyChanged("Item[]");
      this.OnCollectionChanged(NotifyCollectionChangedAction.Move, (object) obj, newIndex, oldIndex);
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, e);
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this.CollectionChanged == null)
        return;
      using (this.BlockReentrancy())
        this.CollectionChanged((object) this, e);
    }

    protected IDisposable BlockReentrancy()
    {
      this._monitor.Enter();
      return (IDisposable) this._monitor;
    }

    protected void CheckReentrancy()
    {
      if (this._monitor.Busy && this.CollectionChanged != null && this.CollectionChanged.GetInvocationList().Length > 1)
        throw new InvalidOperationException(SR.GetString("ObservableCollectionReentrancyNotAllowed"));
    }

    private void OnPropertyChanged(string propertyName)
    {
      this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
    }

    private void OnCollectionReset()
    {
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    [TypeForwardedFrom("WindowsBase, Version=3.0.0.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
    [Serializable]
    private class SimpleMonitor : IDisposable
    {
      private int _busyCount;

      public bool Busy
      {
        get
        {
          return this._busyCount > 0;
        }
      }

      public void Enter()
      {
        ++this._busyCount;
      }

      public void Dispose()
      {
        --this._busyCount;
      }
    }
  }
}
