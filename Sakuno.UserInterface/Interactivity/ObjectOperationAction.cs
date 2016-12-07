using Sakuno.UserInterface.ObjectOperations;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Interactivity
{
    [ContentProperty(nameof(Operations))]
    public class ObjectOperationAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty OperationsProperty = DependencyProperty.Register(nameof(Operations), typeof(ObjectOperationCollection), typeof(ObjectOperationAction),
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

        protected override void Invoke(object rpParameter)
        {
            var rOperations = (ObjectOperationCollection)GetValue(OperationsProperty);
            if (rOperations == null)
                return;

            foreach (var rOperation in rOperations)
                rOperation.Invoke();
        }
    }
}
