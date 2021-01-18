using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using ChartApp.Models;

namespace ChartApp.Factories.Interfaces
{
    /// <summary>
    /// Methods for creating new <see cref="Series"/> with distinct colors and names corresponding to each <see cref="PerformanceCounter"/>
    /// </summary>
    public interface ISeriesFactory
    {
        public Series Create(CounterType counterType);
    }
}
