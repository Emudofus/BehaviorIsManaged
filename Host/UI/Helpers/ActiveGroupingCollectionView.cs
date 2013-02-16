using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;
using Expression = System.Linq.Expressions.Expression;

// credits = http://activecollectionview.codeplex.com

namespace BiM.Host.UI.Helpers
{
    /// <summary>
    /// Active grouping collection view.
    /// </summary>
    public sealed class ActiveGroupingCollectionView : CollectionView, IMultiValueConverter
    {
        #region Private Static Variables 

        private static readonly Action<CollectionView, NotifyCollectionChangedEventArgs> _validateCollectionChangedAction = CreateValidateCollectionChangedAction();
        private static readonly Func<CollectionView, object> _syncRootFunction = CreateSyncRootFunction();

        #endregion Private Static Variables 

        #region Private Static Methods 

        /// <summary>
        /// Clears the binding for group values on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        private static void ClearGroupValuesBinding(ActiveGroupingCollectionViewItem item)
        {
            BindingOperations.ClearBinding(item, ActiveGroupingCollectionViewItem.GroupValuesProperty);
        }

        /// <summary>
        /// Creates a function which given a <see cref="T:System.Windows.Data.CollectionView"/> gets the value of 
        /// <see cref="P:System.Windows.Data.CollectionView.SyncRoot"/>.
        /// </summary>
        /// <returns>
        /// Func.
        /// </returns>
        private static Func<CollectionView, object> CreateSyncRootFunction()
        {
            ParameterExpression collectionViewParameter = Expression.Parameter(
                typeof (CollectionView),
                typeof (CollectionView).Name
                );
            return Expression.Lambda<Func<CollectionView, object>>(
                Expression.Property(
                    collectionViewParameter,
                    "SyncRoot"
                    ),
                collectionViewParameter
                ).Compile();
        }

        /// <summary>
        /// Creates an action which given a <see cref="T:System.Windows.Data.CollectionView"/> and a <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs"/>, 
        /// invokes <see cref="M:System.Windows.Data.CollectionView.ValidateCollectionChanged"/> with the parameter.
        /// </summary>
        /// <returns>
        /// Action.
        /// </returns>
        private static Action<CollectionView, NotifyCollectionChangedEventArgs> CreateValidateCollectionChangedAction()
        {
            ParameterExpression collectionViewParameter = Expression.Parameter(
                typeof (CollectionView),
                typeof (CollectionView).Name
                );
            ParameterExpression eventArgsParameter = Expression.Parameter(
                typeof (NotifyCollectionChangedEventArgs),
                typeof (NotifyCollectionChangedEventArgs).Name
                );
            return Expression.Lambda<Action<CollectionView, NotifyCollectionChangedEventArgs>>(
                Expression.Call(
                    collectionViewParameter,
                    "ValidateCollectionChangedEventArgs",
                    Type.EmptyTypes,
                    eventArgsParameter
                    ),
                collectionViewParameter,
                eventArgsParameter
                ).Compile();
        }

