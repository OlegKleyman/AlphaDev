using System;
using AlphaDev.Core.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data
{
    public interface IContextFactory<out T> where T : DbContext
    {
        T Create();
    }
}