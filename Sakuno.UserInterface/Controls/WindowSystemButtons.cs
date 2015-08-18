using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class WindowSystemButtons : Control
    {
        static WindowSystemButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowSystemButtons), new FrameworkPropertyMetadata(typeof(WindowSystemButtons)));
        }
    }
}
