using System;

namespace AlphaDev.Core
{
    public interface IDateProvider
    {
        DateTime UtcNow { get; }
    }
}