using System;

namespace AlphaDev.Core
{
    public class DateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}