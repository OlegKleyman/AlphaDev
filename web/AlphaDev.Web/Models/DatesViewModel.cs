using System;
using System.ComponentModel.DataAnnotations;
using AlphaDev.Core;
using Optional;

namespace AlphaDev.Web.Models
{
    public struct DatesViewModel
    {
        public DatesViewModel(DateTime created, Option<DateTime> modified)
        {
            Created = created;
            Modified = modified;
        }

        [DisplayFormat(DataFormatString = "{0:dddd, MMMM dd, yyyy}")]
        public DateTime Created { get; }

        [DisplayFormat(DataFormatString = "{0:dddd, MMMM dd, yyyy}")]
        [UIHint("OptionalDate")]
        public Option<DateTime> Modified { get; }
    }
}