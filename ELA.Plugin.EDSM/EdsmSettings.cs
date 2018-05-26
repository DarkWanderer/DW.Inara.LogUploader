﻿using Newtonsoft.Json;

namespace ELA.Plugin.EDSM
{
    internal class EdsmSettings
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; internal set; } = "Not set";

        [JsonProperty("verified")]
        public bool Verified { get; internal set; } = false;
    }
}
