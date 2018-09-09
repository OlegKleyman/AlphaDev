using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Integration
{
    public class InformationContextTests : IDisposable
    {
        public InformationContextTests()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", false, true).Build();

            var optionsBuilder = new DbContextOptionsBuilder();
            var connectionBuilder =
                new SqlConnectionStringBuilder(configuration.GetConnectionString("integration"))
                {
                    InitialCatalog = Guid.NewGuid().ToString()
                };

            _connectionString = connectionBuilder.ToString();
            _configurer = new SqlConfigurer(connectionBuilder.ToString());
            _configurer.Configure(optionsBuilder);

            using (var context = new DbContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();
            }
        }

        public void Dispose()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(_connectionString).Options;

            using (var context = new DbContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }

        private readonly string _connectionString;
        private readonly SqlConfigurer _configurer;

        private void SeedAbouts()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            _configurer.Configure(optionsBuilder);

            using (var context = new DbContext(optionsBuilder.Options))
            {
                context.Database.ExecuteSqlCommand(
                    $"INSERT INTO Abouts(Id, Value) VALUES(1, 'test')");
            }
        }

        [NotNull]
        private InformationContext GetInformationContext()
        {
            var context = new InformationContext(_configurer);
            context.Database.Migrate();

            return context;
        }

        [ItemNotNull]
        private IEnumerable<IDictionary<string, object>> GetTable(string tableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, i =>
                            {
                                var value = reader.GetValue(i);
                                return value == DBNull.Value ? null : value;
                            });
                }
            }
        }

        [Fact]
        public void AboutsShouldAddAbout()
        {
            using (var context = GetInformationContext())
            {
                var about = new About
                {
                    Value = "test"
                };

                context.Abouts.Add(about);

                context.SaveChanges();

                GetTable("Abouts").Should().HaveCount(1);
            }
        }

        [Fact]
        public void AboutsShouldDeleteAbouts()
        {
            using (var context = GetInformationContext())
            {
                context.Abouts.RemoveRange(context.Abouts);
                context.SaveChanges();

                GetTable("Abouts").Should().BeEmpty();
            }
        }

        [Fact]
        public void AboutsShouldReturnAbouts()
        {
            using (var context = GetInformationContext())
            {
                SeedAbouts();
                var abouts = context.Abouts;

                var aboutsDictionary = abouts.Select(
                    about => about.GetType().GetProperties()
                        .ToDictionary(x => x.Name, x => x.GetGetMethod().Invoke(about, null)));

                GetTable("Abouts").Should().BeEquivalentTo(aboutsDictionary);
            }
        }

        [Fact]
        public void AboutsShouldThrowIfAddingMultipleAbouts()
        {
            using (var context = GetInformationContext())
            {
                SeedAbouts();
                var about = new About
                {
                    Id = false,
                    Value = "test"
                };

                context.Abouts.Add(about);

                Action saveChanges = () => context.SaveChanges();
                saveChanges.Should().Throw<DbUpdateException>().WithInnerException<SqlException>().Which.Message
                    .Should().MatchRegex(
                        @"The INSERT statement conflicted with the CHECK constraint ""CK_ABOUTS_SIZE""\. The conflict occurred in database "".*"", table ""dbo.Abouts""");
            }
        }

        [Fact]
        public void AboutsShouldUpdateAbouts()
        {
            using (var context = GetInformationContext())
            {
                SeedAbouts();
                var targetAbout = context.Abouts.First();

                targetAbout.Value = "Updated Value";

                context.SaveChanges();

                GetTable("Abouts").First().Should().BeEquivalentTo(
                    targetAbout.GetType().GetProperties().ToDictionary(
                        x => x.Name,
                        x => x.GetGetMethod().Invoke(targetAbout, null)));
            }
        }
    }
}