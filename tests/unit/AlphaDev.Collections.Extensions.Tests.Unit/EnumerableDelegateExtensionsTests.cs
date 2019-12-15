using System;
using System.Linq;
using AlphaDev.Test.Core.Support;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Collections.Extensions.Tests.Unit
{
    public class EnumerableDelegateExtensionsTests
    {
        [Fact]
        public void CombineCombinesDelegatesIntoOne()
        {
            var actionSwitch1 = new Switch<Action>(x => () => x.On = true);
            var actionSwitch2 = new Switch<Action>(x => () => x.On = true);

            new[] { actionSwitch1.Target, actionSwitch2.Target }.Combine()?.Invoke();
            actionSwitch1.On.Should().BeTrue();
            actionSwitch2.On.Should().BeTrue();
        }

        [Fact]
        public void CombineCombinesNoneNullDelegatesIntoOneWhenEnumerableContainsNullElements()
        {
            var actionSwitch1 = new Switch<Action>(x => () => x.On = true);
            var actionSwitch2 = new Switch<Action>(x => () => x.On = true);

            new[] { actionSwitch1.Target, null, actionSwitch2.Target }.Combine()?.Invoke();
            actionSwitch1.On.Should().BeTrue();
            actionSwitch2.On.Should().BeTrue();
        }

        [Fact]
        public void CombineReturnsNullWhenAllElementsOfEnumerableAreNull()
        {
            Enumerable.Range(1, 10).Select(i => (Action?) null).Combine().Should().BeNull();
        }

        [Fact]
        public void CombineReturnsNullWhenEnumerableIsEmpty()
        {
            Enumerable.Empty<Action>().Combine().Should().BeNull();
        }
    }
}