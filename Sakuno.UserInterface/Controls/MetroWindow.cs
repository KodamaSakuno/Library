﻿using Microsoft.Windows.Shell;
using Sakuno.SystemInterop;
using Sakuno.UserInterface.Behaviors;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Controls
{
    public class MetroWindow : Window
    {
        public static readonly DependencyProperty GlowWindowBehaviorProperty = DependencyProperty.Register(nameof(GlowWindowBehavior), typeof(GlowWindowBehavior), typeof(MetroWindow),
            new UIPropertyMetadata(null, OnGlowWindowBehaviorChanged));
        public GlowWindowBehavior GlowWindowBehavior
        {
            get { return (GlowWindowBehavior)GetValue(GlowWindowBehaviorProperty); }
            set { SetValue(GlowWindowBehaviorProperty, value); }
        }

        public static readonly DependencyProperty IsCaptionBarProperty = DependencyProperty.RegisterAttached("IsCaptionBar", typeof(bool), typeof(MetroWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnIsCaptionBarChanged));
        public static void SetIsCaptionBar(FrameworkElement rpElement, Boolean rpValue) => rpElement.SetValue(IsCaptionBarProperty, rpValue);
        public static bool GetIsCaptionBar(FrameworkElement rpElement) => (bool)rpElement.GetValue(IsCaptionBarProperty);

        static readonly DependencyPropertyKey ScreenOrientationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ScreenOrientation), typeof(ScreenOrientation), typeof(MetroWindow),
            new UIPropertyMetadata(ScreenOrientation.Landscape));
        public static readonly DependencyProperty ScreenOrientationProperty = ScreenOrientationPropertyKey.DependencyProperty;
        public ScreenOrientation ScreenOrientation
        {
            get { return (ScreenOrientation)GetValue(ScreenOrientationProperty); }
            private set { SetValue(ScreenOrientationPropertyKey, value); }
        }

        HwndSource r_HwndSource;

        FrameworkElement r_CaptionBar;

        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(BooleanUtil.True));
        }

        static void OnGlowWindowBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rWindow = (MetroWindow)d;
            var rOldBehavior = (GlowWindowBehavior)e.OldValue;
            var rNewBehavior = (GlowWindowBehavior)e.NewValue;

            if (rOldBehavior == rNewBehavior)
                return;

            var rBehaviors = Interaction.GetBehaviors(rWindow);
            if (rOldBehavior != null)
                rBehaviors.Remove(rOldBehavior);
            if (rNewBehavior != null)
                rBehaviors.Add(rNewBehavior);
        }
        static void OnIsCaptionBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rElement = d as FrameworkElement;
            if (rElement == null)
                return;
            var rWindow = GetWindow(rElement) as MetroWindow;
            if (rWindow == null)
                return;

            rWindow.r_CaptionBar = (bool)e.NewValue ? rElement : null;

            rElement.Loaded += delegate
            {
                var rChrome = WindowChrome.GetWindowChrome(rWindow);
                if (rChrome != null)
                    rChrome.CaptionHeight = rElement.ActualHeight;
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            r_HwndSource = HwndSource.FromVisual(this) as HwndSource;
            if (r_HwndSource != null)
            {
                r_HwndSource.AddHook(WndProc);

                InitializeScreenOrientation();
            }
        }
        void InitializeScreenOrientation()
        {
            var rMonitor = NativeMethods.User32.MonitorFromWindow(r_HwndSource.Handle, NativeConstants.MFW.MONITOR_DEFAULTTONEAREST);
            var rInfo = new NativeStructs.MONITORINFO() { cbSize = Marshal.SizeOf(typeof(NativeStructs.MONITORINFO)) };
            NativeMethods.User32.GetMonitorInfo(rMonitor, ref rInfo);

            var rWidth = rInfo.rcMonitor.Width;
            var rHeight = rInfo.rcMonitor.Height;

            ScreenOrientation = rWidth > rHeight ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
        }
        IntPtr WndProc(IntPtr rpHandle, int rpMessage, IntPtr rpWParam, IntPtr rpLParam, ref bool rrpHandled)
        {
            var rMessage = (NativeConstants.WindowMessage)rpMessage;
            switch (rMessage)
            {
                case NativeConstants.WindowMessage.WM_DISPLAYCHANGE:
                    var rWidth = NativeUtils.LoWord(rpLParam);
                    var rHeight = NativeUtils.HiWord(rpLParam);

                    var rWindowLeft = Top;
                    var rWindowTop = Left;
                    Left = rWindowLeft;
                    Top = rWindowTop;

                    var rWindowWidth = Height;
                    var rWindowHeight = Width;
                    Width = rWindowWidth;
                    Height = rWindowHeight;

                    ScreenOrientation = rWidth > rHeight ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
                    break;
            }

            return IntPtr.Zero;
        }

    }
}
