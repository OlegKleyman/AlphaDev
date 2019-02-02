using JetBrains.Annotations;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public struct Page
    {
        public bool Equals(Page other)
        {
            return Identity.Equals(other.Identity) && DisplayFormat == other.DisplayFormat &&
                   Attributes == other.Attributes;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return !(obj is null) && obj is Page other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Identity.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) DisplayFormat;
                hashCode = (hashCode * 397) ^ Attributes.GetHashCode();
                return hashCode;
            }
        }

        public PageIdentity Identity { get; }
        public DisplayFormat DisplayFormat { get; }
        public PageAttributes Attributes { get; }

        public Page(PageIdentity identity, DisplayFormat displayFormat, PageAttributes attributes)
        {
            Identity = identity;
            DisplayFormat = displayFormat;
            Attributes = attributes;
        }

        public static bool operator ==(Page x, Page y)
        {
            return x.Attributes == y.Attributes && x.DisplayFormat == y.DisplayFormat && x.Identity == y.Identity;
        }

        public static bool operator !=(Page x, Page y)
        {
            return !(x == y);
        }
    }
}