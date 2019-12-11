using System;
using System.Collections.Generic;
using Optional;

namespace AlphaDev.Core
{
    public interface IBlogService
    {
        Option<BlogBase> GetLatest();

        Option<BlogBase> Get(int id);

        BlogBase Add(BlogBase blog);

        void Delete(int id);

        void Edit(int id, Action<BlogEditArguments> edit);

        IEnumerable<BlogBase> GetOrderedByDates(int start, int count);

        int GetCount(int start);
    }
}