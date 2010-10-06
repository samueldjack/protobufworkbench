using System;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// Allows an event to be handled using a Weak Reference
    /// </summary>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public struct WeakReferenceEventHandler<TParent, TEventArgs>
        where TEventArgs : EventArgs
        where TParent : class
    {
        private WeakReference _reference;
        private Action<TParent, object, TEventArgs> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReferenceEventHandler&lt;TParent, TEventArgs&gt;"/> struct.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action that will handle the event.</param>
        public WeakReferenceEventHandler(TParent instance, Action<TParent, object, TEventArgs> action)
        {
            _action = action;
            _reference = new WeakReference(instance);
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="TEventArgs"/> instance containing the event data.</param>
        public void HandleEvent(object sender, TEventArgs eventArgs)
        {
            var target = _reference.Target as TParent;
            if (target != null)
            {
                _action(target, sender, eventArgs);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from 
        /// <see cref="WeakReferenceEventHandler{TParent,TEventArgs}"/>
        /// to <see cref="System.EventHandler{TEventArgs}"/>.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator EventHandler<TEventArgs>(WeakReferenceEventHandler<TParent, TEventArgs> eventHandler)
        {
            return eventHandler.HandleEvent;
        }
    }
}
