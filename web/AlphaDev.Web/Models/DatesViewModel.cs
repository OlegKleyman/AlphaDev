using System;
using System.ComponentModel.DataAnnotations;
using Optional;

namespace AlphaDev.Web.Models
{
    public struct DatesViewModel
    {
        private const string DateDisplayFormat = "{0:dddd, MMMM dd, yyyy}";

        public DatesViewModel(DateTime created, Option<DateTime> modified)
        {
            Created = created;
            Modified = modified;
        }

        [DisplayFormat(DataFormatString = DateDisplayFormat)]
        public DateTime Created { get; }

        [DisplayFormat(DataFormatString = DateDisplayFormat)]
        // ReSharper disable once Mvc.TemplateNotResolved  - it exists
        [UIHint("OptionalDate")]
        public Option<DateTime> Modified { get; }
    }
}