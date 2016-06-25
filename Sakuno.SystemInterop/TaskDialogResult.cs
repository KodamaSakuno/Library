namespace Sakuno.SystemInterop
{
    public struct TaskDialogResult
    {
        public TaskDialogCommonButton ClickedCommonButton { get; }

        public TaskDialogButton SelectedButton { get; }
        public TaskDialogRadioButton SelectedRadioButton { get; }

        public bool IsFooterCheckBoxChecked { get; }

        internal TaskDialogResult(TaskDialog rpOwner, int rpSelectedButton, int rpSelectedRadioButton, bool rpIsFooterCheckBoxChecked)
        {
            switch (rpSelectedButton)
            {
                case 1:
                    ClickedCommonButton = TaskDialogCommonButton.OK;
                    break;

                case 2:
                    ClickedCommonButton = TaskDialogCommonButton.Cancel;
                    break;

                case 4:
                    ClickedCommonButton = TaskDialogCommonButton.Retry;
                    break;

                case 6:
                    ClickedCommonButton = TaskDialogCommonButton.Yes;
                    break;

                case 7:
                    ClickedCommonButton = TaskDialogCommonButton.No;
                    break;

                case 8:
                    ClickedCommonButton = TaskDialogCommonButton.Close;
                    break;

                default:
                    ClickedCommonButton = TaskDialogCommonButton.None;
                    break;
            }

            SelectedButton = null;
            if (ClickedCommonButton == TaskDialogCommonButton.None && rpOwner.r_Buttons != null)
                foreach (var rButton in rpOwner.r_Buttons)
                    if (rButton.ID == rpSelectedButton)
                        SelectedButton = rButton;

            SelectedRadioButton = null;
            if (rpSelectedRadioButton != 0 && rpOwner.r_RadioButtons != null)
                foreach (var rRadioButton in rpOwner.r_RadioButtons)
                    if (rRadioButton.ID == rpSelectedButton)
                        SelectedRadioButton = rRadioButton;

            IsFooterCheckBoxChecked = rpIsFooterCheckBoxChecked;
        }
    }
}
