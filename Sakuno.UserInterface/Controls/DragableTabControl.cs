using Sakuno.SystemInterop;
using Sakuno.UserInterface.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Sakuno.UserInterface.Controls
{
    public class DragableTabControl : TabControl
    {
        public static readonly DependencyProperty NewHostProviderProperty = DependencyProperty.Register(nameof(NewHostProvider), typeof(IDragableTabNewHostProvider), typeof(DragableTabControl));
        public IDragableTabNewHostProvider NewHostProvider
        {
            get { return (IDragableTabNewHostProvider)GetValue(NewHostProviderProperty); }
            set { SetValue(NewHostProviderProperty, value); }
        }

        public static readonly DependencyProperty IsMainHostProperty = DependencyProperty.Register(nameof(IsMainHost), typeof(bool), typeof(DragableTabControl), new UIPropertyMetadata(BooleanUtil.False));
        public bool IsMainHost
        {
            get { return (bool)GetValue(IsMainHostProperty); }
            set { SetValue(IsMainHostProperty, value); }
        }

        static HashSet<DragableTabControl> r_Instances = new HashSet<DragableTabControl>();

        DragableTabPanel r_TabPanel;

        static DragableTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragableTabControl), new FrameworkPropertyMetadata(typeof(DragableTabControl)));
        }
        public DragableTabControl()
        {
            AddHandler(DragableTabItem.PreviewDragDeltaEvent, new DragableTabDragDeltaEventHandler(ItemPreviewDragDelta));
            AddHandler(DragableTabItem.DragDeltaEvent, new DragableTabDragDeltaEventHandler(ItemDragDelta));

            Loaded += delegate { r_Instances.Add(this); };
            Unloaded += delegate { r_Instances.Remove(this); };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_TabPanel = Template.FindName("PART_TabPanel", this) as DragableTabPanel;
        }

        protected override DependencyObject GetContainerForItemOverride() => new DragableTabItem();
        protected override bool IsItemItsOwnContainerOverride(object rpItem) => rpItem is DragableTabItem;
        void ItemPreviewDragDelta(object sender, DragableTabDragDeltaEventArgs e)
        {
            if (Items.Count != 1)
                return;

            if (!IsMainHost && TryMerge(e))
                return;

            var rWindow = Window.GetWindow(this);

            NativeStructs.POINT rCursorPosition;
            NativeMethods.User32.GetCursorPos(out rCursorPosition);

            var rOffset = e.Item.TranslatePoint(default(Point), rWindow);
            rWindow.Left = rCursorPosition.X - rOffset.X;
            rWindow.Top = rCursorPosition.Y - rOffset.Y;

            e.Handled = true;
        }

        void ItemDragDelta(object sender, DragableTabDragDeltaEventArgs e)
        {
            var rPoint = Mouse.GetPosition(this);

            if (rPoint.X < -8 || rPoint.X > ActualWidth + 8 || rPoint.Y < -8 || rPoint.Y > r_TabPanel.ActualHeight + 8)
            {
                var rNewHost = NewHostProvider?.CreateNewHost(this);
                if (rNewHost != null)
                {
                    var rWindow = Window.GetWindow(this);
                    rNewHost.Window.Width = ActualWidth;
                    rNewHost.Window.Height = ActualHeight;

                    rNewHost.Window.Show();

                    var rItem = ItemContainerGenerator.ItemFromContainer(e.Item);
                    RemoveItem(rItem);

                    rNewHost.TabControl.ReceiveDrag(rItem);

                    if (Items.Count == 0 && rWindow != Application.Current.MainWindow)
                        rWindow.Close();

                    e.Cancel = true;
                }
            }

            e.Handled = true;
        }

        void ReceiveDrag(object rpItem)
        {
            var rWindow = Window.GetWindow(this);

            rWindow.Activate();

            AddItem(rpItem);
            SelectedItem = rpItem;

            var rItem = ItemContainerGenerator.ContainerFromItem(rpItem) as DragableTabItem;

            if (Items.Count > 1)
            {
                NativeStructs.POINT rCursorPosition;
                NativeMethods.User32.GetCursorPos(out rCursorPosition);

                var rPosition = rCursorPosition.X - r_TabPanel.PointToScreen(default(Point)).X;
                rItem.PositionAfterMerged = rPosition;
            }

            rItem?.ReceiveDrag();
        }

        bool TryMerge(DragableTabDragDeltaEventArgs e)
        {
            var rInfos = r_Instances.Where(r => r != this).Select(r =>
            {
                var rTabPanel = r.r_TabPanel;
                var rPointTopLeft = rTabPanel.PointToScreen(default(Point));
                var rPointBottomRight = rTabPanel.PointToScreen(new Point(rTabPanel.ActualWidth, rTabPanel.ActualHeight));

                return new { TabControl = r, Rect = new Rect(rPointTopLeft, rPointBottomRight) };
            });

            NativeStructs.POINT rCursorPosition;
            NativeMethods.User32.GetCursorPos(out rCursorPosition);

            rInfos = from rWindow in GetWindowsOrderedByZOrder(Application.Current.Windows.OfType<Window>())
                     join rInfo in rInfos on rWindow equals Window.GetWindow(rInfo.TabControl)
                     select rInfo;
            var rTarget = rInfos.FirstOrDefault(r => r.Rect.Contains(new Point(rCursorPosition.X, rCursorPosition.Y)));
            if (rTarget != null)
            {
                var rWindow = Window.GetWindow(this);

                var rItem = ItemContainerGenerator.ItemFromContainer(e.Item);
                RemoveItem(rItem);

                rTarget.TabControl.ReceiveDrag(rItem);

                if (Items.Count == 0 && rWindow != Application.Current.MainWindow)
                    rWindow.Close();

                e.Cancel = true;
                return true;
            }

            return false;
        }

        IEnumerable<Window> GetWindowsOrderedByZOrder(IEnumerable<Window> rpWindows)
        {
            var rMap = rpWindows.Select(r => new { Window = r, Handle = new WindowInteropHelper(r).Handle }).ToDictionary(r => r.Handle, r => r.Window);

            var rResult = new List<Window>();

            var rHandle = NativeMethods.User32.GetTopWindow(IntPtr.Zero);
            while (rHandle != IntPtr.Zero)
            {
                Window rWindow;
                if (rMap.TryGetValue(rHandle, out rWindow))
                    rResult.Add(rWindow);
                rHandle = NativeMethods.User32.GetWindow(rHandle, NativeConstants.GetWindow.GW_HWNDNEXT);
            }

            return rResult;
        }

        void AddItem(object rpItem)
        {
            CollectionTeaser rCollectionTeaser;
            if (CollectionTeaser.TryCreate(ItemsSource, out rCollectionTeaser))
            {
                rCollectionTeaser.Add(rpItem);
                return;
            }

            Items.Add(rpItem);
        }
        void RemoveItem(object rpItem)
        {
            CollectionTeaser rCollectionTeaser;
            if (CollectionTeaser.TryCreate(ItemsSource, out rCollectionTeaser))
            {
                rCollectionTeaser.Remove(rpItem);
                return;
            }

            Items.Remove(rpItem);
        }
    }
}
