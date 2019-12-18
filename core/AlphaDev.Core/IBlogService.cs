using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Core
{
    public interface IBlogService
    {
        Task<Option<BlogBase>> GetLatestAsync();

        Task<Option<BlogBase>> GetAsync(int id);

        BlogBase Add(BlogBase blog);

        void Delete(int id);

        void Edit(int id, Action<BlogEditArguments> edit);

        IEnumerable<BlogBase> GetOrderedByDates(int start, int count);

        int GetCount(int start);
    }
}