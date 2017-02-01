using Sakuno.UserInterface.ObjectOperations;
using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Commands
{
    public class InvokeMethodExtension : MarkupExtension
    {
        string r_Method;

        object r_Parameter;

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
                rInvokeMethod.Parameters.Add(new MethodParameter() { Value = r_Parameter });

            return new ObjectOperationCommand() { Operations = { rInvokeMethod } };
        }
    }
}
