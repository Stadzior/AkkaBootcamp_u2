using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Akka.Actor;
using ChartApp.Factories.Interfaces;
using ChartApp.Messages;
using ChartApp.Models;

namespace ChartApp.Actors
{    
    /// <summary>
    /// Actor responsible for monitoring a specific <see cref="PerformanceCounter"/>
    /// </summary>
    public class PerformanceCounterActor : UntypedActor
    {
        private readonly IPerformanceCounterFactory _performanceCounterFactory;
        private readonly CounterType _counterType;
        private PerformanceCounter _counter;

        private readonly HashSet<IActorRef> _subscriptions;
        private ICancelable _cancelPublishing;

        public PerformanceCounterActor(CounterType counterType, IPerformanceCounterFactory performanceCounterFactory)
        {
            _performanceCounterFactory = performanceCounterFactory;
            _counterType = counterType;
            _subscriptions = new HashSet<IActorRef>();
        }

        #region Actor lifecycle methods

        protected override void PreStart()
        {
            //create a new instance of the performance counter
            _counter = _performanceCounterFactory.Create(_counterType);
            _cancelPublishing = Context.System.Scheduler
                .ScheduleTellRepeatedlyCancelable(TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(250), Self, new GatherMetrics(), Self);
        }

        protected override void PostStop()
        {
            try
            {
                //terminate the scheduled task
                _cancelPublishing.Cancel(false);
                _counter.Dispose();
            }
            catch
            {
                //don't care about additional "ObjectDisposed" exceptions
            }
            finally
            {
                base.PostStop();
            }
        }

        #endregion
        
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GatherMetrics _:
                {
                    //publish latest counter value to all subscribers
                    var metric = new Metric(_counterType.ToString(), _counter.NextValue());
                    foreach (var sub in _subscriptions)
                        sub.Tell(metric);
                    break;
                }
                case SubscribeCounter subscribeCounter:
                {
                    // add a subscription for this counter (it's parent's job to filter by counter types)
                    _subscriptions.Add(subscribeCounter.Subscriber);
                    break;
                }
                case UnsubscribeCounter unsubscribeCounter:
                {
                    // remove a subscription from this counter
                    _subscriptions.Remove(unsubscribeCounter.Subscriber);
                    break;
                }
            }
        }
    }
}
