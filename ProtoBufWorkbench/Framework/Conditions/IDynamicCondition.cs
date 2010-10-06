using System;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// Implements a condition whose value can change over time.
    /// </summary>
    public interface IDynamicCondition
    {
        /// <summary>
        /// Gets a value indicating whether the condition is true.
        /// </summary>
        /// <value><c>true</c> if value; otherwise, <c>false</c>.</value>
        bool Value { get; }

        /// <summary>
        /// Gets or sets the parent condition.
        /// </summary>
        /// <value>The parent condition.</value>
        IDynamicCondition ParentCondition { get; set; }

        /// <summary>
        /// Occurs when the Value of the Condition has changed.
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Causes the value of the condition to be marked as needing re-evaluation.
        /// </summary>
        void InvalidateValue();
    }
}