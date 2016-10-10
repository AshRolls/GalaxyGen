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
    // AGENT SHOULD NEVER CHANGE IT'S OWN STATE. IT SHOULD TELL SOLARSYSTEM AND THAT WILL MANAGE THE STATE CHANGES.

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

            //_actorTextOutput.Tell("Agent initialised : " + _agent.Name);

            if (_agent.AgentState == AgentStateEnum.PilotingShip && _agent.Location.GalType == TypeEnum.Ship)
            {
                Ship s = (Ship)_agent.Location;
                if (s.ShipState == ShipStateEnum.Docked)
                    PilotingDocked();
                else if (s.ShipState == ShipStateEnum.Cruising)
                    Piloting();
            }
        }

        private void PilotingDocked()
        {
            Receive<MessageTick>(msg => receiveDockedTick(msg));
        }

        private void AwaitingUndockingResponse()
        {
            Receive<MessageTick>(msg => receiveAwaitingTick(msg));
            Receive<MessageShipResponse>(msg => receiveShipResponse(msg));
        }

        private void Piloting()
        {
            Receive<MessageTick>(msg => receivePilotingTick(msg));
        }

        private void receivePilotingTick(MessageTick msg)
        {
            // head towards target
            //_actorTextOutput.Tell(@"I'm flying : " + _agent.Name);
        }

        private void receiveShipResponse(MessageShipResponse msg)
        {
            if (msg.SentCommand.Command == ShipCommandEnum.Undock)
            {
                if (msg.Response == true)
                {
                    Become(Piloting);
                }
                else
                {
                    Become(PilotingDocked);
                }
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
                MessageShipCommand cmd = new MessageShipCommand(ShipCommandEnum.Undock, tick.Tick, s.ShipId);
                _actorSolarSystem.Tell(cmd);
                Become(AwaitingUndockingResponse);
            }
        }

    }
}
