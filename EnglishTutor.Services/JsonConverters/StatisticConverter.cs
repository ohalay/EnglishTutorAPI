using EnglishTutor.Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class StatisticConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(IEnumerable<Statistic>) == objectType;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        var res = new List<Statistic>();

        foreach (var item in obj)
        {
            var statistic = JsonConvert.DeserializeObject<Statistic>(item.Value.ToString());
            statistic.Name = item.Key;
            res.Add(statistic);
        }

        return res;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}