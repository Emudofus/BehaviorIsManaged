#region License GNU GPL
// DelegateCommand.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BiM.Host.UI.Helpers
{
    /// <summary>
    /// Provides an <see cref="Execute"/> implementation which relays the <see cref="CanExecute"/> and <see cref="ICommand"/> 
    /// method to the specified delegates.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> m_canExecute;
        private readonly Action<object> m_execute;
        private List<WeakReference> m_weakHandlers;


        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute = null)
            : this(execute != null ? p => execute() : (Action<object>) null, canExecute != null ? p => canExecute() : (Func<object, bool>) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            m_execute = execute;
            m_canExecute = canExecute;
        }

        #region ICommand Members

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (m_weakHandlers == null)
                {
                    m_weakHandlers = new List<WeakReference>(new[] {new WeakReference(value)});
                }
                else
                {
                    m_weakHandlers.Add(new WeakReference(value));
                }

                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (m_weakHandlers == null)
                {
                    return;
                }

                for (int i = m_weakHandlers.Count - 1; i >= 0; i--)
                {
                    WeakReference weakReference = m_weakHandlers[i];
                    var handler = weakReference.Target as EventHandler;
                    if (handler == null || handler == value)
                    {
                        m_weakHandlers.RemoveAt(i);
                    }
                }

                CommandManager.RequerySuggested -= value;
            }
        }


        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");
            }

            m_execute(parameter);
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            PurgeWeakHandlers();

            if (m_weakHandlers == null)
            {
                return;
            }

            WeakReference[] handlers = m_weakHandlers.ToArray();
            foreach (WeakReference reference in handlers)
            {
                var handler = reference.Target as EventHandler;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        private void PurgeWeakHandlers()
        {
            if (m_weakHandlers == null)
            {
                return;
            }

            for (int i = m_weakHandlers.Count - 1; i >= 0; i--)
            {
                if (!m_weakHandlers[i].IsAlive)
                {
                    m_weakHandlers.RemoveAt(i);
                }
            }

            if (m_weakHandlers.Count == 0)
            {
                m_weakHandlers = null;
            }
        }
    }
}