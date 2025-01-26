using System;

namespace MovieChest.ComponentModel;

public sealed class ActionDisposable(Action dispose) : IDisposable
{
    public void Dispose() => dispose();
}