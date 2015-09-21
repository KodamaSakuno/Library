using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Sakuno.SystemInterop;

namespace Sakuno.UserInterface.Internal
{
    class GlowWindowProcessorTop : GlowWindowProcessor
    {
        public override VerticalAlignment VerticalAlignment => VerticalAlignment.Bottom;
        public override HorizontalAlignment HorizontalAlignment => HorizontalAlignment.Stretch;

        public override Orientation Orientation => Orientation.Horizontal;

        public override double GetLeft(double rpOwnerLeft, double rpOwnerWidth) => rpOwnerLeft;
        public override double GetTop(double rpOwnerTop, double rpOwnerHeight) => rpOwnerTop - GlowSize;

        public override double GetWidth(double rpOwnerLeft, double rpOwnerWidth) => rpOwnerWidth;
        public override double GetHeight(double rpOwnerTop, double rpOwnerHeight) => GlowSize;

        public override Cursor GetCursor(Point rpPoint, double rpWidht, double rpHeight)
        {
            var rLeftCornerRect = new Rect(0, 0, EdgeSize - GlowSize, rpHeight);
            var rRightCornerRect = new Rect(rpWidht - EdgeSize + GlowSize, 0, EdgeSize - GlowSize, rpHeight);

            if (rLeftCornerRect.Contains(rpPoint))
                return Cursors.SizeNWSE;
            if (rRightCornerRect.Contains(rpPoint))
                return Cursors.SizeNESW;
            else
                return Cursors.SizeNS;
        }
        public override NativeConstants.HitTest GetHitTestValue(Point rpPoint, double rpWidht, double rpHeight)
        {
            var rLeftCornerRect = new Rect(0, 0, EdgeSize - GlowSize, rpHeight);
            var rRightCornerRect = new Rect(rpWidht - EdgeSize + GlowSize, 0, EdgeSize - GlowSize, rpHeight);

            if (rLeftCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTTOPLEFT;
            if (rRightCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTTOPRIGHT;
            else
                return NativeConstants.HitTest.HTTOP;
        }
    }
}
