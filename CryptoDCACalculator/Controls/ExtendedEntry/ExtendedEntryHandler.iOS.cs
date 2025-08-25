#if __IOS__
using System.Drawing;
using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace CryptoDCACalculator.Controls.ExtendedEntry;

public partial class ExtendedEntryHandler
{
    protected override void ConnectHandler(MauiTextField nativeView)
    {
        base.ConnectHandler(nativeView);

        // Call ManageDoneButton to add the Done button
        if (VirtualView is ExtendedEntry extendedEntry)
        {
            ManageDoneButton(this, true);
        }
    }

    protected override void DisconnectHandler(MauiTextField platformView)
    {
        base.DisconnectHandler(platformView);
        ManageDoneButton(this, false);
    }

    public static void ManageDoneButton(IEntryHandler handler, bool show)
    {
        if (!show)
            return;

        var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));

        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
        {
            handler.PlatformView.ResignFirstResponder();
            var baseEntry = handler.VirtualView.GetType();
            ((IEntryController)handler.VirtualView).SendCompleted();
        });

        toolbar.Items = new UIBarButtonItem[]
        {
            new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
            doneButton
        };

        handler.PlatformView.InputAccessoryView = toolbar;
    }

    private void SetPadding(ExtendedEntry view)
    {
        PlatformView.LeftView = new UIView(new CGRect(0f, 0f, view.Padding.Left, PlatformView.Frame.Height));
        PlatformView.LeftViewMode = UITextFieldViewMode.Always;

        PlatformView.RightView = new UIView(new CGRect(0f, 0f, view.Padding.Right, PlatformView.Frame.Height));
        PlatformView.RightViewMode = UITextFieldViewMode.Always;
    }

    private void SetCornerRadius(ExtendedEntry view)
    {
        PlatformView.Layer.CornerRadius = view.HasBorder ? Convert.ToSingle(view.CornerRadius) : 0;
    }

    private void SetBorderColor(ExtendedEntry view)
    {
        PlatformView.Layer.BorderColor = view.HasBorder ? view.BorderColor?.ToCGColor() : Colors.Transparent.ToCGColor();
        PlatformView.Layer.BackgroundColor = (view.BackgroundColor ?? Colors.Transparent).ToCGColor();
        PlatformView.BackgroundColor = (view.BackgroundColor ?? Colors.Transparent).ToPlatform();
    }

    private void SetBorderWidth(ExtendedEntry view)
    {
        if (!view.HasBorder || view.BorderWidth == 0)
        {
            PlatformView.BorderStyle = UITextBorderStyle.None;
            return;
        }

        PlatformView.BorderStyle = UITextBorderStyle.RoundedRect;
        PlatformView.Layer.BorderWidth = view.HasBorder ? view.BorderWidth : 0;
        PlatformView.ClipsToBounds = true;
        PlatformView.Layer.BackgroundColor = (view.BackgroundColor ?? Colors.Transparent).ToCGColor();
        PlatformView.BackgroundColor = (view.BackgroundColor ?? Colors.Transparent).ToPlatform();
    }

    private void SetHasBorder(ExtendedEntry view)
    {
        SetBorderWidth(view);
    }
}
#endif
