using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data
{
    public interface IContextFactory<out T> where T : DbContext
    {
        T Create();
    }
}