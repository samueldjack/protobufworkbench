using System.Collections.Generic;
using System.Linq;

namespace ProtoBufWorkbench.Framework.Conditions
{
    /// <summary>
    /// Allows multiple DynamicConditions to be combined.
    /// </summary>
    public class AndCondition : DynamicCondition
    {
        private readonly List<IDynamicCondition> _conditions = new List<IDynamicCondition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AndCondition"/> class.
        /// </summary>
        /// <param name="conditions">The conditions.</param>
        public AndCondition(params IDynamicCondition[] conditions)
        {
            _conditions.AddRange(conditions);
            _conditions.ForEach(condition => condition.ParentCondition = this);
        }

        /// <summary>
        /// Evaluates the value of this condition.
        /// </summary>
        /// <returns>
        /// true if the condition is true, false otherwise.
        /// </returns>
        protected override bool Evaluate()
        {
            // return true if all the child conditions are true
            return _conditions.All(condition => condition.Value);
        }
    }
}