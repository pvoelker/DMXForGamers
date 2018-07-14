using System;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class NestedRelayCommand : ICommand
    {
        private ICommand innerCommand;

        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public NestedRelayCommand(ICommand innerCommand, Action<object> execute, Predicate<object> canExecute)
        {
            if (innerCommand == null)
            {
                throw new ArgumentNullException("innerCommand");
            }

            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;

            this.innerCommand = innerCommand;
            this.innerCommand.CanExecuteChanged += InnerCommand_CanExecuteChanged;
        }

        private void InnerCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter) && innerCommand.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.innerCommand.CanExecuteChanged -= InnerCommand_CanExecuteChanged;

            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }
    }
}
