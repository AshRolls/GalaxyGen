using System;
using Akka.Actor;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGenCore.Framework;
using GalaxyGenCore.Resources;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Engine.Ai.Goap;
using GalaxyGenEngine.Engine.Ai.Goap.Actions;
using GalaxyGenEngine.Model;
using static Akka.Actor.Status;

namespace GalaxyGenEngine.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController, IGoap, IAgentActions
    {        
        private const UInt64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private ulong _curTick;
        private AgentControllerState _state;
        private IActorRef _actorSolarSystem;
        private TextOutputController _textOutput;
        private AgentDefaultMemory _memory;
        private GoapAgent _goapAgent;

        public AgentDefaultController(AgentControllerState state, IActorRef actorSolarSystem, TextOutputController textOutput)
        {
            _state = state;
            _actorSolarSystem = actorSolarSystem;
            _textOutput = textOutput;
            _goapAgent = new GoapAgent(this, this, _state);
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_state.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();
        }

        public void Tick(MessageTick tick)
        {
            _curTick = tick.Tick;
            _goapAgent.Tick();
        }

        public void ReceiveCommand(MessageAgentCommand msg)
        {
            switch (msg.Command.CommandType)
            {
                case AgentCommandEnum.ShipCommandFailed:
                    _textOutput.Write(_state.AgentId, "Plan Failed (Ship)");
                    _goapAgent.ResetPlan();
                    break;
                case AgentCommandEnum.PlanetCommandFailed:
                    _textOutput.Write(_state.AgentId, "Plan Failed (Planet)");
                    _goapAgent.ResetPlan();
                    break;
            }
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
            UInt64 curPlanet = _state.CurrentShipDockedPlanetScId;
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
        private bool checkOverMinimumTimeForMarketCheck(UInt64 planetScId, UInt64 tick)
        {
            if (!_memory.MarketLastCheckedTick.ContainsKey(planetScId))
            {
                return true;
            }
            else
            {
                UInt64 tickForMinNextMarketCheck = _memory.MarketLastCheckedTick[planetScId] + (DAYS_BEFORE_MARKET_RECHECK * Globals.DAYS_TO_TICKS_FACTOR);
                return tick >= tickForMinNextMarketCheck;
            }
        }

        // do we need to go planetside?
        private bool checkLeaveShip(MessageTick tick)
        {
            throw new NotImplementedException();
        }

        private object requestPlanetside(MessageTick tick)
        {
            throw new NotImplementedException();
        }

        private bool checkUndock(MessageTick tick)
        {
            throw new NotImplementedException();
        }

        public void RequestUndock()
        {
            _textOutput.Write(_state.AgentId, "Requesting undock from " + _state.CurrentShipDockedPlanetScId);
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipDocking(ShipCommandEnum.Undock, _state.CurrentShipDockedPlanetScId), _curTick, _state.CurrentShipId, _state.AgentId));
        }

        public void RequestDock()
        {
            _textOutput.Write(_state.AgentId, "Requesting dock from " + _memory.CurrentDestinationScId);
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipDocking(ShipCommandEnum.Dock, _memory.CurrentDestinationScId), _curTick, _state.CurrentShipId, _state.AgentId));
        }

        public void RequestLoadShip(ResourceQuantity resQ)
        {
            _textOutput.Write(_state.AgentId, "Requesting load resources " + resQ.Type + ":" + resQ.Quantity);            
            _actorSolarSystem.Tell(new MessagePlanetCommand(new MessagePlanetRequestShipResources(PlanetCommandEnum.RequestLoadShip, new List<ResourceQuantity>() { resQ }, _state.AgentId, _state.CurrentShipId), _curTick, _state.CurrentShipDockedPlanetScId));
        }

        public void RequestUnloadShip(ResourceQuantity resQ)
        {
            _textOutput.Write(_state.AgentId, "Requesting Unloading resources " + resQ.Type + ":" + resQ.Quantity);            
            _actorSolarSystem.Tell(new MessagePlanetCommand(new MessagePlanetRequestShipResources(PlanetCommandEnum.RequestUnloadShip, new List<ResourceQuantity>() { resQ }, _state.AgentId, _state.CurrentShipId), _curTick, _state.CurrentShipDockedPlanetScId));
        }     
        
        public void RequestCreateSellOrder(ResourceQuantity resQ)
        {
            _textOutput.Write(_state.AgentId, "Requesting create sell order " + resQ.Type + ":" + resQ.Quantity);
             _actorSolarSystem.Tell(new MessageMarketCommand(new MessageMarketGeneral(MarketCommandEnum.PlaceSellOrderLowest, resQ.Type, resQ.Quantity), _state.AgentId, _curTick, _state.CurrentShipDockedPlanetScId));
        }

        private void setNewDestination(UInt64 destinationScId)
        {
            _memory.CurrentDestinationScId = destinationScId;
            saveMemory();
            _textOutput.Write(_state.AgentId, "Setting new destination " + destinationScId);
            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetDestination(ShipCommandEnum.SetDestination, _memory.CurrentDestinationScId), 10, _state.CurrentShipId, _state.AgentId));
        }

        private void saveMemory()
        {
            _state.Memory = JsonConvert.SerializeObject(_memory);
        }

        public GoapStateBit GetWorldState(GoapPlanner _planner)
        {
            GoapStateBit worldData = new();

            if (_state.CurrentShipIsDocked)
            {
                worldData.SetFlagAndVal(GoapStateBitFlagsEnum.IsDocked, 1UL);
                worldData.SetFlagAndVal(GoapStateBitFlagsEnum.DockedAt, _state.CurrentShipDockedPlanetScId);
            }
            else
            {
                worldData.SetFlagAndVal(GoapStateBitFlagsEnum.IsDocked, 0UL);
                worldData.SetFlagAndVal(GoapStateBitFlagsEnum.DockedAt, 0UL);
            }
                        
            worldData.SetFlagAndVal(GoapStateBitFlagsEnum.ShipStoreId, _state.CurrentShipStoreId);

            List<ResourceQuantity> resources = _state.CurrentShipResources();
            foreach (ResourceQuantity resQ in resources)
            {               
                if (resQ.Quantity > 0 && _planner.TryAddResourceLocation(new GoapStateResLoc(resQ.Type, _state.CurrentShipStoreId), out var idx))
                {                             
                    worldData.SetResFlagAndVal(idx, resQ.Quantity);
                }
            }

            foreach (UInt64 destScId in _state.PlanetsInSolarSystemScIds)
            {
                ulong storeId;
                if (_state.TryGetPlanetStoreId(destScId, out storeId))
                {                    
                    resources = _state.PlanetResources(destScId);
                    foreach (ResourceQuantity resQ in resources)
                    {
                        if (resQ.Quantity > 0 && _planner.TryAddResourceLocation(new GoapStateResLoc(resQ.Type, storeId), out var idx))
                        {
                            worldData.SetResFlagAndVal(idx, resQ.Quantity);
                        }
                    }
                }
            }

            return worldData;
        }

        public (GoapStateBit,GoapStateBit) CreateGoalState(GoapPlanner _planner)
        {
            GoapStateBit goalState = new();
            GoapStateBit allowedState = new();

            ulong dest = chooseRandomDestinationScId();
            goalState.SetFlagAndVal(GoapStateBitFlagsEnum.IsDocked, 1UL);
            goalState.SetFlagAndVal(GoapStateBitFlagsEnum.DockedAt, dest);
            allowedState.SetFlagAndVal(GoapStateBitFlagsEnum.AllowedLoc1, dest);
            
            ResourceTypeEnum res = RandomUtils.Random(2) == 1 ? ResourceTypeEnum.Metal_Platinum : ResourceTypeEnum.Exotic_Spice;
            allowedState.SetFlagAndVal(GoapStateBitFlagsEnum.AllowedRes1, (ulong)res);           

            ulong storeId;
            if (_state.TryGetPlanetStoreId(dest, out storeId))
            {
                //long qty = (long)RandomUtils.Random(1) + 1L;
                //long qty = _state.PlanetResourceQuantity(dest, res) + (long)RandomUtils.Random(1) + 1L;
                long qty = _state.PlanetResourceQuantity(dest, res);

                //GoapStateResLoc resLoc = new(res, storeId); // planet store
                GoapStateResLoc resLoc = new(res, dest); // market
                if (_planner.TryGetResourceLocationIdx(resLoc, out int idx))
                {
                    goalState.SetResFlagAndVal(idx, qty);
                }
                else
                {
                    goalState.SetResFlagAndVal(_planner.AddResourceLocation(resLoc), qty);
                }
            }

            //_textOutput.Write(_state.AgentId, "Goal created " + GoapStateBit.PrettyPrint(goalState));
            _textOutput.Write(_state.AgentId, "Goal State created ");
            return (goalState,allowedState);
        }

        private UInt64 chooseRandomDestinationScId()
        {
            // choose randomly        
            List<UInt64> planetsToChooseFrom;
            if (_state.CurrentShipIsDocked)
                planetsToChooseFrom = _state.PlanetsInSolarSystemScIds.Where(x => x != _state.CurrentShipDockedPlanetScId).ToList();
            else
                planetsToChooseFrom = _state.PlanetsInSolarSystemScIds.ToList();
            int index = RandomUtils.Random(planetsToChooseFrom.Count);
            return planetsToChooseFrom[index];
        }

        public void PlanFailed(GoapStateBit failedGoal)
        {
            _textOutput.Write(_state.AgentId, "Plan failed "); //+ GoapStateBit.PrettyPrint(failedGoal);
        }

        public void PlanFound(GoapStateBit goal, Queue<GoapAction> actions, (int iterations, long ms) stats)
        {
            // We found a plan for our goal
            _textOutput.Write(_state.AgentId, "Plan found (" + stats.iterations + "," + stats.ms +"ms) " +GoapAction.PrettyPrint(actions));
       }

        public void ActionsFinished()
        {
            // Everything is done, we completed our actions for this gool. Hooray!
            _textOutput.Write(_state.AgentId, "Plan Completed");
        }

        public void PlanAborted(GoapAction aborter)
        {
            // An action bailed out of the plan. State has been reset to plan again.
            // Take note of what happened and make sure if you run the same goal again
            // that it can succeed.
            _textOutput.Write(_state.AgentId, "Plan Aborted " + GoapAction.PrettyPrint(aborter));
        }

        public bool MoveAgent(GoapAction nextAction)
        {
            UInt64 destinationScId = (UInt64)nextAction.target;
            if (!_state.CurrentShipHasDestination || _state.CurrentShipDestinationScID != destinationScId) // set destination based on action if we don't have one or it has changed
            {
                setNewDestination(destinationScId);
            }
            else
            {
                if (!_state.CurrentShipAutopilotActive) // turn on autopilot if off
                {
                    _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetAutopilot(ShipCommandEnum.SetAutopilot, true), 10, _state.CurrentShipId, _state.AgentId));
                    return false;
                }
                else
                {
                    if (_state.CurrentShipAtDestination(_memory.CurrentDestinationScId)) // check if we are there
                    {
                        nextAction.SetInRange(true);
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
        public List<GoapAction> GetActions()
        {
            List<GoapAction> actionsList = new List<GoapAction>
            {
                new GoapUndockAction(),
                new GoapDockGenericAction(_state.PlanetsInSolarSystemScIds.ToHashSet()),
                new GoapLoadShipGenericAction(),
                new GoapUnloadShipGenericAction(),
                new GoapCreateSellOrderGenericAction()
            };

            //foreach (Int64 destScId in _state.PlanetsInSolarSystemScIds)
            //{
            //    actionsList.Add(new GoapUndockAction(destScId));
            //    actionsList.Add(new GoapDockAction(destScId));
            //    long storeId;
            //    if (_state.TryGetPlanetStoreId(destScId, out storeId))
            //    {
            //        List<ResourceQuantity> resources = _state.PlanetResources(destScId);
            //        foreach (ResourceQuantity resQ in resources)
            //        {
            //            //if (resQ.Quantity > 0) actionsList.Add(new GoapLoadShipAction(destScId, storeId, _state.CurrentShipStoreId, resQ));
            //            if (resQ.Quantity > 0) actionsList.Add(new GoapLoadShipAction(destScId, storeId, _state.CurrentShipStoreId, new ResourceQuantity(resQ.Type,1L)));
            //        }
            //    }
            //}

            return actionsList;
        }

    }
}
