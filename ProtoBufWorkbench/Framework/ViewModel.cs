using System.ComponentModel;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// Provides a base class for implementing ViewModels.
    /// </summary>
    public class ViewModel : IViewModel
    {
        /// <summary>
        /// Gets the Name of the Title Property.
        /// </summary>
        public static readonly string TitlePropertyName = SymbolExtensions.GetPropertySymbol((ViewModel vm) => vm.Title);

        /// <summary>
        /// Gets the string used for signalling that All Properties of the View Model have changed
        /// </summary>
        protected static readonly string AllProperties = string.Empty;

        private string _title;
        private int _viewReferenceCount;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether an view attached to this ViewModel is currently loaded.
        /// </summary>
        /// <value>
        ///     <c>true</c> if an view is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsViewLoaded
        {
            get
            {
                return _viewReferenceCount > 0;
            }
        }

        /// <summary>
        /// Should be called whenever a View associated with this <see cref="ViewModel"/> is loaded.
        /// </summary>
        public void NotifyViewLoaded()
        {
            var notificationRequired = _viewReferenceCount == 0;

            _viewReferenceCount++;

            if (notificationRequired)
            {
                OnViewLoaded();
            }
        }

        /// <summary>
        /// Should be called whenever a View associated with this <see cref="ViewModel"/> is unloaded.
        /// </summary>
        public void NotifyViewUnloaded()
        {
            if (_viewReferenceCount > 0)
            {
                _viewReferenceCount--;
            }

            if (_viewReferenceCount == 0)
            {
                OnViewUnloaded();
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> for <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when a View associated with this <see cref="ViewModel"/> is loaded.
        /// </summary>
        protected virtual void OnViewLoaded()
        {
        }

        /// <summary>
        /// Called when a View associated with this <see cref="ViewModel"/> is unloaded.
        /// </summary>
        protected virtual void OnViewUnloaded()
        {
        }

    }
}