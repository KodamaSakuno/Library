using Sakuno.SystemInterop;
using Sakuno.UserInterface.Controls.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sakuno.UserInterface.Controls
{
    public class DockableZone : ContentControl
    {
        public static readonly DependencyProperty DockingControllerProperty = DependencyProperty.Register(nameof(DockingController), typeof(ITabDockingController), typeof(DockableZone), new UIPropertyMetadata(null));
        public ITabDockingController DockingController
        {
            get { return (ITabDockingController)GetValue(DockingControllerProperty); }
            set { SetValue(DockingControllerProperty, value); }
        }

        static readonly DependencyPropertyKey IsParticipatingInDockingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsParticipatingInDocking), typeof(bool), typeof(DockableZone), new UIPropertyMetadata(BooleanUtil.False));
        public static readonly DependencyProperty IsParticipatingInDockingProperty = IsParticipatingInDockingPropertyKey.DependencyProperty;
        public bool IsParticipatingInDocking
        {
            get { return (bool)GetValue(IsParticipatingInDockingProperty); }
            private set { SetValue(IsParticipatingInDockingPropertyKey, BooleanUtil.GetBoxed(value)); }
        }

        static HashSet<DockableZone> r_Instances = new HashSet<DockableZone>();
        static Dictionary<Window, List<DockableZone>> r_InstancesGroupByWindow = new Dictionary<Window, List<DockableZone>>();

        static DockAdornerWindow r_DockAdornerWindow;

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
                if (!r_Instances.Add(this))
                    return;

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

        void OnItemPreviewDragDelta(AdvancedTabDragDeltaEventArgs e)
        {
            if (DockingController == null || e.Cancel)
                return;

            DockableZone rParticipatingDockableZone = null;
            var rMousePosition = default(Point);

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

                if (r_DockAdornerWindow == null)
                    r_DockAdornerWindow = new DockAdornerWindow();

                NativeStructs.POINT rCursorPosition;
                NativeMethods.User32.GetCursorPos(out rCursorPosition);
                rMousePosition = new Point(rCursorPosition.X, rCursorPosition.Y);

                foreach (var rTarget in rInfos)
                {
                    rTarget.DockableZone.IsParticipatingInDocking = rTarget.Rect.Contains(rMousePosition);
                    if (rTarget.DockableZone.IsParticipatingInDocking)
                    {
                        rParticipatingDockableZone = rTarget.DockableZone;
                        r_DockAdornerWindow.Show(rParticipatingDockableZone);
                    }
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

            var rHitAdorner = r_DockAdornerWindow.DockAdorners.Where(r => r != null).Select(r =>
            {
                var rPoint = rWindow.PointFromScreen(rpMousePosition);
                rPoint = rWindow.TranslatePoint(rPoint, r);

                return new { DockAdorner = r, Result = r.InputHitTest(rPoint) != null };
            }).FirstOrDefault(r => r.Result);

            foreach (var rDockAdorner in r_DockAdornerWindow.DockAdorners.Where(r => r != null))
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

            if (r_DockAdornerWindow != null)
            {
                r_DockAdornerWindow.Hide();
                r_DockAdornerWindow = null;
            }

            if (r_OperationCompletionInfo == null)
                return;

            r_OperationCompletionInfo.DockableZone.Dock(e.Item, r_OperationCompletionInfo.DockAdorner.Direction);

            r_OperationCompletionInfo = null;
        }
        void Dock(AdvancedTabItem rpItem, DockDirection rpDirection)
        {
            var rHeaderItemsControl = ItemsControl.ItemsControlFromItemContainer(rpItem) as AdvancedTabHeaderItemsControl;
            if (rHeaderItemsControl == null)
                throw new InvalidOperationException();

            var rTabControl = rHeaderItemsControl.Owner;
            if (rTabControl == null)
                throw new InvalidOperationException();

            var rItem = rHeaderItemsControl.ItemContainerGenerator.ItemFromContainer(rpItem);
            rTabControl.RemoveItem(rpItem);

            var rDockGroup = new DockGroup() { Orientation = rpDirection == DockDirection.Left || rpDirection == DockDirection.Right ? Orientation.Horizontal : Orientation.Vertical };

            var rHost = DockingController.CreateHost(rTabControl, null);
            if (rHost == null)
                throw new InvalidOperationException();

            rHost.AddItem(rItem);
            rHost.SelectedItem = rItem;

            var rContent = rHost;

            if (rpDirection == DockDirection.Right || rpDirection == DockDirection.Bottom)
            {
                rDockGroup.FirstItem = Content;
                rDockGroup.SecondItem = rContent;
            }
            else
            {
                rDockGroup.FirstItem = rContent;
                rDockGroup.SecondItem = Content;
            }

            SetCurrentValue(ContentProperty, rDockGroup);
        }

        internal static bool MergeDockGroup(DependencyObject rpObject)
        {
            bool rIsSecondItemContentPresenter;
            var rDockGroup = FindAncestor(rpObject, out rIsSecondItemContentPresenter) as DockGroup;
            if (rDockGroup == null)
                return false;

            var rItem = !rIsSecondItemContentPresenter ? rDockGroup.FirstItem : rDockGroup.SecondItem;

            var rAncestor = FindAncestor(rDockGroup, out rIsSecondItemContentPresenter);

            var rDockableZone = rAncestor as DockableZone;
            if (rDockableZone != null)
            {
                rDockableZone.SetCurrentValue(ContentProperty, rItem);
                return true;
            }

            rDockGroup = rAncestor as DockGroup;
            if (rDockGroup != null)
            {
                rDockGroup.SetCurrentValue(rIsSecondItemContentPresenter ? DockGroup.FirstItemProperty : DockGroup.SecondItemProperty, rItem);
                return true;
            }

            throw new InvalidOperationException();
        }
        static object FindAncestor(DependencyObject rpObject, out bool ropIsSecondItemContentPresenter)
        {
            ropIsSecondItemContentPresenter = false;

            var rElements = new List<DependencyObject>();
            do
            {
                rElements.Add(rpObject);

                rpObject = VisualTreeHelper.GetParent(rpObject);

                if (rpObject is DockableZone)
                    return rpObject;

                var rDockGroup = rpObject as DockGroup;
                if (rDockGroup == null)
                    continue;

                ropIsSecondItemContentPresenter = rElements.Contains(rDockGroup.FirstItemContentPresenter);

                return rDockGroup;

            } while (rpObject != null);

            return null;
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
