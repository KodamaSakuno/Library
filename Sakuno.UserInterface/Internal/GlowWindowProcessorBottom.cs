using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Sakuno.SystemInterop;

namespace Sakuno.UserInterface.Internal
{
    class GlowWindowProcessorBottom : GlowWindowProcessor
    {
        public override VerticalAlignment VerticalAlignment => VerticalAlignment.Top;
        public override HorizontalAlignment HorizontalAlignment => HorizontalAlignment.Stretch;

        public override Orientation Orientation => Orientation.Horizontal;

        public override double GetLeft(double rpOwnerLeft, double rpOwnerWidth) => rpOwnerLeft;
        public override double GetTop(double rpOwnerTop, double rpOwnerHeight) => rpOwnerTop + rpOwnerHeight;

        public override double GetWidth(double rpOwnerLeft, double rpOwnerWidth) => rpOwnerWidth;
        public override double GetHeight(double rpOwnerTop, double rpOwnerHeight) => GlowSize;

        public override Cursor GetCursor(Point rpPoint, double rpWidth, double rpHeight)
        {
            var rLeftCornerRect = new Rect(0, 0, EdgeSize - GlowSize, rpHeight);
            var rRightCornerRect = new Rect(rpWidth - EdgeSize + GlowSize, 0, EdgeSize - GlowSize, rpHeight);

            if (rLeftCornerRect.Contains(rpPoint))
                return Cursors.SizeNESW;
            else if (rRightCornerRect.Contains(rpPoint))
                return Cursors.SizeNWSE;
            else
                return Cursors.SizeNS;
        }
        public override NativeConstants.HitTest GetHitTestValue(Point rpPoint, double rpWidth, double rpHeight)
        {
            var rLeftCornerRect = new Rect(0, 0, EdgeSize - GlowSize, rpHeight);
            var rRightCornerRect = new Rect(rpWidth - EdgeSize + GlowSize, 0, EdgeSize - GlowSize, rpHeight);

            if (rLeftCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTBOTTOMLEFT;
            else if (rRightCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTBOTTOMRIGHT;
            else
                return NativeConstants.HitTest.HTBOTTOM;
        }
    }
}
