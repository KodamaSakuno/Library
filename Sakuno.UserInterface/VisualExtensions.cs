using System.Windows;
using System.Windows.Media;

namespace Sakuno.UserInterface
{
    public static class VisualExtensions
    {
        public static Dpi? GetSystemDpi(this Visual rpVisual)
        {
            var rSource = PresentationSource.FromVisual(rpVisual);
            return new Dpi((uint)(Dpi.Default.X * rSource?.CompositionTarget?.TransformToDevice.M11), (uint)(Dpi.Default.Y * rSource?.CompositionTarget?.TransformToDevice.M22));
        }
    }
}
