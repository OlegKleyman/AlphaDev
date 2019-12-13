using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AlphaDev.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit
{
    public class SaveFilterAttributeTests
    {
        [Fact]
        public async Task CreateInstanceCreatesAnInstanceOfSaveFilterUsingTheSaveTokenDependency()
        {
            var saveToken = Substitute.For<ISaveToken>();
            var context = (ActionExecutingContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutingContext));
            var executedContext = (ActionExecutedContext)FormatterServices.GetSafeUninitializedObject(typeof(ActionExecutedContext));

            await new SaveFilterAttribute
                {
                    Arguments = new object[] { saveToken }
                }.CreateInstance(Substitute.For<IServiceProvider>())
                 .Should()
                 .BeOfType<SaveFilter>()
                 .Which.OnActionExecutionAsync(context, () => Task.FromResult(executedContext));

            await saveToken.Received().SaveAsync();
        }
    }
}
