using System;
using Akka.Actor;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using GalaxyGenCore.StarChart;
using GalaxyGen.Engine.Messages;
using GalaxyGenCore.Framework;
using GalaxyGen.Framework;
using GalaxyGen.Engine.Goap;
using GalaxyGen.Engine.Goap.Actions;
using GalaxyGenCore.Resources;
using GalaxyGen.Engine.Goap.Core;
using GalaxyGen.Engine.Goap.Planner;
using GalaxyGen.Engine.Goap.Utilities;
using GalaxyGen.Engine.Goap.Fsm;

namespace GalaxyGen.Engine.Controllers.AgentDefault
{
    public class AgentDefaultController<T,W> : IAgentController, IReGoapAgent<T, W>
    {
        private const Int64 DAYS_BEFORE_MARKET_RECHECK = 7;
        private AgentControllerState _state;
        private IActorRef _actorSolarSystem;
        private IActorRef _actorTextOutput;
        private AgentDefaultMemory _memory;

        protected List<IReGoapGoal<T, W>> goals;
        protected List<IReGoapAction<T, W>> actions;
        protected IReGoapMemory<T, W> _goapMemory;
        protected IReGoapGoal<T, W> currentGoal;

        protected Dictionary<IReGoapGoal<T, W>, float> goalBlacklist;
        protected List<IReGoapGoal<T, W>> possibleGoals;
        protected bool possibleGoalsDirty;
        protected Queue<ReGoapActionState<T, W>> startingPlan;
        protected Dictionary<T, W> planValues;
        protected bool interruptOnNextTransition;
        public bool BlackListGoalOnFailure;

        public int CalculationDelay = 7;
        protected Int64 lastCalculationTime;
        private Int64 curTick;

        private IGoapPlanner<T, W> _planner;

        private FSM _stateMachine;
        private FSM.FSMState _idleState; // finds something to do
        private FSM.FSMState _moveToState; // moves to a target
        private FSM.FSMState _performActionState; // performs an action

        public AgentDefaultController(AgentControllerState ag, IActorRef actorSolarSystem, IActorRef actorTextOutput)
        {
            _state = ag;
            _actorSolarSystem = actorSolarSystem;
            _actorTextOutput = actorTextOutput;         
            _memory = JsonConvert.DeserializeObject<AgentDefaultMemory>(_state.Memory);
            if (_memory == null) _memory = new AgentDefaultMemory();

            _planner = new ReGoapPlanner<T, W>();
            _goapMemory  = new ReGoapMemory<T, W>();
            goals = new List<IReGoapGoal<T, W>>();
            possibleGoalsDirty = true;
            actions = new List<IReGoapAction<T, W>>();

            _stateMachine = new FSM();
            createIdleState();
            createMoveToState();
            createPerformActionState();
            _stateMachine.pushState(_idleState);
        }

