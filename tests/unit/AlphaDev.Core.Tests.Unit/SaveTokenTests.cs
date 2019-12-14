using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Collections.Extensions;
using AlphaDev.Core.Tests.Unit.Extensions;
using AlphaDev.Test.Core.Support;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class SaveTokenTests
    {
        [Fact]
        public async Task SaveAsyncExecutesAllSaveActions()
        {
            SaveAction GetSaveAction(Switch<SaveAction> x) => () => Task.Run(() => x.On = true);
            var switches = Enumerable.Range(1, 10)
                                     .Select(i => new Switch<SaveAction>(GetSaveAction))
                                     .ToArray();

            // ReSharper disable once CoVariantArrayConversion - Can combine delegates of the same type
            var saveAction = switches.Select(x => x.Target).Combine() ??
                             throw new InvalidOperationException("Delegate is null.");
            
            var token = new SaveToken(saveAction);
            await token.SaveAsync();
            switches.Select(x => x.On).Should().AllBeEquivalentTo(true);
        }
    }
}
