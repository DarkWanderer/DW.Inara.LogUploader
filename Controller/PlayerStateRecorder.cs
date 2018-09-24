﻿using DW.ELA.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using NLog;
using DW.ELA.Interfaces.Events;
using System.Collections.Concurrent;
using DW.ELA.Utility.Extensions;

namespace Controller
{
    public class PlayerStateRecorder : IPlayerStateHistoryRecorder
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly StateRecorder<ShipRecord> ShipRecorder = new StateRecorder<ShipRecord>();
        private readonly StateRecorder<string> StarSystemRecorder = new StateRecorder<string>();
        private readonly StateRecorder<string> StationRecorder = new StateRecorder<string>();
        private readonly StateRecorder<bool> CrewRecorder = new StateRecorder<bool>();
        private readonly ConcurrentDictionary<string, double[]> SystemCoordinates = new ConcurrentDictionary<string, double[]>();

        public string GetPlayerSystem(DateTime atTime) => StarSystemRecorder.GetStateAt(atTime);
        public string GetPlayerStation(DateTime atTime) => StationRecorder.GetStateAt(atTime);
        public string GetPlayerShipType(DateTime atTime) => ShipRecorder.GetStateAt(atTime)?.ShipType;
        public long? GetPlayerShipId(DateTime atTime) => ShipRecorder.GetStateAt(atTime)?.ShipID;
        public bool GetPlayerIsInCrew(DateTime atTime) => CrewRecorder.GetStateAt(atTime);
        public double[] GetStarPos(string systemName) => SystemCoordinates.GetValueOrDefault(systemName);

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(LogEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            try
            {
                switch (@event)
                {
                    // Ship change events
                    case ShipyardSwap e: Process(e); break;
                    case LoadGame e: Process(e); break;
                    case Loadout e: Process(e); break;

                    // Location change events
                    case Location e: Process(e); break;
                    case FsdJump e: Process(e); break;
                    case Docked e: Process(e); break;
                    case SupercruiseEntry e: Process(e); break;
                    case Undocked e: Process(e); break;

                    // Crew status change events
                    case JoinACrew e: Process(e); break;
                    case QuitACrew e: Process(e); break;

                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in OnNext");
            }
        }

        private void Process(Undocked e) => StationRecorder.RecordState(null, e.Timestamp);
        private void Process(Loadout e) => ProcessShipIDEvent(e.ShipId, e.Ship, e.Timestamp);
        private void Process(LoadGame e) => ProcessShipIDEvent(e.ShipId, e.Ship, e.Timestamp);
        private void Process(ShipyardSwap e) => ProcessShipIDEvent(e.ShipId, e.ShipType, e.Timestamp);

        private void Process(Location e) => ProcessLocation(e.StarSystem, e.StarPos, e.Timestamp);
        private void Process(FsdJump e) => ProcessLocation(e.StarSystem, e.StarPos, e.Timestamp);
        private void Process(Docked e)
        {
            StarSystemRecorder.RecordState(e.StarSystem, e.Timestamp);
            StationRecorder.RecordState(e.StationName, e.Timestamp);
        }

        private void Process(SupercruiseEntry e) => StarSystemRecorder.RecordState(e.StarSystem, e.Timestamp);

        private void Process(QuitACrew e) => CrewRecorder.RecordState(false, e.Timestamp);
        private void Process(JoinACrew e) => CrewRecorder.RecordState(true, e.Timestamp);

        private void ProcessLocation(string starSystem, double[] starPos, DateTime timestamp)
        {
            if (SystemCoordinates.TryAdd(starSystem, starPos))
                logger.Trace("Recorded location for {0}", starSystem);
            else
                logger.Trace("Location for {0} already recorded", starSystem);
            StarSystemRecorder.RecordState(starSystem, timestamp);
        }

        private void ProcessShipIDEvent(long? shipId, string shipType, DateTime timestamp)
        {
            try
            {
                if (shipId == null ||
                    shipType == null ||
                    shipType.ToLower() == "testbuggy" ||
                    shipType.Contains("Fighter"))
                    return;

                ShipRecorder.RecordState(new ShipRecord { ShipID = shipId.Value, ShipType = shipType }, timestamp);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error decoding used ship reference");
            }
        }

        private class ShipRecord
        {
            public long ShipID;
            public string ShipType;

            public override bool Equals(object obj)
            {
                var record = obj as ShipRecord;
                return record != null &&
                       ShipID == record.ShipID &&
                       ShipType == record.ShipType;
            }

            public override int GetHashCode()
            {
                var hashCode = -1167275223;
                hashCode = hashCode * -1521134295 + ShipID.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ShipType);
                return hashCode;
            }

            public override string ToString() => $"{ShipType}-{ShipID}";
        }

        private class StateRecorder<T>
        {
            private readonly SortedList<DateTime, T> stateRecording = new SortedList<DateTime, T>();

            public T GetStateAt(DateTime atTime)
            {
                try
                {
                    lock (stateRecording)
                        return stateRecording
                            .Where(l => l.Key <= atTime)
                            .DefaultIfEmpty()
                            .MaxBy(l => l.Key)
                            .FirstOrDefault()
                            .Value;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    return default(T);
                }
            }

            public void RecordState(T state, DateTime at)
            {
                try
                {
                    lock (stateRecording)
                    {
                        var current = GetStateAt(at);
                        if (Equals(current, state))
                            return;

                        stateRecording.Add(at, state);
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }
        }
    }
}
