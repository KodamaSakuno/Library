using System.Windows;

namespace Sakuno.UserInterface
{
    public static class WindowExtensions
    {
        public static bool? ShowDialog(this Window rpWindow, Window rpOwner)
        {
            rpWindow.Owner = rpOwner;
            rpWindow.ShowInTaskbar = false;
            return rpWindow.ShowDialog();
        }
    }
}
