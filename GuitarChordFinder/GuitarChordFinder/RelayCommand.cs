using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GuitarChordFinder
{
    public class RelayCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object o)
        {
            return this.canExecute == null || this.canExecute();
        }

        public void Execute(object o)
        {
            this.execute();
        }
    }
}
