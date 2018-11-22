﻿namespace DW.ELA.Plugin.EDDN
{
    using DW.ELA.Plugin.EDDN.Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JournalEvent : EddnEvent
    {
        [JsonProperty("message")]
        public JObject Message { get; set; }

        public override string SchemaRef => "https://eddn.edcd.io/schemas/journal/1";
    }
}