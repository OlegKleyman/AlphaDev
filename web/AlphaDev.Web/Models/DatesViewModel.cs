namespace AlphaDev.Web.Models
{
    using System;

    using Optional;

    public class DatesViewModel
    {
        public DateTime Created { get; }

        public Option<DateTime> Modified { get; }

        public DatesViewModel(DateTime created, Option<DateTime> modified)
        {
            Created = created;
            Modified = modified;
        }
    }
}