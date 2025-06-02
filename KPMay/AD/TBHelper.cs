using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Linq;


namespace KPMay
{
    public static class TBHelper
    {
        public static string GetPlaceholder(DependencyObject obj) => (string)obj.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(DependencyObject obj, string value) => obj.SetValue(PlaceholderProperty, value);

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TBHelper), new FrameworkPropertyMetadata(defaultValue: null, propertyChangedCallback: OnPlaceholderChanged));

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                if (tb.IsLoaded)
                {
                    tb.Loaded -= TextBoxControl_Loaded;
                    tb.Loaded += TextBoxControl_Loaded;
                }

                tb.TextChanged -= TextBoxControl_TextChanged;
                tb.TextChanged += TextBoxControl_TextChanged;

                if (GetOrCreateAdorner(tb, out PlaceHolderAdorner adorner))
                {
                    adorner.InvalidateVisual();
                }
            }
        }

        private static void TextBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Loaded -= TextBoxControl_Loaded;
                GetOrCreateAdorner(tb, out _);
            }
        }

        private static void TextBoxControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && GetOrCreateAdorner(tb, out PlaceHolderAdorner adorner))
            {
                if (tb.Text.Length > 0)
                {
                    adorner.Visibility = Visibility.Hidden;
                }
                else
                {
                    adorner.Visibility = Visibility.Visible;
                }
            }
        }

        private static bool GetOrCreateAdorner(TextBox tb, out PlaceHolderAdorner adorner)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(tb);
            if (layer == null)
            {
                adorner = null;
                return false;
            }
            adorner = layer.GetAdorners(tb)?.OfType<PlaceHolderAdorner>().FirstOrDefault();
            if (adorner == null)
            {
                adorner = new PlaceHolderAdorner(tb);
                layer.Add(adorner);
            }
            return true;
        }

        public class PlaceHolderAdorner : Adorner
        {
            public PlaceHolderAdorner(TextBox tb) : base(tb) { }
            protected override void OnRender(DrawingContext drawingContext)
            {
                TextBox tb = (TextBox)AdornedElement;
                string placeholderValue = TBHelper.GetPlaceholder(tb);
                if (string.IsNullOrEmpty(placeholderValue))
                {
                    return;
                }
                FormattedText text = new FormattedText(
                    placeholderValue,
                    System.Globalization.CultureInfo.CurrentCulture,
                    tb.FlowDirection,
                    new Typeface(tb.FontFamily,
                                 tb.FontStyle,
                                 tb.FontWeight,
                                 tb.FontStretch),
                    tb.FontSize,
                    SystemColors.InactiveCaptionBrush,
                    VisualTreeHelper.GetDpi(tb).PixelsPerDip);
                text.MaxTextWidth = System.Math.Max(tb.ActualWidth - tb.Padding.Left - tb.Padding.Right, 10);
                text.MaxTextHeight = System.Math.Max(tb.ActualHeight, 10);
                Point renderingOffset = new Point(tb.Padding.Left, tb.Padding.Top);
                if (tb.Template.FindName("PART_ContentHost", tb) is FrameworkElement part)
                {
                    Point partPosition = part.TransformToAncestor(tb).Transform(new Point(0, 0));
                    renderingOffset.X += partPosition.X;
                    renderingOffset.Y += partPosition.Y;

                    text.MaxTextWidth = System.Math.Max(part.ActualWidth - renderingOffset.X, 10);
                    text.MaxTextHeight = System.Math.Max(part.ActualHeight, 10);
                }
                drawingContext.DrawText(text, renderingOffset);
            }
        }
    }

}
