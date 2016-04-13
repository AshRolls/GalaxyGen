using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorAgent : ReceiveActor, IWithUnboundedStash
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        Agent _agent;

        public ActorAgent(IActorRef actorTextOutput, Agent ag, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _agent = ag;
            _agent.Actor = Self;          

            _actorTextOutput.Tell("Agent initialised : " + _agent.Name);

            PilotingDocked();           
        }

        private void PilotingDocked()
        {
            Receive<MessageTick>(msg => receiveDockedTick(msg));
        }

        private void AwaitingUndockingResponse()
        {
            Receive<MessageTick>(msg => receiveAwaitingTick(msg));
            Receive<MessageShipDockResponse>(msg => receiveUndockResponse(msg));
        }

        private void Piloting()
        {
            Receive<MessageTick>(msg => receivePilotingTick(msg));
        }

        private void receivePilotingTick(MessageTick msg)
        {
            // head towards target
            _actorTextOutput.Tell(@"I'm flying : " + _agent.Name);
        }

        private void receiveUndockResponse(MessageShipDockResponse msg)
        {
            if (msg.Response == true)
            {                
                Become(Piloting);
            }
            else
            {
                Become(PilotingDocked);
            }
            Stash.UnstashAll();
        }

        public IStash Stash { get; set; }

        // TODO put in system for when we never receive a response!
        private void receiveAwaitingTick(MessageTick tick)
        {
            Stash.Stash(); // stash messages while we are waiting for our response.
        }

        private void receiveDockedTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV H: " + _agentVm.Name + " " + tick.Tick.ToString());
            if (_agent.AgentState == AgentStateEnum.PilotingShip && _agent.Location.GalType == TypeEnum.Ship)
            {
                Ship s = (Ship)_agent.Location;
                MessageShipDockCommand cmd = new MessageShipDockCommand(ShipDockCommandEnum.Undock, tick.Tick, null);
                s.Actor.Tell(cmd);
                Become(AwaitingUndockingResponse);
            }
        }

    }
}
