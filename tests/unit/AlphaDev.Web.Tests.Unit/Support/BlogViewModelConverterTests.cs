using System;
using System.Collections.Generic;
using System.IO;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class BlogViewModelConverterTests
    {
        [Theory]
        [InlineData(typeof(BlogViewModel), true)]
        [InlineData(typeof(object), false)]
        public void CanConvertShouldReturnWhetherTypeCanBeConverted(Type type, bool expected)
        {
            var converter = GetBlogViewModelConverter();

            converter.CanConvert(type).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(BlogViewModelConverterTestsTheories
            .ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreMissingValueTheory), MemberType =
            typeof(BlogViewModelConverterTestsTheories))]
        public void ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreMissingValue(JsonReader reader,
            string expectedKey, string type)
        {
            var converter = GetBlogViewModelConverter();

            Action readJson = () => converter.ReadJson(reader, null, null, null);

            readJson.Should().Throw<ArgumentException>()
                .WithMessage($"{expectedKey} [NULL] is not castable to {type}.\r\nParameter name: reader")
                .Which.ParamName.Should().BeEquivalentTo("reader");
        }

        [Theory]
        [MemberData(nameof(BlogViewModelConverterTestsTheories
            .ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreNotOfTheRightTypeTheory), MemberType =
            typeof(BlogViewModelConverterTestsTheories))]
        public void ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreNotOfTheRightType(JsonReader reader,
            string expectedKey, string type)
        {
            var converter = GetBlogViewModelConverter();

            Action readJson = () => converter.ReadJson(reader, null, null, null);

            readJson.Should().Throw<ArgumentException>()
                .WithMessage($"{expectedKey} [NULL] is not castable to {type}.\r\nParameter name: reader")
                .Which.ParamName.Should().BeEquivalentTo("reader");
        }

        [Theory]
        [MemberData(nameof(BlogViewModelConverterTestsTheories
            .ReadJsonShouldThrowAnArgumentNullExceptionWhenRequiredArgumentsAreMissingTheory), MemberType =
            typeof(BlogViewModelConverterTestsTheories))]
        public void ReadJsonShouldThrowAnArgumentNullExceptionWhenRequiredArgumentsAreMissing(JsonWriter writer,
            object value, JsonSerializer serializer, string expectedParamName)
        {
            var converter = GetBlogViewModelConverter();

            Action readJson = () => converter.WriteJson(writer, value, serializer);

            readJson.Should().Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null.\r\nParameter name: {expectedParamName}")
                .Which.ParamName.Should().BeEquivalentTo(expectedParamName);
        }

        [NotNull]
        private BlogViewModelConverter GetBlogViewModelConverter()
        {
            return BlogViewModelConverter.Default;
        }

        private class BlogViewModelConverterTestsTheories
        {
            public static IEnumerable<object[]>
                ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreMissingValueTheory = new[]
                {
                    new object[]
                    {
                        Serialize(new { }), "Id", "Int32"
                    },
                    new object[]
                    {
                        Serialize(new {Id = default(int), Dates = new { }}), "Created", "DateTime"
                    }
                };

            public static IEnumerable<object[]>
                ReadJsonShouldThrowAnArgumentExceptionWhenRequiredKeysAreNotOfTheRightTypeTheory = new[]
                {
                    new object[]
                    {
                        Serialize(new {Id = default(object)}), "Id", "Int32"
                    },
                    new object[]
                    {
                        Serialize(new {Id = default(int), Dates = new {Created = default(object)}}), "Created",
                        "DateTime"
                    }
                };

            public static IEnumerable<object[]>
                ReadJsonShouldThrowAnArgumentNullExceptionWhenRequiredArgumentsAreMissingTheory = new[]
                {
                    new object[]
                    {
                        null, null, null, "writer"
                    },
                    new object[]
                    {
                        Substitute.For<JsonWriter>(), null, null, "value"
                    },
                    new[]
                    {
                        Substitute.For<JsonWriter>(), new object(), null, "serializer"
                    }
                };

            [NotNull]
            private static JsonReader Serialize<T>(T target)
            {
                var reader = new StringReader(JsonConvert.SerializeObject(target));
                return new JsonTextReader(reader);
            }
        }

        [Fact]
        public void CanWriteShouldBeTrue()
        {
            var converter = GetBlogViewModelConverter();

            converter.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void DefaultShouldReturnSingletonInstance()
        {
            BlogViewModelConverter.Default.Should().BeSameAs(BlogViewModelConverter.Default);
        }

        [Fact]
        public void ReadJsonShouldReturnBlogViewModelWithModifiedDateWhenItExists()
        {
            var converter = GetBlogViewModelConverter();

            var cache = new StringReader(
                "{\"Id\":0,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2018-01-01T00:00:00\",\"Modified\":\"2018-02-18T00:00:00\"}}");

            var serializer = new JsonSerializer();

            converter.ReadJson(new JsonTextReader(cache), typeof(BlogViewModel), default, serializer).Should()
                .BeEquivalentTo(new
                {
                    Id = 0,
                    Title = "title",
                    Content = "content",
                    Dates = new {Created = new DateTime(2018, 1, 1), Modified = Option.Some(new DateTime(2018, 2, 18))}
                });
        }

        [Fact]
        public void ReadJsonShouldReturnBlogViewModelWithoutModifiedDateWhenItDoesntExist()
        {
            var converter = GetBlogViewModelConverter();

            var cache = new StringReader(
                "{\"Id\":0,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2018-01-01T00:00:00\"}}");

            var serializer = new JsonSerializer();

            converter.ReadJson(new JsonTextReader(cache), typeof(BlogViewModel), default, serializer).Should()
                .BeEquivalentTo(new
                {
                    Id = 0,
                    Title = "title",
                    Content = "content",
                    Dates = new {Created = new DateTime(2018, 1, 1), Modified = Option.None<DateTime>()}
                });
        }

        [Fact]
        public void ReadJsonShouldReturnBlogViewModelWithoutModifiedDateWhenItExistsButIsNotDateTime()
        {
            var converter = GetBlogViewModelConverter();

            var cache = new StringReader(
                "{\"Id\":0,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2018-01-01T00:00:00\",\"Modified\": null}}");

            var serializer = new JsonSerializer();

            converter.ReadJson(new JsonTextReader(cache), typeof(BlogViewModel), default, serializer).Should()
                .BeEquivalentTo(new
                {
                    Id = 0,
                    Title = "title",
                    Content = "content",
                    Dates = new {Created = new DateTime(2018, 1, 1), Modified = Option.None<DateTime>()}
                });
        }

        [Fact]
        public void ReadJsonShouldThrowArgumentExceptionWhenDatesFieldIsMissing()
        {
            var converter = GetBlogViewModelConverter();
            var reader = new StringReader(JsonConvert.SerializeObject(new {Id = default(int)}));

            Action readJson = () => converter.ReadJson(new JsonTextReader(reader), null, null, null);

            readJson.Should().Throw<ArgumentException>()
                .WithMessage("Dates field is missing.\r\nParameter name: reader")
                .Which.ParamName.Should().BeEquivalentTo("reader");
        }

        [Fact]
        public void WriteJsonShouldSerializeBlogViewModelToJsonWriterWithModifiedDateWhenItExists()
        {
            var converter = GetBlogViewModelConverter();

            var cache = new StringWriter();
            var blog = new BlogViewModel(default, "title", "content",
                new DatesViewModel(new DateTime(2018, 1, 1), Option.Some(new DateTime(2018, 2, 18))));
            var serializer = new JsonSerializer();

            converter.WriteJson(new JsonTextWriter(cache), blog, serializer);

            cache.ToString().Should()
                .BeEquivalentTo(
                    "{\"Id\":0,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2018-01-01T00:00:00\",\"Modified\":\"2018-02-18T00:00:00\"}}");
        }

        [Fact]
        public void WriteJsonShouldSerializeBlogViewModelToJsonWriterWithoutModifiedDateWhenItDoesntExist()
        {
            var converter = GetBlogViewModelConverter();

            var cache = new StringWriter();
            var blog = new BlogViewModel(default, "title", "content",
                new DatesViewModel(new DateTime(2018, 1, 1), Option.None<DateTime>()));
            var serializer = new JsonSerializer();

            converter.WriteJson(new JsonTextWriter(cache), blog, serializer);

            cache.ToString().Should()
                .BeEquivalentTo(
                    "{\"Id\":0,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2018-01-01T00:00:00\"}}");
        }
    }
}