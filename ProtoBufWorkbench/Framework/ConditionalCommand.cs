using System;
using ProtoBufWorkbench.Framework;
using ProtoBufWorkbench.Framework.Conditions;

namespace Paragon.Exiio.ExcelAddin.Framework.UI
{
    /// <summary>
    /// A Command whose CanExecute status is determined using an <see cref="IDynamicCondition"/>.
    /// </summary>
    public abstract class ConditionalCommand : CommandBase
    {
        private IDynamicCondition _canExecuteCondition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalCommand"/> class.
        /// </summary>
        protected ConditionalCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalCommand"/> class.
        /// </summary>
        /// <param name="canExecuteCondition">The condition that decides if the command can execute.</param>
        protected ConditionalCommand(IDynamicCondition canExecuteCondition)
        {
            Condition = canExecuteCondition;
        }

        /// <summary>
        /// Gets or sets the condition that is used to determine if the command is enabled.
        /// </summary>
        /// <value>The condition.</value>
        protected IDynamicCondition Condition
        {
            get
            {
                return _canExecuteCondition;
            }
            set
            {
                if (_canExecuteCondition != null)
                {
                    _canExecuteCondition.ValueChanged -= HandleConditionValueChanged;
                }

                _canExecuteCondition = value;
                
                if (_canExecuteCondition != null)
                {
                    _canExecuteCondition.ValueChanged += HandleConditionValueChanged;
                }

                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return _canExecuteCondition == null ? true : _canExecuteCondition.Value;
        }

        private void HandleConditionValueChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}