using ChartApp.Models;

namespace ChartApp.Messages
{
    /// <summary>
    /// Unsubscribe the <see cref="ChartingActor"/> to updates for <see cref="CounterType"/>
    /// </summary>
    public class Unsubscribe
    {
        public Unsubscribe(CounterType counterType)
        {
            CounterType = counterType;
        }

        public CounterType CounterType { get; }
    }

}
