using System;
using Optional;

namespace AlphaDev.Services
{
    public readonly struct Dates
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