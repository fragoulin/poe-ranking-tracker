﻿using Newtonsoft.Json;
using PoeApiClient.Models;
using System;
using System.Diagnostics.Contracts;

namespace PoeApiClient.Converters
{
    public class ChallengesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IChallenges));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Contract.Requires(serializer != null);

            return serializer.Deserialize(reader, typeof(Challenges));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
