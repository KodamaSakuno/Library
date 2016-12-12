using Sakuno.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class RelativePanel : Panel
    {
        public static readonly DependencyProperty AlignLeftWithPanelProperty = DependencyProperty.RegisterAttached("AlignLeftWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static bool GetAlignLeftWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignLeftWithPanelProperty);
        public static void SetAlignLeftWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignLeftWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignTopWithPanelProperty = DependencyProperty.RegisterAttached("AlignTopWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static bool GetAlignTopWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignTopWithPanelProperty);
        public static void SetAlignTopWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignTopWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignRightWithPanelProperty = DependencyProperty.RegisterAttached("AlignRightWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static bool GetAlignRightWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignRightWithPanelProperty);
        public static void SetAlignRightWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignRightWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignBottomWithPanelProperty = DependencyProperty.RegisterAttached("AlignBottomWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static bool GetAlignBottomWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignBottomWithPanelProperty);
        public static void SetAlignBottomWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignBottomWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignLeftWithProperty = DependencyProperty.RegisterAttached("AlignLeftWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static object GetAlignLeftWith(DependencyObject rpObject) => rpObject.GetValue(AlignLeftWithProperty);
        public static void SetAlignLeftWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignLeftWithProperty, rpValue);

        public static readonly DependencyProperty AlignTopWithProperty = DependencyProperty.RegisterAttached("AlignTopWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static object GetAlignTopWith(DependencyObject rpObject) => rpObject.GetValue(AlignTopWithProperty);
        public static void SetAlignTopWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignTopWithProperty, rpValue);

        public static readonly DependencyProperty AlignRightWithProperty = DependencyProperty.RegisterAttached("AlignRightWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static object GetAlignRightWith(DependencyObject rpObject) => rpObject.GetValue(AlignRightWithProperty);
        public static void SetAlignRightWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignRightWithProperty, rpValue);

        public static readonly DependencyProperty AlignBottomWithProperty = DependencyProperty.RegisterAttached("AlignBottomWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static object GetAlignBottomWith(DependencyObject rpObject) => rpObject.GetValue(AlignBottomWithProperty);
        public static void SetAlignBottomWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignBottomWithProperty, rpValue);

        public static readonly DependencyProperty LeftOfProperty = DependencyProperty.RegisterAttached("LeftOf", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static object GetLeftOf(DependencyObject rpObject) => rpObject.GetValue(LeftOfProperty);
        public static void SetLeftOf(DependencyObject rpObject, object rpValue) => rpObject.SetValue(LeftOfProperty, rpValue);

        public static readonly DependencyProperty AboveProperty = DependencyProperty.RegisterAttached("Above", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static object GetAbove(DependencyObject rpObject) => rpObject.GetValue(AboveProperty);
        public static void SetAbove(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AboveProperty, rpValue);

        public static readonly DependencyProperty RightOfProperty = DependencyProperty.RegisterAttached("RightOf", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static object GetRightOf(DependencyObject rpObject) => rpObject.GetValue(RightOfProperty);
        public static void SetRightOf(DependencyObject rpObject, object rpValue) => rpObject.SetValue(RightOfProperty, rpValue);

        public static readonly DependencyProperty BelowProperty = DependencyProperty.RegisterAttached("Below", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static object GetBelow(DependencyObject rpObject) => rpObject.GetValue(BelowProperty);
        public static void SetBelow(DependencyObject rpObject, object rpValue) => rpObject.SetValue(BelowProperty, rpValue);

        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty = DependencyProperty.RegisterAttached("AlignHorizontalCenterWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignHorizontalCenterWithPanelProperty);
        public static void SetAlignHorizontalCenterWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignHorizontalCenterWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignHorizontalCenterWithProperty = DependencyProperty.RegisterAttached("AlignHorizontalCenterWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnHorizontalDirectionPropertyChanged));
        public static object GetAlignHorizontalCenterWith(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignHorizontalCenterWithProperty);
        public static void SetAlignHorizontalCenterWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignHorizontalCenterWithProperty, rpValue);

        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty = DependencyProperty.RegisterAttached("AlignVerticalCenterWithPanel", typeof(bool), typeof(RelativePanel),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static bool GetAlignVerticalCenterWithPanel(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignVerticalCenterWithPanelProperty);
        public static void SetAlignVerticalCenterWithPanel(DependencyObject rpObject, bool rpValue) => rpObject.SetValue(AlignVerticalCenterWithPanelProperty, rpValue);

        public static readonly DependencyProperty AlignVerticalCenterWithProperty = DependencyProperty.RegisterAttached("AlignVerticalCenterWith", typeof(object), typeof(RelativePanel),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange, OnVerticalDirectionPropertyChanged));
        public static object GetAlignVerticalCenterWith(DependencyObject rpObject) => (bool)rpObject.GetValue(AlignVerticalCenterWithProperty);
        public static void SetAlignVerticalCenterWith(DependencyObject rpObject, object rpValue) => rpObject.SetValue(AlignVerticalCenterWithProperty, rpValue);

        static DependencyProperty[] r_HorizontalEdgeAProperties, r_HorizontalEdgeBProperties, r_HorizontalCenterAlignmentProperties;

        static DependencyProperty[] r_VerticalEdgeAProperties, r_VerticalEdgeBProperties, r_VerticalCenterAlignmentProperties;

        static readonly DependencyProperty LayoutInfoProperty = DependencyProperty.RegisterAttached("LayoutInfo", typeof(LayoutInfo), typeof(RelativePanel),
            new PropertyMetadata(null));
        static LayoutInfo GetLayoutInfo(DependencyObject rpObject) => (LayoutInfo)rpObject.GetValue(LayoutInfoProperty);
        static void SetLayoutInfo(DependencyObject rpObject, LayoutInfo rpValue) => rpObject.SetValue(LayoutInfoProperty, rpValue);

        List<LayoutInfo> r_LayoutInfos = new List<LayoutInfo>();

        bool r_IsInfosDirty = true;

        LayoutInfo[] r_HorizontalDirection;
        bool r_IsHorizontalDirectionDirty = true;

        LayoutInfo[] r_VerticalDirection;
        bool r_IsVerticalDirectionDirty = true;

        ListDictionary<string, LayoutInfo> r_NamedLayoutInfos = new ListDictionary<string, LayoutInfo>();

        static RelativePanel()
        {
            r_HorizontalEdgeAProperties = new[]
            {
                AlignLeftWithPanelProperty,
                AlignLeftWithProperty,
                RightOfProperty,
            };
            r_HorizontalEdgeBProperties = new[]
            {
                AlignRightWithPanelProperty,
                AlignRightWithProperty,
                LeftOfProperty,
            };
            r_HorizontalCenterAlignmentProperties = new[]
            {
                AlignHorizontalCenterWithPanelProperty,
                AlignHorizontalCenterWithProperty,
            };

            r_VerticalEdgeAProperties = new[]
            {
                AlignTopWithPanelProperty,
                AlignTopWithProperty,
                BelowProperty,
            };
            r_VerticalEdgeBProperties = new[]
            {
                AlignBottomWithPanelProperty,
                AlignBottomWithProperty,
                AboveProperty,
            };
            r_VerticalCenterAlignmentProperties = new[]
            {
                AlignVerticalCenterWithPanelProperty,
                AlignVerticalCenterWithProperty,
            };
        }

        protected override Size MeasureOverride(Size rpAvailableSize)
        {
            if (r_IsInfosDirty)
            {
                r_NamedLayoutInfos.Clear();

                foreach (var rInfo in r_LayoutInfos)
                    if (rInfo.Name != null)
                        r_NamedLayoutInfos.Add(rInfo.Name, rInfo);

                r_IsInfosDirty = false;
            }

            if (r_IsHorizontalDirectionDirty)
                SortHorizontalDirection();

            if (r_IsVerticalDirectionDirty)
                SortVerticalDirection();

            var rWidth = rpAvailableSize.Width;
            var rHeight = rpAvailableSize.Height;

            foreach (var rInfo in r_HorizontalDirection)
            {
                var rElement = rInfo.Element;

                var rLeftProperty = rInfo.LeftProperty;
                if (rLeftProperty == null || rInfo.IsHorizontalDirectionCenterAlignment)
                    rInfo.Left = double.NaN;
                else if (rLeftProperty == AlignLeftWithPanelProperty)
                    rInfo.Left = .0;
                else if (rLeftProperty == AlignLeftWithProperty)
                    rInfo.Left = rInfo.LeftDependency.Left;
                else if (rLeftProperty == RightOfProperty)
                    rInfo.Left = rWidth - rInfo.LeftDependency.Right;

                var rRightProperty = rInfo.RightProperty;
                if (rRightProperty == null || rInfo.IsHorizontalDirectionCenterAlignment)
                    rInfo.Right = double.NaN;
                else if (rRightProperty == AlignRightWithPanelProperty)
                    rInfo.Right = .0;
                else if (rRightProperty == AlignRightWithProperty)
                    rInfo.Right = rInfo.RightDependency.Right;
                else if (rRightProperty == LeftOfProperty)
                    rInfo.Right = rWidth - rInfo.RightDependency.Left;

                if (!rInfo.Left.IsNaN() && !rInfo.Right.IsNaN())
                    continue;

                var rAvailableWidth = rWidth - rInfo.Right - rInfo.Left;
                if (rAvailableWidth.IsNaN())
                {
                    rAvailableWidth = rWidth;

                    if (!rInfo.Left.IsNaN() && rInfo.Right.IsNaN())
                        rAvailableWidth -= rInfo.Left;
                    else if (rInfo.Left.IsNaN() && !rInfo.Right.IsNaN())
                        rAvailableWidth -= rInfo.Right;
                }

                rElement.Measure(new Size(rAvailableWidth, rHeight));

                if (!rInfo.IsHorizontalDirectionCenterAlignment)
                {
                    if (rInfo.Left.IsNaN() && !rInfo.Right.IsNaN())
                        rInfo.Left = rWidth - rInfo.Right - rElement.DesiredSize.Width;
                    else if (rInfo.Right.IsNaN())
                    {
                        if (rInfo.Left.IsNaN())
                            rInfo.Left = .0;

                        rInfo.Right = rWidth - rInfo.Left - rElement.DesiredSize.Width;
                    }

                    continue;
                }

                if (rLeftProperty == AlignHorizontalCenterWithPanelProperty)
                    rInfo.Left = rInfo.Right = (rWidth - rElement.DesiredSize.Width) / 2.0;
                else
                {
                    var rDependencyWidth = rWidth - rInfo.LeftDependency.Right - rInfo.LeftDependency.Left;
                    var rHalfWidthDifference = (rElement.DesiredSize.Width - rDependencyWidth) / 2.0;

                    rInfo.Left = rInfo.LeftDependency.Left - rHalfWidthDifference;
                    rInfo.Right = rInfo.LeftDependency.Right - rHalfWidthDifference;
                }
            }

            foreach (var rInfo in r_VerticalDirection)
            {
                var rElement = rInfo.Element;

                var rTopProperty = rInfo.TopProperty;
                if (rTopProperty == null || rInfo.IsVerticalDirectionCenterAlignment)
                    rInfo.Top = double.NaN;
                else if (rTopProperty == AlignTopWithPanelProperty)
                    rInfo.Top = .0;
                else if (rTopProperty == AlignTopWithProperty)
                    rInfo.Top = rInfo.TopDependency.Top;
                else if (rTopProperty == BelowProperty)
                    rInfo.Top = rHeight - rInfo.TopDependency.Bottom;

                var rBottomProperty = rInfo.BottomProperty;
                if (rBottomProperty == null || rInfo.IsVerticalDirectionCenterAlignment)
                    rInfo.Bottom = double.NaN;
                else if (rBottomProperty == AlignBottomWithPanelProperty)
                    rInfo.Bottom = .0;
                else if (rBottomProperty == AlignBottomWithProperty)
                    rInfo.Bottom = rInfo.BottomDependency.Bottom;
                else if (rBottomProperty == AboveProperty)
                    rInfo.Bottom = rHeight - rInfo.BottomDependency.Top;

                var rAvailableHeight = rHeight - rInfo.Bottom - rInfo.Top;
                if (rAvailableHeight.IsNaN())
                {
                    rAvailableHeight = rHeight;

                    if (!rInfo.Top.IsNaN() && rInfo.Bottom.IsNaN())
                        rAvailableHeight -= rInfo.Top;
                    else if (rInfo.Top.IsNaN() && !rInfo.Bottom.IsNaN())
                        rAvailableHeight -= rInfo.Bottom;
                }

                rElement.Measure(new Size(rWidth - rInfo.Right - rInfo.Left, rAvailableHeight));

                if (!rInfo.IsVerticalDirectionCenterAlignment)
                {
                    if (rInfo.Top.IsNaN() && !rInfo.Bottom.IsNaN())
                        rInfo.Top = rHeight - rInfo.Bottom - rElement.DesiredSize.Height;
                    else if (rInfo.Bottom.IsNaN())
                    {
                        if (rInfo.Top.IsNaN())
                            rInfo.Top = .0;

                        rInfo.Bottom = rHeight - rInfo.Top - rElement.DesiredSize.Height;
                    }

                    continue;
                }

                if (rTopProperty == AlignVerticalCenterWithPanelProperty)
                    rInfo.Top = rInfo.Bottom = (rHeight - rElement.DesiredSize.Height) / 2.0;
                else
                {
                    var rDependencyHeight = rHeight - rInfo.TopDependency.Bottom - rInfo.TopDependency.Top;
                    var rHalfHeightDifference = (rElement.DesiredSize.Height - rDependencyHeight) / 2.0;

                    rInfo.Top = rInfo.TopDependency.Top - rHalfHeightDifference;
                    rInfo.Bottom = rInfo.TopDependency.Bottom - rHalfHeightDifference;
                }
            }

            return rpAvailableSize;
        }

        void SortHorizontalDirection()
        {
            var rQueue = new Queue<LayoutInfo>(r_LayoutInfos.Count);

            foreach (var rInfo in r_LayoutInfos)
            {
                rInfo.GraphDependencies = new HashSet<LayoutInfo>();
                rInfo.GraphDependents = new HashSet<LayoutInfo>();

                rInfo.LeftProperty = null;
                rInfo.LeftDependency = null;
                rInfo.RightProperty = null;
                rInfo.RightDependency = null;

                rInfo.IsHorizontalDirectionCenterAlignment = false;
            }

            foreach (var rInfo in r_LayoutInfos)
                if (!CheckDependenciesAndBuildGraph(rInfo, r_HorizontalEdgeAProperties, r_HorizontalEdgeBProperties, r_HorizontalCenterAlignmentProperties))
                    rQueue.Enqueue(rInfo);

            var rSortedElementCount = 0;

            r_HorizontalDirection = new LayoutInfo[r_LayoutInfos.Count];

            while (rQueue.Count > 0)
            {
                var rInfo = rQueue.Dequeue();

                r_HorizontalDirection[rSortedElementCount++] = rInfo;

                foreach (var rDependent in rInfo.GraphDependents)
                {
                    var rDependencies = rDependent.GraphDependencies;

                    rDependencies.Remove(rInfo);

                    if (rDependencies.Count == 0)
                        rQueue.Enqueue(rDependent);
                }

                rInfo.GraphDependencies = null;
                rInfo.GraphDependents = null;
            }

            if (rSortedElementCount < r_HorizontalDirection.Length)
                throw new InvalidOperationException("RelativePanel error: Circular dependency detected. Layout could not complete.");

            r_IsHorizontalDirectionDirty = false;
        }
        void SortVerticalDirection()
        {
            var rQueue = new Queue<LayoutInfo>(r_LayoutInfos.Count);

            foreach (var rInfo in r_LayoutInfos)
            {
                rInfo.GraphDependencies = new HashSet<LayoutInfo>();
                rInfo.GraphDependents = new HashSet<LayoutInfo>();

                rInfo.TopProperty = null;
                rInfo.TopDependency = null;
                rInfo.BottomProperty = null;
                rInfo.BottomDependency = null;

                rInfo.IsVerticalDirectionCenterAlignment = false;
            }

            foreach (var rInfo in r_LayoutInfos)
                if (!CheckDependenciesAndBuildGraph(rInfo, r_VerticalEdgeAProperties, r_VerticalEdgeBProperties, r_VerticalCenterAlignmentProperties))
                    rQueue.Enqueue(rInfo);

            var rSortedElementCount = 0;

            r_VerticalDirection = new LayoutInfo[r_LayoutInfos.Count];

            while (rQueue.Count > 0)
            {
                var rInfo = rQueue.Dequeue();

                r_VerticalDirection[rSortedElementCount++] = rInfo;

                foreach (var rDependent in rInfo.GraphDependents)
                {
                    var rDependencies = rDependent.GraphDependencies;

                    rDependencies.Remove(rInfo);

                    if (rDependencies.Count == 0)
                        rQueue.Enqueue(rDependent);
                }

                rInfo.GraphDependencies = null;
                rInfo.GraphDependents = null;
            }

            if (rSortedElementCount < r_VerticalDirection.Length)
                throw new InvalidOperationException("RelativePanel error: Circular dependency detected. Layout could not complete.");

            r_IsVerticalDirectionDirty = false;
        }

        bool CheckDependenciesAndBuildGraph(LayoutInfo rpInfo, DependencyProperty[] rpEdgeAProperties, DependencyProperty[] rpEdgeBProperties, DependencyProperty[] rpCenterAlignmentProperties)
        {
            var rHasEdgeADependency = CheckDependencyAndBuildGraphCore(rpInfo, rpEdgeAProperties);
            var rHasEdgeBDependency = CheckDependencyAndBuildGraphCore(rpInfo, rpEdgeBProperties);

            var rResult = rHasEdgeADependency || rHasEdgeBDependency;
            if (!rResult)
            {
                var rElement = rpInfo.Element;

                var rCenterAlignmentProperty = rpCenterAlignmentProperties[0];
                if ((bool)rElement.GetValue(rCenterAlignmentProperty))
                {
                    if (rpCenterAlignmentProperties == r_HorizontalCenterAlignmentProperties)
                    {
                        rpInfo.LeftProperty = rpInfo.RightProperty = rCenterAlignmentProperty;
                        rpInfo.IsHorizontalDirectionCenterAlignment = true;
                    }
                    else
                    {
                        rpInfo.TopProperty = rpInfo.BottomProperty = rCenterAlignmentProperty;
                        rpInfo.IsVerticalDirectionCenterAlignment = true;
                    }

                    return false;
                }

                rCenterAlignmentProperty = rpCenterAlignmentProperties[1];
                var rDependency = GetDependency(rElement, rCenterAlignmentProperty);
                if (rDependency == null)
                    return false;

                if (rpCenterAlignmentProperties == r_HorizontalCenterAlignmentProperties)
                {
                    rpInfo.LeftProperty = rpInfo.RightProperty = rCenterAlignmentProperty;
                    rpInfo.LeftDependency = rpInfo.RightDependency = rDependency;
                    rpInfo.IsHorizontalDirectionCenterAlignment = true;
                }
                else
                {
                    rpInfo.TopProperty = rpInfo.BottomProperty = rCenterAlignmentProperty;
                    rpInfo.TopDependency = rpInfo.BottomDependency = rDependency;
                    rpInfo.IsVerticalDirectionCenterAlignment = true;
                }

                rDependency.GraphDependents.Add(rpInfo);
                rpInfo.GraphDependencies.Add(rDependency);

                return true;
            }

            return rResult;
        }
        bool CheckDependencyAndBuildGraphCore(LayoutInfo rpInfo, DependencyProperty[] rpProperties)
        {
            var rElement = rpInfo.Element;

            var rAlignWithPanelProperty = rpProperties[0];
            if ((bool)rElement.GetValue(rAlignWithPanelProperty))
            {
                if (rAlignWithPanelProperty == AlignLeftWithPanelProperty)
                    rpInfo.LeftProperty = rAlignWithPanelProperty;
                else if (rAlignWithPanelProperty == AlignTopWithPanelProperty)
                    rpInfo.TopProperty = rAlignWithPanelProperty;
                else if (rAlignWithPanelProperty == AlignRightWithPanelProperty)
                    rpInfo.RightProperty = rAlignWithPanelProperty;
                else if (rAlignWithPanelProperty == AlignBottomWithPanelProperty)
                    rpInfo.BottomProperty = rAlignWithPanelProperty;

                return false;
            }

            for (var i = 1; i < rpProperties.Length; i++)
            {
                var rProperty = rpProperties[i];

                var rDependency = GetDependency(rElement, rProperty);
                if (rDependency == null)
                    continue;

                if (rProperty == AlignLeftWithProperty || rProperty == RightOfProperty)
                {
                    rpInfo.LeftProperty = rProperty;
                    rpInfo.LeftDependency = rDependency;
                }
                else if (rProperty == AlignTopWithProperty || rProperty == BelowProperty)
                {
                    rpInfo.TopProperty = rProperty;
                    rpInfo.TopDependency = rDependency;
                }
                else if (rProperty == AlignRightWithProperty || rProperty == LeftOfProperty)
                {
                    rpInfo.RightProperty = rProperty;
                    rpInfo.RightDependency = rDependency;
                }
                else if (rProperty == AlignBottomWithProperty || rProperty == AboveProperty)
                {
                    rpInfo.BottomProperty = rProperty;
                    rpInfo.BottomDependency = rDependency;
                }

                rDependency.GraphDependents.Add(rpInfo);
                rpInfo.GraphDependencies.Add(rDependency);

                return true;
            }

            return false;
        }
        LayoutInfo GetDependency(DependencyObject rpElement, DependencyProperty rpProperty)
        {
            var rValue = rpElement.GetValue(rpProperty);
            if (rValue == null)
                return null;

            var rElementName = rValue as string;
            if (rElementName != null)
            {
                LayoutInfo rResult;
                if (!r_NamedLayoutInfos.TryGetValue(rElementName, out rResult))
                    throw new ArgumentException($"RelativePanel error: The name '{rElementName}' does not exist in the current context");

                return rResult;
            }

            var rElement = rValue as UIElement;
            if (rElement != null)
            {
                if (!InternalChildren.Contains(rElement))
                    throw new ArgumentException("RelativePanel error: Element does not exist in the current context");

                return GetLayoutInfo(rElement);
            }

            throw new ArgumentException("RelativePanel error: Value must be of type UIElement");
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            var rWidth = rpFinalSize.Width;
            var rHeight = rpFinalSize.Height;

            foreach (var rInfo in r_LayoutInfos)
                rInfo.Element.Arrange(new Rect(rInfo.Left, rInfo.Top, rWidth - rInfo.Left - rInfo.Right, rHeight - rInfo.Top - rInfo.Bottom));

            return rpFinalSize;
        }

        static void OnHorizontalDirectionPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var rOwner = (s as FrameworkElement)?.Parent as RelativePanel;
            if (rOwner != null)
                rOwner.r_IsHorizontalDirectionDirty = true;
        }
        static void OnVerticalDirectionPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var rOwner = (s as FrameworkElement)?.Parent as RelativePanel;
            if (rOwner != null)
                rOwner.r_IsVerticalDirectionDirty = true;
        }

        protected override void OnVisualChildrenChanged(DependencyObject rpVisualAdded, DependencyObject rpVisualRemoved)
        {
            r_IsInfosDirty = true;
            r_IsHorizontalDirectionDirty = true;
            r_IsVerticalDirectionDirty = true;

            UIElement rElement;
            LayoutInfo rLayoutInfo;

            if (rpVisualAdded != null)
            {
                rElement = (UIElement)rpVisualAdded;
                rLayoutInfo = new LayoutInfo(rElement);

                r_LayoutInfos.Add(rLayoutInfo);

                SetLayoutInfo(rElement, rLayoutInfo);
            }
            else if (rpVisualRemoved != null)
            {
                rElement = (UIElement)rpVisualRemoved;
                rLayoutInfo = GetLayoutInfo(rElement);

                r_LayoutInfos.Remove(rLayoutInfo);

                SetLayoutInfo(rElement, null);
            }

            base.OnVisualChildrenChanged(rpVisualAdded, rpVisualRemoved);
        }

        [DebuggerDisplay("Element = {Element}, Name = {Name}, Placement = [{Left}, {Top}, {Right}, {Bottom}]")]
        class LayoutInfo
        {
            public UIElement Element { get; }

            public string Name => (Element as FrameworkElement)?.Name;

            public ISet<LayoutInfo> GraphDependencies { get; set; }
            public ISet<LayoutInfo> GraphDependents { get; set; }

            public DependencyProperty LeftProperty, TopProperty, RightProperty, BottomProperty;
            public LayoutInfo LeftDependency, TopDependency, RightDependency, BottomDependency;

            public bool IsHorizontalDirectionCenterAlignment, IsVerticalDirectionCenterAlignment;

            public double Left, Top, Right, Bottom;

            public LayoutInfo(UIElement rpElement)
            {
                Element = rpElement;
            }
        }
    }
}
