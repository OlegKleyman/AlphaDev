using AlphaDev.Test.Core.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;

namespace AlphaDev.Test.Core.Extensions
{
    public static class ObjectExtensions
    {
#pragma warning disable IDE0060 // Remove unused parameter - deliberately unused for testing
        public static void EmptyCall(this object target)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }
    }
}