namespace Sakuno.SystemInterop
{
    public class TaskDialogRadioButton : TaskDialogButtonBase
    {
        public TaskDialogRadioButton(string rpText) : base(rpText) { }
        public TaskDialogRadioButton(int rpID, string rpText) : base(rpID, rpText) { }
        public TaskDialogRadioButton(TaskDialogCommonButton rpID, string rpText) : base(rpID, rpText) { }
    }
}
