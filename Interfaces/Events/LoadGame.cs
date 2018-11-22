﻿namespace DW.ELA.Interfaces.Events
{
    using DW.ELA.Interfaces;
    using Newtonsoft.Json;

    public class LoadGame : LogEvent
    {
        [JsonProperty("Commander")]
        public string Commander { get; set; }

        [JsonProperty("Horizons")]
        public bool? Horizons { get; set; }

        [JsonProperty("Ship")]
        public string Ship { get; set; }

        [JsonProperty("Ship_Localised")]
        public string ShipLocalised { get; set; }

        [JsonProperty("ShipID")]
        public long ShipId { get; set; }

        [JsonProperty("ShipName")]
        public string ShipName { get; set; }

        [JsonProperty("ShipIdent")]
        public string ShipIdent { get; set; }

        [JsonProperty("FuelLevel")]
        public double? FuelLevel { get; set; }

        [JsonProperty("FuelCapacity")]
        public double? FuelCapacity { get; set; }

        [JsonProperty("GameMode")]
        public string GameMode { get; set; }

        [JsonProperty("Credits")]
        public long Credits { get; set; }

        [JsonProperty("Loan")]
        public long Loan { get; set; }

        [JsonProperty]
        public string Group { get; set; }

        [JsonProperty("FID")]
        public string FrontierId { get; set; }

        [JsonProperty]
        public bool? StartLanded { get; set; }
    }
}
