using Akka.Actor;
using ChartApp.Models;

namespace ChartApp.Messages
{
    /// <summary>
    /// Enables a counter and begins publishing values to <see cref="Subscriber"/>.
    /// </summary>
    public class SubscribeCounter
    {
        public SubscribeCounter(CounterType counterType, IActorRef subscriber)
        {
            Subscriber = subscriber;
            CounterType = counterType;
        }

        public CounterType CounterType { get; }

        public IActorRef Subscriber { get; }
    }
}
