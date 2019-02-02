using System;
using JetBrains.Annotations;

namespace AlphaDev.Web.Support
{
    public class PositiveInteger : IEquatable<PositiveInteger>
    {
        public static readonly PositiveInteger MinValue = new PositiveInteger(1);
        public static readonly PositiveInteger MaxValue = new PositiveInteger(int.MaxValue);

        public int Value { get; }

        public PositiveInteger(int value)
        {
            Value = value > 0 ? value : throw new ArgumentException("Must be positive.", nameof(value));
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return obj is PositiveInteger p && Equals(p);
        }

        public bool Equals([CanBeNull] PositiveInteger other)
        {
            return !(other is null) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static int operator +([NotNull] PositiveInteger x, int y)
        {
            return x.Value + y;
        }

        public static int operator /([NotNull] PositiveInteger dividend, int divisor) => dividend.Value / divisor;

        public static int operator /(int dividend, [NotNull] PositiveInteger divisor) => dividend / divisor.Value;

        public static int operator /([NotNull] PositiveInteger dividend, [NotNull] PositiveInteger divisor) => dividend.Value / divisor.Value;
    }
}