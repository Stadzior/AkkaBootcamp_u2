namespace ChartApp.Messages
{
    /// <summary>
    /// Remove an existing <see cref="Series"/> from the chart
    /// </summary>
    public class RemoveSeries
    {
        public RemoveSeries(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; }
    }
}