        /// <summary>
        /// Gets the first item in the specified group that is not itself a group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        /// Object.
        /// </returns>
        private static object GetFirstItem(ActiveGroupingCollectionViewGroup group)
        {
            if (group.Items.Count != 0)
            {
                if (group.IsBottomLevel)
                {
                    return group.Items[0];
                }
                else
                {
                    return GetFirstItem((ActiveGroupingCollectionViewGroup) group.Items[0]);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the string comparer for the given comparison type.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        /// <returns>
        /// String comparer.
        /// </returns>
        private static StringComparer GetStringComparer(StringComparison comparison)
        {
            switch (comparison)
            {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;
                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
            }
            return StringComparer.Ordinal;
        }

        #endregion Private Static Methods 

        #region Public Static Methods 

        /// <summary>
        /// Removes the specified item from its groups.  If this causes there to be no items within that group, the group is removed.
        /// </summary>
        /// <param name="item">The item.</param>
        internal static void Ungroup(ActiveGroupingCollectionViewItem item)
        {
            ActiveGroupingCollectionViewGroup group = item.Group;
            object itemToRemove = item.SourceCollectionItem;
            do
            {
                if (group.InternalItems.Remove(itemToRemove))
                {
                    if (--group.InternalItemCount == 0)
                    {
                        if (group.ParentGroup == null)
                        {
                            group.CollectionView._groups.Remove(group);
                            break;
                        }
                        else
                        {
                            itemToRemove = group;
                            group = group.ParentGroup;
                        }
                    }
                    else
                    {
                        while ((group = group.ParentGroup) != null)
                        {
                            group.InternalItemCount--;
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            } while (true);
            var comparer = item.Group.CollectionView._sortComparerUsed as SortDescriptionCollectionComparer;
            if (comparer != null)
            {
                comparer.ClearValuesForItem(item.SourceCollectionItem);
            }
            item.Group = null;
        }

        #endregion Public Static Methods 

        #region Private Variables 

        private readonly ObservableCollection<GroupDescription> _groupDescriptions;
        private readonly ObservableCollection<object> _groups;
        private readonly ReadOnlyObservableCollection<object> _groupsWrapper;
        private readonly ActiveGroupingCollectionViewItemCollection _items;
        private IComparer _customSort;
        private bool _isGrouping;
        private IComparer _sortComparerUsed;
        private SortDescriptionCollection _sortDescriptions;

        #endregion Private Variables 

        #region Private Properties 

        /// <summary>
        /// Gets a value indicating whether this instance's current item is in view.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance's current item is in view; otherwise, <c>false</c>.
        /// </value>
        private bool IsCurrentInView
        {
            get { return CurrentPosition >= 0 && CurrentPosition <= (Count - 1); }
        }

        /// <summary>
        /// Returns the underlying unfiltered collection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerable"/> object that is the underlying collection.
        /// </returns>
        private new IList SourceCollection
        {
            get { return (IList) base.SourceCollection; }
        }

        #endregion Private Properties 

        #region Public Properties 

        /// <summary>
        /// Gets a value that indicates whether the view supports grouping.
        /// </summary>
        /// <returns><c>true</c> in all cases.</returns>
        public override bool CanGroup
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value that indicates whether the view supports sorting.
        /// </summary>
        /// <returns><c>true</c> in all cases.</returns>
        public override bool CanSort
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the number of records in the view.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of records in the view, or –1 if the number of records is unknown.
        /// </returns>
        public override int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Returns an object that you can use to sort items in the view.
        /// </summary>
        /// <value>A custom sorting comparer.</value>
        /// <returns>
        /// An <see cref="T:System.Collections.IComparer"/> object that you can use to sort items in the view.
        /// </returns>
        public IComparer CustomSort
        {
            get { return _customSort; }
            set
            {
                _customSort = value;
                SetSortDescriptions(null);
                RefreshOrDefer();
            }
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.GroupDescription"/> objects that describes how the items in the collection are grouped in the view.
        /// </summary>
        /// <value></value>
        /// <returns>null in all cases.
        /// </returns>
        public override ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return _groupDescriptions; }
        }

        /// <summary>
        /// Gets a collection of the top-level groups that is constructed based in the <see cref="P:System.Windows.Data.CollectionView.GroupDescriptions"/> property.
        /// </summary>
        /// <value></value>
        /// <returns>null in all cases.
        /// </returns>
        public override ReadOnlyObservableCollection<object> Groups
        {
            get
            {
                if (_isGrouping)
                {
                    return _groupsWrapper;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the resulting (filtered) view is empty.
        /// </summary>
        /// <value></value>
        /// <returns>true if the resulting view is empty; otherwise, false.
        /// </returns>
        public override bool IsEmpty
        {
            get { return _items.Count == 0; }
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.ComponentModel.SortDescription"/> structures that describes how the items in the collection are sorted in the view.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An empty <see cref="T:System.ComponentModel.SortDescriptionCollection"/> in all cases.
        /// </returns>
        public override SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (_sortDescriptions == null)
                {
                    SetSortDescriptions(new SortDescriptionCollection());
                }
                return _sortDescriptions;
            }
        }

        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        internal object SyncRoot
        {
            get { return _syncRootFunction.Invoke(this); }
        }

        #endregion Public Properties 

        #region Private Methods 

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds the items collection.
        /// </summary>
        private void BuildItemsCollection()
        {
            for (int index = 0; index < SourceCollection.Count; index++)
            {
                object sourceItem = SourceCollection[index];
                if (PassesFilter(sourceItem))
                {
                    var item = new ActiveGroupingCollectionViewItem
                                   {
                                       SourceCollectionIndex = index,
                                       SourceCollectionItem = sourceItem
                                   };
                    if (_isGrouping)
                    {
                        SetGroupValuesBinding(item);
                    }
                    _items.Add(item);
                }
            }
        }

        /// <summary>
        /// Changes the item index and consequently the key.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="newIndex">The new index.</param>
        private void ChangeItemKey(ActiveGroupingCollectionViewItem item, int newIndex)
        {
            _items.ChangeItemKey(item, newIndex);
            item.SourceCollectionIndex = newIndex;
        }

        /// <summary>
        /// Clears the items collection.
        /// </summary>
        private void ClearItemsCollection()
        {
            for (int index = _items.Count - 1; index != -1; index--)
            {
                ClearGroupValuesBinding(_items[index]);
                _items.RemoveAt(index);
            }
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="comparand">The comparand.</param>
        /// <param name="other">The other item the <paramref name="comparand"/> is to be compared against.</param>
        /// <returns>Int32.</returns>
        private int CompareItems(ActiveGroupingCollectionViewItem comparand, ActiveGroupingCollectionViewItem other)
        {
            return _sortComparerUsed.Compare(comparand.SourceCollectionItem, other.SourceCollectionItem);
        }

        /// <summary>
        /// Finds the group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="groupDescriptionIndex">Index of the group description.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="parentGroup">The parent group.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemsAlreadySorted">if set to <c>true</c> items already sorted.</param>
        /// <returns>Active grouping collection view group.</returns>
        private ActiveGroupingCollectionViewGroup FindGroup(object name, int groupDescriptionIndex, ObservableCollection<object> collection, ActiveGroupingCollectionViewGroup parentGroup, object item,
                                                            bool itemsAlreadySorted)
        {
            IEqualityComparer comparer = GetEqualityComparer(name, groupDescriptionIndex);
            ActiveGroupingCollectionViewGroup group;
            for (int i = 0; i < collection.Count; i++)
            {
                if (comparer.Equals((group = (ActiveGroupingCollectionViewGroup) collection[i]).Name, name))
                {
                    group.InternalItemCount++;
                    return group;
                }
            }
            group = new ActiveGroupingCollectionViewGroup(name, groupDescriptionIndex, this, parentGroup);
            if (itemsAlreadySorted || _sortComparerUsed == null || collection.Count == 0)
            {
                collection.Add(group);
            }
            else
            {
                bool inserted = false;
                for (int i = 0; i < collection.Count; i++)
                {
                    if (_sortComparerUsed.Compare(item, GetFirstItem((ActiveGroupingCollectionViewGroup) collection[i])) != 1)
                    {
                        collection.Insert(i, group);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    collection.Add(group);
                }
            }
            group.InternalItemCount++;
            return group;
        }

        /// <summary>
        /// Gets the equality comparer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="groupDescriptionIndex">Index of the group description.</param>
        /// <returns>
        /// Equality comparer.
        /// </returns>
        private IEqualityComparer GetEqualityComparer(object value, int groupDescriptionIndex)
        {
            if (value is string)
            {
                var description = (PropertyGroupDescription) _groupDescriptions[groupDescriptionIndex];
                return GetStringComparer(description.StringComparison);
            }
            return EqualityComparer<object>.Default;
        }

        /// <summary>
        /// Handles the CollectionChanged event of the GroupDescriptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void GroupDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshOrDefer();
        }

        /// <summary>
        /// Inserts a bottom level item in the group.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="group">The group.</param>
        /// <param name="itemsAlreadySorted">if set to <c>true</c> items already sorted.</param>
        private void InsertBottomLevelGroupItem(ActiveGroupingCollectionViewItem item, ActiveGroupingCollectionViewGroup group, bool itemsAlreadySorted)
        {
            if (group.InternalItems.Count == 0 || itemsAlreadySorted || _sortComparerUsed == null)
            {
                group.InternalItems.Add(item.SourceCollectionItem);
            }
            else
            {
                // Can't use BinarySearch as items sorting criteria may have changed.
                bool inserted = false;
                for (int i = 0; i < group.InternalItems.Count; i++)
                {
                    if (_sortComparerUsed.Compare(item.SourceCollectionItem, group.InternalItems[i]) != 1)
                    {
                        group.InternalItems.Insert(i, item.SourceCollectionItem);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    group.InternalItems.Add(item.SourceCollectionItem);
                }
            }
            //group.InternalItemCount++;
            item.Group = group;
        }

        /// <summary>
        /// Inserts the item into the items collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Int32.
        /// </returns>
        private int InsertItem(ActiveGroupingCollectionViewItem item)
        {
            if (_sortComparerUsed != null)
            {
                for (int index = 0; index < _items.Count; index++)
                {
                    if (_sortComparerUsed.Compare(item.SourceCollectionItem, _items[index].SourceCollectionItem) != 1)
                    {
                        _items.Insert(index, item);
                        return index;
                    }
                }
            }
            if (_items.Count <= item.SourceCollectionIndex)
            _items.Add(item);
            return _items.Count;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Processes the add.
        /// </summary>
        /// <param name="sourceCollectionIndex">Index of the source collection.</param>
        /// <param name="newItem">The new item.</param>
        /// <returns>
        /// Boolean.
        /// </returns>
        private bool ProcessAdd(int sourceCollectionIndex, object newItem)
        {
            int insertionIndex;
            ActiveGroupingCollectionViewItem item;
            lock (SyncRoot)
            {
                // Move indices down if not inserted at tail end.
                if (sourceCollectionIndex != (SourceCollection.Count - 1))
                {
                    for (int index = SourceCollection.Count - 2; index >= sourceCollectionIndex; index--)
                    {
                        if (_items.TryGetItem(index, out item))
                        {
                            ChangeItemKey(item, index + 1);
                        }
                    }
                }
                if (PassesFilter(newItem))
                {
                    insertionIndex = InsertItem(item = new ActiveGroupingCollectionViewItem
                                                           {
                                                               SourceCollectionIndex = sourceCollectionIndex,
                                                               SourceCollectionItem = SourceCollection[sourceCollectionIndex]
                                                           });
                    if (_isGrouping)
                    {
                        SetGroupValuesBinding(item);
                        Group(item, false);
                    }
                }
                else
                {
                    insertionIndex = -1;
                }
            }
            if (insertionIndex != -1)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, insertionIndex));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the move.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        /// <param name="oldIndex">The old index.</param>
        private void ProcessMove(int newIndex, int oldIndex)
        {
            ActiveGroupingCollectionViewItem item;
            lock (SyncRoot)
            {
                if (_items.TryGetItem(oldIndex, out item))
                {
                    ChangeItemKey(item, -1);
                }
                ActiveGroupingCollectionViewItem other;
                if (newIndex > oldIndex)
                {
                    for (int index = oldIndex + 1; index <= newIndex; index++)
                    {
                        if (_items.TryGetItem(index, out other))
                        {
                            ChangeItemKey(other, index - 1);
                        }
                    }
                }
                else
                {
                    for (int index = oldIndex - 1; index >= newIndex; index--)
                    {
                        if (_items.TryGetItem(index, out other))
                        {
                            ChangeItemKey(other, index + 1);
                        }
                    }
                }
                if (item != null)
                {
                    ChangeItemKey(item, newIndex);
                }
            }
        }

        /// <summary>
        /// Processes the remove.
        /// </summary>
        /// <param name="sourceCollectionIndex">Index of the item in the source collection.</param>
        /// <returns>
        /// Boolean.
        /// </returns>
        private bool ProcessRemove(int sourceCollectionIndex)
        {
            ActiveGroupingCollectionViewItem item;
            int itemIndex = -1;
            lock (SyncRoot)
            {
                if (_items.TryGetItem(sourceCollectionIndex, out item))
                {
                    if (_isGrouping)
                    {
                        ClearGroupValuesBinding(item);
                        Ungroup(item);
                    }
                    _items.RemoveAt(itemIndex = _items.IndexOf(item));
                }
                if (sourceCollectionIndex != SourceCollection.Count)
                {
                    ActiveGroupingCollectionViewItem other;
                    for (int index = sourceCollectionIndex + 1; index <= SourceCollection.Count; index++)
                    {
                        if (_items.TryGetItem(index, out other))
                        {
                            ChangeItemKey(other, index - 1);
                        }
                    }
                }
            }
            if (itemIndex != -1)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item.SourceCollectionItem, itemIndex));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the replace.
        /// </summary>
        /// <param name="sourceCollectionIndex">Index of the item in the source collection.</param>
        /// <param name="newItem">The new item.</param>
        /// <returns>Boolean.</returns>
        private bool ProcessReplace(int sourceCollectionIndex, object newItem)
        {
            ActiveGroupingCollectionViewItem item;
            bool itemAlreadyExists = _items.TryGetItem(sourceCollectionIndex, out item);
            bool passesFilter = PassesFilter(newItem);
            NotifyCollectionChangedEventArgs args = null;
            lock (SyncRoot)
            {
                if (passesFilter && itemAlreadyExists)
                {
                    if (_isGrouping)
                    {
                        ClearGroupValuesBinding(item);
                        Ungroup(item);
                    }
                    object oldItem = item.SourceCollectionItem;
                    item.SourceCollectionItem = newItem;
                    if (_isGrouping)
                    {
                        SetGroupValuesBinding(item);
                        Group(item, false);
                    }
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, _items.IndexOf(item));
                }
                else if (passesFilter)
                {
                    int insertionIndex = InsertItem(item = new ActiveGroupingCollectionViewItem
                                                               {
                                                                   SourceCollectionItem = newItem,
                                                                   SourceCollectionIndex = sourceCollectionIndex
                                                               });
                    if (_isGrouping)
                    {
                        SetGroupValuesBinding(item);
                        Group(item, false);
                    }
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, insertionIndex);
                }
                else if (itemAlreadyExists)
                {
                    if (_isGrouping)
                    {
                        ClearGroupValuesBinding(item);
                        Ungroup(item);
                    }
                    int currentIndex = _items.IndexOf(item);
                    _items.RemoveAt(currentIndex);
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item.SourceCollectionItem, currentIndex);
                }
                else
                {
                    return false;
                }
            }
            OnCollectionChanged(args);
            return true;
        }

        /// <summary>
        /// Sets a <see cref="T:System.Windows.Data.MultiBinding"/> for change notifications on the grouping criteria.
        /// </summary>
        /// <param name="item">The item.</param>
        private void SetGroupValuesBinding(ActiveGroupingCollectionViewItem item)
        {
            var multiBinding = new MultiBinding {Converter = this};
            for (int i = 0; i < _groupDescriptions.Count; i++)
            {
                var propertyGroupDescription = _groupDescriptions[i] as PropertyGroupDescription;
                if (propertyGroupDescription == null)
                {
                    throw new NotSupportedException(String.Format(Culture, "Only {0} are supported at this time.", typeof (PropertyGroupDescription)));
                }
                multiBinding.Bindings.Add(new Binding(propertyGroupDescription.PropertyName)
                                              {
                                                  Source = item.SourceCollectionItem,
                                                  Converter = propertyGroupDescription.Converter
                                              });
            }
            BindingOperations.SetBinding(item, ActiveGroupingCollectionViewItem.GroupValuesProperty, multiBinding);
        }

        /// <summary>
        /// Sets the sort descriptions.
        /// </summary>
        /// <param name="sortDescriptions">The sort descriptions.</param>
        private void SetSortDescriptions(SortDescriptionCollection sortDescriptions)
        {
            if (_sortDescriptions != null)
            {
                (_sortDescriptions as INotifyCollectionChanged).CollectionChanged -= SortDescriptions_CollectionChanged;
            }
            if ((_sortDescriptions = sortDescriptions) != null)
            {
                (_sortDescriptions as INotifyCollectionChanged).CollectionChanged += SortDescriptions_CollectionChanged;
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event of the SortDescriptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void SortDescriptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_sortDescriptions.Count != 0)
            {
                _customSort = null;
            }
            RefreshOrDefer();
        }

        /// <summary>
        /// Sorts the using comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        private void SortUsingComparer(IComparer comparer)
        {
            _sortComparerUsed = comparer;
            _items.Sort(CompareItems);
        }

        /// <summary>
        /// Validates the collection changed event args do not contain invalid values.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ValidateCollectionChangedEventArgs(NotifyCollectionChangedEventArgs e)
        {
            _validateCollectionChangedAction.Invoke(this, e);
        }

        #endregion Private Methods 

        #region Protected Methods 

        /// <summary>
        /// Returns an object that you can use to enumerate the items in the view.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that you can use to enumerate the items in the view.
        /// </returns>
        protected override IEnumerator GetEnumerator()
        {
            foreach (ActiveGroupingCollectionViewItem item in _items)
            {
                yield return item.SourceCollectionItem;
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a single change on the UI thread.
        /// </summary>
        /// <param name="args">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> object to process.</param>
        protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                RefreshOrDefer();
            }
            else
            {
                ValidateCollectionChangedEventArgs(args);
                object current = CurrentItem;
                bool isCurrentAfterLast = IsCurrentAfterLast;
                bool isCurrentBeforeFirst = IsCurrentBeforeFirst;
                int currentPosition = CurrentPosition;
                bool processChange = false;
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        processChange = ProcessAdd(args.NewStartingIndex, args.NewItems[0]);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        processChange = ProcessRemove(args.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        processChange = ProcessReplace(args.NewStartingIndex, args.NewItems[0]);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        ProcessMove(args.NewStartingIndex, args.OldStartingIndex);
                        break;
                }
                if (processChange)
                {
                    if (CurrentPosition >= -1 && CurrentPosition < Count)
                    {
                        MoveCurrentToPosition(CurrentPosition);
                    }
                    else
                    {
                        MoveCurrentToPosition(-1);
                    }
                }
            }
        }

        /// <summary>
        /// Re-creates the view.
        /// </summary>
        protected override void RefreshOverride()
        {
            lock (SyncRoot)
            {
                _sortComparerUsed = null;
                ClearItemsCollection();
                bool oldIsGrouping = _isGrouping;
                _isGrouping = (_groupDescriptions.Count != 0);
                BuildItemsCollection();
                if (!IsEmpty)
                {
                    if (_sortDescriptions != null && _sortDescriptions.Count != 0)
                    {
                        SortUsingComparer(new SortDescriptionCollectionComparer(_sortDescriptions));
                    }
                    else if (CustomSort != null)
                    {
                        SortUsingComparer(CustomSort);
                    }
                }
                if (!_isGrouping)
                {
                    if (oldIsGrouping)
                    {
                        OnPropertyChanged("Groups");
                    }
                    _groups.Clear();
                }
                else
                {
                    _groups.Clear();
                    for (int index = 0; index < _items.Count; index++)
                    {
                        Group(_items[index], true);
                    }
                    if (!oldIsGrouping)
                    {
                        OnPropertyChanged("Groups");
                    }
                }
                MoveCurrentToFirst();
            }
        }

        #endregion Protected Methods 

        #region Public Methods 

        /// <summary>
        /// Returns a value that indicates whether the specified item belongs to the view.
        /// </summary>
        /// <param name="item">The object to check.</param>
        /// <returns>
        /// true if the item belongs to the view; otherwise, false.
        /// </returns>
        public override bool Contains(object item)
        {
            int sourceIndex = SourceCollection.IndexOf(item);
            return (sourceIndex != -1 && _items.Contains(sourceIndex));
        }

        /// <summary>
        /// Retrieves the item at the specified zero-based index in the view.
        /// </summary>
        /// <param name="index">The zero-based index of the item to retrieve.</param>
        /// <returns>
        /// The item at the specified zero-based index in the view.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than 0.
        /// </exception>
        public override object GetItemAt(int index)
        {
            return _items[index].SourceCollectionItem;
        }

        /// <summary>
        /// Groups the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemsAlreadySorted">if set to <c>true</c> items already sorted.</param>
        internal void Group(ActiveGroupingCollectionViewItem item, bool itemsAlreadySorted)
        {
            ActiveGroupingCollectionViewGroup parent = null;
            for (int i = 0; i < item.GroupValues.Length; i++)
            {
                ActiveGroupingCollectionViewGroup group = FindGroup(
                    item.GroupValues[i] ?? DependencyProperty.UnsetValue,
                    i,
                    (parent != null ? parent.InternalItems : _groups),
                    parent,
                    item.SourceCollectionItem,
                    itemsAlreadySorted
                    );
                if (group.IsBottomLevel)
                {
                    InsertBottomLevelGroupItem(item, group, itemsAlreadySorted);
                }
                else
                {
                    parent = group;
                }
            }
        }

        /// <summary>
        /// Returns the index at which the specified item is located.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>
        /// The index at which the specified item is located, or –1 if the item is unknown.
        /// </returns>
        public override int IndexOf(object item)
        {
            for (int index = 0; index < _items.Count; index++)
            {
                ActiveGroupingCollectionViewItem collectionViewItem = _items[index];
                if (Equals(collectionViewItem.SourceCollectionItem, item))
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Sets the item at the specified index to be the <see cref="P:System.Windows.Data.CollectionView.CurrentItem"/> in the view.
        /// </summary>
        /// <param name="position">The index to set the <see cref="P:System.Windows.Data.CollectionView.CurrentItem"/> to.</param>
        /// <returns>
        /// true if the resulting <see cref="P:System.Windows.Data.CollectionView.CurrentItem"/> is an item within the view; otherwise, false.
        /// </returns>
        public override bool MoveCurrentToPosition(int position)
        {
            if (position < -1 || position > Count)
            {
                throw new ArgumentOutOfRangeException("position");
            }
            if ((position != CurrentPosition || !IsCurrentInSync) && OKToChangeCurrent())
            {
                bool isCurrentAfterLast = IsCurrentAfterLast;
                bool isCurrentBeforeFirst = IsCurrentBeforeFirst;
                if (position < 0)
                {
                    SetCurrent(null, -1);
                }
                else if (position >= Count)
                {
                    SetCurrent(null, Count);
                }
                else
                {
                    SetCurrent(GetItemAt(position), position);
                }
                OnCurrentChanged();
                if (IsCurrentAfterLast != isCurrentAfterLast)
                {
                    OnPropertyChanged("IsCurrentAfterLast");
                }
                if (IsCurrentBeforeFirst != isCurrentBeforeFirst)
                {
                    OnPropertyChanged("IsCurrentBeforeFirst");
                }
                OnPropertyChanged("CurrentPosition");
                OnPropertyChanged("CurrentItem");
            }
            return IsCurrentInView;
        }

        #endregion Public Methods 

        #region Public Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveGroupingCollectionView"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ActiveGroupingCollectionView(IList collection)
            : base(collection)
        {
            _groupDescriptions = new ObservableCollection<GroupDescription>();
            _groupDescriptions.CollectionChanged += GroupDescriptions_CollectionChanged;
            _groups = new ObservableCollection<object>();
            _groupsWrapper = new ReadOnlyObservableCollection<object>(_groups);
            _items = new ActiveGroupingCollectionViewItemCollection();
            BuildItemsCollection();
        }

        #endregion Public Constructors 
    }

    /// <summary>
    /// Collection view group item.
    /// </summary>
    [DebuggerDisplay("SourceCollectionIndex = {SourceCollectionIndex}")]
    internal sealed class ActiveGroupingCollectionViewItem : DependencyObject
    {
        #region Public Static Variables 

        /// <summary>
        /// Defines the GroupValues dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupValuesProperty = DependencyProperty.Register("GroupValues", typeof (object[]), typeof (ActiveGroupingCollectionViewItem),
                                                                                                    new PropertyMetadata(GroupValuesProperty_PropertyChanged));

        #endregion Public Static Variables 

        #region Private Static Methods 

        /// <summary>
        /// Handles the PropertyChanged event of the GroupValuesProperty.
        /// </summary>
        /// <param name="d">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void GroupValuesProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as ActiveGroupingCollectionViewItem;
            if (item != null)
            {
                if (e.OldValue != null && e.NewValue != null && !IsEquivalent((object[]) e.NewValue, (object[]) e.OldValue))
                {
                    ActiveGroupingCollectionView collectionView = item.Group.CollectionView;
                    lock (collectionView.SyncRoot)
                    {
                        ActiveGroupingCollectionView.Ungroup(item);
                        collectionView.Group(item, false);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified comparand is equivalent to the other value.
        /// </summary>
        /// <param name="comparand">The comparand.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        /// 	<c>true</c> if the specified comparand is equivalent to the other value; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEquivalent(object[] comparand, object[] other)
        {
            if (comparand == null)
            {
                return (other == null);
            }
            if (other == null || (comparand.Length != other.Length))
            {
                return false;
            }
            for (int i = 0; i < comparand.Length; i++)
            {
                if (!Equals(comparand[i], other[i]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Private Static Methods 

        #region Public Properties 

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public ActiveGroupingCollectionViewGroup Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the group values.
        /// </summary>
        /// <value>The group values.</value>
        public object[] GroupValues
        {
            get { return (object[]) GetValue(GroupValuesProperty); }
        }

        /// <summary>
        /// Gets or sets the index of this item in the source collection.
        /// </summary>
        /// <value>The index of this item in the source collection.</value>
        public int SourceCollectionIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source collection item.
        /// </summary>
        /// <value>The source collection item.</value>
        public object SourceCollectionItem
        {
            get;
            set;
        }

        #endregion Public Properties 
    }

    /// <summary>
    /// Active grouping collection view item collection is a keyed collection of 
    /// <see cref="ActiveGroupingCollectionViewItem"/> instances keyed by their source collection index.
    /// </summary>
    internal sealed class ActiveGroupingCollectionViewItemCollection : KeyedCollection<int, ActiveGroupingCollectionViewItem>
    {
        #region Public Properties 

        /// <summary>
        /// Gets the <see cref="ActiveGroupingCollectionViewItem"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="ActiveGroupingCollectionViewItem"/> with the specified index.
        /// </value>
        public new ActiveGroupingCollectionViewItem this[int index]
        {
            get { return Items[index]; }
        }

        #endregion Public Properties 

        #region Protected Methods 

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>The key for the specified element.</returns>
        protected override int GetKeyForItem(ActiveGroupingCollectionViewItem item)
        {
            return item.SourceCollectionIndex;
        }

        #endregion Protected Methods 

        #region Public Methods 

        /// <summary>
        /// Changes the key associated with the specified element in the lookup dictionary.
        /// </summary>
        /// <param name="item">The element to change the key of.</param>
        /// <param name="newKey">The new key for <paramref name="item"/>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="item"/> is null.
        /// -or-
        /// <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="item"/> is not found.
        /// -or-
        /// <paramref name="key"/> already exists in the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2"/>.
        /// </exception>
        public new void ChangeItemKey(ActiveGroupingCollectionViewItem item, int newKey)
        {
            base.ChangeItemKey(item, newKey);
        }

        /// <summary>
        /// Sorts using the specified <see cref="T:System.Comparison`1"/>.
        /// </summary>
        /// <param name="comparison">An object to use to create comparisons between items used for sorting.</param>
        public void Sort(Comparison<ActiveGroupingCollectionViewItem> comparison)
        {
            (Items as List<ActiveGroupingCollectionViewItem>).Sort(comparison);
        }

        /// <summary>
        /// Gets the item associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Boolean indicating success.
        /// </returns>
        public bool TryGetItem(int key, out ActiveGroupingCollectionViewItem item)
        {
            if (Dictionary == null)
            {
                item = null;
                return false;
            }
            return Dictionary.TryGetValue(key, out item);
        }

        #endregion Public Methods 

        #region Public Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveGroupingCollectionViewItemCollection"/> class.
        /// </summary>
        public ActiveGroupingCollectionViewItemCollection()
            : base(null, 0)
        {
        }

        #endregion Public Constructors 
    }

    /// <summary>
    /// Active grouping collection view group.
    /// </summary>
    [DebuggerDisplay("Name = {Name}, ItemCount = {ItemCount}")]
    internal sealed class ActiveGroupingCollectionViewGroup : CollectionViewGroup
    {
        #region Private Variables 

        private readonly ActiveGroupingCollectionView _collectionView;
        private readonly int _groupDescriptionIndex;
        private readonly ActiveGroupingCollectionViewGroup _parentGroup;

        #endregion Private Variables 

        #region Public Properties 

        /// <summary>
        /// Gets the collection view.
        /// </summary>
        /// <value>The collection view.</value>
        public ActiveGroupingCollectionView CollectionView
        {
            get { return _collectionView; }
        }

        /// <summary>
        ///  Gets and sets the number of items in the subtree under this group.
        /// </summary>
        /// <value>The internal item count.</value>
        public int InternalItemCount
        {
            get { return ProtectedItemCount; }
            set { ProtectedItemCount = value; }
        }

        /// <summary>
        /// Gets the internal items.
        /// </summary>
        /// <value>The internal items.</value>
        public ObservableCollection<object> InternalItems
        {
            get { return ProtectedItems; }
        }

        /// <summary>
        /// Gets a value that indicates whether this group has any subgroups.
        /// </summary>
        /// <value></value>
        /// <returns>true if this group is at the bottom level and does not have any subgroups; otherwise, false.
        /// </returns>
        public override bool IsBottomLevel
        {
            get { return _groupDescriptionIndex == (_collectionView.GroupDescriptions.Count - 1); }
        }

        /// <summary>
        /// Gets the parent group.
        /// </summary>
        /// <value>The parent group.</value>
        public ActiveGroupingCollectionViewGroup ParentGroup
        {
            get { return _parentGroup; }
        }

        #endregion Public Properties 

        #region Public Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveGroupingCollectionViewGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="groupDescriptionIndex">Index of the group description.</param>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="parentGroup">The parent group.</param>
        public ActiveGroupingCollectionViewGroup(object name, int groupDescriptionIndex, ActiveGroupingCollectionView collectionView, ActiveGroupingCollectionViewGroup parentGroup)
            : base(name)
        {
            _collectionView = collectionView;
            _groupDescriptionIndex = groupDescriptionIndex;
            _parentGroup = parentGroup;
        }

        #endregion Public Constructors 
    }

    /// <summary>
    /// Sort description collection comparer.
    /// </summary>
    internal sealed class SortDescriptionCollectionComparer : Comparer<object>
    {
        #region Private Static Variables 

        private static readonly DependencyProperty _valueProperty = DependencyProperty.RegisterAttached("Value", typeof (object), typeof (SortDescriptionCollectionComparer));

        #endregion Private Static Variables 

        #region Private Static Methods 

        /// <summary>
        /// Evaluates the property path for the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>The result of the evaluation.</returns>
        private static object Evaluate(object item, string propertyPath)
        {
            var dependencyObject = new DependencyObject();
            try
            {
                BindingOperations.SetBinding(dependencyObject, _valueProperty, new Binding(propertyPath) {Source = item, Mode = BindingMode.OneTime});
                return dependencyObject.GetValue(_valueProperty);
            }
            finally
            {
                BindingOperations.ClearBinding(dependencyObject, _valueProperty);
            }
        }

        /// <summary>
        /// Gets (and caches) the item data.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="arrayList">The array list of cached values.</param>
        /// <param name="index">The index in the array list where the value would be.</param>
        /// <returns>Object.</returns>
        private static object GetItemData(object item, string propertyPath, ArrayList arrayList, int index)
        {
            object itemData;
            if (arrayList.Count == index)
            {
                arrayList.Add(itemData = Evaluate(item, propertyPath));
            }
            else
            {
                itemData = arrayList[index];
            }
            return itemData;
        }

        #endregion Private Static Methods 

        #region Private Variables 

        private readonly Dictionary<object, ArrayList> _dictionary;
        private readonly SortDescriptionCollection _sortDescriptions;

        #endregion Private Variables 

        #region Private Methods 

        /// <summary>
        /// Gets the item array list.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Array list.
        /// </returns>
        private ArrayList GetItemArrayList(object item)
        {
            ArrayList arrayList;
            if (!_dictionary.TryGetValue(item, out arrayList))
            {
                _dictionary.Add(item, arrayList = new ArrayList(_sortDescriptions.Count));
            }
            return arrayList;
        }

        #endregion Private Methods 

        #region Public Methods 

        /// <summary>
        /// Clears the values for the specified item.
        /// </summary>
        public void ClearValuesForItem(object item)
        {
            _dictionary.Remove(item);
        }

        /// <summary>
        /// When overridden in a derived class, performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value
        /// Condition
        /// Less than zero
        /// <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero
        /// <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero
        /// <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// Type <paramref name="T"/> does not implement either the <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface.
        /// </exception>
        public override int Compare(object x, object y)
        {
            ArrayList xArrayList = GetItemArrayList(x);
            ArrayList yArrayList = GetItemArrayList(y);
            int comparison = 0;
            if (!ReferenceEquals(x, y))
            {
                for (int i = 0; comparison == 0 && i < _sortDescriptions.Count; i++)
                {
                    string propertyPath = _sortDescriptions[i].PropertyName;
                    object xData = GetItemData(x, propertyPath, xArrayList, i);
                    object yData = GetItemData(y, propertyPath, yArrayList, i);
                    switch (_sortDescriptions[i].Direction)
                    {
                        case ListSortDirection.Ascending:
                            comparison = Comparer.Default.Compare(xData, yData);
                            break;
                        case ListSortDirection.Descending:
                            comparison = Comparer.Default.Compare(yData, xData);
                            break;
                    }
                }
            }
            return comparison;
        }

        #endregion Public Methods 

        #region Public Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescriptionCollectionComparer"/> class.
        /// </summary>
        /// <param name="sortDescriptions">The sort descriptions.</param>
        public SortDescriptionCollectionComparer(SortDescriptionCollection sortDescriptions)
        {
            _sortDescriptions = sortDescriptions;
            _dictionary = new Dictionary<object, ArrayList>();
        }

        #endregion Public Constructors 
    }
}