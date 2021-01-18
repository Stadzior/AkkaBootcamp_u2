using ChartApp.Models;

namespace ChartApp.Messages
{
    /// <summary>
    /// Subscribe the <see cref="Actors.ChartingActor"/> to updates for <see cref="CounterType"/>.
    /// </summary>
    public class Subscribe
    {
        public Subscribe(CounterType counterType)
        {
            CounterType = counterType;
        }

        public CounterType CounterType { get; }
    }
}
