using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.Test.Core
{
    public static class ObjectExtensions
    {
        public static void EmptyCall(this object target)
        {
        }
    }
}