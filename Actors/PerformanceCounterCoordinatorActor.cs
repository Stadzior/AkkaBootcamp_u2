using System.Collections.Generic;
using Akka.Actor;
using ChartApp.Factories.Interfaces;
using ChartApp.Messages;
using ChartApp.Models;

namespace ChartApp.Actors
{
    public class PerformanceCounterCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<CounterType, IActorRef> _counterActors;
        private readonly IActorRef _chartingActor;
        
        public PerformanceCounterCoordinatorActor(IActorRef chartingActor, IPerformanceCounterFactory counterFactory, ISeriesFactory seriesFactory) 
            : this(chartingActor, counterFactory, seriesFactory, new Dictionary<CounterType, IActorRef>())
        {
        }
        public PerformanceCounterCoordinatorActor(IActorRef chartingActor, IPerformanceCounterFactory counterFactory, ISeriesFactory seriesFactory, Dictionary<CounterType, IActorRef> counterActors)
        {
            _chartingActor = chartingActor;
            _counterActors = counterActors;

            Receive<Subscribe>(subscribe =>
            {
                if (!_counterActors.ContainsKey(subscribe.CounterType))
                {
                    // create a child actor to monitor this counter if one doesn't exist already
                    var counterActor = Context.ActorOf(Props.Create(() => new PerformanceCounterActor(subscribe.CounterType, counterFactory)));

                    // add this counter actor to our index
                    _counterActors[subscribe.CounterType] = counterActor;
                }

                // register this series with the ChartingActor
                _chartingActor.Tell(new AddSeries(seriesFactory.Create(subscribe.CounterType)));

                // tell the counter actor to begin publishing its statistics to the _chartingActor
                _counterActors[subscribe.CounterType].Tell(new SubscribeCounter(subscribe.CounterType, _chartingActor));
            });

            Receive<Unsubscribe>(unsubscribe =>
            {
                if (!_counterActors.ContainsKey(unsubscribe.CounterType))
                    return;

                // unsubscribe the ChartingActor from receiving any more updates
                _counterActors[unsubscribe.CounterType].Tell(new UnsubscribeCounter(unsubscribe.CounterType, _chartingActor));

                // remove this series from the ChartingActor
                _chartingActor.Tell(new RemoveSeries(unsubscribe.CounterType.ToString()));
            });
        }
    }
}
