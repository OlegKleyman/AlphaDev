using AlphaDev.Core.Data.Sql.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class MockBlogContext:BlogContext
    {
        public MockBlogContext():base((string) "Server={server};Database={database}")
        {
        }

        public void OnModelCreatingProxy(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }
    }
}