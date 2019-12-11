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
using Microsoft.Data.SqlClient;
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
                context.Database.ExecuteSqlRaw(
                    $"INSERT INTO Abouts(Id, Value) VALUES(1, 'test')");
            }
        }

        private void SeedContacts()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            _configurer.Configure(optionsBuilder);

            using (var context = new DbContext(optionsBuilder.Options))
            {
                context.Database.ExecuteSqlRaw(
                    $"INSERT INTO Contacts(Id, Value) VALUES(1, 'test')");
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
                    {
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, i =>
                            {
                                var value = reader.GetValue(i);
                                return value == DBNull.Value ? null : value;
                            });
                    }
                }
            }
        }

        [Fact]
        public void AboutShouldReturnAbout()
        {
            using (var context = GetInformationContext())
            {
                SeedAbouts();

                context.About.Should().BeEquivalentTo(new { Id = 1, Value = "test" });
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

                var aboutsDictionary = abouts.AsEnumerable().Select(
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

        [Fact]
        public void ContactShouldReturnContact()
        {
            using (var context = GetInformationContext())
            {
                SeedContacts();

                context.Contact.Should().BeEquivalentTo(new { Id = 1, Value = "test" });
            }
        }

        [Fact]
        public void ContactsShouldAddContact()
        {
            using (var context = GetInformationContext())
            {
                var contact = new Contact
                {
                    Value = "test"
                };

                context.Contacts.Add(contact);

                context.SaveChanges();

                GetTable("Contacts").Should().HaveCount(1);
            }
        }

        [Fact]
        public void ContactsShouldDeleteContacts()
        {
            using (var context = GetInformationContext())
            {
                context.Contacts.RemoveRange(context.Contacts);
                context.SaveChanges();

                GetTable("Contacts").Should().BeEmpty();
            }
        }

        [Fact]
        public void ContactsShouldReturnContacts()
        {
            using (var context = GetInformationContext())
            {
                SeedContacts();
                var aboutsDictionary = context.Contacts.AsEnumerable().Select(
                    about => about.GetType().GetProperties()
                        .ToDictionary(x => x.Name, x => x.GetGetMethod().Invoke(about, null)));

                GetTable("Contacts").Should().BeEquivalentTo(aboutsDictionary);
            }
        }

        [Fact]
        public void ContactsShouldThrowIfAddingMultipleContacts()
        {
            using (var context = GetInformationContext())
            {
                SeedContacts();
                var contact = new Contact
                {
                    Id = false,
                    Value = "test"
                };

                context.Contacts.Add(contact);

                Action saveChanges = () => context.SaveChanges();
                saveChanges.Should().Throw<DbUpdateException>().WithInnerException<SqlException>().Which.Message
                    .Should().MatchRegex(
                        @"The INSERT statement conflicted with the CHECK constraint ""CK_CONTACTS_SIZE""\. The conflict occurred in database "".*"", table ""dbo.Contacts""");
            }
        }

        [Fact]
        public void ContactsShouldUpdateContacts()
        {
            using (var context = GetInformationContext())
            {
                SeedContacts();
                var targetContact = context.Contact;

                // ReSharper disable once PossibleNullReferenceException -- Contacts have already been seeded
                targetContact.Value = "Updated Value";

                context.SaveChanges();

                GetTable("Contacts").First().Should().BeEquivalentTo(
                    targetContact.GetType().GetProperties().ToDictionary(
                        x => x.Name,
                        x => x.GetGetMethod().Invoke(targetContact, null)));
            }
        }
    }
}