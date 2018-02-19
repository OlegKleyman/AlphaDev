using System;
using System.Text.RegularExpressions;
using AlphaDev.Core.Data.Contexts;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class BlogServiceTests
    {
        private BlogService GetBlogService(BlogContext context)
        {
            return new BlogService(context ?? new MockBlogContext("default"));
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetAllShouldReturnBlogsWithCreatedDate));
            context.Blogs.Add(new Data.Entities.Blog {Created = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Dates = new {Created = testValue}}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithEmptyContentWhenDbContentIsNull()
        {
            var context = new MockBlogContext(nameof(GetAllShouldReturnBlogsWithEmptyContentWhenDbContentIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Content = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Content = string.Empty}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithEmptyTitleWhenDbTitleIsNull()
        {
            var context = new MockBlogContext(nameof(GetAllShouldReturnBlogsWithEmptyTitleWhenDbTitleIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Title = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Title = string.Empty}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context =
                new MockBlogContext(nameof(GetAllShouldReturnBlogsWithNoModifiedDateWhenDbModifiedDateIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Modified = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Dates = new {Modified = Option.Some(testValue)}}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var context = new MockBlogContext(nameof(GetAllShouldReturnBlogsWithModifiedDate));
            context.Blogs.Add(new Data.Entities.Blog {Modified = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Dates = new {Modified = Option.None<DateTime>()}}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogsWithTitle()
        {
            const string testValue = "test";

            var context = new MockBlogContext(nameof(GetAllShouldReturnBlogsWithTitle));
            context.Blogs.Add(new Data.Entities.Blog {Title = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Title = testValue}},
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithContent));
            context.Blogs.Add(new Data.Entities.Blog {Content = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetAll().Should().BeEquivalentTo(
                new[] {new {Content = "test content"}},
                options => options.Including(info => Regex.IsMatch(info.SelectedMemberPath, @"\[.*\]\.Content")));
        }

        [Fact]
        public void GetAllShouldReturnEmptyBlogsWhenNoBlogIsFound()
        {
            var service = GetBlogService(new MockBlogContext(nameof(GetAllShouldReturnEmptyBlogsWhenNoBlogIsFound)));

            service.GetAll().Should().BeEmpty();
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithContent));
            context.Blogs.Add(new Data.Entities.Blog {Content = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Content = testValue},
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithCreatedDate));
            context.Blogs.Add(new Data.Entities.Blog {Created = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Dates = new {Created = testValue}},
                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithEmptyContentWhenDbContentIsNull()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithEmptyContentWhenDbContentIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Content = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Content = string.Empty},
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithEmptyTitleWhenDbTitleIsNull()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithEmptyTitleWhenDbTitleIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Title = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Title = string.Empty},
                options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithModifiedDate));
            context.Blogs.Add(new Data.Entities.Blog {Modified = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Dates = new {Modified = Option.Some(testValue)}},
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var context = new MockBlogContext(
                nameof(GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Modified = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Dates = new {Modified = Option.None<DateTime>()}},
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithTitle()
        {
            const string testValue = "test";

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithTitle));
            context.Blogs.Add(new Data.Entities.Blog {Title = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Title = testValue},
                options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithId()
        {
            const int testValue = 1;

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithId));
            context.Blogs.Add(new Data.Entities.Blog { Id = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new { Id = testValue },
                options => options.Including(info => info.Id));
        }

        [Fact]
        public void GetLatestShouldReturnEmptyBlogWhenNoBlogIsFound()
        {
            var service = GetBlogService(new MockBlogContext(nameof(GetLatestShouldReturnEmptyBlogWhenNoBlogIsFound)));

            service.GetLatest().Should().BeSameAs(BlogBase.Empty);
        }

        [Fact]
        public void GetLatestShouldReturnLatestBlog()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnLatestBlog));
            context.Blogs.AddRange(
                new Data.Entities.Blog {Created = new DateTime(2017, 1, 1)},
                new Data.Entities.Blog {Created = new DateTime(2013, 1, 1)},
                new Data.Entities.Blog {Created = new DateTime(2017, 6, 20)},
                new Data.Entities.Blog {Created = new DateTime(2014, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().Should().BeEquivalentTo(
                new {Dates = new {Created = new DateTime(2017, 6, 20)}},
                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetShouldReturnBlogWithCreatedDate()
        {
            const int id = 1;

            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetShouldReturnBlogWithCreatedDate));
            context.Blogs.Add(new Data.Entities.Blog { Id = id, Created = new DateTime(2017, 1, 1) });
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Dates = new { Created = testValue } } ,
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithContent()
        {
            const int id = 1;

            var testValue = "test";

            var context = new MockBlogContext(nameof(GetShouldReturnBlogWithContent));
            context.Blogs.Add(new Data.Entities.Blog { Id = id, Content = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Content = testValue },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithModificationDate()
        {
            const int id = 1;

            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetShouldReturnBlogWithModificationDate));
            context.Blogs.Add(new Data.Entities.Blog { Id = id, Modified = new DateTime(2017, 1, 1) });
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Dates = new { Modified = testValue.Some() } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            const int id = 1;

            var context = new MockBlogContext(
                nameof(GetShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull));
            context.Blogs.Add(new Data.Entities.Blog { Id = id, Modified = null });
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Dates = new { Modified = Option.None<DateTime>() } },
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetShouldReturnBlogWithTitle()
        {
            const int id = 1;

            var testValue = "test";

            var context = new MockBlogContext(nameof(GetShouldReturnBlogWithTitle));
            context.Blogs.Add(new Data.Entities.Blog { Id = id, Title = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Title = testValue },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithId()
        {
            const int id = 1;

            var context = new MockBlogContext(nameof(GetShouldReturnBlogWithId));
            context.Blogs.Add(new Data.Entities.Blog { Id = id });
            context.SaveChanges();

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Id = id },
                options => options.ExcludingMissingMembers());
        }
    }
}