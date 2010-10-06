using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// An implementation of DynamicCondition that can use events to determine when its value should be updated.
    /// </summary>
    internal class EventDrivenDynamicCondition : DynamicCondition
    {
        private Func<bool> _condition;

        /// <summary>
        /// Creates a new DynamicCondition that is reevaluated whenever the given event is raised.
        /// </summary>
        /// <param name="eventAttacher">An action that will attach an event handler to the relevant object.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>A new <see cref="EventDrivenDynamicCondition"/>.</returns>
        public static EventDrivenDynamicCondition Create(Action<EventHandler> eventAttacher, Func<bool> evaluator)
        {
            var condition = new EventDrivenDynamicCondition();
            condition.SetCondition(eventAttacher, evaluator);

            return condition;
        }


        /// <summary>
        /// Creates a new DynamicCondition that is reevaluated always.
        /// </summary>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>A new <see cref="EventDrivenDynamicCondition"/>.</returns>
        public static EventDrivenDynamicCondition Create(Func<bool> evaluator)
        {
            return Create(e => { }, evaluator);
        }

        /// <summary>
        /// Creates a new DynamicCondition that is reevaluated whenever the a given event is raised.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="eventAttacher">The event attacher.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>A new <see cref="EventDrivenDynamicCondition"/>.</returns>
        public static EventDrivenDynamicCondition Create<TEventArgs>(Action<EventHandler<TEventArgs>> eventAttacher,
                                                                     Func<bool> evaluator) where TEventArgs : EventArgs
        {
            var condition = new EventDrivenDynamicCondition();
            condition.SetCondition(eventAttacher, evaluator);

            return condition;
        }

        /// <summary>
        /// Creates a new DynamicCondition that is reevaluated whenever the given event is raised.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>
        /// A new <see cref="EventDrivenDynamicCondition"/>.
        /// </returns>
        public static EventDrivenDynamicCondition CreateForPropertyChanged(INotifyPropertyChanged source,
                                                                           Func<bool> evaluator)
        {
            return
                Create<PropertyChangedEventArgs>(
                    handler => new PropertyChangedEventAdapter(source).PropertyChanged += handler, evaluator);
        }

        /// <summary>
        /// Creates a new DynamicCondition that is reevaluated whenever the given event is raised.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns>
        /// A new <see cref="EventDrivenDynamicCondition"/>.
        /// </returns>
        public static EventDrivenDynamicCondition CreateForCollectionChanged(INotifyCollectionChanged source,
                                                                             Func<bool> evaluator)
        {
            return
                Create<NotifyCollectionChangedEventArgs>(
                    handler => new CollectionChangedEventAdapter(source).CollectionChanged += handler, evaluator);
        }

        /// <summary>
        /// When implemented, evaluates the value of this condition.
        /// </summary>
        /// <returns>
        /// true if the condition is true, false otherwise.
        /// </returns>
        protected override bool Evaluate()
        {
            return _condition();
        }

        /// <summary>
        /// Sets condition which must be true in order for the parent condition to be true.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the EventArgs used in the event.</typeparam>
        /// <param name="eventAttacher">An action that will attach an event handler to the relevant object.</param>
        /// <param name="evaluator">The evaluator.</param>
        protected void SetCondition<TEventArgs>(Action<EventHandler<TEventArgs>> eventAttacher, Func<bool> evaluator)
            where TEventArgs : EventArgs
        {
            // ask the calling class to hook up the appropriate event to our event handler
            var weakReferenceEventHandler = this.CreateWeakEventHandler<EventDrivenDynamicCondition, TEventArgs>(
                (instance, sender, e) => instance.InvalidateValue());

            eventAttacher(weakReferenceEventHandler);

            _condition = evaluator;
        }

        /// <summary>
        /// Adds a condition which must be true in order for the parent condition to be true.
        /// </summary>
        /// <param name="eventAttacher">An action that will attach an event handler to the relevant object.</param>
        /// <param name="evaluator">The evaluator.</param>
        protected void SetCondition(Action<EventHandler> eventAttacher, Func<bool> evaluator)
        {
            // ask the calling class to hook up the appropriate event to our event handler
            var weakReferenceEventHandler = this.CreateWeakEventHandler<EventDrivenDynamicCondition, EventArgs>(
                (instance, sender, e) => instance.InvalidateValue());

            eventAttacher(weakReferenceEventHandler.HandleEvent);

            _condition = evaluator;
        }
    }
}