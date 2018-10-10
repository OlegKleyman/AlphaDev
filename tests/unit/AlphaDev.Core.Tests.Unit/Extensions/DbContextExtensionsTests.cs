using System;
using System.Collections.Generic;
using AlphaDev.Core.Extensions;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class DbContextExtensionsTests
    {
        private class TestEntity
        {
            public bool Field { get; set; }
        }

        [Fact]
        public void AddAndSaveSingleOrThrowShouldAddSingleEntity()
        {
            var context = Substitute.For<DbContext>();
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            context.SaveChanges().Returns(1);
            var mocks = new List<object>();
            var mockSet = mocks.ToMockDbSet();
            var target = new object();
            context.AddAndSaveSingleOrThrow(x => mockSet, target);
            mockSet.Should().ContainSingle().Which.Should().BeSameAs(target);
        }

        [Fact]
        public void AddAndSaveSingleOrThrowShouldThrowInvalidOperationExceptionWhenAddingEntityReturnsNull()
        {
            var context = Substitute.For<DbContext>();
            var mocks = new List<object>();
            var mockSet = mocks.ToMockDbSet();
            mockSet.Add(Arg.Any<object>()).Returns(default(EntityEntry<object>));
            Action addAndSaveSingleOrThrow = () => context.AddAndSaveSingleOrThrow(x => mockSet, new object());
            addAndSaveSingleOrThrow.Should().Throw<InvalidOperationException>()
                .WithMessage("Unable to retrieve added entry.");
        }

        [Fact]
        public void SaveSingleOrThrowShouldCommitTransaction()
        {
            var context = Substitute.For<DbContext>();
            var databaseFacade = Substitute.For<DatabaseFacade>(context);
            var dbContextTransaction = Substitute.For<IDbContextTransaction>();
            databaseFacade.BeginTransaction().Returns(dbContextTransaction);
            context.Database.Returns(databaseFacade);
            context.SaveChanges().Returns(1);
            context.SaveSingleOrThrow();
            dbContextTransaction.Received(1).Commit();
        }

        [Fact]
        public void SaveSingleOrThrowShouldCommitTransactionWhenNoException()
        {
            var context = Substitute.For<DbContext>();
            var databaseFacade = Substitute.For<DatabaseFacade>(context);
            var transaction = Substitute.For<IDbContextTransaction>();
            databaseFacade.BeginTransaction().Returns(transaction);
            context.Database.Returns(databaseFacade);
            context.SaveChanges().Returns(1);
            context.SaveSingleOrThrow();
            transaction.Received(1).Commit();
        }

        [Fact]
        public void SaveSingleOrThrowShouldSaveOne()
        {
            var context = Substitute.For<DbContext>();
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            context.SaveChanges().Returns(1);
            context.SaveSingleOrThrow();
            context.Received(1).SaveChanges();
        }

        [Fact]
        public void SaveSingleOrThrowShouldStartTransaction()
        {
            var context = Substitute.For<DbContext>();
            var databaseFacade = Substitute.For<DatabaseFacade>(context);
            context.Database.Returns(databaseFacade);
            context.SaveChanges().Returns(1);
            context.SaveSingleOrThrow();
            databaseFacade.Received(1).BeginTransaction();
        }

        [Fact]
        public void SaveSingleOrThrowShouldThrowInvalidOperationExceptionWhenSavingDoesNotReturnChangeCountOfOne()
        {
            var context = Substitute.For<DbContext>();
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            context.SaveChanges().Returns(0);
            Action saveSingleOrThrow = () => context.SaveSingleOrThrow();
            saveSingleOrThrow.Should().Throw<InvalidOperationException>()
                .WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void UpdateAndSaveSingleOrThrowShouldThrowInvalidOperationExceptionWhenEntityIsNull()
        {
            var context = Substitute.For<DbContext>();
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            context.SaveChanges().Returns(1);
            Action updateAndSaveSingleOrThrow =
                () => context.UpdateAndSaveSingleOrThrow(x => (TestEntity) null, default);
            updateAndSaveSingleOrThrow.Should().Throw<InvalidOperationException>()
                .WithMessage("TestEntity not found.");
        }

        [Fact]
        public void UpdateAndSaveSingleOrThrowShouldUpdateEntity()
        {
            var context = Substitute.For<DbContext>();
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            context.SaveChanges().Returns(1);
            var entity = new TestEntity { Field = false };
            context.UpdateAndSaveSingleOrThrow(x => entity, obj => entity.Field = true);
            entity.Field.Should().BeTrue();
        }
    }
}