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

        Task<BlogBase> AddAsync(BlogBase blog);

        Task DeleteAsync(int id);

        Task EditAsync(int id, Action<BlogEditArguments> edit);

        Task<IEnumerable<BlogBase>> GetOrderedByDatesAsync(int start, int count);

        Task<int> GetCountAsync(int start);
    }
}