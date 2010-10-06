using System;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// Implements the <see cref="IDynamicCondition"/> interface.
    /// </summary>
    public abstract class DynamicCondition : IDynamicCondition
    {
        private bool? _value;

        #region IDynamicCondition Members

        /// <summary>
        /// Occurs when the Value of the Condition has changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Gets a value indicating whether the condition is true.
        /// </summary>
        /// <value><c>true</c> if value; otherwise, <c>false</c>.</value>
        public bool Value
        {
            get
            {
                if (!_value.HasValue)
                {
                    _value = Evaluate();
                }

                return _value.Value;
            }
        }

        /// <summary>
        /// Gets or sets the parent condition.
        /// </summary>
        /// <value>The parent condition.</value>
        public IDynamicCondition ParentCondition { get; set; }

        /// <summary>
        /// Causes the value of the condition to be marked as needing re-evaluation.
        /// </summary>
        public void InvalidateValue()
        {
            _value = null;

            if (ParentCondition != null)
            {
                ParentCondition.InvalidateValue();
            }

            OnValueChanged(EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// When implemented, evaluates the value of this condition.
        /// </summary>
        /// <returns>true if the condition is true, false otherwise.</returns>
        protected abstract bool Evaluate();

        /// <summary>
        /// Raises the <see cref="EventDrivenDynamicCondition.ValueChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}