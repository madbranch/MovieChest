using System;

namespace MovieChest.ComponentModel;

[Serializable]
public readonly struct Unit : IEquatable<Unit>
{
    public bool Equals(Unit other) => true;
    public override bool Equals(object? other) => other is Unit;
    public override int GetHashCode() => 0;
    public override string ToString() => "()";
    public static bool operator ==(Unit first, Unit second) => true;
    public static bool operator !=(Unit first, Unit second) => false;
    public static Unit Default => default;
}