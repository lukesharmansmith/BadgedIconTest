using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadgedIconTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BadgedIcon : ContentView
    {
        public static readonly BindableProperty FillColourProperty = BindableProperty.Create(
          propertyName: "FillColour",
          returnType: typeof(Color),
          declaringType: typeof(BadgedIcon),
          defaultValue: Color.Red,
          propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty BackingImageSourceProperty = BindableProperty.Create(
          propertyName: nameof(BackingImageSource),
          returnType: typeof(ImageSource),
          declaringType: typeof(BadgedIcon),
          defaultValue: default(ImageSource),
          propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty BadgeNumberProperty = BindableProperty.Create(
          propertyName: "BadgeNumber",
          returnType: typeof(int),
          declaringType: typeof(BadgedIcon),
          defaultValue: 0,
          propertyChanged: OnPropertyChanged);

        public BadgedIcon()
        {
            InitializeComponent();
        }

        public Color FillColour
        {
            get { return (Color)GetValue(FillColourProperty); }
            set { SetValue(FillColourProperty, value); }
        }

        public ImageSource BackingImageSource
        {
            get { return (ImageSource)GetValue(BackingImageSourceProperty); }
            set { SetValue(BackingImageSourceProperty, value); }
        }

        public int BadgeNumber
        {
            get { return (int)GetValue(BadgeNumberProperty); }
            set { SetValue(BadgeNumberProperty, value); }
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            var arcPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = this.FillColour.ToSKColor(),
                IsAntialias = true
            };

            var textPaint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColors.White,
                IsAntialias = true
            };

            canvas.Clear();

            var bounds = info.Rect;

            var vH = bounds.MidY / 2;
            var vX = (bounds.MidX / 2) + bounds.MidX;

            canvas.DrawCircle(vX, vH, vH, arcPaint);

            // Draw badge count message inside the circle.
            var badgeText = this.BadgeNumber.ToString();

            var textWidth = textPaint.MeasureText(badgeText);
            textPaint.TextSize = 0.6f * (vH * 2);

            var textBounds = new SKRect();
            textPaint.MeasureText(badgeText, ref textBounds);

            float xText = vX - textBounds.MidX;
            float yText = vH - textBounds.MidY;

            canvas.DrawText(badgeText, xText, yText, textPaint);
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Force a redraw if a property has changed
            if (bindable is BadgedIcon objCast)
            {
                objCast.canvas.InvalidateSurface();
            }
        }
    }
}