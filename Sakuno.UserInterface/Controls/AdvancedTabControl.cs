using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabControl : TabControl
    {
        public static readonly DependencyProperty DisableTabReorderProperty = DependencyProperty.Register(nameof(DisableTabReorder), typeof(bool), typeof(AdvancedTabControl), new UIPropertyMetadata(BooleanUtil.False));
        public bool DisableTabReorder
        {
            get { return (bool)GetValue(DisableTabReorderProperty); }
            set { SetValue(DisableTabReorderProperty, value); }
        }

        static readonly DependencyPropertyKey IsTabItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsTabItem", typeof(bool), typeof(AdvancedTabControl), new PropertyMetadata(BooleanUtil.False));
        public static readonly DependencyProperty IsTabItemProperty = IsTabItemPropertyKey.DependencyProperty;
        internal static void SetIsTabItem(DependencyObject rpElement, bool rpValue) => rpElement.SetValue(IsTabItemPropertyKey, BooleanUtil.GetBoxed(rpValue));
        public static bool GetIsTabItem(DependencyObject rpElement) => (bool)rpElement.GetValue(IsTabItemProperty);

        List<object> r_ItemsPendingToAdd;

        AdvancedTabHeaderItemsControl r_HeaderItemsControl;
        Panel r_ItemsHolder;

        static AdvancedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabControl), new FrameworkPropertyMetadata(typeof(AdvancedTabControl)));

            EventManager.RegisterClassHandler(typeof(AdvancedTabControl), AdvancedTabItem.DragStartedEvent, new AdvancedTabDragStartedEventHandler((s, e) => ((AdvancedTabControl)s).OnItemDragStarted(e)), true);
        }

        public override void OnApplyTemplate()
        {
            if (r_HeaderItemsControl != null)
                r_HeaderItemsControl.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;

            base.OnApplyTemplate();

            r_HeaderItemsControl = Template.FindName("PART_HeaderItemsControl", this) as AdvancedTabHeaderItemsControl;
            if (r_HeaderItemsControl != null)
                r_HeaderItemsControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

            r_ItemsHolder = Template.FindName("PART_ItemsHolder", this) as Panel;
            if (r_ItemsHolder != null && r_ItemsPendingToAdd!=null)
            {
                foreach (var rItem in r_ItemsPendingToAdd)
                    CreateItemContentPresenter(rItem);

                r_ItemsPendingToAdd = null;
            }

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

            if (r_ItemsHolder == null)
                r_ItemsPendingToAdd = rpNewValue.OfType<object>().ToList();
            else
                foreach (var rItem in rpNewValue)
                    CreateItemContentPresenter(rItem);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
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

            if (r_ItemsHolder == null)
                return;

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
                            r_ItemsHolder.Children.Remove(rContentPresenter);
                    }

                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Reset:
                    r_ItemsHolder.Children.Clear();

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
            if (r_ItemsHolder != null)
                CreateItemContentPresenter(SelectedItem);
        }
        void CreateItemContentPresenter(object rpItem)
        {
            if (rpItem == null)
                return;

            var rContentPresenter = GetItemContentPresenter(rpItem);
            if (rContentPresenter != null)
                return;

            object rContent;
            var rTabItem = rpItem as TabItem;
            if (rTabItem != null)
                rContent = rTabItem.Content;
            else
                rContent = rpItem;

            rContentPresenter = new ContentPresenter
            {
                Content = rContent,
                ContentTemplate = ContentTemplate,
                ContentTemplateSelector = ContentTemplateSelector,
                ContentStringFormat = ContentStringFormat,
            };
            rContentPresenter.SetBinding(MarginProperty, new Binding() { Path = new PropertyPath(PaddingProperty), Source = this });

            r_ItemsHolder.Children.Add(rContentPresenter);
        }
        ContentPresenter GetItemContentPresenter(object rpItem)
        {
            var rTabItem = rpItem as TabItem;
            if (rTabItem == null)
                return null;

            rpItem = rTabItem.Content;

            if (rpItem == null || r_ItemsHolder == null)
                return null;

            return r_ItemsHolder.Children.OfType<ContentPresenter>().FirstOrDefault(r => r.Content == rpItem);
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
    }
}
