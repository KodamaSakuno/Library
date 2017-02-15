using Sakuno.UserInterface.ObjectOperations;
using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Commands
{
    public class InvokeMethodExtension : MarkupExtension
    {
        string r_Method;

        object r_Parameter;

        public object Target { get; set; }

        public InvokeMethodExtension(string rpMethod) : this(rpMethod, null) { }
        public InvokeMethodExtension(string rpMethod, object rpParameter)
        {
            r_Method = rpMethod;
            r_Parameter = rpParameter;
        }

        public override object ProvideValue(IServiceProvider rpServiceProvider)
        {
            var rInvokeMethod = new InvokeMethod() { Method = r_Method };

            if (r_Parameter != null)
            {
                var rParameter = new MethodParameter();

                var rBinding = r_Parameter as Binding;
                if (rBinding == null)
                    rParameter.Value = r_Parameter;
                else
                    BindingOperations.SetBinding(rParameter, MethodParameter.ValueProperty, rBinding);

                rInvokeMethod.Parameters.Add(rParameter);
            }

            if (Target != null)
            {
                var rBinding = Target as Binding;
                if (rBinding != null)
                    BindingOperations.SetBinding(rInvokeMethod, InvokeMethod.TargetProperty, rBinding);
                else
                    rInvokeMethod.Target = Target;
            }

            return new ObjectOperationCommand() { Operations = { rInvokeMethod } };
        }
    }
}
