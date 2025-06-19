using Avalonia;
using Avalonia.Controls;

namespace MvvmExample.Components;

public partial class InputText : UserControl
{
    public static readonly AvaloniaProperty<object> LabelProperty =
        AvaloniaProperty.Register<InputText, object>(nameof(Label));
    public object Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly AvaloniaProperty<string?> TextProperty =
        AvaloniaProperty.Register<InputText, string?>(nameof(Text));
    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly AvaloniaProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<InputText, bool>(nameof(IsReadOnly));
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly AvaloniaProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<InputText, string?>(nameof(Watermark));
    public string? Watermark
    {
        get => (string?)GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public InputText()
    {
        InitializeComponent();
    }
}