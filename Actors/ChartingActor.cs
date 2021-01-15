using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using ChartApp.Messages;

namespace ChartApp.Actors
{
    public class ChartingActor : UntypedActor
    {
        private readonly Chart _chart;
        private Dictionary<string, Series> _seriesIndex;

        public ChartingActor(Chart chart) : this(chart, new Dictionary<string, Series>())
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;
        }

        protected override void OnReceive(object message)
        {
            if (message is InitializeChart initializeChartMessage)
                HandleInitialize(initializeChartMessage);
        }

        #region Individual Message Type Handlers

        private void HandleInitialize(InitializeChart message)
        {
            if (message.InitialSeries != null)
            {
                //swap the two series out
                _seriesIndex = message.InitialSeries;
            }

            //delete any existing series
            _chart.Series.Clear();

            //attempt to render the initial chart
            if (!_seriesIndex.Any()) return;
            foreach (var (seriesName, series) in _seriesIndex)
            {
                //force both the chart and the internal index to use the same names
                series.Name = seriesName;
                _chart.Series.Add(series);
            }
        }
        #endregion
    }
}
