using System;
using Optional;

namespace AlphaDev.Web.Models
{
    public class DatesViewModel
    {
        public const string LongDataFormat = "dddd, MMMM dd, yyyy";

        public DatesViewModel(DateTime created, Option<DateTime> modified)
        {
            Created = created;
            Modified = modified;
        }

        public DateTime Created { get; }

        public Option<DateTime> Modified { get; }
    }
}