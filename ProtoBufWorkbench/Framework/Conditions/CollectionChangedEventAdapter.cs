using System;
using System.Collections.Specialized;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// Adapts NotifyCollectionChangedEventHandler to generic EventHandler.
    /// </summary>
    internal class CollectionChangedEventAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventAdapter"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public CollectionChangedEventAdapter(INotifyCollectionChanged source)
        {
            source.CollectionChanged += InvokeCollectionChanged;
        }

        /// <summary>
        /// Occurs when A CollectionChanged event is received by the source.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanged;

        private void InvokeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            EventHandler<NotifyCollectionChangedEventArgs> collectionChangedHandler = CollectionChanged;
            if (collectionChangedHandler != null)
            {
                collectionChangedHandler(sender, e);
            }
        }
    }
}