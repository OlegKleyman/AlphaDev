namespace AppDev.Core
{
    using System;

    using Optional;

    public class Dates
    {
        public Dates(DateTime created, Option<DateTime> modified)
        {
            Created = created;
            Modified = modified;
        }

        public DateTime Created { get; }

        public Option<DateTime> Modified { get; }
    }
}