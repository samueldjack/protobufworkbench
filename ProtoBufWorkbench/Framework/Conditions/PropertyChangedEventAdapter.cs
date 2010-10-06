using System;
using System.ComponentModel;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// An adapter for the PropertyChanged event.
    /// </summary>
    public class PropertyChangedEventAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedEventAdapter"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public PropertyChangedEventAdapter(INotifyPropertyChanged source)
        {
            source.PropertyChanged += HandleEvent;
        }

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        private void HandleEvent(object sender, PropertyChangedEventArgs e)
        {
            EventHandler<PropertyChangedEventArgs> handler = PropertyChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}