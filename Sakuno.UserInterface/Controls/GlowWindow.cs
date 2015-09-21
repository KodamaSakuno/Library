﻿using Sakuno.SystemInterop;
using Sakuno.UserInterface.Behaviors;
using Sakuno.UserInterface.Internal;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;

namespace Sakuno.UserInterface.Controls
{
    class GlowWindow : Window
    {
        public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof(GlowBrush), typeof(Brush), typeof(GlowWindow),
            new UIPropertyMetadata(Brushes.Transparent));
        public Brush GlowBrush
        {
            get { return (Brush)GetValue(GlowBrushProperty); }
            set { SetValue(GlowBrushProperty, value); }
        }

        public static readonly DependencyProperty InactiveGlowBrushProperty = DependencyProperty.Register(nameof(InactiveGlowBrush), typeof(Brush), typeof(GlowWindow),
            new UIPropertyMetadata(Brushes.Transparent));
        public Brush InactiveGlowBrush
        {
            get { return (Brush)GetValue(InactiveGlowBrushProperty); }
            set { SetValue(InactiveGlowBrushProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(GlowWindow),
            new UIPropertyMetadata(Orientation.Vertical));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty IsGlowingProperty = DependencyProperty.Register(nameof(IsGlowing), typeof(bool), typeof(GlowWindow),
            new UIPropertyMetadata(BooleanUtil.True));
        public bool IsGlowing
        {
            get { return (bool)GetValue(IsGlowingProperty); }
            set { SetValue(IsGlowingProperty, value); }
        }

        Window r_Owner;
        IntPtr r_Handle, r_OwnerHandle;
        WindowState r_OwnerOldState;
        bool r_OwnerClosed;

        HwndSource r_HwndSource;

        GlowWindowProcessor r_Processor;

        static GlowWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(typeof(GlowWindow)));
        }
        public GlowWindow(GlowWindowBehavior rpBehavior, GlowWindowProcessor rpProcessor)
        {
            r_Owner = rpBehavior.Window;
            r_Processor = rpProcessor;

            AllowsTransparency = true;
            ShowActivated = false;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Visibility = Visibility.Collapsed;

            VerticalContentAlignment = rpProcessor.VerticalAlignment;
            HorizontalContentAlignment = rpProcessor.HorizontalAlignment;
            Orientation = rpProcessor.Orientation;
            Left = rpProcessor.GetLeft(r_Owner.Left, r_Owner.ActualWidth);
            Top = rpProcessor.GetTop(r_Owner.Top, r_Owner.ActualHeight);
            Width = rpProcessor.GetWidth(r_Owner.Left, r_Owner.ActualWidth);
            Height = rpProcessor.GetHeight(r_Owner.Top, r_Owner.ActualHeight);

            SetBinding(GlowBrushProperty, new Binding(nameof(rpBehavior.GlowBrush)) { Source = rpBehavior });
            SetBinding(InactiveGlowBrushProperty, new Binding(nameof(rpBehavior.InactiveGlowBrush)) { Source = rpBehavior });

            EventHandler rUpdate = (s, e) => Update();
            r_Owner.Activated += rUpdate;
            r_Owner.Deactivated += rUpdate;
            r_Owner.LocationChanged += rUpdate;
            r_Owner.SizeChanged += (s, e) => Update();
            r_Owner.StateChanged += (s, e) =>
            {
                Update();
                r_OwnerOldState = r_Owner.WindowState;
            };

            r_Owner.Closed += (s, e) => Close();

            r_Owner.ContentRendered += (s, e) =>
            {
                Show();
                Update();
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            r_HwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (r_HwndSource != null)
            {
                r_Handle = r_HwndSource.Handle;

                var rExtentedWindowStyle = (NativeEnums.ExtendedWindowStyle)NativeMethods.User32.GetWindowLongPtr(r_Handle, NativeConstants.GetWindowLong.GWL_EXSTYLE);
                rExtentedWindowStyle ^= NativeEnums.ExtendedWindowStyle.WS_EX_APPWINDOW;
                rExtentedWindowStyle |= NativeEnums.ExtendedWindowStyle.WS_EX_NOACTIVATE;
                NativeMethods.User32.SetWindowLongPtr(r_Handle, NativeConstants.GetWindowLong.GWL_EXSTYLE, (IntPtr)rExtentedWindowStyle);
                
                r_HwndSource.AddHook(WndProc);
            }

            Owner = r_Owner;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            r_HwndSource?.RemoveHook(WndProc);
            r_OwnerClosed = true;
        }

        public async void Update()
        {
            if (r_OwnerClosed)
                return;

            if (r_Owner.WindowState != WindowState.Normal)
                Visibility = Visibility.Collapsed;
            else
            {
                if (SystemParameters.MinimizeAnimation && r_OwnerOldState == WindowState.Minimized)
                    await Task.Delay(250);

                Visibility = Visibility.Visible;
                UpdateCore();
            }
        }
        void UpdateCore()
        {
            if (r_OwnerHandle == IntPtr.Zero)
                r_OwnerHandle = new WindowInteropHelper(r_Owner).Handle;

            IsGlowing = r_Owner.IsActive;

            var rLeft = (int)Math.Round(r_Processor.GetLeft(r_Owner.Left, r_Owner.ActualWidth) * DpiUtil.ScaleX);
            var rTop = (int)Math.Round(r_Processor.GetTop(r_Owner.Top, r_Owner.ActualHeight) * DpiUtil.ScaleY);
            var rWidth = (int)Math.Round(r_Processor.GetWidth(r_Owner.Left, r_Owner.ActualWidth) * DpiUtil.ScaleX);
            var rHeight = (int)Math.Round(r_Processor.GetHeight(r_Owner.Top, r_Owner.ActualHeight) * DpiUtil.ScaleY);

            NativeMethods.User32.SetWindowPos(r_Handle, r_OwnerHandle, rLeft, rTop, rWidth, rHeight, NativeEnums.SetWindowPosition.SWP_NOACTIVATE);
        }

        IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            Point rScreenPoint;

            var rMessage = (NativeConstants.WindowMessage)rpMessage;
            switch (rMessage)
            {
                case NativeConstants.WindowMessage.WM_MOUSEACTIVATE:
                    rrpHandled = true;
                    return (IntPtr)NativeConstants.MouseActivate.MA_NOACTIVATE;

                case NativeConstants.WindowMessage.WM_NCHITTEST:
                    if (r_Owner.ResizeMode == ResizeMode.NoResize)
                        break;

                    rScreenPoint = rpLParam.ToPoint();
                    var rClientPoint = PointFromScreen(rScreenPoint);
                    Cursor = r_Processor.GetCursor(rClientPoint, ActualWidth, ActualHeight);
                    break;

                case NativeConstants.WindowMessage.WM_LBUTTONDOWN:
                    if (!r_Owner.IsActive)
                        r_Owner.Activate();

                    rScreenPoint = rpLParam.ToPoint();
                    var rHitTest = (IntPtr)r_Processor.GetHitTestValue(rScreenPoint, ActualWidth, ActualHeight);
                    NativeMethods.User32.PostMessageW(r_OwnerHandle, NativeConstants.WindowMessage.WM_NCLBUTTONDOWN, rHitTest, IntPtr.Zero);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
