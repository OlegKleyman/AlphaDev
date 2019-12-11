using System;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public struct PageAttributes
    {
        public bool Equals(PageAttributes other) => Active == other.Active && Url.Equals(other.Url);

        public override bool Equals([CanBeNull] object obj) =>
            !(obj is null) && obj is PageAttributes other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Active.GetHashCode() * 397) ^ Url.GetHashCode();
            }
        }

        public ActivityStatus Active { get; }

        public Option<Uri> Url { get; }

        public PageAttributes(Uri baseUrl, int number)
        {
            Active = ActivityStatus.Inactive;
            Url = new Uri(baseUrl, $"page/{number}").Some();
        }

        public PageAttributes ToActive(int pageNumber) => new PageAttributes();

        public static bool operator ==(PageAttributes x, PageAttributes y) => x.Equals(y);

        public static bool operator !=(PageAttributes x, PageAttributes y) => !(x == y);
    }
}