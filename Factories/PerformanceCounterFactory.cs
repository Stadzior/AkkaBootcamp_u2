using System;
using System.Diagnostics;
using ChartApp.Factories.Interfaces;
using ChartApp.Models;

namespace ChartApp.Factories
{
    /// <inheritdoc />
    public class PerformanceCounterFactory : IPerformanceCounterFactory
    {
        public PerformanceCounter Create(CounterType counterType)
            => counterType switch
            {
                CounterType.Cpu => new PerformanceCounter("Processor", "% Processor Time", "_Total", true),
                CounterType.Memory => new PerformanceCounter("Memory", "% Committed Bytes In Use", true),
                CounterType.Disk => new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total", true),
                _ => throw new ArgumentOutOfRangeException(nameof(counterType), counterType, null)
            };
    }
}
