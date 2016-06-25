using System;

namespace Sakuno.SystemInterop
{
    public class TaskDialogCommandLink : TaskDialogButton
    {
        public TaskDialogCommandLink(string rpText) : base(rpText) { }
        public TaskDialogCommandLink(string rpText, string rpInstruction) : base(rpText + Environment.NewLine + rpInstruction) { }

        public TaskDialogCommandLink(int rpID, string rpText) : base(rpID, rpText) { }
        public TaskDialogCommandLink(int rpID, string rpText, string rpInstruction) : base(rpID, rpText + Environment.NewLine + rpInstruction) { }

        public TaskDialogCommandLink(TaskDialogCommonButton rpID, string rpText) : base(rpID, rpText) { }
        public TaskDialogCommandLink(TaskDialogCommonButton rpID, string rpText, string rpInstruction) : this((int)rpID, rpText, rpInstruction) { }
    }
}
