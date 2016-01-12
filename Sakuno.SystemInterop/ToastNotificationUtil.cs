using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace Sakuno.SystemInterop
{
    public static class ToastNotificationUtil
    {
        static ToastNotifier r_Notifier;

        public static void Initialize(string rpShortcutName, string rpShortcutTargetPath, string rpAppUserModelID)
        {
            ShellUtil.InstallShortcutInStartScreen(rpShortcutName, rpShortcutTargetPath, rpAppUserModelID);

            r_Notifier = ToastNotificationManager.CreateToastNotifier(rpAppUserModelID);
        }

        public static async void Show(ToastContent rpContent)
        {
            if (!OS.IsWin8OrLater)
                throw new NotSupportedException();

            var rDocument = FillDocument(rpContent);

            var rResult = await ShowCore(rDocument);
            if (rResult)
                rpContent.InvokeActivated();
        }
        static async Task<bool> ShowCore(XmlDocument rpDocument)
        {
            var rCompletionSource = new TaskCompletionSource<bool>();

            var rNotification = new ToastNotification(rpDocument);

            TypedEventHandler<ToastNotification, object> rActivated = (s, e) => rCompletionSource.SetResult(true);
            rNotification.Activated += rActivated;

            TypedEventHandler<ToastNotification, ToastDismissedEventArgs> rDismissed = (s, e) => rCompletionSource.SetResult(false);
            rNotification.Dismissed += rDismissed;

            TypedEventHandler<ToastNotification, ToastFailedEventArgs> rFailed = (s, e) => rCompletionSource.SetException(e.ErrorCode);
            rNotification.Failed += rFailed;

            r_Notifier.Show(rNotification);

            var rResult = await rCompletionSource.Task;

            rNotification.Activated -= rActivated;
            rNotification.Dismissed -= rDismissed;
            rNotification.Failed -= rFailed;

            return rResult;
        }

        static ToastTemplateType GetTemplateType(ToastContent rpContent)
        {
            if (rpContent.ImagePath.IsNullOrEmpty())
            {
                if (rpContent.Title.IsNullOrEmpty())
                    return ToastTemplateType.ToastText01;
                if (rpContent.ShowTitleInTwoLines)
                    return ToastTemplateType.ToastText03;
                if (!rpContent.BodySecondLine.IsNullOrEmpty())
                    return ToastTemplateType.ToastText04;
                return ToastTemplateType.ToastText02;
            }
            else
            {
                if (rpContent.Title.IsNullOrEmpty())
                    return ToastTemplateType.ToastImageAndText01;
                if (rpContent.ShowTitleInTwoLines)
                    return ToastTemplateType.ToastImageAndText03;
                if (!rpContent.BodySecondLine.IsNullOrEmpty())
                    return ToastTemplateType.ToastImageAndText04;
                return ToastTemplateType.ToastImageAndText02;
            }
        }
        static XmlDocument FillDocument(ToastContent rpContent)
        {
            var rType = GetTemplateType(rpContent);
            var rDocument = ToastNotificationManager.GetTemplateContent(rType);

            switch (rType)
            {
                case ToastTemplateType.ToastImageAndText01:
                case ToastTemplateType.ToastImageAndText02:
                case ToastTemplateType.ToastImageAndText03:
                case ToastTemplateType.ToastImageAndText04:
                    var rImageElement = rDocument.GetElementsByTagName("image").First();
                    rImageElement.Attributes.GetNamedItem("src").NodeValue = new Uri(rpContent.ImagePath).ToString();
                    break;
            }

            var rTextElements = rDocument.GetElementsByTagName("text");
            switch (rType)
            {
                case ToastTemplateType.ToastImageAndText01:
                case ToastTemplateType.ToastText01:
                    rTextElements[0].AppendChild(rDocument.CreateTextNode(rpContent.Body));
                    break;

                case ToastTemplateType.ToastImageAndText02:
                case ToastTemplateType.ToastImageAndText03:
                case ToastTemplateType.ToastText02:
                case ToastTemplateType.ToastText03:
                    rTextElements[0].AppendChild(rDocument.CreateTextNode(rpContent.Title));
                    rTextElements[1].AppendChild(rDocument.CreateTextNode(rpContent.Body));
                    break;

                case ToastTemplateType.ToastImageAndText04:
                case ToastTemplateType.ToastText04:
                    rTextElements[0].AppendChild(rDocument.CreateTextNode(rpContent.Title));
                    rTextElements[1].AppendChild(rDocument.CreateTextNode(rpContent.Body));
                    rTextElements[2].AppendChild(rDocument.CreateTextNode(rpContent.BodySecondLine));
                    break;
            }

            var rAudioElement = rDocument.CreateElement("audio");
            switch (rpContent.Audio)
            {
                case ToastAudio.None:
                    rAudioElement.SetAttribute("silent", "true");
                    break;

                case ToastAudio.Default:
                case ToastAudio.IM:
                case ToastAudio.Mail:
                case ToastAudio.Reminder:
                case ToastAudio.SMS:
                    rAudioElement.SetAttribute("src", "ms-winsoundevent:Notification." + rpContent.Audio.ToString());
                    rAudioElement.SetAttribute("loop", "false");
                    break;

                default:
                    rDocument.DocumentElement.SetAttribute("duration", "long");

                    var rAudioValue = (int)rpContent.Audio - (int)ToastAudio.LoopingAlarm;
                    string rPrefix;
                    if (rAudioValue / 10 == 0)
                        rPrefix = "Looping.Alarm";
                    else
                        rPrefix = "Looping.Call";

                    string rID;
                    if (rAudioValue % 10 == 0)
                        rID = string.Empty;
                    else
                        rID = (rAudioValue % 10 + 1).ToString();

                    rAudioElement.SetAttribute("src", $"ms-winsoundevent:Notification." + rPrefix + rID);
                    rAudioElement.SetAttribute("loop", "true");
                    break;
            }

            rDocument.DocumentElement.AppendChild(rAudioElement);

            return rDocument;
        }
    }
}
