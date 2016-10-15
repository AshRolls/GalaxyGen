using System;
using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Model;
using GalaxyGenCore.StarChart;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Controllers.AgentDefault
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
        private AgentDefaultMemory _memory;
        private static Random _random;

        public AgentDefaultController(IReadOnlyAgent ag, IActorRef actorTextOutput)
        {
            _model = ag;
            _actorTextOutput = actorTextOutput;
            _random = new Random();

            setupInitialStateFromModel();
        }

        private void setupInitialStateFromModel()
        {
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_model.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();

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
                setNewDestinationFromDocked();                                             
                _currentState = InternalAgentState.PilotingAwaitingUndockingResponse;
                _actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
                return new MessageShipCommand(ShipCommandEnum.Undock, tick.Tick, _currentShip.ShipId);
            }
            return null;
        }

        private void setNewDestinationFromDocked()
        {
            // choose randomly
            ScPlanet curDest = null;            
            if (_memory.CurrentDestinationScId != 0)
            {
                curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
            }

            List<Int64> planetsToChooseFrom = _model.SolarSystem.Planets.Select(x => x.StarChartId).Where(x => x != _currentShip.DockedPlanet.StarChartId).ToList();
            int index = _random.Next(planetsToChooseFrom.Count);
            _memory.CurrentDestinationScId = planetsToChooseFrom[index];
            saveMemory();
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
                ScPlanet curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
                _actorTextOutput.Tell("Agent Piloting Ship towards " + curDest.Name);
            }
            return null;
        }

        private void saveMemory()
        {
            _model.Memory = JsonConvert.SerializeObject(_memory);
        }

    }
}
