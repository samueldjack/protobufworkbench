namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// A <see cref="IDynamicCondition"/> implementation that is always true.
    /// </summary>
    public class TrueCondition : DynamicCondition
    {
        /// <summary>
        /// When implemented, evaluates the value of this condition.
        /// </summary>
        /// <returns>
        /// true if the condition is true, false otherwise.
        /// </returns>
        protected override bool Evaluate()
        {
            return true;
        }
    }
}