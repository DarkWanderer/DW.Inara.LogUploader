﻿using Interfaces;
using Interfaces.Settings;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Utility;

namespace ELA.Plugin.EDSM
{
    public class EdsmPlugin : IPlugin, IObserver<JObject>
    {
        private static readonly IRestClient RestClient = new ThrottlingRestClient("https://www.edsm.net/api-journal-v1/");
        private Task<HashSet<string>> ignoredEvents;
        private readonly ISettingsProvider settingsProvider;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IPlayerStateHistoryRecorder playerStateRecorder;
        private readonly List<JObject> eventQueue = new List<JObject>();
        private readonly Timer logFlushTimer = new Timer();
        private IEdsmApiFacade apiFacade;

        public EdsmPlugin(ISettingsProvider settingsProvider, IPlayerStateHistoryRecorder playerStateRecorder)
        {
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.playerStateRecorder = playerStateRecorder ?? throw new ArgumentNullException(nameof(playerStateRecorder));

            logFlushTimer.AutoReset = true;
            logFlushTimer.Interval = 5000; // send data every n seconds
            logFlushTimer.Elapsed += (o, e) => Task.Factory.StartNew(FlushQueue);
            logFlushTimer.Enabled = true;

            ignoredEvents =
                 RestClient.GetAsync("discard")
                    .ContinueWith((t) => new HashSet<string>(JArray.Parse(t.Result).ToObject<string[]>()));

            ReloadSettings();
        }

        private void ReloadSettings()
        {
            apiFacade = new EdsmApiFacade(RestClient, GlobalSettings.CommanderName, Settings.ApiKey);
        }

        private async void FlushQueue()
        {
            JObject[] apiEvents;
            lock (eventQueue)
            {
                apiEvents = eventQueue.ToArray();
                eventQueue.Clear();
            }
            if (apiEvents.Length > 0)
                await apiFacade.PostLogEvents(apiEvents);
        }

        public string SettingsLabel => "EDSM Upload";
        public string PluginId => "EdsmUploader";

        public IObserver<JObject> GetLogObserver() => this;
        public AbstractSettingsControl GetPluginSettingsControl() => new EdsmSettingsControl() { Plugin = this, ActualSettings = Settings };

        public void OnCompleted() { FlushQueue(); }
        public void OnError(Exception error) { }
        public void OnNext(JObject @event)
        {
            var eventName = @event["event"].ToString();
            if (ignoredEvents.Result.Contains(eventName))
                return;
            @event = (JObject)@event.DeepClone(); // have to clone the object here as we'll have to make modifications to it
            EnrichEvent(@event);
            lock (eventQueue)
            {
                eventQueue.Add(@event);
                if (eventQueue.Count > 1000)
                    Task.Factory.StartNew(FlushQueue);
            }
            logger.Trace("Queued event {0}", @event);
        }

        private void EnrichEvent(JObject @event)
        {
            var timestamp = DateTime.Parse(@event["timestamp"].ToString());
            @event["_systemName"] = playerStateRecorder.GetPlayerSystem(timestamp);
            @event["_shipId"] = playerStateRecorder.GetPlayerShipId(timestamp);
        }

        internal EdsmSettings Settings
        {
            get
            {
                try
                {
                    return settingsProvider.GetPluginSettings(SettingsLabel)?.ToObject<EdsmSettings>()
                        ?? new EdsmSettings();
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    return new EdsmSettings();
                }
            }
        }

        internal GlobalSettings GlobalSettings => settingsProvider.Settings;
    }
}
