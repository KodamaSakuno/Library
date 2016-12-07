using Sakuno.SystemInterop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sakuno.UserInterface.Internal
{
    abstract class GlowWindowProcessor
    {
        public static double GlowSize { get; set; } = 9.0;
        public static double EdgeSize { get; set; } = 20.0;

        public abstract Orientation Orientation { get; }

        public abstract VerticalAlignment VerticalAlignment { get; }
        public abstract HorizontalAlignment HorizontalAlignment { get; }

        public abstract double GetLeft(double rpOwnerLeft, double rpOwnerWidth);
        public abstract double GetTop(double rpOwnerTop, double rpOwnerHeight);

        public abstract double GetWidth(double rpOwnerLeft, double rpOwnerWidth);
        public abstract double GetHeight(double rpOwnerTop, double rpOwnerHeight);

        public abstract Cursor GetCursor(Point rpPoint, double rpWidth, double rpHeight);
        public abstract NativeConstants.HitTest GetHitTestValue(Point rpPoint, double rpWidth, double rpHeight);
    }
}
