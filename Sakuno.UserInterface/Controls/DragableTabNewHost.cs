using System.Windows;

namespace Sakuno.UserInterface.Controls
{
    public class DragableTabNewHost
    {
        public Window Window { get; }
        public DragableTabControl TabControl { get; }

        public DragableTabNewHost(Window rpWindow, DragableTabControl rpTabControl)
        {
            Window = rpWindow;
            TabControl = rpTabControl;
        }
    }
}
