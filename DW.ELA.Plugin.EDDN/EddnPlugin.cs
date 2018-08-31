﻿using DW.ELA.Controller;
using DW.ELA.Interfaces;
using DW.ELA.Interfaces.Settings;
using DW.ELA.Plugin.EDDN.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace DW.ELA.Plugin.EDDN
{
    public class EddnPlugin : AbstractPlugin<EddnEvent,EddnSettings>
    {
        private static readonly IRestClient RestClient = new ThrottlingRestClient("https://eddn.edcd.io:4430/upload/");
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly ISettingsProvider settingsProvider;
        private readonly IPlayerStateHistoryRecorder playerStateRecorder;

        private readonly IEddnApiFacade apiFacade = new EddnApiFacade(RestClient);
        private readonly EddnEventConverter eventConverter;
        private readonly EventSchemaValidator schemaManager = new EventSchemaValidator();

        protected override IEventConverter<EddnEvent> EventConverter => eventConverter;

        public EddnPlugin(ISettingsProvider settingsProvider, IPlayerStateHistoryRecorder playerStateRecorder) : base(settingsProvider)
        {
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.playerStateRecorder = playerStateRecorder ?? throw new ArgumentNullException(nameof(playerStateRecorder));
            eventConverter = new EddnEventConverter() { UploaderID = settingsProvider.Settings.CommanderName };
            settingsProvider.SettingsChanged += (o, e) => ReloadSettings();
            ReloadSettings();
        }

        public override string PluginName => "EDDN";
        public override string PluginId => "EDDN";
        // EDDN accepts events one-by-one, so no need to batch
        public override TimeSpan FlushInterval => TimeSpan.FromSeconds(1); 

        public override AbstractSettingsControl GetPluginSettingsControl(GlobalSettings settings) => null;
        public override async void FlushEvents(ICollection<EddnEvent> events)
        {
            foreach (var @event in events)
                schemaManager.ValidateSchema(@event);
            await apiFacade.PostEventsAsync(events.ToArray());
        }

        public override void ReloadSettings() => eventConverter.UploaderID = settingsProvider.Settings.CommanderName;
    }
}
