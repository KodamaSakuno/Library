using Sakuno.SystemInterop;
using Sakuno.UserInterface.Controls.Docking;
using Sakuno.UserInterface.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_HeaderItemsControl", Type = typeof(AdvancedTabHeaderItemsControl))]
    [TemplatePart(Name = "PART_ContentItemsControl", Type = typeof(AdvancedTabContentItemsControl))]
    public class AdvancedTabControl : TabControl
    {
        public static readonly DependencyProperty DisableTabReorderProperty = DependencyProperty.Register(nameof(DisableTabReorder), typeof(bool), typeof(AdvancedTabControl), new UIPropertyMetadata(BooleanUtil.False));
        public bool DisableTabReorder
        {
            get { return (bool)GetValue(DisableTabReorderProperty); }
            set { SetValue(DisableTabReorderProperty, value); }
        }

        public static readonly DependencyProperty TabControllerProperty = DependencyProperty.Register(nameof(TabController), typeof(TabController), typeof(AdvancedTabControl), new UIPropertyMetadata(null));
        public TabController TabController
        {
            get { return (TabController)GetValue(TabControllerProperty); }
            set { SetValue(TabControllerProperty, value); }
        }

        static readonly DependencyPropertyKey IsTabItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsTabItem", typeof(bool), typeof(AdvancedTabControl), new PropertyMetadata(BooleanUtil.False));
        public static readonly DependencyProperty IsTabItemProperty = IsTabItemPropertyKey.DependencyProperty;
        internal static void SetIsTabItem(DependencyObject rpElement, bool rpValue) => rpElement.SetValue(IsTabItemPropertyKey, BooleanUtil.GetBoxed(rpValue));
        public static bool GetIsTabItem(DependencyObject rpElement) => (bool)rpElement.GetValue(IsTabItemProperty);

        static HashSet<AdvancedTabControl> r_Instances = new HashSet<AdvancedTabControl>();

        AdvancedTabHeaderItemsControl r_HeaderItemsControl;

        AdvancedTabContentItemsControl r_ContentItemsControl;
        ObservableCollection<ContentPresenter> r_Contents;
        public ReadOnlyObservableCollection<ContentPresenter> Contents { get; }

        WeakReference<object> r_PreviousSelection;

        static AdvancedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabControl), new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));

            EventManager.RegisterClassHandler(typeof(AdvancedTabControl), AdvancedTabItem.DragStartedEvent, new AdvancedTabDragStartedEventHandler((s, e) => ((AdvancedTabControl)s).OnItemDragStarted(e)), true);
            EventManager.RegisterClassHandler(typeof(AdvancedTabControl), AdvancedTabItem.PreviewDragDeltaEvent, new AdvancedTabDragDeltaEventHandler((s, e) => ((AdvancedTabControl)s).OnItemPreviewDragDelta(e)), true);
            EventManager.RegisterClassHandler(typeof(AdvancedTabControl), AdvancedTabItem.DragDeltaEvent, new AdvancedTabDragDeltaEventHandler((s, e) => ((AdvancedTabControl)s).OnItemDragDelta(e)), true);
            EventManager.RegisterClassHandler(typeof(AdvancedTabControl), AdvancedTabItem.DragCompletedEvent, new AdvancedTabDragCompletedEventHandler((s, e) => ((AdvancedTabControl)s).OnItemDragCompleted(e)), true);
        }
        public AdvancedTabControl()
        {
            r_Contents = new ObservableCollection<ContentPresenter>();
            Contents = new ReadOnlyObservableCollection<ContentPresenter>(r_Contents);

            Loaded += (s, e) => r_Instances.Add(this);
            Unloaded += (s, e) => r_Instances.Remove(this);
        }

        public override void OnApplyTemplate()
        {
            if (r_HeaderItemsControl != null)
            {
                r_HeaderItemsControl.Owner = null;
                r_HeaderItemsControl.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
            }

            base.OnApplyTemplate();

            r_HeaderItemsControl = Template.FindName("PART_HeaderItemsControl", this) as AdvancedTabHeaderItemsControl;
            if (r_HeaderItemsControl != null)
            {
                r_HeaderItemsControl.Owner = this;
                r_HeaderItemsControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            }

            r_ContentItemsControl = Template.FindName("PART_ContentItemsControl", this) as AdvancedTabContentItemsControl;
            if (r_ContentItemsControl != null)
                r_ContentItemsControl.Owner = this;

            if (SelectedItem == null)
                SetCurrentValue(SelectedItemProperty, Items.OfType<object>().FirstOrDefault());

            UpdateSelectedItem();
            SetIsTabItemFlag();
            SetInitialSelection();
        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (r_HeaderItemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                foreach (var rItem in r_HeaderItemsControl.Items)
                    CreateItemContentPresenter(rItem);

            SetIsTabItemFlag();
            SetInitialSelection();
        }
        void SetIsTabItemFlag()
        {
            foreach (var rItem in r_HeaderItemsControl.Items.OfType<TabItem>().Select(r => r_HeaderItemsControl.ItemContainerGenerator.ContainerFromItem(r)).OfType<AdvancedTabItem>())
                SetIsTabItem(rItem, true);
        }
        void SetInitialSelection()
        {
            if (r_HeaderItemsControl == null || r_HeaderItemsControl.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return;

            var rSelectedItem = SelectedItem;
            if (rSelectedItem != null)
            {
                var rTabItem = rSelectedItem as TabItem;
                rTabItem?.SetCurrentValue(TabItem.IsSelectedProperty, BooleanUtil.True);

                var rItem = r_HeaderItemsControl.ItemContainerGenerator.ContainerFromItem(rSelectedItem) as AdvancedTabItem;
                rItem?.SetCurrentValue(AdvancedTabItem.IsSelectedProperty, BooleanUtil.True);
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable rpOldValue, IEnumerable rpNewValue)
        {
            base.OnItemsSourceChanged(rpOldValue, rpNewValue);

            if (rpNewValue == null)
                return;

            foreach (var rItem in rpNewValue)
                CreateItemContentPresenter(rItem);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && e.AddedItems.Count > 0)
                r_PreviousSelection = new WeakReference<object>(e.RemovedItems[0]);

            base.OnSelectionChanged(e);

            UpdateSelectedItem();

            if (r_HeaderItemsControl == null)
                return;

            foreach (var rItem in e.AddedItems.OfType<object>().Select(r => r_HeaderItemsControl.ItemContainerGenerator.ContainerFromItem(r)).OfType<AdvancedTabItem>())
                rItem.IsSelected = true;
            foreach (var rItem in e.RemovedItems.OfType<object>().Select(r => r_HeaderItemsControl.ItemContainerGenerator.ContainerFromItem(r)).OfType<AdvancedTabItem>())
                rItem.IsSelected = false;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var rRemovedItem in e.OldItems)
                    {
                        var rContentPresenter = GetItemContentPresenter(rRemovedItem);
                        if (rContentPresenter != null)
                            r_Contents.Remove(rContentPresenter);
                    }

                    if (SelectedItem == null)
                    {
                        object rPreviousSelection;
                        if (r_PreviousSelection != null && r_PreviousSelection.TryGetTarget(out rPreviousSelection))
                            SelectedItem = rPreviousSelection;
                        else
                            SelectedItem = Items.OfType<object>().FirstOrDefault();
                    }

                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Reset:
                    r_Contents.Clear();

                    if (Items.Count > 0)
                    {
                        SelectedItem = Items[0];
                        UpdateSelectedItem();
                    }
                    break;
            }
        }

        void UpdateSelectedItem()
        {
            CreateItemContentPresenter(SelectedItem);

            if (r_ContentItemsControl != null)
                r_ContentItemsControl.SelectedIndex = SelectedIndex;
        }
        void CreateItemContentPresenter(object rpItem)
        {
            if (rpItem == null)
                return;

            var rContentPresenter = GetItemContentPresenter(rpItem);
            if (rContentPresenter != null)
                return;

            var rTabItem = rpItem as TabItem;
            var rContent = rTabItem?.Content ?? rpItem;

            rContentPresenter = new ContentPresenter
            {
                Content = rContent,
                ContentTemplate = ContentTemplate,
                ContentTemplateSelector = ContentTemplateSelector,
                ContentStringFormat = ContentStringFormat,
            };
            rContentPresenter.SetBinding(MarginProperty, new Binding() { Path = new PropertyPath(PaddingProperty), Source = this });

            r_Contents.Add(rContentPresenter);
        }
        ContentPresenter GetItemContentPresenter(object rpItem)
        {
            var rTabItem = rpItem as TabItem;
            rpItem = rTabItem?.Content ?? rpItem;

            if (rpItem == null)
                return null;

            return r_Contents.FirstOrDefault(r => r.Content == rpItem);
        }

        internal void AddItem(object rpItem)
        {
            CollectionTeaser rCollectionTeaser;
            if (CollectionTeaser.TryCreate(ItemsSource, rpItem.GetType(), out rCollectionTeaser))
            {
                rCollectionTeaser.Add(rpItem);
                return;
            }

            if (ItemsSource == null)
                Items.Add(rpItem);
        }
        void RemoveItem(object rpItem)
        {
            CollectionTeaser rCollectionTeaser;
            if (CollectionTeaser.TryCreate(ItemsSource, rpItem.GetType(), out rCollectionTeaser))
            {
                rCollectionTeaser.Remove(rpItem);
                return;
            }

            if (ItemsSource == null)
                Items.Remove(rpItem);
        }

        internal object RemoveItem(AdvancedTabItem rpTabItem)
        {
            var rItem = r_HeaderItemsControl.ItemContainerGenerator.ItemFromContainer(rpTabItem);
            RemoveItem(rItem);

            var rContentPresenter = GetItemContentPresenter(rItem);
            if (rContentPresenter != null)
                r_Contents.Remove(rContentPresenter);

            if (Items.Count > 0)
                return rItem;

            var rWindow = Window.GetWindow(this);
            if (Items.Count == 0 && TabController?.TearOffController.OnTabEmptied(this, rWindow) == TabEmptiedAction.CloseWindow)
            {
                if (DockableZone.MergeDockGroup(this))
                    return rItem;

                rWindow.Close();
            }

            return rItem;
        }

        void OnItemDragStarted(AdvancedTabDragStartedEventArgs e)
        {
            if (r_HeaderItemsControl == null)
                return;

            var rHeaderItems = new HashSet<AdvancedTabItem>(r_HeaderItemsControl.GetItems());
            if (!rHeaderItems.Contains(e.Item))
                return;

            foreach (var rHeaderItem in rHeaderItems)
                rHeaderItem.IsSelected = false;
            e.Item.IsSelected = true;

            var rItem = r_HeaderItemsControl.ItemContainerGenerator.ItemFromContainer(e.Item);
            var rTabItem = rItem as TabItem;
            if (rTabItem != null)
                rTabItem.IsSelected = true;
            SelectedItem = rItem;
        }

        void OnItemPreviewDragDelta(AdvancedTabDragDeltaEventArgs e)
        {
            if (r_HeaderItemsControl.Items.Count > 1 || r_HeaderItemsControl.IsAncestorContained<DockGroup>())
                return;

            if (TryMerge(e))
                return;

            var rWindow = Window.GetWindow(this);
            if (rWindow == null)
                return;

            NativeStructs.POINT rMousePosition;
            NativeMethods.User32.GetCursorPos(out rMousePosition);

            rWindow.Left += e.DragEventArgs.HorizontalChange;
            rWindow.Top += e.DragEventArgs.VerticalChange;

            e.Handled = true;
        }
        void OnItemDragDelta(AdvancedTabDragDeltaEventArgs e)
        {
            if (TabController == null)
                return;

            if (TabController.TearOffController == null)
                throw new InvalidOperationException("A TearOffController must be provided.");

            var rOwnerWindow = Window.GetWindow(this);
            if (rOwnerWindow == null)
                throw new InvalidOperationException();

            var rPoint = Mouse.GetPosition(this);

            if (rPoint.X < -8 || rPoint.X > r_HeaderItemsControl.ActualWidth + 8 || rPoint.Y < -8 || rPoint.Y > r_HeaderItemsControl.ActualHeight + 8)
            {
                var rHost = TabController.TearOffController.CreateHost(this, TabController.Partition);
                if (rHost != null)
                {
                    var rTabControl = rHost.Item1;
                    var rWindow = rHost.Item2;

                    rPoint = PointToScreen(Mouse.GetPosition(rOwnerWindow));
                    rWindow.Left = rPoint.X;
                    rWindow.Top = rPoint.Y;

                    rWindow.Show();

                    var rItem = r_HeaderItemsControl.ItemContainerGenerator.ItemFromContainer(e.Item);
                    RemoveItem(rItem);

                    var rContentPresenter = GetItemContentPresenter(rItem);
                    if (rContentPresenter != null)
                        r_Contents.Remove(rContentPresenter);

                    if (Items.Count == 0)
                        DockableZone.MergeDockGroup(this);

                    rTabControl.ReceiveDrag(rItem);

                    e.Cancel = true;
                }
            }

            e.Handled = true;
        }
        void ReceiveDrag(object rpItem)
        {
            var rWindow = Window.GetWindow(this);
            if (rWindow == null)
                throw new InvalidOperationException();

            rWindow.Activate();

            AddItem(rpItem);
            SelectedItem = rpItem;

            ((AdvancedTabItem)r_HeaderItemsControl.ItemContainerGenerator.ContainerFromItem(rpItem)).ReceiveDrag();
        }

        void OnItemDragCompleted(AdvancedTabDragCompletedEventArgs e)
        {
        }

        bool TryMerge(AdvancedTabDragDeltaEventArgs e)
        {
            var rInfos = r_Instances.Where(r => r != this).Select(r =>
            {
                var rHeaderItemsControl = r.r_HeaderItemsControl;
                var rPointTopLeft = rHeaderItemsControl.PointToScreen(default(Point));
                var rPointBottomRight = rHeaderItemsControl.PointToScreen(new Point(rHeaderItemsControl.ActualWidth, rHeaderItemsControl.ActualHeight));

                return new { TabControl = r, Rect = new Rect(rPointTopLeft, rPointBottomRight) };
            });

            rInfos = from rWindow in WindowUtil.GetWindowsOrderedByZOrder(Application.Current.Windows.OfType<Window>())
                     join rInfo in rInfos on rWindow equals Window.GetWindow(rInfo.TabControl)
                     select rInfo;

            NativeStructs.POINT rMousePosition;
            NativeMethods.User32.GetCursorPos(out rMousePosition);

            var rTarget = rInfos.FirstOrDefault(r => r.Rect.Contains(new Point(rMousePosition.X, rMousePosition.Y)));
            if (rTarget != null)
            {
                var rItem = r_HeaderItemsControl.ItemContainerGenerator.ItemFromContainer(e.Item);
                if (rTarget.TabControl.ItemsSource != null)
                {
                    var rList = rTarget.TabControl.ItemsSource as IList;
                    if (rList != null)
                    {
                        var rElementType = rList.GetType().GetGenericArguments()?[0];
                        if (rElementType == null || !rItem.GetType().IsSubclassOf(rElementType))
                            return false;
                    }
                }

                RemoveItem(e.Item);

                rTarget.TabControl.ReceiveDrag(rItem);

                e.Cancel = true;
                return true;
            }

            return false;
        }
    }
}
