using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using ChartApp.Messages;
using ChartApp.Models;

namespace ChartApp.Actors
{
    public class ChartingActor : ReceiveActor
    {
        /// <summary>
        /// Maximum number of points we will allow in a series
        /// </summary>
        public const int MaxPoints = 250;

        /// <summary>
        /// Incrementing counter we use to plot along the X-axis
        /// </summary>
        private int xPosCounter = 0;

        private readonly Chart _chart;
        private Dictionary<string, Series> _seriesIndex;

        private readonly Button _pauseButton;

        public ChartingActor(Chart chart, Button pauseButton) : this(chart, new Dictionary<string, Series>(), pauseButton)
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;
            _pauseButton = pauseButton;
            Charting();
        }

        private void Charting()
        {
            Receive<InitializeChart>(HandleInitialize);
            Receive<AddSeries>(HandleAddSeries);
            Receive<RemoveSeries>(HandleRemoveSeries);
            Receive<Metric>(HandleMetrics);

            //new receive handler for the TogglePause message type
            Receive<TogglePause>(pause =>
            {
                SetPauseButtonText(true);
                BecomeStacked(Paused);
            });
        }

        private void Paused()
        {
            Receive<Metric>(metric =>
            {
                metric.CounterValue = 0;
                HandleMetrics(metric);
            });
            Receive<TogglePause>(pause =>
            {
                SetPauseButtonText(false);
                UnbecomeStacked();
            });
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

            // set the axes up
            var area = _chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            SetChartBoundaries();

            //nothing to render
            if (!_seriesIndex.Any()) 
                return;

            //attempt to render the initial chart
            foreach (var (seriesName, series) in _seriesIndex)
            {
                //force both the chart and the internal index to use the same names
                series.Name = seriesName;
                _chart.Series.Add(series);
            }

            SetChartBoundaries();
        }

        private void HandleAddSeries(AddSeries series)
        {
            if (string.IsNullOrEmpty(series.Series.Name) || _seriesIndex.ContainsKey(series.Series.Name)) 
                return;

            _seriesIndex.Add(series.Series.Name, series.Series);
            _chart.Series.Add(series.Series);
            SetChartBoundaries();
        }

        private void HandleRemoveSeries(RemoveSeries series)
        {
            if (string.IsNullOrEmpty(series.SeriesName) || !_seriesIndex.ContainsKey(series.SeriesName)) 
                return;

            var seriesToRemove = _seriesIndex[series.SeriesName];
            _seriesIndex.Remove(series.SeriesName);
            _chart.Series.Remove(seriesToRemove);
            SetChartBoundaries();
        }

        private void HandleMetrics(Metric metric)
        {
            if (string.IsNullOrEmpty(metric.Series) || !_seriesIndex.ContainsKey(metric.Series)) 
                return;

            var series = _seriesIndex[metric.Series];
            series.Points.AddXY(xPosCounter++, metric.CounterValue);
            while (series.Points.Count > MaxPoints) 
                series.Points.RemoveAt(0);
            SetChartBoundaries();
        }
        #endregion

        #region "UI related stuff"

        private void SetPauseButtonText(bool paused)
            => _pauseButton.Text = $@"{(!paused ? "PAUSE ⏸" : "RESUME ►")}";

        private void SetChartBoundaries()
        {
            var allPoints = _seriesIndex.Values.SelectMany(series => series.Points).ToList();
            var yValues = allPoints.SelectMany(point => point.YValues).ToList();
            double maxAxisX = xPosCounter;
            double minAxisX = xPosCounter - MaxPoints;
            var maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
            var minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;

            if (allPoints.Count <= 2) 
                return;

            var area = _chart.ChartAreas[0];
            area.AxisX.Minimum = minAxisX;
            area.AxisX.Maximum = maxAxisX;
            area.AxisY.Minimum = minAxisY;
            area.AxisY.Maximum = maxAxisY;
        }

        #endregion
    }
}
