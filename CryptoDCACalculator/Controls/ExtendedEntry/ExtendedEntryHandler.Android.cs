#if __ANDROID__
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Util;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;

namespace CryptoDCACalculator.Controls.ExtendedEntry;

public partial class ExtendedEntryHandler
{
    private readonly GradientDrawable _gradientBackground = new ();

    protected override void ConnectHandler(AppCompatEditText platformView)
    {
        base.ConnectHandler(platformView);
        platformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
    }

    private void SetCornerRadius(ExtendedEntry view)
    {
        UpdateBorders(view);
    }

    private void SetBorderColor(ExtendedEntry view)
    {
        UpdateBorders(view);
    }

    private void SetBorderWidth(ExtendedEntry view)
    {
        UpdateBorders(view);
    }

    private void SetHasBorder(ExtendedEntry view)
    {
        UpdateBorders(view);
    }

    private void SetPadding(ExtendedEntry view)
    {
        PlatformView.SetPadding(
            (int)DpToPixels(Convert.ToSingle(view.Padding.Left)),
            (int)DpToPixels(Convert.ToSingle(view.Padding.Top)),
            (int)DpToPixels(Convert.ToSingle(view.Padding.Right)),
            (int)DpToPixels(Convert.ToSingle(view.Padding.Bottom)));
    }

    private void UpdateBorders(ExtendedEntry view)
    {
        if (!view.HasBorder)
            return;

        _gradientBackground.SetShape(ShapeType.Rectangle);
        if (view.BackgroundColor != null)
            _gradientBackground.SetColor(view.BackgroundColor.ToPlatform());
        if (view.BorderColor != null)
            _gradientBackground.SetStroke((int)DpToPixels(Convert.ToSingle(view.BorderWidth)), view.BorderColor.ToPlatform());
        _gradientBackground.SetCornerRadius(DpToPixels(Convert.ToSingle(view.CornerRadius)));
        
        PlatformView.SetBackground(_gradientBackground);
    }

    private float DpToPixels(float valueInDp)
    {
        var metrics = Context.Resources!.DisplayMetrics;
        return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
    }
}
#endif
