using System;
using Paragon.Exiio.ExcelAddin.Framework.UI;
using ProtoBufWorkbench.Framework.Conditions;

namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// A Command that invokes an Action when executed.
    /// </summary>
    public class ActionInvokingCommand : ConditionalCommand
    {
        private readonly Action<object> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvokingCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionInvokingCommand(Action<object> action)
        {
            _action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvokingCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionInvokingCommand(Action action)
        {
            _action = parameter => action();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvokingCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="condition">The condition.</param>
        public ActionInvokingCommand(Action action, IDynamicCondition condition) : this(action)
        {
            Condition = condition;    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvokingCommand"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="condition">The condition.</param>
        public ActionInvokingCommand(Action<object> action, IDynamicCondition condition) : this(action)
        {
            Condition = condition;    
        }


        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public override void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}