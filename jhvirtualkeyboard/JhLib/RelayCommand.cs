// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#if WIN8
using Windows.UI.Xaml.Input;
using EventHandler = Windows.UI.Xaml.EventHandler;
#else
using System.Windows.Input;
#endif

// Credit for this belongs to Josh Smith's article "WPF Apps With The Model-
// View-ViewModel Design Pattern".

namespace JhLib
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Constructors

        /// <summary>
        /// Create a new command, given an Action to execute.
        /// </summary>
        /// <param name="executionAction">The code to run when this command executs.</param>
        public RelayCommand(Action<object> executionAction)
            : this(executionAction, null)
        {
        }

        /// <summary>
        /// Create a new command given an Action to execute and a Predicate that dicates it's enabled state.
        /// </summary>
        /// <param name="executionAction">The code that this command will run when Execute is called</param>
        /// <param name="canExecutePredicate">The Predicate that dictates whether this command is enabled</param>
        public RelayCommand(Action<object> executionAction, Predicate<object> canExecutePredicate)
        {
            if (executionAction == null)
            {
                throw new ArgumentNullException("executionAction");
            }
            _executionAction = executionAction;
            _canExecutePredicate = canExecutePredicate;
        }
        #endregion Constructors

        #region ICommmand implementation

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate == null ? true : _canExecutePredicate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            // Invoke the Action.
            _executionAction(parameter);
        }
        #endregion ICommand implementation

        #region Fields

        /// <summary>
        /// This is the Action that this command exists to execute.
        /// </summary>
        private readonly Action<object> _executionAction;

        /// <summary>
        /// To say when changes happen that affect whether the command be enabled.
        /// </summary>
        readonly Predicate<object> _canExecutePredicate;

        #endregion Fields
    }
}