        public void Tick(MessageTick tick)
        {
            curTick = tick.Tick;
            _stateMachine.Update(this);
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
            //_actorTextOutput.Tell("Loading resources " + resQ.Type + ":" + resQ.Quantity);
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

        //public Dictionary<string, object> GetWorldState()
        //{
        //    Dictionary<string, object> worldData = new Dictionary<string, object>();

        //    if (_state.CurrentShipIsDocked)
        //    {
        //        worldData.Add("isDocked", true);
        //        worldData.Add("DockedAt", _state.CurrentShipDockedPlanetScId);
        //    }
        //    else
        //    {
        //        worldData.Add("isDocked", false);
        //        worldData.Add("DockedAt", 0);
        //    }            

        //    return worldData;
        //}

        //public Dictionary<Int64, Int64> GetResourceState()
        //{
        //    Dictionary<Int64, Int64> resourceData = new Dictionary<Int64, Int64>();            

        //    // no resources

        //    return resourceData;
        //}

        //public Dictionary<string, object> CreateGoalState()
        //{
        //    Dictionary<string, object> goalState = new Dictionary<string, object>();

        //    goalState.Add("isDocked", true);
        //    goalState.Add("DockedAt", chooseRandomDestinationScId());

        //    return goalState;
        //}

        //public Dictionary<Int64, Int64> CreateResourceGoal()
        //{
        //    Dictionary<Int64, Int64> resourceGoal = new Dictionary<Int64, Int64>();

        //    resourceGoal.Add((Int64)ResourceTypeEnum.Platinum, 10);

        //    return resourceGoal;
        //}

        public void PlanFailed(IReGoapGoal<T, W> plan)
        {
            _actorTextOutput.Tell("Plan failed " + plan.ToString());
        }

        public void PlanFound(IReGoapGoal<T, W> plan)
        {
            // Yay we found a plan for our goal
            _actorTextOutput.Tell("Plan found " + plan.ToString());
        }

        public void ActionsFinished()
        {
            // Everything is done, we completed our actions for this gool. Hooray!
            _actorTextOutput.Tell("Plan Completed");
            // Console.WriteLine("<color=blue>Actions completed</color>");
        }

        public void PlanAborted(IReGoapAction<T, W> plan)
        {
            // An action bailed out of the plan. State has been reset to plan again.
            // Take note of what happened and make sure if you run the same goal again
            // that it can succeed.
            // Console.WriteLine("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
            _actorTextOutput.Tell("Plan Aborted " + plan.ToString());
        }

        //public bool MoveAgent(GoapAction nextAction)
        //{
        //    Int64 destinationScId = (Int64)nextAction.target;
        //    if (!_state.CurrentShipHasDestination || _state.CurrentShipDestinationScID != destinationScId) // set destination based on action if we don't have one or it has changed
        //    {
        //        setNewDestination(destinationScId);
        //    }
        //    else
        //    { 
        //        if (!_state.CurrentShipAutopilotActive) // turn on autopilot if off
        //        {
        //            _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetAutopilot(ShipCommandEnum.SetAutopilot, true), 10, _state.CurrentShipId));
        //            return false;
        //        }
        //        else
        //        {
        //            if (_state.CurrentShipAtDestination(_memory.CurrentDestinationScId)) // check if we are there
        //            {
        //                nextAction.setInRange(true);
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    //else
        //    //{
        //    //    // this code will never be reached at the moment as we use autopilot to move
        //    //    PointD curPoint = _state.CurrentShipXY;
        //    //    PointD newPoint = NavigationUtils.GetNewPointForShip(_state.CurrentShipCruisingSpeed, curPoint.X, curPoint.Y, _state.DestinationX(_memory.CurrentDestinationScId), _state.DestinationY(_memory.CurrentDestinationScId));
        //    //    _actorSolarSystem.Tell(new MessageShipCommand(new MessageShipSetXY(ShipCommandEnum.SetXY, newPoint.X, newPoint.Y), 10, _state.CurrentShipId));
        //    //    if (_state.XYAtDestination(_memory.CurrentDestinationScId, newPoint.X, newPoint.Y))
        //    //    {
        //    //        nextAction.setInRange(true);
        //    //        return true;
        //    //    }
        //    //    else
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    return false;
        //}

        //// actions this agent is capable of
        //public GoapAction[] GetActions()
        //{
        //    List<GoapAction> actionsList = new List<GoapAction>();


        //    // TODO limit number of destination actions we add to avoid combinatorial explosion
        //    foreach (Int64 destScId in _state.PlanetsInSolarSystemScIds)
        //    {
        //        actionsList.Add(new GoapUndockAction(destScId));
        //        actionsList.Add(new GoapDockAction(destScId));
        //        List<ResourceQuantity> resources = _state.PlanetResources(destScId);
        //        foreach (ResourceQuantity resQ in resources)
        //        {
        //            actionsList.Add(new GoapLoadShipAction(destScId, resQ));
        //        }                
        //    }

        //    GoapAction[] actions = actionsList.ToArray();
        //    return actions;
        //}

        public virtual IReGoapMemory<T, W> GetMemory()
        {
            return _goapMemory;
        }

        public virtual IReGoapGoal<T, W> GetCurrentGoal()
        {
            return currentGoal;
        }

        //public virtual void WarnPossibleGoal(IReGoapGoal<T, W> goal)
        //{
        //    if ((currentGoal != null) && (goal.GetPriority() <= currentGoal.GetPriority()))
        //        return;
        //    if (currentActionState != null && !currentActionState.Action.IsInterruptable())
        //    {
        //        interruptOnNextTransition = true;
        //        currentActionState.Action.AskForInterruption();
        //    }
        //    else
        //        CalculateNewGoal();
        //}

        protected virtual void CalculateNewGoal(bool forceStart = false)
        {
            if (!forceStart && (curTick - lastCalculationTime <= CalculationDelay))
                return;
            lastCalculationTime = curTick;

            interruptOnNextTransition = false;
            UpdatePossibleGoals();
        }

        protected virtual void UpdatePossibleGoals()
        {
            possibleGoalsDirty = false;
            if (goalBlacklist.Count > 0)
            {
                possibleGoals = new List<IReGoapGoal<T, W>>(goals.Count);
                foreach (var goal in goals)
                    if (!goalBlacklist.ContainsKey(goal))
                    {
                        possibleGoals.Add(goal);
                    }
                    else if (goalBlacklist[goal] < curTick)
                    {
                        goalBlacklist.Remove(goal);
                        possibleGoals.Add(goal);
                    }
            }
            else
            {
                possibleGoals = goals;
            }
        }

        protected virtual void ClearPlanValues()
        {
            if (planValues == null)
                planValues = new Dictionary<T, W>();
            else
            {
                planValues.Clear();
            }
        }

        //// TODO this shouldn't be threaded.
        //public virtual void WarnActionEnd(IReGoapAction<T, W> thisAction)
        //{
        //    if (thisAction != currentActionState.Action)
        //        return;
        //    //PushAction();
        //}

        //// TODO this shouldn't be threaded.
        //public virtual void WarnActionFailure(IReGoapAction<T, W> thisAction)
        //{
        //    if (currentActionState != null && thisAction != currentActionState.Action)
        //    {
        //        ReGoapLogger.LogWarning(string.Format("[GoapAgent] Action {0} warned for failure but is not current action.", thisAction));
        //        return;
        //    }
        //    if (BlackListGoalOnFailure)
        //        goalBlacklist[currentGoal] = curTick + currentGoal.GetErrorDelay();
        //    CalculateNewGoal(true);
        //}

        public virtual Queue<ReGoapActionState<T, W>> GetStartingPlan()
        {
            return startingPlan;
        }

        public virtual W GetPlanValue(T key)
        {
            return planValues[key];
        }

        public virtual void SetPlanValue(T key, W value)
        {
            planValues[key] = value;
        }

        public virtual bool HasPlanValue(T key)
        {
            return planValues.ContainsKey(key);
        }

        public virtual List<IReGoapGoal<T, W>> GetGoalsSet()
        {
            if (possibleGoalsDirty)
                UpdatePossibleGoals();
            return possibleGoals;
        }

        public virtual List<IReGoapAction<T, W>> GetActionsSet()
        {
            return actions;
        }

        public virtual ReGoapState<T, W> InstantiateNewState()
        {
            return ReGoapState<T, W>.Instantiate();
        }

        private void createIdleState()
        {
            _idleState = (fsm, gameObj) =>
            {
                // GOAP planning


                // Plan
                IReGoapGoal<T, W> plan = _planner.Plan(this, BlackListGoalOnFailure ? currentGoal : null, currentGoal != null ? currentGoal.GetPlan() : null, null);
                if (plan != null)
                {
                    // we have a plan, hooray!                    
                    currentGoal = plan;
                    startingPlan = currentGoal.GetPlan();
                    ClearPlanValues();
                    PlanFound(plan);

                    fsm.popState(); // move to PerformAction state
                    fsm.pushState(_performActionState);

                    foreach (var actionState in startingPlan)
                    {
                        actionState.Action.PostPlanCalculations(this);
                    }
                }
                else
                {
                    // ugh, we couldn't get a plan
                    // Console.WriteLine("<color=orange>Failed Plan:</color>" + PrettyPrint(goal));
                    PlanFailed(plan);
                    fsm.popState(); // move back to IdleAction state
                    fsm.pushState(_idleState);
                }

            };
        }

        private void createPerformActionState()
        {
            _performActionState = (fsm, gameObj) =>
            {
                // perform the action
                var plan = currentGoal.GetPlan();
                if (plan.Count == 0)
                {
                    // no actions to perform
                    // Console.WriteLine("<color=red>Done actions</color>");
                    fsm.popState();
                    fsm.pushState(_idleState);
                    ActionsFinished();
                    return;
                }

                IReGoapAction<T, W> action = plan.Peek().Action;
                if (action.isDone())
                {
                    // the action is done. Remove it so we can perform the next one
                    plan.Dequeue();
                }

                if (plan.Count > 0)
                {
                    // perform the next action
                    action = plan.Peek().Action;
                    bool inRange = action.requiresInRange() ? action.isInRange() : true;

                    if (inRange)
                    {
                        // we are in range, so perform the action
                        bool success = action.Perform(plan.Peek().Settings, currentGoal.GetGoalState());

                        if (!success)
                        {
                            // action failed, we need to plan again
                            fsm.popState();
                            fsm.pushState(_idleState);
                            PlanAborted(action);
                        }
                    }
                    else
                    {
                        // we need to move there first
                        // push moveTo state
                        fsm.pushState(_moveToState);
                    }

                }
                else
                {
                    // no actions left, move to Plan state
                    fsm.popState();
                    fsm.pushState(_idleState);
                    ActionsFinished();
                }

            };
        }

        private void createMoveToState()
        {
            _moveToState = (fsm, gameObj) =>
            {
                // move the game object
                var plan = currentGoal.GetPlan();
                IReGoapAction<T, W> action = plan.Peek().Action;
                if (action.requiresInRange() && action.target == null)
                {
                    // Console.WriteLine("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                    fsm.popState(); // move
                    fsm.popState(); // perform
                    fsm.pushState(_idleState);
                    return;
                }

                // get the agent to move itself
                if (_dataProvider.MoveAgent(action))
                {
                    fsm.popState();
                }
            };
        }

        
    }
}
