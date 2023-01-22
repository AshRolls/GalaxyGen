using Akka.Actor;
using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace GalaxyGenEngine.Engine
{
    public enum GalaxyRunState
    {
        Running,
        RunningThrottled,
        RunningMax,
        Stopped
    }

    public class ActorGalaxy : ReceiveActor
    {
        private Galaxy _state;
        private HashSet<IActorRef> _subscribedActorSolarSystems; 
        private TextOutputController _textOutput;
        private GalaxyRunState _runState;
        private int _numberOfIncompleteSS;
        private Timer _secondTimer;
        private Timer _msTimer;
        private UInt64 _ticksAtTimerStart;
        private bool _receivedAll;

        public ActorGalaxy(TextOutputController textOutput, Galaxy state)
        {
            _runState = GalaxyRunState.Stopped;
            _state = state;
            _state.Actor = Self;
            _textOutput = textOutput;
            _receivedAll = false;

            setupChildSolarSystemActors();

            setupTimers();

            Receive<MessageEngineRunCommand>(msg => receiveEngineRunCommand(msg));
            Receive<MessageTick>(msg => sendTick()); // needed to receive scheduled
            Receive<MessageEngineSSCompletedCommand>(msg => receiveSSCompleted(msg));
        }

        private void setupChildSolarSystemActors()
        {
            // create child actors for each solar system
            _subscribedActorSolarSystems = new HashSet<IActorRef>();
            _numberOfIncompleteSS = _state.SolarSystems.Count();
            foreach (SolarSystem ss in _state.SolarSystems)
            {
                Props ssProps = Props.Create<ActorSolarSystem>(Self, _textOutput, ss);
                IActorRef actor = Context.ActorOf(ssProps, "SolarSystem" + ss.SolarSystemId.ToString());
                _subscribedActorSolarSystems.Add(actor);
            }
        }

        private void setupTimers()
        {
            _secondTimer = new Timer(3000);
            _secondTimer.Elapsed += secondTimer_Elapsed;
            _msTimer = new Timer(5);
            _msTimer.Elapsed += msTimer_Elapsed;
        }

        private void startSecondTimer()
        {
            _ticksAtTimerStart = _state.CurrentTick;
            _secondTimer.Start();       
        }        

        private void stopTimers()
        {
            _secondTimer.Stop();
            _msTimer.Stop();
        }

        private void secondTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _state.TicksPerSecond = (_state.CurrentTick - _ticksAtTimerStart) / 3;
            _ticksAtTimerStart = _state.CurrentTick;
        }

        private void msTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_receivedAll) 
            {
                _receivedAll = false;
                sendTick();
            }
        }

        ICancelable _runCancel;
        private void receiveEngineRunCommand(MessageEngineRunCommand msg)
        {
            MessageTick pulse = new MessageTick(0);
            stop();
            if (msg.RunCommand == EngineRunCommand.RunMax && _runState != GalaxyRunState.RunningMax)
            {
                _textOutput.Disable();
                _numberOfIncompleteSS = _subscribedActorSolarSystems.Count();                
                _runState = GalaxyRunState.RunningMax;
                startSecondTimer();
                sendTick();
            }
            else if (msg.RunCommand == EngineRunCommand.RunThrottled && _runState != GalaxyRunState.RunningThrottled)
            {
                _textOutput.Enable();
                _numberOfIncompleteSS = _subscribedActorSolarSystems.Count();
                _runState = GalaxyRunState.RunningThrottled;
                startSecondTimer();
                _msTimer.Start();
                sendTick();
            }
            else if (msg.RunCommand == EngineRunCommand.RunPulse && _runState != GalaxyRunState.Running)
            {
                _textOutput.Enable();
                _runState = GalaxyRunState.Running;
                _runCancel = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(0, 5, Self, pulse, ActorRefs.Nobody);
                startSecondTimer();
                sendTick();
            }
            else if (msg.RunCommand == EngineRunCommand.SingleTick)
            {
                _textOutput.Enable();
                sendTick();
            }
        }        

        private void stop()
        {
            cancelPulse();
            stopTimers();
            _runState = GalaxyRunState.Stopped;
        }

        private void cancelPulse()
        {
            if (_runCancel != null) _runCancel.Cancel();            
        }

        private void sendTick()
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
            if (_runState == GalaxyRunState.RunningMax || _runState == GalaxyRunState.RunningThrottled)
            {
                _numberOfIncompleteSS--;
                if (_numberOfIncompleteSS <= 0)
                {
                    _numberOfIncompleteSS = _subscribedActorSolarSystems.Count();
                    if (_runState == GalaxyRunState.RunningMax) sendTick();
                    else _receivedAll = true;
                }                
            }            
        }

    }
}
