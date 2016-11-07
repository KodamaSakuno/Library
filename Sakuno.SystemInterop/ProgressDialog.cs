using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Sakuno.SystemInterop
{
    public class ProgressDialog : DisposableObject
    {
        NativeInterfaces.IProgressDialog r_Dialog = (NativeInterfaces.IProgressDialog)new NativeInterfaces.CProgressDialog();

        public IntPtr OwnerHandle { get; set; }
        public Window OwnerWindow { get; set; }

        public bool ModalWindow { get; set; }

        string r_Title;
        public string Title
        {
            get { return r_Title; }
            set
            {
                r_Title = value;
                r_Dialog.SetTitle(value);
            }
        }

        string r_Header;
        public string Header
        {
            get { return r_Header; }
            set
            {
                r_Header = value;
                r_Dialog.SetLine(1, value, CompactLongPath);
            }
        }

        string r_Message;
        public string Message
        {
            get { return r_Message; }
            set
            {
                r_Message = value;
                r_Dialog.SetLine(2, value, CompactLongPath);
            }
        }

        public bool CompactLongPath { get; set; }

        string r_CancellingMessage;
        public string CancellingMessage
        {
            get { return r_CancellingMessage; }
            set
            {
                r_CancellingMessage = value;
                r_Dialog.SetCancelMsg(value);
            }
        }

        long r_Total;
        public long Total
        {
            get { return r_Total; }
            set
            {
                r_Total = value;
                r_Dialog.SetProgress64(r_Progress, value);
            }
        }

        long r_Progress;
        public long Progress
        {
            get { return r_Progress; }
            set
            {
                r_Progress = value;
                r_Dialog.SetProgress64(value, r_Total);
            }
        }

        public bool ShowRemainingTime { get; set; }

        public bool NoMinimizeButton { get; set; }

        public bool NoProgressBar { get; set; }

        public bool MarqueeMode { get; set; } = true;

        public bool DisableCancelButton { get; set; }

        public bool HasCancelled => r_Dialog.HasUserCancelled();

        public void Show()
        {
            var rOptions = NativeEnums.PROGDLG.PROGDLG_NORMAL;

            if (ModalWindow)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_MODAL;
            if (ShowRemainingTime)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_AUTOTIME;
            if (NoMinimizeButton)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_NOMINIMIZE;
            if (NoProgressBar)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_NOPROGRESSBAR;
            if (MarqueeMode)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_MARQUEEPROGRESS;
            if (DisableCancelButton)
                rOptions |= NativeEnums.PROGDLG.PROGDLG_NOCANCEL;

            var rOwner = OwnerWindow != null ? new WindowInteropHelper(OwnerWindow).Handle : OwnerHandle;

            r_Dialog.StartProgressDialog(rOwner, null, rOptions);
        }

        public void SetTimer(ProgressDialogTimerAction rpAction) => r_Dialog.Timer(rpAction);

        public void Close() => r_Dialog.StopProgressDialog();

        protected override void DisposeManagedResources() => Close();
        protected override void DisposeNativeResources() => Marshal.FinalReleaseComObject(r_Dialog);
    }
}
