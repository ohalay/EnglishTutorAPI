using System;
using System.Linq;
using EnglishTutor.Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnglishTutor.Services.JsonConverters
{
    public class WordConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Word) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var word = new Word();

            var lexicalEntry = obj.SelectToken("results[0].lexicalEntries[0]");

            var pronunciaton = lexicalEntry.SelectToken("pronunciations[?(@.audioFile)]");
            if (pronunciaton.HasValues)
            {
                word.PhoneticSpelling = pronunciaton.Value<string>("phoneticSpelling");
                word.AudioFilePath = pronunciaton.Value<string>("audioFile");
            }

            var sense = lexicalEntry.SelectTokens("entries[0].senses[?(@.examples)]").First();
            if (sense.HasValues)
            {
                word.Defination = (string)sense.SelectToken("definitions[0]");
                word.Examples = sense.SelectToken("examples")
                    .Select(s => s.Value<string>("text"))
                    .ToArray();
            }

            return word;
   
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}