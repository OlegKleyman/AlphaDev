using System;
using JetBrains.Annotations;

namespace AlphaDev.Core
{
    public class PositiveInteger : IEquatable<PositiveInteger>
    {
        public static readonly PositiveInteger MinValue = new PositiveInteger(1);
        public static readonly PositiveInteger MaxValue = new PositiveInteger(int.MaxValue);

        public PositiveInteger(int value)
        {
            Value = value > 0 ? value : throw new ArgumentException("Must be positive.", nameof(value));
        }

        public int Value { get; }

        public bool Equals(PositiveInteger? other)
        {
            return !(other is null) && Value == other.Value;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return obj is PositiveInteger p && Equals(p);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static int operator +([NotNull] PositiveInteger x, int y)
        {
            return x.Value + y;
        }

        public static int operator /([NotNull] PositiveInteger dividend, int divisor)
        {
            return dividend.Value / divisor;
        }

        public static int operator /(int dividend, [NotNull] PositiveInteger divisor)
        {
            return dividend / divisor.Value;
        }

        public static int operator /([NotNull] PositiveInteger dividend, [NotNull] PositiveInteger divisor)
        {
            return dividend.Value / divisor.Value;
        }
    }
}