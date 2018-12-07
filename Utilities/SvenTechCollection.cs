using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Utilities
{
    public class SvenTechCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Raises the OnItemPropertyChanged event with the provided arguments when any property of a list item is changed.
        /// </summary>
        /// <param name="sender">This Collection</param>
        /// <param name="item">Item that has changed</param>
        /// <param name="e">Arguments of the event being raised</param>
        public delegate void OnItemPropertyChangedEvent(object sender, object item, PropertyChangedEventArgs e);

        /// <summary>
        ///     Initializes a new instance of the AsincoList class
        /// </summary>
        public SvenTechCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AsincoList class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public SvenTechCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the AsincoList class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The collection from which the elements are copied.</param>
        public SvenTechCollection(List<T> list) : base(list)
        {
        }

        public event OnItemPropertyChangedEvent OnItemPropertyChanged;

        /// <summary>
        ///     Adds an object to the end of the AsincoList.
        /// </summary>
        /// <param name="item">The object to be added to the end of the AsincoList. The value can be null for reference types.</param>
        public new void Add(T item)
        {
            base.Add(item);
            if (item is INotifyPropertyChanged) ((INotifyPropertyChanged) item).PropertyChanged += Item_PropertyChanged;
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the
        /// </summary>
        /// <param name="collection">
        ///     The collection whose elements should be added to the end of the AsincoList.
        ///     The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
        /// </param>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection.IsNull()) throw new ArgumentNullException("collection");

            foreach (var item in collection) Add(item);
        }

        private void Item_PropertyChanged(object senderItem, PropertyChangedEventArgs e)
        {
            if (!OnItemPropertyChanged.IsNull()) OnItemPropertyChanged(this, senderItem, e);
        }
    }
}