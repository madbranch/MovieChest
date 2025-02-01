using Avalonia;

namespace MovieChest;

public class BindingProxy : AvaloniaObject
{
    public static readonly StyledProperty<object?> DataProperty =
        AvaloniaProperty.Register<BindingProxy, object?>(nameof(Data), defaultValue: null);

    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}