using JetBrains.Annotations;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public struct PageIdentity
    {
        public bool Equals(PageIdentity other) =>
            string.Equals(DisplayValue, other.DisplayValue) && Number == other.Number;

        public override bool Equals([CanBeNull] object obj) =>
            !(obj is null) && obj is PageIdentity other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DisplayValue != null ? DisplayValue.GetHashCode() : 0) * 397) ^ Number;
            }
        }

        public string DisplayValue { get; }

        public int Number { get; }

        public PageIdentity(string displayValue, int number)
        {
            DisplayValue = displayValue;
            Number = number;
        }

        public static bool operator ==(PageIdentity x, PageIdentity y) =>
            x.DisplayValue == y.DisplayValue && x.Number == y.Number;

        public static bool operator !=(PageIdentity x, PageIdentity y) => !(x == y);
    }
}