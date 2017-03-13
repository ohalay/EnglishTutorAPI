﻿using EnglishTutor.Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

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

        var pronunciaton = lexicalEntry.SelectToken("pronunciaton[?(@.audioFile)]");
        if (pronunciaton.HasValues)
        {
            word.PhoneticSpelling = pronunciaton.Value<string>("phoneticSpelling");
            word.AudioFilePath = pronunciaton.Value<string>("audioFile");
        }

        var sense = lexicalEntry.SelectToken("entries[0].senses[0]");
        if (sense.HasValues)
        {
            word.Defination = (string)sense.SelectToken("definitions[0]");
            word.Examples = sense.SelectTokens("examples")
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