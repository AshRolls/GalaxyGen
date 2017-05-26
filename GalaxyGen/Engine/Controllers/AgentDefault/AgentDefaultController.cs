using System;
using Akka.Actor;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGenCore.StarChart;
using GalaxyGen.Engine.Messages;
using GalaxyGenCore.Framework;
using GalaxyGen.Framework;
using GalaxyGen.Engine.Ai.Goap;
using GalaxyGen.Engine.Ai.Goap.Actions;
using GalaxyGenCore.Resources;

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController, IGoap, IAgentActions
    {
        private const Int64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private AgentControllerState _state;
        private IActorRef _actorSolarSystem;
        private IActorRef _actorTextOutput;
        private AgentDefaultMemory _memory;
        private GoapAgent _goapAgent;

        public AgentDefaultController(AgentControllerState ag, IActorRef actorSolarSystem, IActorRef actorTextOutput)
        {
            _state = ag;
            _actorSolarSystem = actorSolarSystem;
            _actorTextOutput = actorTextOutput;
            _goapAgent = new GoapAgent(this, this, _state);            
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_state.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();
        }

        public void Tick(MessageTick tick)
        {
            _goapAgent.Tick();
        }

        //private object pilotingCruisingShip(MessageTick tick)
        //{
        //    // new destination
        //    if (_state.CurrentShipDestinationScId != _memory.CurrentDestinationScId)
        //    {
        //        IMessageShipCommandData msd = new MessageShipBasic(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId);
        //        MessageShipCommand msc = new MessageShipCommand(msd, tick.Tick, _state.CurrentShipId);
        //        //ScPlanet curDest = StarChart.GetPlanet(_memory.CurrentDestinationScId);
        //        //_actorTextOutput.Tell("Agent Piloting Ship towards " + curDest.Name);
        //        return msc;
        //    }

        //    // Am I at my current destination
        //    if (_state.CurrentShipAtDestination)
        //    {
        //        // request docking
        //        MessageShipCommand msc = new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Dock,_), tick.Tick, _state.CurrentShipId);
        //        return msc;
        //    }

        //    return null;
        //}


        //private object pilotingDockedShip(MessageTick tick)
        //{
        //    object msg = null;

        //    checkMarkets(tick);
        //    if (checkLeaveShip(tick))
        //    {
        //        msg = requestPlanetside(tick);
        //    }
        //    else if (checkUndock(tick))
        //    {
        //        msg = requestUndock(tick);
        //    }

        //    return msg;
        //}

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

        public void RequestUndock()
        {
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            //setNewDestination();
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipDocking(ShipCommandEnum.Undock, _state.CurrentShipDockedPlanetScId), 10, _state.CurrentShipId));
        }

        public void RequestDock()
        {
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipDocking(ShipCommandEnum.Dock, _memory.CurrentDestinationScId), 10, _state.CurrentShipId));
        }

        public void RequestLoadShip(ResourceQuantity resQ)
        {
            _actorTextOutput.Tell("Loading resources " + resQ.Type + ":" + resQ.Quantity);
        }

        private Int64 chooseRandomDestinationScId()
        {
            // choose randomly        
            List<Int64> planetsToChooseFrom;
            if (_state.CurrentShipIsDocked)
                planetsToChooseFrom = _state.PlanetsInSolarSystemScIds.Where(x => x != _state.CurrentShipDockedPlanetScId).ToList();
            else
                planetsToChooseFrom = _state.PlanetsInSolarSystemScIds.ToList();
            int index = RandomUtils.Random(planetsToChooseFrom.Count);
            return planetsToChooseFrom[index];
        }

        private void setNewDestination(Int64 destinationScId)
        {
            _memory.CurrentDestinationScId = destinationScId;
            saveMemory();
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetDestination(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId), 10, _state.CurrentShipId));
        }


        private void saveMemory()
        {
            _state.Memory = JsonConvert.SerializeObject(_memory);
        }

        public Dictionary<string, object> GetWorldState()
        {
            Dictionary<string, object> worldData = new Dictionary<string, object>();

            if (_state.CurrentShipIsDocked)
            {
                worldData.Add("isDocked", true);
                worldData.Add("DockedAt", _state.CurrentShipDockedPlanetScId);
            }
            else
            {
                worldData.Add("isDocked", false);
                worldData.Add("DockedAt", 0);
            }            

            return worldData;
        }

        public Dictionary<string, object> CreateGoalState()
        {
            Dictionary<string, object> goalState = new Dictionary<string, object>();

            goalState.Add("isDocked", true);
            goalState.Add("DockedAt", chooseRandomDestinationScId());
            //ResourceQuantity resQ = new ResourceQuantity(ResourceTypeEnum.Platinum, 10);
            //goalState.Add("HasResource"+resQ.Type, resQ.Quantity);

            return goalState;
        }

        public void PlanFailed(Dictionary<string, object> failedGoal)
        {
            _actorTextOutput.Tell("Plan failed " + GoapAgent.PrettyPrint(failedGoal));
        }

        public void PlanFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
        {
            // Yay we found a plan for our goal
            // Console.WriteLine("<color=green>Plan found</color> " + GoapAgent.PrettyPrint(actions));
            _actorTextOutput.Tell("Plan found " + GoapAgent.PrettyPrint(actions));
        }

        public void ActionsFinished()
        {
            // Everything is done, we completed our actions for this gool. Hooray!
            _actorTextOutput.Tell("Plan Completed");
            // Console.WriteLine("<color=blue>Actions completed</color>");
        }

        public void PlanAborted(GoapAction aborter)
        {
            // An action bailed out of the plan. State has been reset to plan again.
            // Take note of what happened and make sure if you run the same goal again
            // that it can succeed.
            // Console.WriteLine("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
            _actorTextOutput.Tell("Plan Aborted " + GoapAgent.prettyPrint(aborter));
        }

        public bool MoveAgent(GoapAction nextAction)
        {
            Int64 destinationScId = (Int64)nextAction.target;
            if (!_state.CurrentShipHasDestination || _state.CurrentShipDestinationScID != destinationScId) // set destination based on action if we don't have one or it has changed
            {
                setNewDestination(destinationScId);
            }
            else
            { 
                if (!_state.CurrentShipAutopilotActive) // turn on autopilot if off
                {
                    _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetAutopilot(ShipCommandEnum.SetAutopilot, true), 10, _state.CurrentShipId));
                    return false;
                }
                else
                {
                    if (_state.CurrentShipAtDestination(_memory.CurrentDestinationScId)) // check if we are there
                    {
                        nextAction.setInRange(true);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            //else
            //{
            //    // this code will never be reached at the moment as we use autopilot to move
            //    PointD curPoint = _state.CurrentShipXY;
            //    PointD newPoint = NavigationUtils.GetNewPointForShip(_state.CurrentShipCruisingSpeed, curPoint.X, curPoint.Y, _state.DestinationX(_memory.CurrentDestinationScId), _state.DestinationY(_memory.CurrentDestinationScId));
            //    _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetXY(ShipCommandEnum.SetXY, newPoint.X, newPoint.Y), 10, _state.CurrentShipId));
            //    if (_state.XYAtDestination(_memory.CurrentDestinationScId, newPoint.X, newPoint.Y))
            //    {
            //        nextAction.setInRange(true);
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            return false;
        }
        
        // actions this agent is capable of
        public GoapAction[] GetActions()
        {
            List<GoapAction> actionsList = new List<GoapAction>();

            actionsList.Add(new GoapUndockAction());

            // TODO limit number of destination actions we add to avoid combinatorial explosion
            foreach (Int64 destScId in _state.PlanetsInSolarSystemScIds)
            {
                actionsList.Add(new GoapDockAction(destScId));
                List<ResourceQuantity> resources = _state.PlanetResources(destScId);
                foreach (ResourceQuantity resQ in resources)
                {
                    actionsList.Add(new GoapLoadShipAction(destScId, resQ));
                }                
            }

            GoapAction[] actions = actionsList.ToArray();
            return actions;
        }

    }
}
