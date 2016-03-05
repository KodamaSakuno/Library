using Microsoft.Windows.Shell;
using Sakuno.SystemInterop;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls
{
    public class WindowCaptionButton : Button
    {
        public static readonly DependencyProperty WindowActionProperty = DependencyProperty.Register(nameof(WindowAction), typeof(WindowAction), typeof(WindowCaptionButton),
            new UIPropertyMetadata(WindowAction.None));
        public WindowAction WindowAction
        {
            get { return (WindowAction)GetValue(WindowActionProperty); }
            set { SetValue(WindowActionProperty, value); }
        }

        Window r_Owner;
        IntPtr r_OwnerHandle;

        static WindowCaptionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCaptionButton), new FrameworkPropertyMetadata(typeof(WindowCaptionButton)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            r_Owner = Window.GetWindow(this);
            if (r_Owner != null)
            {
                r_OwnerHandle = new WindowInteropHelper(r_Owner).Handle;

                r_Owner.StateChanged += delegate { OnOwnerStateChanged(); };
                OnOwnerStateChanged();
            }
        }

        protected override void OnClick()
        {
            switch (WindowAction)
            {
                case WindowAction.Close: PostSystemCommand(NativeConstants.SystemCommand.SC_CLOSE); break;
                case WindowAction.Minimize: PostSystemCommand(NativeConstants.SystemCommand.SC_MINIMIZE); break;
                case WindowAction.Normalize: PostSystemCommand(NativeConstants.SystemCommand.SC_RESTORE); break;
                case WindowAction.Maximize: PostSystemCommand(NativeConstants.SystemCommand.SC_MAXIMIZE); break;
                case WindowAction.ShowSystemMenu:
                    var rLocation = PointToScreen(new Point(0, ActualHeight));
                    SystemCommands.ShowSystemMenu(r_Owner, rLocation);
                    break;
            }

            base.OnClick();
        }

        void PostSystemCommand(NativeConstants.SystemCommand rpCommand)
        {
            if (r_OwnerHandle == IntPtr.Zero)
                r_OwnerHandle = new WindowInteropHelper(r_Owner).Handle;

            NativeMethods.User32.PostMessageW(r_OwnerHandle, NativeConstants.WindowMessage.WM_SYSCOMMAND, (IntPtr)rpCommand, IntPtr.Zero);
        }

        void OnOwnerStateChanged()
        {
            switch (WindowAction)
            {
                case WindowAction.Minimize:
                    Visibility = BooleanToVisibility(r_Owner.WindowState != WindowState.Minimized); break;
                case WindowAction.Normalize:
                    Visibility = BooleanToVisibility(r_Owner.WindowState != WindowState.Normal); break;
                case WindowAction.Maximize:
                    Visibility = BooleanToVisibility(r_Owner.WindowState != WindowState.Maximized); break;
            }
        }
        Visibility BooleanToVisibility(bool rpValue) => rpValue ? Visibility.Visible : Visibility.Collapsed;
    }
}
