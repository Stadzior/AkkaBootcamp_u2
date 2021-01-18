using Akka.Actor;
using ChartApp.Models;

namespace ChartApp.Messages
{
    /// <summary>
    /// Unsubscribes <see cref="Subscriber"/> from receiving updates for a given counter.
    /// </summary>
    public class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counterType, IActorRef subscriber)
        {
            Subscriber = subscriber;
            CounterType = counterType;
        }

        public CounterType CounterType { get; }

        public IActorRef Subscriber { get; }
    }
}
