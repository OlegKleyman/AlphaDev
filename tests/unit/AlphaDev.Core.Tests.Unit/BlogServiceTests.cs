using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.EntityFramework.Unit.Testing.Extensions;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class BlogServiceTests
    {
        public BlogServiceTests() => _dateProvider = Substitute.For<IDateProvider>();

        private readonly IDateProvider _dateProvider;

        [NotNull]
        private BlogService GetBlogService([NotNull] DbSet<Data.Entities.Blog> blogs) =>
            new BlogService(blogs, _dateProvider);

        [Fact]
        public async Task AddShouldReturnBlog()
        {
            const string title = "title";
            const string content = "content";

            var blog = new Blog(title, content);
            var blogsDbSet = Enumerable.Empty<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();
            blogsDbSet.AddAsync(Arg.Is<Data.Entities.Blog>(b => b.Title == title && b.Content == content))
                      .Returns(info => info.Arg<Data.Entities.Blog>().ToMockEntityEntry());
            var service = GetBlogService(blogsDbSet);
            var addedBlog = await service.AddAsync(blog);

            addedBlog.Should()
                     .BeEquivalentTo(new
                     {
                         Id = 0,
                         Title = title,
                         Content = content,
                         Dates = new { Created = default(DateTime), Modified = Option.None<DateTime>() }
                     }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task DeleteAsyncShouldReturnObjectNotFoundExceptionWhenBlogIsNotFound()
        {
            var blogsDbSet = new List<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();

            var result = await GetBlogService(blogsDbSet).DeleteAsync(1);

            result.Should()
                  .BeNone()
                  .Which.Should()
                  .BeEquivalentTo(new
                  {
                      Type = typeof(BlogBase),
                      Values = new Dictionary<string, object[]>
                      {
                          ["Id"] = new object[] { 1 }
                      }
                  });
        }

        [Fact]
        public async Task DeleteShouldDeleteBlog()
        {
            var blogsDbSet = new List<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();
            blogsDbSet.FindAsync(1)
                      .Returns(new Data.Entities.Blog
                      {
                          Id = 1
                      });

            var service = GetBlogService(blogsDbSet);
            await service.DeleteAsync(1);
            blogsDbSet.Received(1).Remove(Arg.Is<Data.Entities.Blog>(blog => blog.Id == 1));
        }

        [Fact]
        public async Task DeleteShouldReturnSomeWhenBlogIsDeleted()
        {
            var blogsDbSet = new List<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();
            blogsDbSet.FindAsync(1)
                      .Returns(new Data.Entities.Blog
                      {
                          Id = 1
                      });

            var result = await GetBlogService(blogsDbSet).DeleteAsync(1);
            result.Should().HaveSome().Which.Should().Be(Core.Unit.Value);
        }

        [Fact]
        public async Task EditAsyncShouldReturnObjectNotFoundExceptionWhenBlogWasNotFound()
        {
            var service = GetBlogService(new Data.Entities.Blog[0].AsQueryable().BuildMockDbSet());

            var result = await service.EditAsync(1, arguments => { });

            result.Should()
                  .BeNone()
                  .Which.Should()
                  .BeEquivalentTo(new
                  {
                      Type = typeof(BlogBase),
                      Values = new Dictionary<string, object[]>
                      {
                          ["Id"] = new object[] { 1 }
                      }
                  });
        }

        [Fact]
        public async Task EditShouldEditBlogTitleAndContentInDataStore()
        {
            var blogs = new List<Data.Entities.Blog>();
            var blogsDbSet = blogs.AsQueryable().BuildMockDbSet();
            const int id = default;
            var blog = new Data.Entities.Blog();
            blogsDbSet.FindAsync(id).Returns(blog);
            var service = GetBlogService(blogsDbSet);

            await service.EditAsync(id, arguments =>
            {
                arguments.Title = "Title";
                arguments.Content = "Content";
            });

            blog.Should()
                .BeEquivalentTo(new
                {
                    Title = "Title",
                    Content = "Content"
                });
        }

        [Fact]
        public async Task EditShouldReturnSomeWhenBlogIsEditedSuccessfully()
        {
            var blogsDbSet = Enumerable.Empty<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();
            const int id = 1;
            blogsDbSet.FindAsync(id).Returns(new Data.Entities.Blog());

            var service = GetBlogService(blogsDbSet);

            var result = await service.EditAsync(id, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            result.Should().HaveSome();
        }

        [Fact]
        public async Task EditShouldSetModifiedFromDateProvider()
        {
            var entities = new[] { new Data.Entities.Blog() };
            var blogsDbSet = entities.AsQueryable().BuildMockDbSet();
            const int id = default;
            var blog = new Data.Entities.Blog();
            blogsDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogsDbSet);
            _dateProvider.UtcNow.Returns(new DateTime(2018, 1, 2));

            await service.EditAsync(id, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            blog.Modified.Should().Be(new DateTime(2018, 1, 2));
        }

        [Fact]
        public async Task GetCountAsyncReturnsCountOfAllBlogs()
        {
            var blogDbSet = Enumerable.Range(1, 10)
                                      .Select(i => new Data.Entities.Blog())
                                      .AsQueryable()
                                      .BuildMockDbSet();
            var service = GetBlogService(blogDbSet);
            var result = await service.GetCountAsync();
            result.Should().Be(10);
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var blogsDbSet = new[]
                {
                    new Data.Entities.Blog
                    {
                        Content = testValue,
                        Title = string.Empty
                    }
                }.AsQueryable()
                 .BuildMockDbSet();

            var service = GetBlogService(blogsDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Content = testValue },
                                                options => options.Including(info => info.Content));
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var blogDbSet = new[]
                {
                    new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty }
                }.AsQueryable()
                 .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Dates = new { Created = testValue } },
                                                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithId()
        {
            const int testValue = 1;
            var blogDbSet = new[]
                {
                    new Data.Entities.Blog
                    {
                        Id = testValue,
                        Title = string.Empty,
                        Content = string.Empty
                    }
                }.AsQueryable()
                 .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Id = testValue },
                                                options => options.Including(info => info.Id));
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var blogs = new List<Data.Entities.Blog>
            {
                new Data.Entities.Blog
                {
                    Modified = testValue, Title = string.Empty, Content = string.Empty
                }
            };

            var blogDbSet = blogs.AsQueryable().BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Dates = new { Modified = Option.Some(testValue) } },
                                                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var blogs = new List<Data.Entities.Blog>
            {
                new Data.Entities.Blog { Modified = null, Title = string.Empty, Content = string.Empty }
            };

            var blogDbSet = blogs.AsQueryable().BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Dates = new { Modified = Option.None<DateTime>() } },
                                                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public async Task GetLatestShouldReturnBlogWithTitle()
        {
            const string testValue = "test";
            var blogsDbSet = new List<Data.Entities.Blog>
                    { new Data.Entities.Blog { Title = testValue, Content = string.Empty } }.AsQueryable()
                                                                                            .BuildMockDbSet();

            var service = GetBlogService(blogsDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Title = testValue },
                                                options => options.Including(info => info.Title));
        }

        [Fact]
        public async Task GetLatestShouldReturnLatestBlog()
        {
            var blogDbSet = new List<Data.Entities.Blog>
                {
                    new Data.Entities.Blog
                        { Created = new DateTime(2017, 1, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2013, 1, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2017, 6, 20), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2014, 1, 1), Title = string.Empty, Content = string.Empty }
                }.AsQueryable()
                 .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should()
                                            .HaveSome()
                                            .Which
                                            .Should()
                                            .BeEquivalentTo(
                                                new { Dates = new { Created = new DateTime(2017, 6, 20) } },
                                                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public async Task GetLatestShouldReturnNoBlogWhenNoBlogIsFound()
        {
            var blogDbSet = new List<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();
            var service = GetBlogService(blogDbSet);

            (await service.GetLatestAsync()).Should().BeNone();
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogCountBasedOnTheCountArgument()
        {
            var blogDbSet = Enumerable.Range(1, 10)
                                      .Select(x => new Data.Entities.Blog
                                          { Id = x, Title = string.Empty, Content = string.Empty })
                                      .ToArray()
                                      .AsQueryable()
                                      .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 4))
                .Should()
                .HaveCount(4)
                .And.Subject.Select(x => x.Id)
                .Should()
                .BeEquivalentTo(Queryable.Take(blogDbSet, 4).Select(x => x.Id));
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsOrderedByModifiedThenByCreatedDateDescending()
        {
            var blogDbSet = new[]
                {
                    new Data.Entities.Blog
                        { Created = new DateTime(2010, 5, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2010, 7, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2010, 4, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                    {
                        Created = new DateTime(2010, 1, 1), Modified = new DateTime(2011, 1, 1), Title = string.Empty,
                        Content = string.Empty
                    },
                    new Data.Entities.Blog
                        { Created = new DateTime(2010, 1, 1), Title = string.Empty, Content = string.Empty },
                    new Data.Entities.Blog
                        { Created = new DateTime(2010, 11, 1), Title = string.Empty, Content = string.Empty }
                }.AsQueryable()
                 .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, blogDbSet.Count()))
                .Select(x => x.Dates)
                .Should()
                .BeEquivalentTo(Queryable.OrderByDescending(blogDbSet, x => x.Modified)
                                         .ThenByDescending(x => x.Created)
                                         .Select(x => new { x.Created, Modified = x.Modified.ToOption() }));
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsStartingFromTheStartArgument()
        {
            var blogDbSet = Enumerable.Range(1, 10)
                                      .Select(x => new Data.Entities.Blog
                                          { Id = x, Title = string.Empty, Content = string.Empty })
                                      .ToArray()
                                      .AsQueryable()
                                      .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(7, 10))
                .Should()
                .HaveCount(4)
                .And.Subject.Select(x => x.Id)
                .Should()
                .BeEquivalentTo(Queryable.Skip(blogDbSet, 6).Select(x => x.Id));
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1))
                .Should()
                .BeEquivalentTo(
                    new[] { new { Dates = new { Created = testValue } } },
                    options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var blogDbSet = new[]
                            {
                                new Data.Entities.Blog
                                    { Modified = testValue, Title = string.Empty, Content = string.Empty }
                            }
                            .AsQueryable()
                            .BuildMockDbSet();
            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1))
                .Should()
                .BeEquivalentTo(
                    new[] { new { Dates = new { Modified = Option.Some(testValue) } } },
                    options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var blogDbSet = new[] { new Data.Entities.Blog { Title = string.Empty, Content = string.Empty } }
                            .AsQueryable()
                            .BuildMockDbSet();
            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1))
                .Should()
                .BeEquivalentTo(
                    new[] { new { Dates = new { Modified = Option.None<DateTime>() } } },
                    options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogsWithTitle()
        {
            const string testValue = "test";

            var blogDbSet = new[]
                                { new Data.Entities.Blog { Title = testValue, Content = string.Empty } }
                            .AsQueryable()
                            .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1))
                .Should()
                .BeEquivalentTo(
                    new[] { new { Title = testValue } },
                    options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var blogDbSet = new[] { new Data.Entities.Blog { Title = string.Empty, Content = testValue } }
                            .AsQueryable()
                            .BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1))
                .Should()
                .BeEquivalentTo(
                    new[] { new { Content = "test content" } },
                    options => options.Including(info => info.Content));
        }

        [Fact]
        public async Task GetOrderedByDatesShouldReturnEmptyBlogsWhenNoBlogIsFound()
        {
            var blogDbSet = new Data.Entities.Blog[0].AsQueryable().BuildMockDbSet();
            var service = GetBlogService(blogDbSet);

            (await service.GetOrderedByDatesAsync(1, 1)).Should().BeEmpty();
        }

        [Fact]
        public async Task GetShouldReturnBlogWithContent()
        {
            const int id = default;

            var testValue = "test";

            var blog = new Data.Entities.Blog { Content = testValue, Title = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);
            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Content = testValue },
                                            options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetShouldReturnBlogWithCreatedDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Dates = new { Created = testValue } },
                                            options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetShouldReturnBlogWithId()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Id = id, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Id = id },
                                            options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetShouldReturnBlogWithModificationDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Modified = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Dates = new { Modified = testValue.Some() } },
                                            options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Modified = null, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Dates = new { Modified = Option.None<DateTime>() } },
                                            options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public async Task GetShouldReturnBlogWithTitle()
        {
            const int id = 1;

            var testValue = "test";

            var blog = new Data.Entities.Blog { Title = testValue, Content = string.Empty };
            var blogDbSet = new[] { blog }.AsQueryable().BuildMockDbSet();
            blogDbSet.FindAsync(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(id)).Should()
                                        .HaveSome()
                                        .Which.Should()
                                        .BeEquivalentTo(
                                            new { Title = testValue },
                                            options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetShouldReturnNoneWhenBlogIsNotFound()
        {
            var blogDbSet = Enumerable.Empty<Data.Entities.Blog>().AsQueryable().BuildMockDbSet();

            var service = GetBlogService(blogDbSet);

            (await service.GetAsync(default)).Should().BeNone();
        }
    }
}