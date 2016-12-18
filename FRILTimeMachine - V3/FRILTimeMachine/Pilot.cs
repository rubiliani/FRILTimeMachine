﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FRILTimeMachine.CRaceJSONDataJsonTypes;

namespace FRILTimeMachine.CRaceJSONDataJsonTypes
{

    internal class Pilot
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("transponder_token")]
        public string TransponderToken { get; set; }

        [JsonProperty("deleted_at")]
        public object DeletedAt { get; set; }

        [JsonProperty("quad")]
        public string Quad { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }
    }

}
