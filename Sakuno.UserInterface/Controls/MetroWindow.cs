using Sakuno.SystemInterop;
using Sakuno.UserInterface.Behaviors;
using System;
using System.ComponentModel;
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

        public static readonly DependencyProperty WindowPlacementPreferenceProperty = DependencyProperty.Register(nameof(WindowPlacementPreference), typeof(IWindowPlacementPreference), typeof(MetroWindow),
            new UIPropertyMetadata(null));
        public IWindowPlacementPreference WindowPlacementPreference
        {
            get { return (IWindowPlacementPreference)GetValue(WindowPlacementPreferenceProperty); }
            set { SetValue(WindowPlacementPreferenceProperty, value); }
        }

        public static readonly DependencyProperty IsCaptionBarProperty = DependencyProperty.RegisterAttached("IsCaptionBar", typeof(bool), typeof(MetroWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnIsCaptionBarChanged));
        public static void SetIsCaptionBar(FrameworkElement rpElement, bool rpValue) => rpElement.SetValue(IsCaptionBarProperty, rpValue);
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

                if (WindowPlacementPreference != null)
                {
                    var rPlacementData = WindowPlacementPreference.Load(this);
                    if (rPlacementData.HasValue)
                    {
                        var rPlacement = rPlacementData.Value;

                        rPlacement.length = Marshal.SizeOf(typeof(NativeStructs.WINDOWPLACEMENT));
                        rPlacement.flags = 0;

                        if (rPlacement.showCmd == NativeConstants.ShowCommand.SW_SHOWMINIMIZED)
                            rPlacement.showCmd = NativeConstants.ShowCommand.SW_SHOWNORMAL;

                        NativeMethods.User32.SetWindowPlacement(r_HwndSource.Handle, ref rPlacement);
                    }
                }
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

                    var rScreenOrientation = rWidth > rHeight ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
                    if (rScreenOrientation != ScreenOrientation)
                    {

                        var rWindowLeft = Top;
                        var rWindowTop = Left;
                        Left = rWindowLeft;
                        Top = rWindowTop;

                        var rWindowWidth = Height;
                        var rWindowHeight = Width;
                        Width = rWindowWidth;
                        Height = rWindowHeight;

                        ScreenOrientation = rScreenOrientation;
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && WindowPlacementPreference != null)
            {
                NativeStructs.WINDOWPLACEMENT rPlacement;
                NativeMethods.User32.GetWindowPlacement(r_HwndSource.Handle, out rPlacement);

                WindowPlacementPreference.Save(this, rPlacement);
            }
        }
    }
}
