using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using Akka.Util.Internal;
using Autofac;
using ChartApp.Actors;
using ChartApp.Factories.Interfaces;
using ChartApp.Messages;
using ChartApp.Models;

namespace ChartApp
{
    public partial class Main : Form
    {
        /// <summary>
        /// ActorSystem we'll be using to publish data to charts
        /// and subscribe from performance counters
        /// </summary>
        public static ActorSystem ChartActors;
        
        private IActorRef _chartingActor;
        private IActorRef _coordinatorActor;
        private IDictionary<CounterType, IActorRef> _toggleActors;

        public Main()
        {
            InitializeComponent();
        }

        private void BtnCpu_Click(object sender, EventArgs e)
            => _toggleActors[CounterType.Cpu].Tell(new Toggle());

        private void BtnMemory_Click(object sender, EventArgs e)
            => _toggleActors[CounterType.Memory].Tell(new Toggle());

        private void BtnDisk_Click(object sender, EventArgs e)
            => _toggleActors[CounterType.Disk].Tell(new Toggle());

        #region Initialization

        private void Main_Load(object sender, EventArgs e)
        {
            ChartActors = ActorSystem.Create("ChartActors");
            _chartingActor = ChartActors.ActorOf(Props.Create(() => new ChartingActor(sysChart)), "charting");
            _chartingActor.Tell(new InitializeChart());
            
            IPerformanceCounterFactory performanceCounterFactory;
            ISeriesFactory seriesFactory;
            using (var scope = Program.Container.BeginLifetimeScope())
            {
                performanceCounterFactory = scope.Resolve<IPerformanceCounterFactory>();
                seriesFactory = scope.Resolve<ISeriesFactory>();
            }

            _coordinatorActor = ChartActors.ActorOf(Props.Create(() =>
                new PerformanceCounterCoordinatorActor(_chartingActor, performanceCounterFactory, seriesFactory)), "counters");

            _toggleActors = new Dictionary<CounterType, IActorRef>
            {
                {
                    CounterType.Cpu, 
                    ChartActors.ActorOf(Props.Create(() =>
                        new ButtonToggleActor(_coordinatorActor, btnCpu, CounterType.Cpu, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    CounterType.Memory, 
                    ChartActors.ActorOf(Props.Create(() =>
                        new ButtonToggleActor(_coordinatorActor, btnMemory, CounterType.Memory, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    CounterType.Disk, 
                    ChartActors.ActorOf(Props.Create(() =>
                        new ButtonToggleActor(_coordinatorActor, btnDisk, CounterType.Disk, false))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"))
                }
            };
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //shut down the charting actor
            _chartingActor.Tell(PoisonPill.Instance);

            //shut down the ActorSystem
            ChartActors.Terminate();
        }

        #endregion
    }
}
