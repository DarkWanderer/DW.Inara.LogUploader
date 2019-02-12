﻿namespace DW.ELA.UnitTests.INARA
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DW.ELA.Controller;
    using DW.ELA.Plugin.Inara;
    using DW.ELA.Plugin.Inara.Model;
    using DW.ELA.UnitTests.Utility;
    using DW.ELA.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class InaraIntegrationTests
    {
        private readonly IReadOnlyCollection<string> ignoredErrors = new HashSet<string>
        {
            "Everything was alright, the near-neutral status just wasn't stored.",
            "There is a newer inventory state recorded already.",
            "This ship was not found but was added automatically."
        };

        [Test]
        [Explicit]
        public async Task IntegrationTestUploadToInara()
        {
            var logEventSource = new LogBurstPlayer(new SavedGamesDirectoryHelper().Directory, 5);
            var logCounter = new LogEventTypeCounter();
            var stateRecorder = new PlayerStateRecorder();

            var inaraRestClient = new ThrottlingRestClient.Factory().CreateRestClient("https://inara.cz/inapi/v1/");
            var inaraConverter = new InaraEventConverter(stateRecorder);
            var inaraApiFacade = new InaraApiFacade(inaraRestClient, TestCredentials.Inara.ApiKey, TestCredentials.UserName);

            // Populate the state recorder to avoid missing ships/starsystems data
            foreach (var ev in logEventSource.Events)
                stateRecorder.OnNext(ev);

            var convertedEvents = logEventSource
                .Events
                .SelectMany(inaraConverter.Convert)
                .ToArray();

            var results = await inaraApiFacade.ApiCall(convertedEvents);
            results = results
                .Where(e => e.EventStatus != 200)
                .Where(e => !ignoredErrors.Contains(e.EventStatusText))
                .ToList();

            CollectionAssert.IsEmpty(results);
            Assert.Pass("Uploaded {0} events", convertedEvents.Length);
        }
    }
}
