using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class AlphaContext : DbContext
    {
        private readonly Configurer _configurer;

        protected AlphaContext(Configurer configurer)
        {
            _configurer = configurer;
        }

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            _configurer.Configure(optionsBuilder);
        }
    }
}