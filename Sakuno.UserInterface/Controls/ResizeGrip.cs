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
            Unloaded += ResizeGrip_Unloaded;
        }

        void ResizeGrip_Loaded(object sender, RoutedEventArgs e)
        {
            r_Owner = Window.GetWindow(this);
            if (r_Owner == null)
                return;

            var rHwndSource = (HwndSource)PresentationSource.FromVisual(r_Owner);
            rHwndSource?.AddHook(WndProc);

            r_Owner.StateChanged += OnOwnerStateChanged;
            r_Owner.ContentRendered += OnOwnerStateChanged;
        }
        void ResizeGrip_Unloaded(object sender, RoutedEventArgs e)
        {
            r_Owner.StateChanged -= OnOwnerStateChanged;
            r_Owner.ContentRendered -= OnOwnerStateChanged;
        }

        void OnOwnerStateChanged(object sender, EventArgs e) => r_CanResize = r_Owner.WindowState == WindowState.Normal;

        IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            if (rpMessage == (int)NativeConstants.WindowMessage.WM_NCHITTEST && r_CanResize)
            {
                var rScreenPoint = rpLParam.ToPoint();
                var rClientPoint = PointFromScreen(rScreenPoint);
                var rRect = new Rect(0, 0, ActualWidth, ActualHeight);

                if (rRect.Contains(rClientPoint))
                {
                    rrpHandled = true;
                    return (IntPtr)NativeConstants.HitTest.HTBOTTOMRIGHT;
                }
            }

            return IntPtr.Zero;
        }
    }
}
