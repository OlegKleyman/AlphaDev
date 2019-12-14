using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Support;
using AlphaDev.Core.Tests.Unit.Extensions.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Optional;
using Optional.Unsafe;
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
        public void AddShouldReturnBlog()
        {
            const string title = "title";
            const string content = "content";

            var blog = new Blog(title, content);
            var blogs = new List<Data.Entities.Blog>();
            var blogsDbSet = blogs.ToMockDbSet().WithAddReturns(blogs);
            
            var service = GetBlogService(blogsDbSet);
            var addedBlog = service.Add(blog);

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
        public void DeleteShouldDeleteBlog()
        {
            var blogsDbSet = new List<Data.Entities.Blog>().ToMockDbSet();
            blogsDbSet.Find(1).Returns(new Data.Entities.Blog
            {
                Id = 1
            });

            var service = GetBlogService(blogsDbSet);
            service.Delete(1);
            blogsDbSet.Received(1).Remove(Arg.Is<Data.Entities.Blog>(blog => blog.Id == 1));
        }

        [Fact]
        public void EditShouldEditBlogTitleAndContentInDataStore()
        {
            var blogs = new List<Data.Entities.Blog>();
            var blogsDbSet = blogs.ToMockDbSet();
            const int id = default;
            var blog = new Data.Entities.Blog();
            blogsDbSet.Find(id).Returns(blog);
            var service = GetBlogService(blogsDbSet);

            service.Edit(id, arguments =>
            {
                arguments.Title = "Title";
                arguments.Content = "Content";
            });

            blog.Should().BeEquivalentTo(new
            {
                Title = "Title",
                Content = "Content"
            });
        }

        [Fact]
        public void EditShouldSetModifiedFromDateProvider()
        {
            var entities = new[] { new Data.Entities.Blog() };
            var blogsDbSet = entities.ToMockDbSet();
            const int id = default;
            var blog = new Data.Entities.Blog();
            blogsDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogsDbSet);
            _dateProvider.UtcNow.Returns(new DateTime(2018, 1, 2));

            service.Edit(id, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            blog.Modified.Should().Be(new DateTime(2018, 1, 2));
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenBlogWasNotFound()
        {
            var service = GetBlogService(new Data.Entities.Blog[0].ToMockDbSet());

            Action edit = () => service.Edit(default, arguments => { });

            edit.Should().Throw<InvalidOperationException>().WithMessage($"Blog {default(int)} was not found.");
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var blogsDbSet = new[]
                {
                    new Data.Entities.Blog
                    {
                        Content = testValue, 
                        Title = string.Empty
                    }
                }.ToMockDbSet();

            var service = GetBlogService(blogsDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Content = testValue },
                       options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var blogDbSet = new[]
            {
                new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty }
            }.ToMockDbSet();
            
            var service = GetBlogService(blogDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Created = testValue } },
                       options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithId()
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
            }.ToMockDbSet();
            
            var service = GetBlogService(blogDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Id = testValue },
                       options => options.Including(info => info.Id));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var blogs = new List<Data.Entities.Blog>
            {
                new Data.Entities.Blog
                {
                    Modified = testValue, Title = string.Empty, Content = string.Empty
                }
            };

            var blogDbSet = blogs.ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Modified = Option.Some(testValue) } },
                       options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var blogs = new List<Data.Entities.Blog>
            {
                new Data.Entities.Blog { Modified = null, Title = string.Empty, Content = string.Empty }
            };

            var blogDbSet = blogs.ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Modified = Option.None<DateTime>() } },
                       options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithTitle()
        {
            const string testValue = "test";
            var blogsDbSet = new List<Data.Entities.Blog>
                { new Data.Entities.Blog { Title = testValue, Content = string.Empty } }.ToMockDbSet();

            var service = GetBlogService(blogsDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Title = testValue },
                       options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnLatestBlog()
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
            }.ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetLatest()
                   .ValueOrFailure()
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Created = new DateTime(2017, 6, 20) } },
                       options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnNoBlogWhenNoBlogIsFound()
        {
            var blogDbSet = new List<Data.Entities.Blog>().ToMockDbSet();
            var service = GetBlogService(blogDbSet);

            service.GetLatest().HasValue.Should().BeFalse();
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogCountBasedOnTheCountArgument()
        {
            var blogDbSet = Enumerable.Range(1, 10)
                                      .Select(x => new Data.Entities.Blog
                                          { Id = x, Title = string.Empty, Content = string.Empty })
                                      .ToArray()
                                      .ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 4)
                   .Should()
                   .HaveCount(4)
                   .And.Subject.Select(x => x.Id)
                   .Should()
                   .BeEquivalentTo(blogDbSet.Take(4).Select(x => x.Id));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsOrderedByModifiedThenByCreatedDateDescending()
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
            }.ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, blogDbSet.Count())
                   .Select(x => x.Dates)
                   .Should()
                   .BeEquivalentTo(blogDbSet.OrderByDescending(x => x.Modified)
                                           .ThenByDescending(x => x.Created)
                                           .Select(x => new { x.Created, Modified = x.Modified.ToOption() }));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsStartingFromTheStartArgument()
        {
            var blogDbSet = Enumerable.Range(1, 10)
                                      .Select(x => new Data.Entities.Blog
                                          { Id = x, Title = string.Empty, Content = string.Empty })
                                      .ToArray()
                                      .ToMockDbSet();

            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(7, 10)
                   .Should()
                   .HaveCount(4)
                   .And.Subject.Select(x => x.Id)
                   .Should()
                   .BeEquivalentTo(blogDbSet.Skip(6).Select(x => x.Id));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1)
                   .Should()
                   .BeEquivalentTo(
                       new[] { new { Dates = new { Created = testValue } } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var blogDbSet = new[]
                { new Data.Entities.Blog { Modified = testValue, Title = string.Empty, Content = string.Empty } }
                .ToMockDbSet();
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1)
                   .Should()
                   .BeEquivalentTo(
                       new[] { new { Dates = new { Modified = Option.Some(testValue) } } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var blogDbSet = new[] { new Data.Entities.Blog { Title = string.Empty, Content = string.Empty } }.ToMockDbSet();
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1)
                   .Should()
                   .BeEquivalentTo(
                       new[] { new { Dates = new { Modified = Option.None<DateTime>() } } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithTitle()
        {
            const string testValue = "test";

            var blogDbSet = new[]
                { new Data.Entities.Blog { Title = testValue, Content = string.Empty } }
                .ToMockDbSet();
            
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1)
                   .Should()
                   .BeEquivalentTo(
                       new[] { new { Title = testValue } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var blogDbSet = new[] { new Data.Entities.Blog { Title = string.Empty, Content = testValue } }.ToMockDbSet();
            
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1)
                   .Should()
                   .BeEquivalentTo(
                       new[] { new { Content = "test content" } },
                       options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnEmptyBlogsWhenNoBlogIsFound()
        {
            var blogDbSet = new Data.Entities.Blog[0].ToMockDbSet();
            var service = GetBlogService(blogDbSet);

            service.GetOrderedByDates(1, 1).Should().BeEmpty();
        }

        [Fact]
        public void GetShouldReturnBlogWithContent()
        {
            const int id = default;

            var testValue = "test";

            var blog = new Data.Entities.Blog { Content = testValue, Title = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);
            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Content = testValue },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithCreatedDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Created = testValue } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithId()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Id = id, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Id = id },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithModificationDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Modified = testValue, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Modified = testValue.Some() } },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Modified = null, Title = string.Empty, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Dates = new { Modified = Option.None<DateTime>() } },
                       options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetShouldReturnBlogWithTitle()
        {
            const int id = 1;

            var testValue = "test";

            var blog = new Data.Entities.Blog { Title = testValue, Content = string.Empty };
            var blogDbSet = new[] { blog }.ToMockDbSet();
            blogDbSet.Find(id).Returns(blog);

            var service = GetBlogService(blogDbSet);

            service.Get(id)
                   .ValueOr(BlogBase.Empty)
                   .Should()
                   .BeEquivalentTo(
                       new { Title = testValue },
                       options => options.ExcludingMissingMembers());
        }
    }
}