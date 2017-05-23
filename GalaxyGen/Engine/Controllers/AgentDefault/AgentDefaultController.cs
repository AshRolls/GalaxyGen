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

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController : IAgentController, IGoap
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
            _goapAgent = new GoapAgent(this);            
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

        public bool RequestUndock()
        {
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            setNewDestinationFromDocked();
            _actorSolarSystem.Ask(new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Undock, _state.CurrentShipDockedPlanetScId), 10, _state.CurrentShipId),TimeSpan.FromSeconds(60));
            return true;
        }

        public bool RequestDock()
        {
            //_actorTextOutput.Tell("Agent Requesting Undock from " + _currentShip.DockedPlanet.Name);
            _actorSolarSystem.Ask(new MessageShipCommand(new MessageShipBasic(ShipCommandEnum.Dock, _memory.CurrentDestinationScId), 10, _state.CurrentShipId), TimeSpan.FromSeconds(60));
            return true;
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

        public HashSet<KeyValuePair<string, object>> getWorldState()
        {
            HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

            worldData.Add(new KeyValuePair<string, object>("isDocked", (_state.CurrentShipIsDocked)));

            return worldData;
        }

        public HashSet<KeyValuePair<string, object>> createGoalState()
        {
            HashSet<KeyValuePair<string, object>> goalState = new HashSet<KeyValuePair<string, object>>();
            
            goalState.Add(new KeyValuePair<string, object>("isDocked", !_state.CurrentShipIsDocked));

            return goalState;
        }

        public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
        {

        }

        public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
        {
            // Yay we found a plan for our goal
            Console.WriteLine("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
        }

        public void actionsFinished()
        {
            // Everything is done, we completed our actions for this gool. Hooray!
            Console.WriteLine("<color=blue>Actions completed</color>");
        }

        public void planAborted(GoapAction aborter)
        {
            // An action bailed out of the plan. State has been reset to plan again.
            // Take note of what happened and make sure if you run the same goal again
            // that it can succeed.
            Console.WriteLine("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
        }

        public bool moveAgent(GoapAction nextAction)
        {
            //// move towards the NextAction's target
            //float step = moveSpeed * Time.deltaTime;
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);

            //if (gameObject.transform.position.Equals(nextAction.target.transform.position))
            //{
            //    // we are at the target location, we are done
            //    nextAction.setInRange(true);
            //    return true;
            //}
            //else
            //    return false;
            return true;
        }

        // actions this agent is capable of
        public GoapAction[] GetActions()
        {
            GoapAction[] actions = new GoapAction[2];
            actions[0] = new GoapUndockAction();
            actions[1] = new GoapDockAction();          
            return actions;
        }

    }
}
