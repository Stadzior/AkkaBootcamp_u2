using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using ChartApp.Factories.Interfaces;
using ChartApp.Models;

namespace ChartApp.Factories
{
    /// <inheritdoc />
    public class SeriesFactory : ISeriesFactory
    { 
        public Series Create(CounterType counterType)
            => counterType switch
            {
                CounterType.Cpu => new Series(CounterType.Cpu.ToString())
                {
                    ChartType = SeriesChartType.SplineArea,
                    Color = Color.DarkGreen
                },
                CounterType.Memory => new Series(CounterType.Memory.ToString())
                {
                    ChartType = SeriesChartType.FastLine,
                    Color = Color.MediumBlue
                },
                CounterType.Disk => new Series(CounterType.Disk.ToString())
                {
                    ChartType = SeriesChartType.SplineArea,
                    Color = Color.DarkRed
                },
                _ => throw new ArgumentOutOfRangeException(nameof(counterType), counterType, null)
            };
    }
}
