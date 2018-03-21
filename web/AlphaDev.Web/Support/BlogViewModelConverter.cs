using System;
using AlphaDev.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optional;

namespace AlphaDev.Web.Support
{
    public class BlogViewModelConverter : JsonConverter
    {
        private static readonly Lazy<BlogViewModelConverter> Singleton = new Lazy<BlogViewModelConverter>(() => new BlogViewModelConverter());
        public static BlogViewModelConverter Default => Singleton.Value;

        private BlogViewModelConverter() { }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(BlogViewModel));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var loadedObject = JObject.Load(reader);
            
            var id = loadedObject["Id"].SomeWhen(token => token?.Type == JTokenType.Integer).Map(token => (int)token).ValueOr(() =>
                throw new ArgumentException($"{GetKeyValue(loadedObject, "Id")} is not castable to Int32.", nameof(reader)));

            var title = loadedObject["Title"].SomeNotNull().Map(token => token.ToString()).ValueOr(string.Empty);
            var content = loadedObject["Content"].SomeNotNull().Map(token => token.ToString()).ValueOr(string.Empty); ;

            var dateToken = loadedObject["Dates"] ?? throw new ArgumentException("Dates field is missing.", nameof(reader));

            var created = dateToken["Created"].SomeWhen(token => token?.Type == JTokenType.Date).Map(token => (DateTime)token).ValueOr(() =>
                throw new ArgumentException($"{GetKeyValue(dateToken, "Created")} is not castable to DateTime.", nameof(reader)));

            var modified = dateToken["Modified"].SomeNotNull().Filter(token => token.Type == JTokenType.Date).Map(token => (DateTime) token);
            
            var result = new BlogViewModel(id, title, content, new DatesViewModel(created, modified));
            
            return result;
        }

        private static string GetKeyValue(JToken token, string key)
        {
            return
                $"{key} {token[key].SomeNotNull().Filter(jToken => jToken.HasValues).Map(jToken => jToken.ToString()).ValueOr("[NULL]")}";
        }

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(writer == null) throw new ArgumentNullException(nameof(writer));
            if (value== null) throw new ArgumentNullException(nameof(value));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            var blog = (BlogViewModel)value;

            writer.WriteStartObject();
            writer.WritePropertyName("Id");
            serializer.Serialize(writer, blog.Id);
            writer.WritePropertyName("Title");
            serializer.Serialize(writer, blog.Title);
            writer.WritePropertyName("Content");
            serializer.Serialize(writer, blog.Content);
            writer.WritePropertyName("Dates");
            writer.WriteStartObject();
            writer.WritePropertyName("Created");
            serializer.Serialize(writer, blog.Dates.Created);
            
            blog.Dates.Modified.MatchSome(time =>
            {
                writer.WritePropertyName("Modified");
                serializer.Serialize(writer, time);
            });
            
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}