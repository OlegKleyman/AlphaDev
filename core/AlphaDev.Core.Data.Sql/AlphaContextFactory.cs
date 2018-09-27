using System;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlphaDev.Core.Data.Sql
{
    public abstract class AlphaContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {
        private static readonly SqlConfigurer Configurer = new SqlConfigurer(@"Data Source=(LocalDB)\v11.0;");
        private readonly Configurer _configurer;

        protected AlphaContextFactory(Configurer configurer)
        {
            _configurer = configurer;
        }

        [NotNull]
        public T CreateDbContext([CanBeNull] params string[] args)
        {
            var type = typeof(T);
            var configurerType = typeof(Configurer);

            if (type.IsAbstract)
                throw new InvalidOperationException($"Unable to instantiate {type.FullName} because it is abstract.");

            if (type.GetConstructor(new[] { configurerType }) == null)
                throw new MissingMethodException(
                    $"Unable to find a public constructor with a single {configurerType.FullName} argument.");

            return (T) Activator.CreateInstance(typeof(T), _configurer ?? Configurer);
        }
    }
}