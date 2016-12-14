using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Sakuno.UserInterface.Commands
{
    public class NavigateCommand : Freezable, ICommand
    {
        public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register(nameof(TargetPage), typeof(Page), typeof(NavigateCommand),
            new PropertyMetadata(null));
        public Page TargetPage
        {
            get { return (Page)GetValue(TargetPageProperty); }
            set { SetValue(TargetPageProperty, value); }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NavigateCommand()
        {
            BindingOperations.SetBinding(this, TargetPageProperty, new Binding() { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Page), 1) });
        }

        public void Execute(object rpParameter)
        {
            var rPage = TargetPage;
            if (rPage == null)
                return;

            var rUri = rpParameter as Uri;
            if (rUri == null)
            {
                var rUriString = rpParameter as string;
                if (rUriString != null)
                    rUri = new Uri(rUriString, UriKind.RelativeOrAbsolute);
            }

            if (rUri != null)
            {
                rPage.NavigationService.Navigate(rUri);
                return;
            }

            rPage.NavigationService.Navigate(rpParameter);
        }

        public bool CanExecute(object rpParameter) => true;

        protected override Freezable CreateInstanceCore() => new NavigateCommand();
    }
}
