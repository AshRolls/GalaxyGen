using Akka.Actor;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public enum TickEngineRunState
    {
        Running,
        Stopped
    }

    public class ActorTickEngineCoordinator : ReceiveActor
    {
        IGalaxyViewModel _state;
        private HashSet<IActorRef> _subscribedActorSolarSystems; // hashset here as faster than list for large number of items (>~20) http://stackoverflow.com/questions/150750/hashset-vs-list-performance
        IActorRef _actorTextOutput;
        TickEngineRunState _runState;

        public ActorTickEngineCoordinator(IActorRef actorTextOutput, IGalaxyViewModel state)
        {
            _runState = TickEngineRunState.Stopped;
            _state = state;
            _state.Actor = Self;
            _subscribedActorSolarSystems = new HashSet<IActorRef>();
            _actorTextOutput = actorTextOutput;          

            // create child actors for each solar system
            foreach (ISolarSystemViewModel ssVm in _state.SolarSystems)
            {
                Props ssProps = Props.Create<ActorSolarSystem>(_actorTextOutput, ssVm);
                IActorRef actor = Context.ActorOf(ssProps, "SolarSystem" + ssVm.Model.SolarSystemId.ToString());
                _subscribedActorSolarSystems.Add(actor);
            }

            Receive<MessageEngineRunCommand>(msg => receiveEngineRunCommand(msg));
            Receive<MessageTick>(msg => receiveTick(msg));
        }

        ICancelable _runCancel;
        private void receiveEngineRunCommand(MessageEngineRunCommand msg)
        {
            MessageTick pulse = new MessageTick(0);
            if (msg.RunCommand == EngineRunCommand.Run && _runState != TickEngineRunState.Running)
            {
                _runState = TickEngineRunState.Running;
                _runCancel = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(0, 500, Self, pulse, ActorRefs.Nobody);
            }
            else if (msg.RunCommand == EngineRunCommand.Stop && _runState == TickEngineRunState.Running)
            {
                if (_runCancel != null)
                {
                    _runState = TickEngineRunState.Stopped;
                    _runCancel.Cancel();                    
                }
            }
        }

        private void receiveTick(MessageTick pulse)
        {
            _state.CurrentTick++;
            MessageTick tick = new MessageTick(_state.CurrentTick);
            foreach (IActorRef ssActor in _subscribedActorSolarSystems)
            {
                ssActor.Tell(tick);
            }           
        }

    }
}
