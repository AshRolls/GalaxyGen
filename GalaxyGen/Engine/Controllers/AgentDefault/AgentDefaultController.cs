using System;
using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Model;
using GalaxyGenCore.StarChart;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGen.Engine.Messages;
using GalaxyGenCore.Framework;

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController
    {
        private enum InternalAgentState
        {
            PlanetsideIdle,
            PlanetsideCheckingMarket,
            Piloting,
            PilotingDockedShip,
            PilotingAwaitingUndockingResponse,
            PilotingAwaitingDockingResponse
        }


        private const Int64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private AgentControllerState _model;
        private IActorRef _actorTextOutput;
        private InternalAgentState _currentState;
        private AgentDefaultMemory _memory;
        private static Random _random;        

        public AgentDefaultController(AgentControllerState ag, IActorRef actorTextOutput)
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
                if (_model.CurrentShipState == ShipStateEnum.Docked)
                    _currentState = InternalAgentState.PilotingDockedShip;
                else if (_model.CurrentShipState == ShipStateEnum.SpaceCruising)
                    _currentState = InternalAgentState.Piloting;
            }
            else
            {
                _currentState = InternalAgentState.PlanetsideIdle;
            }

        }

        public object Tick(MessageTick tick)
        {
            object message = null;

            switch (_currentState)
            {
                case InternalAgentState.PlanetsideIdle:
                case InternalAgentState.PlanetsideCheckingMarket:
                case InternalAgentState.PilotingAwaitingUndockingResponse:
                case InternalAgentState.PilotingAwaitingDockingResponse:
                    break;
                case InternalAgentState.Piloting:
                    message = pilotingShip(tick);                
                    break;
                case InternalAgentState.PilotingDockedShip:
                    message = pilotingDockedShip(tick);
                    break;                                    
            }

            return message;
        }        

        private bool isPilotingShip()
        {
            return _model.AgentState == AgentStateEnum.PilotingShip;                                      
        }

        private object pilotingDockedShip(MessageTick tick)
        {
            if (isPilotingShip())
            {
                if (checkLeaveShip(tick))
                {
                    return requestPlanetside(tick);
                }
                else
                {
                    return requestUndock(tick);
                }
            }
            return null;
        }

        private bool checkLeaveShip(MessageTick tick)
        {
            // do i need to place a place / fulfil a market order, if so leave ship to interact with market            
            Int64 curPlanet = _model.CurrentShipDockedPlanetScId;
            if (checkOverMinimumTimeForMarketCheck(curPlanet, tick.Tick))
            {
                return true;
            }
            return false;            
        }

        private bool checkOverMinimumTimeForMarketCheck(Int64 planetScId, Int64 tick)
        {
            if (!_memory.MarketLastCheckedTick.ContainsKey(planetScId))
            {
                return true;
            }
            else
            {
                Int64 tickForMinNextMarketCheck = _memory.MarketLastCheckedTick[planetScId] + (DAYS_BEFORE_MARKET_RECHECK * Globals.DAYS_TO_TICKS_FACTOR);
                return tick >= tickForMinNextMarketCheck;
            }
        }

        private object requestPlanetside(MessageTick tick)
        {
            throw new NotImplementedException();
        }

        private object requestUndock(MessageTick tick)
        {
            setNewDestinationFromDocked();
            _currentState = InternalAgentState.PilotingAwaitingUndockingResponse;
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            return new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Undock), tick.Tick, _model.CurrentShipId);
        }

        private void setNewDestinationFromDocked()
        {
            // choose randomly
            ScPlanet curDest = null;            
            if (_memory.CurrentDestinationScId != 0)
            {
                curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
            }

            List<Int64> planetsToChooseFrom = _model.PlanetsInSolarSystemScIds.Where(x => x != _model.CurrentShipDockedPlanetScId).ToList();
            int index = _random.Next(planetsToChooseFrom.Count);
            _memory.CurrentDestinationScId = planetsToChooseFrom[index];
            saveMemory();
        }

        public void ReceiveShipResponse(MessageShipResponse msg)
        {
            if (msg.SentCommand.Command.CommandType == ShipCommandEnum.Undock)
            {
                if (msg.Response == true)
                {
                    _currentState = InternalAgentState.Piloting;
                    //_actorTextOutput.Tell("Agent Undock Granted");
                }
                else
                {
                    _currentState = InternalAgentState.PilotingDockedShip;
                }
            }
            else if (msg.SentCommand.Command.CommandType == ShipCommandEnum.Dock)
            {
                if (msg.Response == true)
                {
                    _currentState = InternalAgentState.PilotingDockedShip;
                    //_actorTextOutput.Tell("Agent Dock Granted");
                    _memory.CurrentDestinationScId = 0;
                }
            }
        }

        public object ReceiveShipDestinationReached(MessageAgentDestinationReached msg)
        {
            if (isPilotingShip())
            {
                _currentState = InternalAgentState.PilotingAwaitingDockingResponse;
                //_actorTextOutput.Tell("Agent Requesting dock from " + StarChart.GetPlanet(_memory.CurrentDestinationScId).Name);
                return new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Dock), msg.TickSent, _model.CurrentShipId);
            }
            return null;
        }

        private object pilotingShip(MessageTick tick)
        {
            if (isPilotingShip())
            {
                if (_model.CurrentShipDestinationScId != _memory.CurrentDestinationScId)
                {
                    IMessageShipCommandData msd = new MessageShipSetDestination(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId);
                    MessageShipCommand msc = new MessageShipCommand(msd, tick.Tick, _model.CurrentShipId);                    
                    ScPlanet curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
                    //_actorTextOutput.Tell("Agent Piloting Ship towards " + curDest.Name);
                    return msc;
                }
            }
            return null;
        }

        private void saveMemory()
        {
            _model.Memory = JsonConvert.SerializeObject(_memory);
        }

    }
}
