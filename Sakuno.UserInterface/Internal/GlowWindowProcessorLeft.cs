using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Sakuno.SystemInterop;

namespace Sakuno.UserInterface.Internal
{
    class GlowWindowProcessorLeft : GlowWindowProcessor
    {
        public override VerticalAlignment VerticalAlignment => VerticalAlignment.Stretch;
        public override HorizontalAlignment HorizontalAlignment => HorizontalAlignment.Right;

        public override Orientation Orientation => Orientation.Vertical;

        public override double GetLeft(double rpOwnerLeft, double rpOwnerWidth) => rpOwnerLeft - GlowSize;
        public override double GetTop(double rpOwnerTop, double rpOwnerHeight) => rpOwnerTop - GlowSize;

        public override double GetWidth(double rpOwnerLeft, double rpOwnerWidth) => GlowSize;
        public override double GetHeight(double rpOwnerTop, double rpOwnerHeight) => rpOwnerHeight + GlowSize * 2;

        public override Cursor GetCursor(Point rpPoint, double rpWidth, double rpHeight)
        {
            var rTopCornerRect = new Rect(0, 0, rpWidth, EdgeSize);
            var rBottomCornerRect = new Rect(0, rpHeight - EdgeSize, rpWidth, EdgeSize);

            if (rTopCornerRect.Contains(rpPoint))
                return Cursors.SizeNWSE;
            else if (rBottomCornerRect.Contains(rpPoint))
                return Cursors.SizeNESW;
            else
                return Cursors.SizeWE;
        }
        public override NativeConstants.HitTest GetHitTestValue(Point rpPoint, double rpWidth, double rpHeight)
        {
            var rTopCornerRect = new Rect(0, 0, rpWidth, EdgeSize);
            var rBottomCornerRect = new Rect(0, rpHeight - EdgeSize, rpWidth, EdgeSize);

            if (rTopCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTTOPLEFT;
            else if (rBottomCornerRect.Contains(rpPoint))
                return NativeConstants.HitTest.HTBOTTOMLEFT;
            else
                return NativeConstants.HitTest.HTLEFT;
        }
    }
}
