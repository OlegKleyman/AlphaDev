namespace AlphaDev.Core.Data
{
    using System.Linq;

    using AlphaDev.Core.Data.Entties;

    public interface IBlogContext
    {
        IQueryable<Blog> Blogs { get; }
    }
}