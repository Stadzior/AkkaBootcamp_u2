using System.Windows.Forms;
using Akka.Actor;
using ChartApp.Messages;
using ChartApp.Models;

namespace ChartApp.Actors
{    
    /// <summary>
    /// Actor responsible for managing button toggles
    /// </summary>
    public class ButtonToggleActor : UntypedActor
    {
        private readonly CounterType _counterType;
        private bool _isToggledOn;
        private readonly Button _myButton;
        private readonly IActorRef _coordinatorActor;

        public ButtonToggleActor(IActorRef coordinatorActor, Button myButton, CounterType counterType, bool isToggledOn = false)
        {
            _coordinatorActor = coordinatorActor;
            _myButton = myButton;
            _isToggledOn = isToggledOn;
            _counterType = counterType;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Toggle _ when _isToggledOn:
                    // toggle is currently on
                    // stop watching this counter
                    _coordinatorActor.Tell(new Unsubscribe(_counterType));
                    FlipToggle();
                    break;
                case Toggle _ when !_isToggledOn:
                    // toggle is currently off
                    // start watching this counter
                    _coordinatorActor.Tell(new Subscribe(_counterType));
                    FlipToggle();
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }

        private void FlipToggle()
        {
            // flip the toggle
            _isToggledOn = !_isToggledOn;

            // change the text of the button
            _myButton.Text = $@"{_counterType.ToString().ToUpperInvariant()} ({(_isToggledOn ? "ON" : "OFF")})";
        }
    }
}
