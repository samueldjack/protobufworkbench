using System.Collections.Generic;
using System.Linq;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// Allows multiple DynamicConditions to be combined in an OR fashion.
    /// </summary>
    public class OrCondition : DynamicCondition
    {
        private readonly List<IDynamicCondition> _conditions = new List<IDynamicCondition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OrCondition"/> class.
        /// </summary>
        /// <param name="conditions">The child conditions.</param>
        public OrCondition(params IDynamicCondition[] conditions)
        {
            _conditions.AddRange(conditions);
            _conditions.ForEach(condition => condition.ParentCondition = this);
        }

        /// <summary>
        /// When implemented, evaluates the value of this condition.
        /// </summary>
        /// <returns>
        /// true if the condition is true, false otherwise.
        /// </returns>
        protected override bool Evaluate()
        {
            // return true if any of the child conditions is true
            return _conditions.Any(condition => condition.Value);
        }
    }
}