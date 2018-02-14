using System.Collections.Generic;

namespace AlphaDev.Core
{
    public interface IBlogService
    {
        BlogBase GetLatest();
    }
}