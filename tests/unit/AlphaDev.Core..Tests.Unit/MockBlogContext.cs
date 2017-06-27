namespace AlphaDev.Core.Tests.Unit
{
    using System;

    using AlphaDev.Core.Data.Contexts;

    using Microsoft.EntityFrameworkCore;

    public class MockBlogContext : BlogContext
    {
        private readonly string testName;

        public MockBlogContext(string testName) => this.testName = testName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase(testName);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}