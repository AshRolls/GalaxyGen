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
        private Ship _currentShip;      

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
                _currentShip = (Ship)_model.Location;
                if (_currentShip.ShipState == ShipStateEnum.Docked)
                    _currentState = InternalAgentState.PilotingDockedShip;
                else if (_currentShip.ShipState == ShipStateEnum.Cruising)
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
                case InternalAgentState.PilotingAwaitingUndockingResponse:
                    break;
                case InternalAgentState.Piloting:
                    message = pilotingShip();                
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
                _currentState = InternalAgentState.PilotingAwaitingUndockingResponse;
                _actorTextOutput.Tell("Agent Requesting Undock");
                return new MessageShipCommand(ShipCommandEnum.Undock, tick.Tick, _currentShip.ShipId);
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
                    _actorTextOutput.Tell("Agent Undock Granted");
                }
                else
                {
                    _currentState = InternalAgentState.PilotingDockedShip;
                }
            }
        }

        private object pilotingShip()
        {
            if (isPilotingShip())
            {
                double x = _currentShip.PositionX;
                double y = _currentShip.PositionY;
                //_actorTextOutput.Tell("Agent Piloting Ship");
            }
            return null;
        }

    }
}
