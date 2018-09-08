using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Support
{
    public abstract class Configurer
    {
        public abstract void Configure(DbContextOptionsBuilder optionsBuilder);
    }
}