using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    public class SplitButton : HeaderedContentControl
    {
        public static readonly DependencyProperty CommandProperty;
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty;
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty;
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));

            CommandProperty = Button.CommandProperty.AddOwner(typeof(SplitButton));
            CommandParameterProperty = Button.CommandParameterProperty.AddOwner(typeof(SplitButton));
            CommandTargetProperty = Button.CommandTargetProperty.AddOwner(typeof(SplitButton));
        }
    }
}
