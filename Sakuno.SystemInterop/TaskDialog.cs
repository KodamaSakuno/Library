using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.SystemInterop
{
    public class TaskDialog : DisposableObject
    {
        public IntPtr OwnerWindowHandle { get; set; }
        public Window OwnerWindow { get; set; }
        public bool ShowAtTheCenterOfOwner { get; set; }

        public string Caption { get; set; }

        public TaskDialogIcon Icon { get; set; }

        public string Instruction { get; set; }
        public string Content { get; set; }

        public TaskDialogCommonButtons CommonButtons { get; set; }
        public TaskDialogCommonButton DefaultCommonButton { get; set; }

        public string FooterCheckboxText { get; set; }
        public bool IsFooterCheckboxChecked { get; set; }

        public string Detail { get; set; }
        public string DetailExpanderText { get; set; }
        public string DetailCollapserText { get; set; }
        public bool ShowDetailAtTheBottom { get; set; }
        public bool ExpandDetailByDefault { get; set; }

        public TaskDialogIcon FooterIcon { get; set; }
        public string Footer { get; set; }

        internal List<TaskDialogButton> r_Buttons;
        IntPtr r_ButtonsPointer;
        public IList<TaskDialogButton> Buttons => r_Buttons ?? (r_Buttons = new List<TaskDialogButton>());

        public TaskDialogButtonStyle ButtonStyle { get; set; }

        internal List<TaskDialogRadioButton> r_RadioButtons;
        IntPtr r_RadioButtonsPointer;
        public IList<TaskDialogRadioButton> RadioButtons => r_RadioButtons ?? (r_RadioButtons = new List<TaskDialogRadioButton>());

        public bool CanBeMinimized { get; set; }
        public bool CanBeClosedDirectly { get; set; }

        public bool EnableHyperlinks { get; set; }

        TaskDialogTickEventArgs r_TickEventArgs;

        public event EventHandler Opened;
        public event EventHandler<TaskDialogButtonClickedEventArgs> ButtonClicked;
        public event EventHandler<string> HyperlinkClicked;
        public event EventHandler<TaskDialogTickEventArgs> Tick;
        public event EventHandler Closed;

        public TaskDialogResult Show()
        {
            var rOptions = NativeEnums.TASKDIALOG_FLAGS.TDF_NONE;
            if (ShowAtTheCenterOfOwner)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW;
            if (EnableHyperlinks)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS;
            if (IsFooterCheckboxChecked)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED;
            if (ShowDetailAtTheBottom)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA;
            if (ExpandDetailByDefault)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT;
            if (CanBeMinimized)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED;
            if (CanBeClosedDirectly)
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION;

            if (Tick != null)
            {
                r_TickEventArgs = new TaskDialogTickEventArgs();
                rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER;
            }

            switch (ButtonStyle)
            {
                case TaskDialogButtonStyle.Normal:
                    if (r_Buttons != null && r_Buttons.OfType<TaskDialogCommandLink>().Any())
                        rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS;
                    break;

                case TaskDialogButtonStyle.CommandLink:
                    rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS;
                    break;

                case TaskDialogButtonStyle.CommandLinkWithoutIcon:
                    rOptions |= NativeEnums.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON;
                    break;
            }

            var rConfig = new NativeStructs.TASKDIALOGCONFIG()
            {
                cbSize = Marshal.SizeOf(typeof(NativeStructs.TASKDIALOGCONFIG)),
                hwndParent = OwnerWindow != null ? new WindowInteropHelper(OwnerWindow).Handle : OwnerWindowHandle,
                dwFlags = rOptions,
                hMainIcon = Icon,
                pszWindowTitle = Caption,
                pszMainInstruction = Instruction,
                pszContent = Content,
                pszVerificationText = FooterCheckboxText,
                pszExpandedInformation = Detail,
                pszExpandedControlText = DetailCollapserText,
                pszCollapsedControlText = DetailExpanderText,
                hFooterIcon = FooterIcon,
                pszFooter = Footer,
                dwCommonButtons = CommonButtons,
                pfCallback = DialogCallback,
            };

            SetButtons(ref rConfig);

            int rSelectedButton;
            int rSelectedRatioButton;
            bool rIsFooterCheckBoxChecked;
            var rResult = NativeMethods.ComCtl32.TaskDialogIndirect(ref rConfig, out rSelectedButton, out rSelectedRatioButton, out rIsFooterCheckBoxChecked);

            IsFooterCheckboxChecked = rIsFooterCheckBoxChecked;

            return new TaskDialogResult(this, rSelectedButton, rSelectedRatioButton, rIsFooterCheckBoxChecked);
        }

        void SetButtons(ref NativeStructs.TASKDIALOGCONFIG rrpConfig)
        {
            if (r_Buttons != null && r_Buttons.Count > 0)
            {
                rrpConfig.cButtons = r_Buttons.Count;
                rrpConfig.pButtons = r_ButtonsPointer = GetPointerOfButtons(r_Buttons);
            }
            if (r_RadioButtons != null && r_RadioButtons.Count > 0)
            {
                rrpConfig.cRadioButtons = r_RadioButtons.Count;
                rrpConfig.pRadioButtons = r_RadioButtonsPointer = GetPointerOfButtons(r_RadioButtons);

                var rDefaultRadioButton = r_Buttons.FirstOrDefault(r => r.IsDefault);
                if (rDefaultRadioButton != null)
                    rrpConfig.nDefaultRadioButton = rDefaultRadioButton.ID;
            }

            if (DefaultCommonButton != TaskDialogCommonButton.None)
                rrpConfig.nDefaultButton = (int)DefaultCommonButton;
            else if (r_Buttons != null)
            {
                var rDefaultButton = r_Buttons.FirstOrDefault(r => r.IsDefault);
                if (rDefaultButton != null)
                    rrpConfig.nDefaultButton = rDefaultButton.ID;
            }
        }

        IntPtr GetPointerOfButtons<T>(IList<T> rpButtons) where T : TaskDialogButtonBase
        {
            var rSize = Marshal.SizeOf(typeof(NativeStructs.TASKDIALOG_BUTTON));
            var rBuffer = Marshal.AllocHGlobal(rSize * rpButtons.Count);
            var rPointer = rBuffer;

            for (var i = 0; i < rpButtons.Count; i++)
            {
                var rButton = new NativeStructs.TASKDIALOG_BUTTON(rpButtons[i].ID, rpButtons[i].Text);

                Marshal.StructureToPtr(rButton, rPointer, false);
                rPointer = (IntPtr)(rPointer.ToInt32() + rSize);
            }

            return rBuffer;
        }

        int DialogCallback(IntPtr rpHandle, NativeConstants.TASKDIALOG_NOTIFICATIONS rpNotification, IntPtr rpWParam, IntPtr rpLParam, IntPtr rpCallbackData)
        {
            switch (rpNotification)
            {
                case NativeConstants.TASKDIALOG_NOTIFICATIONS.CREATED:
                    Opened?.Invoke(this, EventArgs.Empty);
                    break;

                case NativeConstants.TASKDIALOG_NOTIFICATIONS.BUTTONCLICKED:
                    var rButtonID = (int)rpWParam;
                    if (r_Buttons != null)
                    {
                        var rButton = r_Buttons.FirstOrDefault(r => r.ID == rButtonID);
                        if (rButton != null)
                        {
                            var rEventArgs = new TaskDialogButtonClickedEventArgs(rButton);

                            ButtonClicked?.Invoke(this, rEventArgs);
                            return !rEventArgs.Cancel ? 0 : 1;
                        }
                    }
                    break;

                case NativeConstants.TASKDIALOG_NOTIFICATIONS.HYPERLINKCLICKED:
                    HyperlinkClicked?.Invoke(this, Marshal.PtrToStringUni(rpLParam));
                    break;

                case NativeConstants.TASKDIALOG_NOTIFICATIONS.TIMER:
                    r_TickEventArgs.Ticks = (int)rpWParam;

                    if (r_TickEventArgs.Reset)
                    {
                        r_TickEventArgs.Reset = false;
                        return 1;
                    }
                    break;

                case NativeConstants.TASKDIALOG_NOTIFICATIONS.DESTROYED:
                    Closed?.Invoke(this, EventArgs.Empty);
                    break;
            }

            return 0;
        }

        protected override void DisposeManagedResources()
        {
            if (r_ButtonsPointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(r_ButtonsPointer);
                r_ButtonsPointer = IntPtr.Zero;
            }
            if (r_RadioButtonsPointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(r_RadioButtonsPointer);
                r_RadioButtonsPointer = IntPtr.Zero;
            }
        }
    }
}
