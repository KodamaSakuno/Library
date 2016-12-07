using Sakuno.UserInterface.ObjectOperations;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Commands
{
    [ContentProperty(nameof(Operations))]
    public class ObjectOperationCommand : Freezable, ICommand
    {
        public static readonly DependencyProperty OperationsProperty = DependencyProperty.Register(nameof(Operations), typeof(ObjectOperationCollection), typeof(ObjectOperationCommand),
            new PropertyMetadata(null));
        public ObjectOperationCollection Operations
        {
            get
            {
                var rResult = (ObjectOperationCollection)GetValue(OperationsProperty);
                if (rResult == null)
                {
                    rResult = new ObjectOperationCollection();
                    Operations = rResult;
                }

                return rResult;
            }
            set { SetValue(OperationsProperty, value); }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object rpParameter)
        {
            var rOperations = (ObjectOperationCollection)GetValue(OperationsProperty);
            if (rOperations == null)
                return;

            foreach (var rOperation in rOperations)
                rOperation.Invoke();
        }

        public bool CanExecute(object parameter) => true;

        protected override Freezable CreateInstanceCore() => new ObjectOperationCommand();
    }
}
