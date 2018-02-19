using System.Collections.Generic;
using Optional;

namespace AlphaDev.Core
{
    public interface IBlogService
    {
        BlogBase GetLatest();
        IEnumerable<BlogBase> GetAll();
        Option<BlogBase> Get(int id);
    }
}