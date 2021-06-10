namespace DW.ELA.UnitTests.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DW.ELA.Utility.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    public static class TestDeveloperTools
    {
        /// <summary>
        /// Utility method for developer to get canned events from own logs
        /// </summary>
        [Test]
        [Ignore("Developer tool")]
        public static void ToolPrepareCannedEvents()
        {
            var eventExamples = Enumerable.Empty<JObject>()
                                          .Concat(TestEventSource.CannedEventsRaw)
                                          .Concat(TestEventSource.LocalBetaEvents)
                                          .Concat(TestEventSource.LocalEventsRaw)
                                          .Concat(TestEventSource.LocalStaticEvents)
                                          .ExtractSamples()
                                          .ToHashSet();

            string eventsString = string.Join("\n", eventExamples);
            Assert.IsNotEmpty(eventsString);
            Console.WriteLine(eventsString);
        }

        private static JObject Sanitize(JObject input)
        {
            var output = (JObject)input.DeepClone();
            ReplaceValue(output, "timestamp", new DateTime(2018, 08, 28));
            ReplaceValue(output, "Commander", "PlayerCommander");
            ReplaceValue(output, "Amount", 3.456);
            ReplaceValue(output, "Cost", 555);
            ReplaceValue(output, "FID", "F12345");
            ReplaceValue(output, "SquadronName", "Test Squadron");
            ReplaceValue(output, "Offender", "CriminalCommander");
            ReplaceValue(output, "ID", 1234);
            if (input["event"].ToString() != "Statistics")
                ReplaceValue(output, "Crew", "CrewName");
            return output;
        }

        private static void ReplaceValue(JObject jObject, string key, object value)
        {
            if (jObject[key] != null)
                jObject[key] = JToken.FromObject(value);
        }

        private static IEnumerable<string> ExtractSamples(this IEnumerable<JObject> events)
        {
            var eventGroups = from @event in events
                              let eventName = @event.Property("event").Value.ToString()
                              let timestamp = @event.Property("timestamp").Value.ToObject<DateTime>()
                              orderby eventName ascending, timestamp descending
                              group @event by eventName into eventGroup
                              select eventGroup;

            var processedEvents = eventGroups
                .SelectMany(g => g.Take(3))
                .Select(Sanitize)
                .Select(Serialize.ToJson)
                .ToList();

            return processedEvents;
        }
    }
}
