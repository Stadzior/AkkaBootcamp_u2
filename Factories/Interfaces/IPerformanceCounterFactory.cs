using System.Diagnostics;
using ChartApp.Models;

namespace ChartApp.Factories.Interfaces
{
    /// <summary>
    /// Creates performance counter based on <see cref="CounterType"/>
    /// </summary>
    public interface IPerformanceCounterFactory
    {
        public PerformanceCounter Create(CounterType counterType);
    }
}
