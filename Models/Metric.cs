using System;
using System.Collections.Generic;
using System.Text;

namespace ChartApp.Models
{
    /// <summary>
    /// Metric data at the time of sample
    /// </summary>
    public class Metric
    {
        public Metric(string series, float counterValue)
        {
            CounterValue = counterValue;
            Series = series;
        }

        public string Series { get; }

        public float CounterValue { get; }
    }
}
