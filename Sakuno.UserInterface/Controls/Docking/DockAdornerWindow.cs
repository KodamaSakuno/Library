using Sakuno.SystemInterop;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls.Docking
{
    [TemplatePart(Name = "PART_LeftDockAdorner", Type = typeof(DockAdorner))]
    [TemplatePart(Name = "PART_TopDockAdorner", Type = typeof(DockAdorner))]
    [TemplatePart(Name = "PART_RightDockAdorner", Type = typeof(DockAdorner))]
    [TemplatePart(Name = "PART_BottomDockAdorner", Type = typeof(DockAdorner))]
    public class DockAdornerWindow : Control
    {
        Dictionary<DockDirection, DockAdorner> r_DockAdorners = new Dictionary<DockDirection, DockAdorner>();
        internal IEnumerable<DockAdorner> DockAdorners => r_DockAdorners.Values;

        HwndSource r_HwndSource;

        static DockAdornerWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockAdornerWindow), new FrameworkPropertyMetadata(typeof(DockAdornerWindow)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_DockAdorners[DockDirection.Left] = Template.FindName("PART_LeftDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Top] = Template.FindName("PART_TopDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Right] = Template.FindName("PART_RightDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Bottom] = Template.FindName("PART_BottomDockAdorner", this) as DockAdorner;
        }

        public void Show(DockableZone rpDockableZone)
        {
            if (r_HwndSource == null)
            {
                var rParameters = new HwndSourceParameters("DockAdornerWindow")
                {
                    Width = 0,
                    Height = 0,
                    PositionX = 0,
                    PositionY = 0,
                    UsesPerPixelOpacity = true,
                    WindowStyle = unchecked((int)(NativeEnums.WindowStyle.WS_POPUP | NativeEnums.WindowStyle.WS_DISABLED)),
                };
                r_HwndSource = new HwndSource(rParameters) { RootVisual = this };
            }

            var rPosition = rpDockableZone.PointToScreen(new Point(.0, .0));
            var rSize = rpDockableZone.RenderSize;
            NativeMethods.User32.SetWindowPos(r_HwndSource.Handle, IntPtr.Zero, (int)rPosition.X, (int)rPosition.Y, (int)rSize.Width, (int)rSize.Height, NativeEnums.SetWindowPosition.SWP_NOZORDER | NativeEnums.SetWindowPosition.SWP_NOACTIVATE | NativeEnums.SetWindowPosition.SWP_SHOWWINDOW);
        }

        public void Hide()
        {
            r_HwndSource?.Dispose();
            r_HwndSource = null;
        }
    }
}
