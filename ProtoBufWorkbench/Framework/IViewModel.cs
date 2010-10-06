using System.ComponentModel;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// An interface implemented by View Models.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Should be called whenever a View associated with this <see cref="IViewModel"/> is loaded.
        /// </summary>
        void NotifyViewLoaded();

        /// <summary>
        /// Should be called whenever a View associated with this <see cref="IViewModel"/> is unloaded.
        /// </summary>
        void NotifyViewUnloaded();
    }
}