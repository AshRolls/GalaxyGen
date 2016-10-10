using Akka.Actor;
using GalaxyGen.Model;
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
        RunningMax,
        Stopped
    }

    public class ActorTickEngineCoordinator : ReceiveActor
    {
        Galaxy _state;
        private HashSet<IActorRef> _subscribedActorSolarSystems; // hashset here as faster than list for large number of items (>~20) http://stackoverflow.com/questions/150750/hashset-vs-list-performance
        IActorRef _actorTextOutput;
        TickEngineRunState _runState;
        private Int64 _numberOfIncompleteSS;

        public ActorTickEngineCoordinator(IActorRef actorTextOutput, Galaxy state)
        {
            _runState = TickEngineRunState.Stopped;
            _state = state;
            _state.Actor = Self;
            _subscribedActorSolarSystems = new HashSet<IActorRef>();
            _actorTextOutput = actorTextOutput;

            // create child actors for each solar system
            // TODO only subscribe child solar systems that are 'active' (ie have producer, agent, society etc)
            _numberOfIncompleteSS = _state.SolarSystems.Count();
            foreach (SolarSystem ss in _state.SolarSystems)
            {
                Props ssProps = Props.Create<ActorSolarSystem>(_actorTextOutput, ss);
                IActorRef actor = Context.ActorOf(ssProps, "SolarSystem" + ss.SolarSystemId.ToString());
                _subscribedActorSolarSystems.Add(actor);
            }
            
            Receive<MessageEngineRunCommand>(msg => receiveEngineRunCommand(msg));
            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageEngineSSCompletedCommand>(msg => receiveSSCompleted(msg));
        }

        ICancelable _runCancel;
        private void receiveEngineRunCommand(MessageEngineRunCommand msg)
        {
            MessageTick pulse = new MessageTick(0);
            if (msg.RunCommand == EngineRunCommand.RunMax && _runState != TickEngineRunState.RunningMax)
            {
                cancelPulse();
                _numberOfIncompleteSS = _subscribedActorSolarSystems.Count();                
                _runState = TickEngineRunState.RunningMax;                
                receiveTick(pulse);
            }
            else if (msg.RunCommand == EngineRunCommand.RunPulse && _runState != TickEngineRunState.Running)
            {
                _runState = TickEngineRunState.Running;
                _runCancel = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(0, 20, Self, pulse, ActorRefs.Nobody);
                receiveTick(pulse);
            }
            else if (msg.RunCommand == EngineRunCommand.Stop && _runState != TickEngineRunState.Stopped)
            {
                _runState = TickEngineRunState.Stopped;
                cancelPulse();
            }
        }

        private void cancelPulse()
        {
            if (_runCancel != null)
            {
                _runCancel.Cancel();
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

        private void receiveSSCompleted(MessageEngineSSCompletedCommand msg)
        {
            if (_runState == TickEngineRunState.RunningMax)
            {
                _numberOfIncompleteSS--;
                if (_numberOfIncompleteSS <= 0)
                {
                    _numberOfIncompleteSS = _subscribedActorSolarSystems.Count();
                    receiveTick(null);
                }
            }
        }

    }
}
