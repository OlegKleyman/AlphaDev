using System;

namespace AlphaDev.Web.Support
{
    public struct PositiveInteger
    {
        private readonly int _value;
        public static readonly PositiveInteger MinValue = new PositiveInteger(1);
        public static readonly PositiveInteger MaxValue = new PositiveInteger(int.MaxValue);

        public int Value => _value > 0 ? _value : throw new InvalidOperationException("The value is invalid.");

        public PositiveInteger(int value)
        {
            _value = value > 0 ? value : throw new ArgumentException("Must be positive.", nameof(value));
        }

        public static int operator +(PositiveInteger x, int y)
        {
            return x.Value + y;
        }

        public static int operator /(PositiveInteger dividend, int divisor) => dividend.Value / divisor;

        public static int operator /(int dividend, PositiveInteger divisor) => dividend / divisor.Value;

        public static int operator /(PositiveInteger dividend, PositiveInteger divisor) => dividend.Value / divisor.Value;
    }
}