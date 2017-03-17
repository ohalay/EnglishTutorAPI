using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnglishTutor.Services.JsonConverters
{
    public class JPathConverter : JsonConverter
    {
        private string _path;

        public JPathConverter(string path)
        {
            _path = path;
        }
        public override bool CanConvert(Type objectType)
        {
            return typeof(string) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            return (string)obj.SelectToken(_path);
   
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}