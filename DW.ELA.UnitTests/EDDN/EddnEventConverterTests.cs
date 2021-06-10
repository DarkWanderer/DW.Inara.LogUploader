namespace DW.ELA.UnitTests.EDDN
{
    using System;
    using System.Linq;
    using DW.ELA.Interfaces;
    using DW.ELA.Plugin.EDDN;
    using DW.ELA.Plugin.EDDN.Model;
    using DW.ELA.UnitTests.Utility;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EddnEventConverterTests
    {
        //[Test]
        //[Parallelizable]
        //[TestCaseSource(typeof(TestEventSource), nameof(TestEventSource.CannedEvents))]
        //public void EddnConverterShouldConvertAndValidate(JournalEvent e)
        //{
        //    var recorderMock = GetRecorderMock();

        //    var eventConverter = new EddnEventConverter(recorderMock) { MaxAge = TimeSpan.FromDays(5000) };
        //    var result = eventConverter.Convert(e, TestCredentials.UserName).ToList();
        //    Assert.NotNull(result);
        //    CollectionAssert.AllItemsAreInstancesOfType(result, typeof(EddnEvent));
        //    foreach (var @event in result)
        //        Assert.IsTrue(validator.ValidateSchema(@event), "Event {0} should have validated", e.Event);
        //}

        [Test]
        [Parallelizable]
        public void ShouldEnrichJournalEventsWithStarSystemFields()
        {
            var recorderMock = GetRecorderMock();

            var eventConverter = new EddnEventConverter(recorderMock) { MaxAge = TimeSpan.FromDays(5000) };

            var convertedEvents = TestEventSource.CannedEvents
                .SelectMany(e => eventConverter.Convert(e, TestCredentials.UserName))
                .OfType<EddnJournalEvent>()
                .ToList();

            CollectionAssert.IsNotEmpty(convertedEvents);
            CollectionAssert.AllItemsAreNotNull(convertedEvents);

            foreach (var e in convertedEvents.OfType<EddnJournalEvent>())
            {
                Assert.NotNull(e.Message.Property("SystemAddress"));
                Assert.NotNull(e.Message.Property("StarPos"));
                Assert.NotNull(e.Message.Property("StarSystem"));
            }
        }

        [Test]
        [Parallelizable]
        public void JournalEventsShouldNotContainPersonalInfo()
        {
            var recorderMock = GetRecorderMock();

            var eventConverter = new EddnEventConverter(recorderMock) { MaxAge = TimeSpan.FromDays(5000) };

            var convertedEvents = TestEventSource.CannedEvents
                .SelectMany(e => eventConverter.Convert(e, TestCredentials.UserName))
                .OfType<EddnJournalEvent>()
                .ToList();

            CollectionAssert.IsNotEmpty(convertedEvents);
            CollectionAssert.AllItemsAreNotNull(convertedEvents);

            foreach (var e in convertedEvents.OfType<EddnJournalEvent>())
            {
                Assert.Null(e.Message.Property("ActiveFine"));
                Assert.Null(e.Message.Property("BoostUsed"));
                Assert.Null(e.Message.Property("CockpitBreach"));
                Assert.Null(e.Message.Property("FuelLevel"));
                Assert.Null(e.Message.Property("FuelUsed"));
                Assert.Null(e.Message.Property("JumpDist"));
                Assert.Null(e.Message.Property("Latitude"));
                Assert.Null(e.Message.Property("Longitude"));
                Assert.Null(e.Message.Property("Wanted"));
            }
        }

        private IPlayerStateHistoryRecorder GetRecorderMock()
        {
            var recorderMock = new Mock<IPlayerStateHistoryRecorder>();
            recorderMock.Setup(r => r.GetStarPos(It.IsAny<string>())).Returns(new[] { 0.0, 1.1, 2.2 });
            recorderMock.Setup(r => r.GetPlayerSystem(It.IsAny<DateTime>())).Returns("SomeSystem");
            recorderMock.Setup(r => r.GetSystemAddress(It.IsAny<string>())).Returns(123456789456);
            return recorderMock.Object;
        }
    }
}