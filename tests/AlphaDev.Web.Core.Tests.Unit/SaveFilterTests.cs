using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AlphaDev.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit
{
    public class SaveFilterTests
    {
        [Fact]
        public async Task OnActionExecutionAsyncExecutesNext()
        {
            var filter = new SaveFilter(Substitute.For<ISaveToken>());
            var context = (ActionExecutingContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutingContext));
            var executedContext = (ActionExecutedContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutedContext));
            var wasCalled = false;
            await filter.OnActionExecutionAsync(context, () =>
            {
                wasCalled = true;
                return Task.FromResult(executedContext);
            });

            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        public async Task OnActionExecutionAsyncSavesTokenWhenExecutedContextOnlyWhenExceptionConditionsAreMet(bool exceptionOccured, bool exceptionHandled, bool saved)
        {
            var saveToken = Substitute.For<ISaveToken>();
            var filter = new SaveFilter(saveToken);
            var context = (ActionExecutingContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutingContext));
            var executedContext = (ActionExecutedContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutedContext));
            executedContext.Exception = exceptionOccured ? new Exception() : null;
            executedContext.ExceptionHandled = exceptionHandled;
            await filter.OnActionExecutionAsync(context, () => Task.FromResult(executedContext));

            saveToken.When(token => token.SaveAsync()).Do(info => saved.Should().BeTrue());
        }
    }
}
