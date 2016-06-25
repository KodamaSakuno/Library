namespace Sakuno.SystemInterop
{
    public class TaskDialogButton : TaskDialogButtonBase
    {
        public TaskDialogButton(string rpText) : base(rpText) { }
        public TaskDialogButton(int rpID, string rpText) : base(rpID, rpText) { }
        public TaskDialogButton(TaskDialogCommonButton rpID, string rpText) : base(rpID, rpText) { }
    }
}
