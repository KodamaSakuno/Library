using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class DropDownButton : HeaderedContentControl
    {
        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }
    }
}
