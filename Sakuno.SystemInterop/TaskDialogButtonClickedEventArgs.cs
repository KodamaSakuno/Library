using System.ComponentModel;

namespace Sakuno.SystemInterop
{
    public class TaskDialogButtonClickedEventArgs : CancelEventArgs
    {
        public TaskDialogResult TaskDialogResult { get; set; }

        public TaskDialogButton Button { get; }

        internal TaskDialogButtonClickedEventArgs(TaskDialogButton rpButton)
        {
            Button = rpButton;
        }
    }
}
