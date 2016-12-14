using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Commands
{
    public class NavigateCommandExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider rpServiceProvider) => new NavigateCommand();
    }
}
