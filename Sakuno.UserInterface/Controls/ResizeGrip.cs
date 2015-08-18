using Sakuno.SystemInterop;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls
{
    public class ResizeGrip : Control
    {
        Window r_Owner;
        bool r_CanResize;

        static ResizeGrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(typeof(ResizeGrip)));
        }

        public ResizeGrip()
        {
            Loaded += ResizeGrip_Loaded;
        }

        void ResizeGrip_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ResizeGrip_Loaded;

            var r_Owner = Window.GetWindow(this);
            if (r_Owner == null)
                return;

            var rHwndSource = (HwndSource)PresentationSource.FromVisual(r_Owner);
            rHwndSource?.AddHook(WndProc);

            EventHandler rHandler = delegate { r_CanResize = r_Owner.WindowState == WindowState.Normal; };
            r_Owner.StateChanged += rHandler;
            r_Owner.ContentRendered += rHandler;
        }

        IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            if ((NativeConstants.WindowMessage)rpMessage == NativeConstants.WindowMessage.WM_NCHITTEST && r_CanResize)
            {
                var rScreenPoint = new Point(NativeUtils.LoWord(rpLParam), NativeUtils.HiWord(rpLParam));
                var rClientPoint = PointFromScreen(rScreenPoint);
                var rRect = new Rect(0, 0, ActualWidth, ActualHeight);

                if (rRect.Contains(rClientPoint))
                {
                    rrpHandled = true;
                    return new IntPtr((int)NativeConstants.HitTest.HTBOTTOMRIGHT);
                }
            }

            return IntPtr.Zero;
        }
    }
}
