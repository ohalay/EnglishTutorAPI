using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnglishTutor.Services.JsonConverters
{
    public class CollectionJPathConverter : JsonConverter
    {
        private readonly string _path;

        public CollectionJPathConverter(string path)
        {
            _path = path;
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<string>) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

           return obj.SelectTokens(_path).Values<string>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}