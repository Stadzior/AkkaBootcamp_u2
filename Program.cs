using System;
using System.Windows.Forms;
using Autofac;
using ChartApp.Factories;
using ChartApp.Factories.Interfaces;

namespace ChartApp
{
    internal static class Program
    {
        public static IContainer Container { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PerformanceCounterFactory>().As<IPerformanceCounterFactory>();
            builder.RegisterType<SeriesFactory>().As<ISeriesFactory>();
            Container = builder.Build();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
