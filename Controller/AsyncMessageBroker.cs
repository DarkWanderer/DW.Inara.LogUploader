﻿namespace DW.ELA.Controller
{
    using System;
    using System.Linq;
    using DW.ELA.Interfaces;
    using DW.ELA.Utility;
    using NLog;
    using Utility.Observable;

    /// <summary>
    /// Forwards events from one IObservables to multiple IObservers in parallel fashion
    /// </summary>
    public class AsyncMessageBroker : BasicObservable<LogEvent>, IMessageBroker, IObserver<LogEvent>, IObservable<LogEvent>
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public override void OnCompleted()
        {
            try
            {
                lock (Observers)
                    Observers.AsParallel().ExecuteManyWithAggregateException(i => i.OnCompleted());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error caught in Async Broker");
            }
        }

        public override void OnError(Exception exception)
        {
            try
            {
                lock (Observers)
                    Observers.AsParallel().ExecuteManyWithAggregateException(i => i.OnError(exception));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error caught in Async Broker");
            }
        }

        public override void OnNext(LogEvent next)
        {
            try
            {
                lock (Observers)
                    Observers.AsParallel().ExecuteManyWithAggregateException(i => i.OnNext(next));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error caught in Async Broker");
            }
        }

        void IObserver<LogEvent>.OnCompleted() => OnCompleted();

        void IObserver<LogEvent>.OnError(Exception error) => OnError(error);

        void IObserver<LogEvent>.OnNext(LogEvent value) => OnNext(value);

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lock (Observers)
                        Observers.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AsyncMessageBroker() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
