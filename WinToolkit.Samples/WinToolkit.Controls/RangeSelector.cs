using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WinToolkit.Controls
{
    public class RangeSelector : Control
    {
        private Thumb _leftThumb;
        private Thumb _rightThumb;
        private RectangleGeometry _trackGeometry;
        private Rectangle _trackRect;
        private const string LeftThumbName = "LeftThumb";
        private const string RightThumbName = "RightThumb";
        private const string TrackRectName = "TrackRect";

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof (int), typeof (RangeSelector), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof (int), typeof (RangeSelector), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty StartRangeProperty = DependencyProperty.Register(
            "StartRange", typeof (int), typeof (RangeSelector),
            new PropertyMetadata(default(int), StartRangePropertyChanged));

        public static readonly DependencyProperty EndRangeProperty = DependencyProperty.Register(
            "EndRange", typeof (int), typeof (RangeSelector),
            new PropertyMetadata(default(int), EndRangePropertyChanged));

        public RangeSelector()
        {
            DefaultStyleKey = typeof (RangeSelector);
        }

        public int EndRange
        {
            get { return (int) GetValue(EndRangeProperty); }
            set { SetValue(EndRangeProperty, value); }
        }

        public int StartRange
        {
            get { return (int) GetValue(StartRangeProperty); }
            set { SetValue(StartRangeProperty, value); }
        }

        public int Maximum
        {
            get { return (int) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int Minimum
        {
            get { return (int) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public event EventHandler RangeChanged;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftThumb = (Thumb) GetTemplateChild(LeftThumbName);
            _leftThumb.DragDelta += LeftThumbDragDelta;
            _leftThumb.DragCompleted += ThumbDragCompleted;
            _leftThumb.Loaded += LeftThumbLoaded;

            _rightThumb = (Thumb) GetTemplateChild(RightThumbName);
            _rightThumb.DragDelta += RightThumbDragDelta;
            _rightThumb.DragCompleted += ThumbDragCompleted;
            _rightThumb.Loaded += RightThumbLoaded;

            _trackGeometry = (RectangleGeometry) GetTemplateChild("TrackRectangle");
            _trackRect = (Rectangle) GetTemplateChild(TrackRectName);
        }

        private static void EndRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (RangeSelector) d;

            if (e.NewValue != null && (int) e.NewValue != selector.Maximum && selector.Maximum > 0)
                return;

            selector.OnEndRangeReset();
        }

        private double GetLeftThumbRelatedPosition()
        {
            GeneralTransform transform = _leftThumb.TransformToVisual(this);
            return transform.TransformPoint(new Point()).X;
        }

        private double GetRightThumbRelatedPosition()
        {
            GeneralTransform transform = _rightThumb.TransformToVisual(this);
            return transform.TransformPoint(new Point()).X;
        }

        private double GetStep()
        {
            return (Maximum - Minimum)/(ActualWidth - _rightThumb.ActualWidth);
        }

        private bool IsLeftThumbShiftIsLessRightThumbPosition(double delta)
        {
            GeneralTransform transformation = _leftThumb.TransformToVisual(_rightThumb);
            Point leftThumbPoint = transformation.TransformPoint(new Point());
            if (leftThumbPoint.X + _leftThumb.ActualWidth + delta > 0.0)
                return false;

            return true;
        }

        private bool IsRightThumbShiftIsLessLeftThumbPosition(double delta)
        {
            GeneralTransform transformation = _rightThumb.TransformToVisual(_leftThumb);
            Point rightThumbPoint = transformation.TransformPoint(new Point());
            if (rightThumbPoint.X + delta < _leftThumb.ActualWidth)
                return false;

            return true;
        }

        private void LeftThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = (Thumb) sender;

            var thumbTransformation = (TranslateTransform) thumb.RenderTransform;
            if (thumbTransformation.X + e.HorizontalChange <= 0.0)
                thumbTransformation.X = 0.0;
            else if (IsLeftThumbShiftIsLessRightThumbPosition(e.HorizontalChange))
                ((TranslateTransform) thumb.RenderTransform).X += e.HorizontalChange;

            UpdateStartRange();
            UpdateTrack();
        }

        private void LeftThumbLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLeftThumbPosition();
            UpdateTrack();
        }

        private void OnEndRangeReset()
        {
            if (EndRange != Maximum) return;
            ((TranslateTransform) _rightThumb.RenderTransform).X = 0.0;
            UpdateTrack();
        }

        private void OnRangeChanged()
        {
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnStartRangeReset()
        {
            if (StartRange != Minimum) return;

            ((TranslateTransform) _leftThumb.RenderTransform).X = 0.0;
            UpdateTrack();
        }

        private void RightThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = (Thumb) sender;

            var thumbTransformation = (TranslateTransform) thumb.RenderTransform;
            if (thumbTransformation.X + e.HorizontalChange >= 0.0)
            {
                thumbTransformation.X = 0.0;
            }
            else if (IsRightThumbShiftIsLessLeftThumbPosition(e.HorizontalChange))
            {
                ((TranslateTransform) thumb.RenderTransform).X += e.HorizontalChange;
            }

            UpdateEndRange();
            UpdateTrack();
        }

        private void RightThumbLoaded(object sender, RoutedEventArgs e)
        {
            UpdateRightThumbPosition();
            UpdateTrack();
        }

        private static void StartRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (RangeSelector) d;
            if (e.NewValue != null && (int) e.NewValue != selector.Minimum && selector.Minimum > 0)
                return;

            selector.OnStartRangeReset();
        }

        private void ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnRangeChanged();
        }

        private void UpdateEndRange()
        {
            EndRange = (int) (GetStep()*GetRightThumbRelatedPosition()) + Minimum;
        }

        private void UpdateLeftThumbPosition()
        {
            if (StartRange != Minimum && EndRange - StartRange > 0 && ActualWidth > 0 && Maximum - Minimum > 0)
            {
                ((TranslateTransform) _leftThumb.RenderTransform).X = (ActualWidth - _leftThumb.ActualWidth/2)/(Maximum - Minimum)*(StartRange - Minimum);
            }
        }

        private void UpdateRightThumbPosition()
        {
            if (Maximum - Minimum > 0 && EndRange - StartRange > 0 && ActualWidth > 0)
            {
                double x = (ActualWidth - _rightThumb.ActualWidth/2)/(Maximum - Minimum)*(EndRange - Minimum);

                Point newPoint = TransformToVisual(_rightThumb).TransformPoint(new Point(x, 0));
                ((TranslateTransform) _rightThumb.RenderTransform).X = newPoint.X;
            }
        }

        private void UpdateStartRange()
        {
            StartRange = (int) (GetStep()*GetLeftThumbRelatedPosition()) + Minimum;
        }

        private void UpdateTrack()
        {

            double rightThumbX = GetRightThumbRelatedPosition();
            double leftThumbX = GetLeftThumbRelatedPosition();
            if (rightThumbX - leftThumbX < 0) //if right position hasn't updated yet
                return;

            double trackWidth = rightThumbX - leftThumbX;

            _trackGeometry.Rect = trackWidth > 0
                ? new Rect(leftThumbX, 0, trackWidth, _trackRect.Height)
                : new Rect(0, 0, Window.Current.Bounds.Width, _trackRect.Height);
        }
    }
}