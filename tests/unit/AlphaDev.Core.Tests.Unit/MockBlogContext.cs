using AlphaDev.Core.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Tests.Unit
{
    public class MockBlogContext : BlogContext
    {
        private readonly string _testName;

        public MockBlogContext(string testName)
        {
            _testName = testName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_testName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}