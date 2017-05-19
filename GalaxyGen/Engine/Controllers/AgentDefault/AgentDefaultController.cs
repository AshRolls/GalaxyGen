using System;
using Akka.Actor;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGenCore.StarChart;
using GalaxyGen.Engine.Messages;
using GalaxyGenCore.Framework;
using FluentBehaviourTree;

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController
    {
        private const Int64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private AgentControllerState _model;
        private IActorRef _actorTextOutput;
        private AgentDefaultMemory _memory;
        private static Random _random;
        private IBehaviourTreeNode _tree;
        private MessageTick _curTick;
        private List<Object> _messages;

        public AgentDefaultController(AgentControllerState ag, IActorRef actorTextOutput)
        {
            _model = ag;
            _actorTextOutput = actorTextOutput;
            _random = new Random();
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_model.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();
            _messages = new List<Object>();

            BehaviourTreeBuilder builder = new BehaviourTreeBuilder();
            _tree = builder
                        .Selector("AgentDefaultControllerRoot")
                            .Selector("Piloting")
                                .Do("PilotingCruising", t => pilotingCruisingShip())
                                .Do("PilotingDocked", t => pilotingDockedShip())
                            .End()
                        .End()
                    .Build();
        }

        public List<Object> Tick(MessageTick tick)
        {
            object message = null;

            _curTick = tick;
            _messages.Clear();
            _tree.Tick(new TimeData(tick.Tick));
            
            return _messages;
        }

        private BehaviourTreeStatus pilotingCruisingShip()
        {
            if (_model.IsPilotingShip && !_model.CurrentShipIsDocked)
            {
                // new destination
                if (_model.CurrentShipDestinationScId != _memory.CurrentDestinationScId)
                {
                    IMessageShipCommandData msd = new MessageShipSetDestination(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId);
                    MessageShipCommand msc = new MessageShipCommand(msd, _curTick.Tick, _model.CurrentShipId);
                    ScPlanet curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
                    //_actorTextOutput.Tell("Agent Piloting Ship towards " + curDest.Name);
                    _messages.Add(msc);
                }

                // Am I at my current destination
                if (_model.CurrentShipAtDestination)
                {
                    // request docking
                    MessageShipCommand msc = new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Dock), _curTick.Tick, _model.CurrentShipId);
                    _messages.Add(msc);
                }
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }


        private BehaviourTreeStatus pilotingDockedShip()
        {
            object msg = null;

            if (_model.IsPilotingShip && _model.CurrentShipIsDocked)
            {
                checkMarkets(_curTick);
                if (checkLeaveShip(_curTick))
                {
                    msg = requestPlanetside(_curTick);
                }
                else if (checkUndock(_curTick))
                {
                    msg = requestUndock(_curTick);
                }
                if (msg != null) _messages.Add(msg);
                return BehaviourTreeStatus.Success;
            }
            return BehaviourTreeStatus.Failure;
        }

        // scan the local and system markets and decide if there is an order we want to place / fulfil
        private void checkMarkets(MessageTick tick)
        {
            // do i need to place a place / fulfil a market order, if so leave ship to interact with market            
            Int64 curPlanet = _model.CurrentShipDockedPlanetScId;
            if (checkOverMinimumTimeForMarketCheck(curPlanet, tick.Tick))
            {
                checkMarketPlaceOrder(tick);
            }
        }

        private void checkMarketPlaceOrder(MessageTick tick)
        {
            // check stock and decide what we need (everywhere) and don't need (at this location)
                              
            // get prices at current location for needs
            // buy any with reasonable price

            // get prices at current location for dont needs
            // sell any at reasonable price
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


        private void saveMemory()
        {
            _model.Memory = JsonConvert.SerializeObject(_memory);
        }

    }
}
