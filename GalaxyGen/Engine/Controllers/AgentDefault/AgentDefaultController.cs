using System;
using Akka.Actor;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGenCore.StarChart;
using GalaxyGen.Engine.Messages;
using GalaxyGenCore.Framework;
using GalaxyGen.Framework;

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController
    {
        private const Int64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private AgentControllerState _state;
        private IActorRef _actorTextOutput;
        private AgentDefaultMemory _memory;

        public AgentDefaultController(AgentControllerState ag, IActorRef actorTextOutput)
        {
            _state = ag;
            _actorTextOutput = actorTextOutput;
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_state.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();
        }

        public Object Tick(MessageTick tick)
        {
            object message = null;

            if (_state.IsPilotingShip)
            {
                if (!_state.CurrentShipIsDocked) return pilotingCruisingShip(tick);
                else return pilotingDockedShip(tick);
            }
                        
            return message;
        }

        private object pilotingCruisingShip(MessageTick tick)
        {
            // new destination
            if (_state.CurrentShipDestinationScId != _memory.CurrentDestinationScId)
            {
                IMessageShipCommandData msd = new MessageShipSetDestination(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId);
                MessageShipCommand msc = new MessageShipCommand(msd, tick.Tick, _state.CurrentShipId);
                //ScPlanet curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
                //_actorTextOutput.Tell("Agent Piloting Ship towards " + curDest.Name);
                return msc;
            }

            // Am I at my current destination
            if (_state.CurrentShipAtDestination)
            {
                // request docking
                MessageShipCommand msc = new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Dock), tick.Tick, _state.CurrentShipId);
                return msc;
            }

            return null;
        }


        private object pilotingDockedShip(MessageTick tick)
        {
            object msg = null;

            checkMarkets(tick);
            if (checkLeaveShip(tick))
            {
                msg = requestPlanetside(tick);
            }
            else if (checkUndock(tick))
            {
                msg = requestUndock(tick);
            }

            return msg;           
        }

        // scan the local and system markets and decide if there is an order we want to place / fulfil
        private void checkMarkets(MessageTick tick)
        {
            // do i need to place a place / fulfil a market order, if so leave ship to interact with market            
            Int64 curPlanet = _state.CurrentShipDockedPlanetScId;
            if (checkOverMinimumTimeForMarketCheck(curPlanet, tick.Tick))
            {
                checkMarketPlaceOrder(tick);
            }
        }

        private void checkMarketPlaceOrder(MessageTick tick)
        {
            // TODO check stock and decide what we need (everywhere) and don't need (at this location)
                              
            // get prices at current location for needs
            // buy TODO buy any with reasonable price

            // get prices at current location for dont needs
            // sell TODO sell any at reasonable price
        }

        // make sure we haven't recently scanned market
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

        // do we need to go planetside?
        private bool checkLeaveShip(MessageTick tick)
        {
            return false;
        }

        private object requestPlanetside(MessageTick tick)
        {
            throw new NotImplementedException();
        }

        private bool checkUndock(MessageTick tick)
        {
            return true;
        }

        private object requestUndock(MessageTick tick)
        {
            setNewDestinationFromDocked();
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            return new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Undock), tick.Tick, _state.CurrentShipId);
        }

        private void setNewDestinationFromDocked()
        {
            // choose randomly
            ScPlanet curDest = null;            
            if (_memory.CurrentDestinationScId != 0)
            {
                curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
            }

            List<Int64> planetsToChooseFrom = _state.PlanetsInSolarSystemScIds.Where(x => x != _state.CurrentShipDockedPlanetScId).ToList();
            int index = RandomUtils.Random(planetsToChooseFrom.Count);
            _memory.CurrentDestinationScId = planetsToChooseFrom[index];
            saveMemory();
        }


        private void saveMemory()
        {
            _state.Memory = JsonConvert.SerializeObject(_memory);
        }

    }
}
