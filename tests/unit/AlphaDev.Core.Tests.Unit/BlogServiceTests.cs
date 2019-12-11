﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Support;
using AlphaDev.Core.Tests.Unit.Extensions.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using NSubstitute;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class BlogServiceTests
    {
        public BlogServiceTests()
        {
            _dateProvider = Substitute.For<IDateProvider>();
        }

        private readonly IDateProvider _dateProvider;

        [NotNull]
        private BlogService GetBlogService([NotNull] BlogContext context)
        {
            var factory = Substitute.For<IContextFactory<BlogContext>>();
            factory.Create().Returns(context);
            return new BlogService(factory, _dateProvider);
        }

        [Fact]
        public void AddShouldReturnBlog()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock();
            context.SaveChanges().Returns(1);

            var service = GetBlogService(context);

            const string title = "title";
            const string content = "content";

            var blog = new Blog(title, content);
            var blogs = new List<Data.Entities.Blog>();
            context.Blogs = blogs.ToMockDbSet().WithAddReturns(blogs);
            var addedBlog = service.Add(blog);

            addedBlog.Should().BeEquivalentTo(new
            {
                Id = 0,
                Title = title,
                Content = content,
                Dates = new { Created = default(DateTime), Modified = Option.None<DateTime>() }
            }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void AddShouldShouldThrowInvalidOperationExceptionWhenUnableToAddBlog()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock();
            var blogs = new List<Data.Entities.Blog>();
            context.Blogs = blogs.ToMockDbSet().WithAddReturns(blogs);
            context.SaveChanges().Returns(0);

            var service = GetBlogService(context);

            Action add = () => service.Add(new Blog(string.Empty, string.Empty));

            add.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void DeleteShouldDeleteBlog()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .With<BlogContext, Data.Entities.Blog>();
            context.SaveChanges().Returns(1);
            var service = GetBlogService(context);
            service.Delete(1);
            context.Received(1).Remove(Arg.Is<Data.Entities.Blog>(blog => blog.Id == 1));
        }

        [Fact]
        public void EditShouldEditBlogTitleAndContentInDataStore()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock();
            context.SaveChanges().Returns(1);
            var blogs = new List<Data.Entities.Blog>();
            context.Blogs = blogs.ToMockDbSet();
            const int id = default;
            context.Blogs.Find(id).Returns(new Data.Entities.Blog());
            var service = GetBlogService(context);

            service.Edit(id, arguments =>
            {
                arguments.Title = string.Empty;
                arguments.Content = string.Empty;
            });

            context.Database.BeginTransaction().Received(1).Commit();
        }

        [Fact]
        public void EditShouldSetModifiedFromDateProvider()
        {
            var entities = new[] { new Data.Entities.Blog() };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock().SuccessSingle()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            const int id = default;
            var blog = new Data.Entities.Blog();
            context.Blogs.Find(id).Returns(blog);

            var service = GetBlogService(context);
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
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(new Data.Entities.Blog[0], (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            Action edit = () => service.Edit(default, arguments => { });

            edit.Should().Throw<InvalidOperationException>().WithMessage("Blog not found.");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenBlogWasNotModified()
        {
            var entities = new[] { new Data.Entities.Blog() };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(Arg.Any<int>()).Returns(new Data.Entities.Blog());
            var service = GetBlogService(context);

            Action edit = () => service.Edit(default, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            edit.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var entities = new[] { new Data.Entities.Blog { Content = testValue, Title = string.Empty } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Content = testValue },
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var entities = new[]
                { new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Dates = new { Created = testValue } },
                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithId()
        {
            const int testValue = 1;
            var entities = new[]
                { new Data.Entities.Blog { Id = testValue, Title = string.Empty, Content = string.Empty } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Id = testValue },
                options => options.Including(info => info.Id));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>());
            var blogs = new List<Data.Entities.Blog>
                { new Data.Entities.Blog { Modified = testValue, Title = string.Empty, Content = string.Empty } };
            context.Blogs = blogs.ToMockDbSet();

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Dates = new { Modified = Option.Some(testValue) } },
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>());
            var blogs = new List<Data.Entities.Blog>
                { new Data.Entities.Blog { Modified = null, Title = string.Empty, Content = string.Empty } };
            context.Blogs = blogs.ToMockDbSet();

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Dates = new { Modified = Option.None<DateTime>() } },
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithTitle()
        {
            const string testValue = "test";
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>());
            var blogs = new List<Data.Entities.Blog>
                { new Data.Entities.Blog { Title = testValue, Content = string.Empty } };
            context.Blogs = blogs.ToMockDbSet();

            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Title = testValue },
                options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnLatestBlog()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>());
            var blogs = new List<Data.Entities.Blog>
            {
                new Data.Entities.Blog
                    { Created = new DateTime(2017, 1, 1), Title = string.Empty, Content = string.Empty },
                new Data.Entities.Blog
                    { Created = new DateTime(2013, 1, 1), Title = string.Empty, Content = string.Empty },
                new Data.Entities.Blog
                    { Created = new DateTime(2017, 6, 20), Title = string.Empty, Content = string.Empty },
                new Data.Entities.Blog
                    { Created = new DateTime(2014, 1, 1), Title = string.Empty, Content = string.Empty }
            };
            
            context.Blogs = blogs.ToMockDbSet();
            var service = GetBlogService(context);

            service.GetLatest().ValueOrFailure().Should().BeEquivalentTo(
                new { Dates = new { Created = new DateTime(2017, 6, 20) } },
                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnNoBlogWhenNoBlogIsFound()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>());
            context.Blogs = new List<Data.Entities.Blog>().ToMockDbSet();
            var service = GetBlogService(context);

            service.GetLatest().HasValue.Should().BeFalse();
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogCountBasedOnTheCountArgument()
        {
            var entities = Enumerable.Range(1, 10)
                                     .Select(x => new Data.Entities.Blog
                                         { Id = x, Title = string.Empty, Content = string.Empty })
                                     .ToArray();

            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 4).Should().HaveCount(4).And.Subject.Select(x => x.Id).Should()
                .BeEquivalentTo(entities.Take(4).Select(x => x.Id));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsOrderedByModifiedThenByCreatedDateDescending()
        {
            var entities = new[]
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
            };

            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, entities.Length).Select(x => x.Dates).Should()
                .BeEquivalentTo(entities.OrderByDescending(x => x.Modified).ThenByDescending(x => x.Created)
                    .Select(x => new { x.Created, Modified = x.Modified.ToOption() }));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsStartingFromTheStartArgument()
        {
            var entities = Enumerable.Range(1, 10).Select(x => new Data.Entities.Blog { Id = x }).ToArray();

            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(7, 10).Should().HaveCount(4).And.Subject.Select(x => x.Id).Should()
                .BeEquivalentTo(entities.Skip(6).Select(x => x.Id));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue, Title = string.Empty, Content = string.Empty };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEquivalentTo(
                new[] { new { Dates = new { Created = testValue } } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var entities = new[] { new Data.Entities.Blog { Modified = testValue } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEquivalentTo(
                new[] { new { Dates = new { Modified = Option.Some(testValue) } } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var entities = new[] { new Data.Entities.Blog{ Title = string.Empty, Content = string.Empty } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEquivalentTo(
                new[] { new { Dates = new { Modified = Option.None<DateTime>() } } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogsWithTitle()
        {
            const string testValue = "test";

            var entities = new[] { new Data.Entities.Blog { Title = testValue } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEquivalentTo(
                new[] { new { Title = testValue } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var entities = new[] { new Data.Entities.Blog { Title = string.Empty, Content = testValue } };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);

            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEquivalentTo(
                new[] { new { Content = "test content" } },
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetOrderedByDatesShouldReturnEmptyBlogsWhenNoBlogIsFound()
        {
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(new Data.Entities.Blog[0], (blogContext, set) => blogContext.Blogs = set);
            var service = GetBlogService(context);

            service.GetOrderedByDates(1, 1).Should().BeEmpty();
        }

        [Fact]
        public void GetShouldReturnBlogWithContent()
        {
            const int id = default;

            var testValue = "test";

            var blog = new Data.Entities.Blog { Content = testValue, Title = string.Empty};
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);
            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Content = testValue },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithCreatedDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Created = testValue };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Dates = new { Created = testValue } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithId()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Id = id, Title = string.Empty, Content = string.Empty };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Id = id },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithModificationDate()
        {
            const int id = default;

            var testValue = new DateTime(2017, 1, 1);

            var blog = new Data.Entities.Blog { Modified = testValue };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Dates = new { Modified = testValue.Some() } },
                options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            const int id = 1;

            var blog = new Data.Entities.Blog { Modified = null };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);

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

            var blog = new Data.Entities.Blog { Title = testValue };
            var entities = new[] { blog };
            var context = Substitute.For<BlogContext>(Substitute.For<Configurer>()).Mock()
                .WithDbSet(entities, (blogContext, set) => blogContext.Blogs = set);
            context.Blogs.Find(id).Returns(blog);

            var service = GetBlogService(context);

            service.Get(id).ValueOr(BlogBase.Empty).Should().BeEquivalentTo(
                new { Title = testValue },
                options => options.ExcludingMissingMembers());
        }
    }
}