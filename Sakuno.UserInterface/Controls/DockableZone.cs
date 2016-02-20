using Sakuno.SystemInterop;
using Sakuno.UserInterface.Controls.Docking;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class DockableZone : ContentControl
    {
        static readonly DependencyPropertyKey IsParticipatingInDockingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsParticipatingInDocking), typeof(bool), typeof(DockableZone), new UIPropertyMetadata(BooleanUtil.False));
        public static readonly DependencyProperty IsParticipatingInDockingProperty = IsParticipatingInDockingPropertyKey.DependencyProperty;
        public bool IsParticipatingInDocking
        {
            get { return (bool)GetValue(IsParticipatingInDockingProperty); }
            private set { SetValue(IsParticipatingInDockingPropertyKey, BooleanUtil.GetBoxed(value)); }
        }

        static HashSet<DockableZone> r_Instances = new HashSet<DockableZone>();
        static Dictionary<Window, List<DockableZone>> r_InstancesGroupByWindow = new Dictionary<Window, List<DockableZone>>();

        Dictionary<DockDirection, DockAdorner> r_DockAdorners = new Dictionary<DockDirection, DockAdorner>();

        DockOperationInfo r_OperationCompletionInfo;

        static DockableZone()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockableZone), new FrameworkPropertyMetadata(typeof(DockableZone)));

            EventManager.RegisterClassHandler(typeof(DockableZone), AdvancedTabItem.PreviewDragDeltaEvent, new AdvancedTabDragDeltaEventHandler((s, e) => ((DockableZone)s).OnItemPreviewDragDelta(e)), true);
            EventManager.RegisterClassHandler(typeof(DockableZone), AdvancedTabItem.DragCompletedEvent, new AdvancedTabDragCompletedEventHandler((s, e) => ((DockableZone)s).OnItemDragCompleted(e)), true);
        }
        public DockableZone()
        {
            Loaded += (s, e) =>
            {
                r_Instances.Add(this);

                var rWindow = Window.GetWindow(this);
                if (rWindow == null)
                    return;

                List<DockableZone> rDockableZones;
                if (!r_InstancesGroupByWindow.TryGetValue(rWindow, out rDockableZones))
                    r_InstancesGroupByWindow[rWindow] = rDockableZones = new List<DockableZone>();

                rDockableZones.Add(this);
            };
            Unloaded += (s, e) =>
            {
                r_Instances.Remove(this);

                var rWindow = Window.GetWindow(this);
                if (rWindow == null)
                    return;

                List<DockableZone> rDockableZones;
                if (!r_InstancesGroupByWindow.TryGetValue(rWindow, out rDockableZones) && rDockableZones.Count == 0)
                    return;

                rDockableZones.Remove(this);

                if (rDockableZones.Count == 0)
                    r_InstancesGroupByWindow.Remove(rWindow);
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_DockAdorners[DockDirection.Left] = Template.FindName("PART_LeftDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Top] = Template.FindName("PART_TopDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Right] = Template.FindName("PART_RightDockAdorner", this) as DockAdorner;
            r_DockAdorners[DockDirection.Bottom] = Template.FindName("PART_BottomDockAdorner", this) as DockAdorner;
        }

        void OnItemPreviewDragDelta(AdvancedTabDragDeltaEventArgs e)
        {
            if (e.Cancel)
                return;

            DockableZone rParticipatingDockableZone = null;
            Point rMousePosition = default(Point);

            var rHeaderItemsControl = ItemsControl.ItemsControlFromItemContainer(e.Item) as AdvancedTabHeaderItemsControl;
            if (rHeaderItemsControl?.Items.Count == 1)
            {
                var rOwnerWindow = Window.GetWindow(this);

                var rInfos = r_InstancesGroupByWindow.Where(r => r.Key != rOwnerWindow).SelectMany(r => r.Value).Select(r =>
                {
                    var rPointTopLeft = r.PointToScreen(default(Point));
                    var rPointBottomRight = r.PointToScreen(new Point(r.ActualWidth, r.ActualHeight));

                    return new { DockableZone = r, Rect = new Rect(rPointTopLeft, rPointBottomRight) };
                });

                rInfos = from rWindow in WindowUtil.GetWindowsOrderedByZOrder(Application.Current.Windows.OfType<Window>())
                         join rInfo in rInfos on rWindow equals Window.GetWindow(rInfo.DockableZone)
                         select rInfo;

                NativeStructs.POINT rCursorPosition;
                NativeMethods.User32.GetCursorPos(out rCursorPosition);
                rMousePosition = new Point(rCursorPosition.X, rCursorPosition.Y);

                foreach (var rTarget in rInfos)
                {
                    rTarget.DockableZone.IsParticipatingInDocking = rTarget.Rect.Contains(rMousePosition);
                    if (rTarget.DockableZone.IsParticipatingInDocking)
                        rParticipatingDockableZone = rTarget.DockableZone;
                }
            }

            if (rParticipatingDockableZone != null)
                r_OperationCompletionInfo = rParticipatingDockableZone.MonitorDockAdorners(rMousePosition);
        }
        DockOperationInfo MonitorDockAdorners(Point rpMousePosition)
        {
            var rWindow = Window.GetWindow(this);
            if (rWindow == null)
                return null;

            var rHitAdorner = r_DockAdorners.Values.Where(r => r != null).Select(r =>
            {
                var rPoint = rWindow.PointFromScreen(rpMousePosition);
                rPoint = rWindow.TranslatePoint(rPoint, r);

                return new { DockAdorner = r, Result = r.InputHitTest(rPoint) != null };
            }).FirstOrDefault(r => r.Result);

            foreach (var rDockAdorner in r_DockAdorners.Values.Where(r => r != null))
                if (rHitAdorner?.DockAdorner == rDockAdorner)
                    rDockAdorner.IsHighlighted = true;
                else
                    rDockAdorner.IsHighlighted = false;

            if (rHitAdorner == null)
                return null;

            return new DockOperationInfo(this, rHitAdorner.DockAdorner);
        }

        void OnItemDragCompleted(AdvancedTabDragCompletedEventArgs e)
        {
            foreach (var rInstance in r_Instances)
                rInstance.IsParticipatingInDocking = false;

            if (r_OperationCompletionInfo == null)
                return;

            r_OperationCompletionInfo.DockableZone.Dock(e.Item, r_OperationCompletionInfo.DockAdorner.Direction);

            r_OperationCompletionInfo = null;
        }
        void Dock(AdvancedTabItem rpItem, DockDirection rpDirection)
        {

        }

        public class DockOperationInfo
        {
            public DockableZone DockableZone { get; }
            public DockAdorner DockAdorner { get; }

            public DockOperationInfo(DockableZone rpDockableZone, DockAdorner rpDockAdorner)
            {
                DockableZone = rpDockableZone;
                DockAdorner = rpDockAdorner;
            }
        }
    }
}
