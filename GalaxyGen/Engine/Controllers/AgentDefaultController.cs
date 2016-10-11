using Akka.Actor;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class AgentDefaultController : IAgentController
    {
        private enum InternalAgentState
        {
            Planetside,
            Piloting,
            PilotingDockedShip,
            PilotingAwaitingUndockingResponse
        }

        private IReadOnlyAgent _model;
        private IActorRef _actorTextOutput;
        private InternalAgentState _currentState;
  

        public AgentDefaultController(IReadOnlyAgent ag, IActorRef actorTextOutput)
        {
            _model = ag;
            _actorTextOutput = actorTextOutput;

            setupInitialStateFromModel();

        }

        private void setupInitialStateFromModel()
        {
            if (isPilotingShip())
            {
                Ship s = (Ship)_model.Location;
                if (s.ShipState == ShipStateEnum.Docked)
                    _currentState = InternalAgentState.PilotingDockedShip;
                else if (s.ShipState == ShipStateEnum.Cruising)
                    _currentState = InternalAgentState.Piloting;
            }
            else
            {
                _currentState = InternalAgentState.Planetside;
            }

        }

        public object Tick(MessageTick tick)
        {
            object message = null;

            switch (_currentState)
            {
                case InternalAgentState.Planetside:
                case InternalAgentState.Piloting:
                case InternalAgentState.PilotingAwaitingUndockingResponse:
                    break;
                case InternalAgentState.PilotingDockedShip:
                    message = pilotingDockedShip(tick);
                    break;                                    
            }

            return message;
        }

        private bool isPilotingShip()
        {
            return _model.AgentState == AgentStateEnum.PilotingShip && _model.Location.GalType == TypeEnum.Ship;                                      
        }

        private object pilotingDockedShip(MessageTick tick)
        {
            if (isPilotingShip())
            {
                Ship s = (Ship)_model.Location;                
                _currentState = InternalAgentState.PilotingAwaitingUndockingResponse;
                return new MessageShipCommand(ShipCommandEnum.Undock, tick.Tick, s.ShipId);
            }
            return null;
        }

        public void ReceiveShipResponse(MessageShipResponse msg)
        {
            if (msg.SentCommand.Command == ShipCommandEnum.Undock)
            {
                if (msg.Response == true)
                {
                    _currentState = InternalAgentState.Piloting;
                }
                else
                {
                    _currentState = InternalAgentState.PilotingDockedShip;
                }
            }
        }
        
    }
}
