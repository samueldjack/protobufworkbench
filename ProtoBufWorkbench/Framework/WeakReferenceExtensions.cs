using System;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class WeakReferenceExtensions
    {
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static WeakReferenceEventHandler<TParent, TEventArgs> CreateWeakEventHandler<TParent, TEventArgs>(this TParent instance, Action<TParent, object, TEventArgs> action)
            where TEventArgs : EventArgs
            where TParent : class 
        {
            return new WeakReferenceEventHandler<TParent, TEventArgs>(instance, action);
        }
    }
}